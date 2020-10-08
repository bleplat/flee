Public Class Ship

    ' mode of behavior
    Public Enum BehaviorMode
        None
        Stand
        Drift
        Folow
        Mine
        GoToPoint
    End Enum

    Public world As World

    'IA
    Public bot_ship As Boolean = True
    Public AllowMining As Boolean = True
    Public behavior As BehaviorMode = BehaviorMode.Stand
    Public target As Ship = Nothing
    Public TargetPTN As New PointF(0, 0)
    Public agressivity As Double = 1.0

    ' shield effect
    Public Const SHIELD_POINTS As Integer = 16
    Public ShieldPoints(SHIELD_POINTS - 1) As Integer
    Public Sub ResetShieldPoint()
        For shield_ptn_value = 0 To SHIELD_POINTS - 1
            ShieldPoints(shield_ptn_value) = Math.Max(ShieldPoints(shield_ptn_value), 128)
        Next
    End Sub

    'main
    Private base_stats As ShipStats = Nothing
    Public stats As ShipStats = Nothing
    Public team As Team = Nothing
    Public color As Color = Color.White
    Public fram As UShort = 0

    'state
    Public integrity As Integer = 20
    Public location As New PointF(5000, 5000)
    Public speed_vec As New PointF()
    Public direction As Single = 0
    Public speed As Single = 0
    Public cold_deflector_charge As Integer = 0
    Public deflectors_loaded As Integer = 0
    Public deflector_loading As Integer = 0
    Public shield As Single = 0
    Public weapons As New List(Of Weapon)
    Public last_damager_team As Team = Nothing

    ' creation
    Public Sub New(ByRef world As World)
        Me.world = world
        ResetShieldPoint()
        Me.TargetPTN = New PointF(Me.location.X, Me.location.Y)
    End Sub
    Public Sub New(ByRef world As World, ship_class As String)
        Me.world = world
        SetStats(ship_class)
        ResetShieldPoint()
        Me.TargetPTN = New PointF(Me.location.X, Me.location.Y)
    End Sub
    Public Sub New(ByRef world As World, team As Team, ship_class As String)
        Me.world = world
        Me.SetTeam(team)
        SetStats(ship_class)
        ResetShieldPoint()
        Me.TargetPTN = New PointF(Me.location.X, Me.location.Y)
    End Sub
    Public Sub SetStats(ship_class As String)
        SetStats(ShipStats.classes(ship_class))
    End Sub
    Public Sub SetStats(stats As ShipStats)
        If Not Me.base_stats Is stats Then
            Me.base_stats = stats
            ' native upgrades
            If Me.Ups.Count = 0 Then
                For Each native_upgrade_name As String In Me.base_stats.native_upgrades
                    Me.Ups.Add(Upgrade.UpgradeFromName(native_upgrade_name))
                Next
            End If
            ' upgrade_slots
            If Me.upgrade_slots < 0 Then ' initial value is -1
                Me.upgrade_slots = Me.base_stats.level
                If Me.base_stats.repair > 0 Then ' for now, a ships which cannot repair is not inhabited and cannot upgrade
                    If Not Me.team Is Nothing Then
                        Me.upgrade_slots += Me.team.upgrade_slots_bonus
                    End If
                End If
            End If
            ' Reset Weapons
            Me.weapons.Clear()
            For Each gun_name As String In Me.base_stats.default_weapons
                Me.weapons.Add(New Weapon(Me, gun_name))
            Next
            ' force a color
            Select Case Me.base_stats.name
                Case "Star"
                    behavior = BehaviorMode.Drift : target = Nothing : color = Color.FromArgb(255, 255, 220)
                Case "Asteroid"
                    behavior = BehaviorMode.Drift : target = Nothing : color = Color.FromArgb(64, 64, 48)
                Case "Meteoroid"
                    behavior = BehaviorMode.Drift : target = Nothing : color = Color.FromArgb(80, 48, 80)
                Case "Comet"
                    behavior = BehaviorMode.Drift : target = Nothing : color = Color.FromArgb(0, 100, 0)
            End Select
            '
            ResetStats()
        End If
    End Sub
    Public Sub SetTeam(team As Team)
        If Not Me.team Is team Then
            Me.team = team
            If Not Me.team Is Nothing Then
                Me.bot_ship = Me.team.bot_team
                Me.color = team.color
            End If
        End If
    End Sub
    Public Sub ResetStats()
        If Me.stats Is Nothing Then
            Me.integrity = Me.base_stats.integrity
            Me.shield = Me.base_stats.shield
        End If
        Me.stats = Me.base_stats.Clone()
        For Each weapon As Weapon In weapons
            weapon.ResetStats()
        Next
    End Sub

    ' TODO: Remove
    Public Sub TempForceUpdateStats()
        If Me.base_stats Is Nothing Then
            Me.base_stats = stats.Clone()
            base_stats.default_weapons.Clear()
            For Each a_weapon As Weapon In weapons
                base_stats.default_weapons.Add(a_weapon.ToString())
            Next
            If Not ShipStats.classes.ContainsKey(base_stats.name) Then
                ShipStats.classes(base_stats.name) = base_stats.Clone()
            End If
        End If
    End Sub

    Public Sub SetType(ByVal SType As String, ByVal STeam As Team, Optional ByVal IsNew As Boolean = False)
        If Me.base_stats Is Nothing Then
            Me.SetStats(SType)
        End If
        Me.SetTeam(STeam)
        ' Ship just created TODO: NOW: move to constructor
        If IsNew Then
            Me.Upgrading = Nothing
            ' state
            integrity = stats.integrity
            shield = stats.shield
            If STeam Is Nothing OrElse STeam.id = 0 Then
                bot_ship = False
            Else
                bot_ship = True
            End If
            If Not STeam Is Nothing AndAlso behavior <> BehaviorMode.Drift Then
                color = STeam.color
            End If
            If Not team Is Nothing AndAlso upgrade_slots > 0 Then
                upgrade_slots += team.upgrade_slots_bonus
            End If
        End If
    End Sub
    Public Sub Check()
        '===' Fram '==='
        fram = fram + 1
        If fram > 7 Then fram = 0
        '===' Bordures '==='
        If Not Me.team Is Nothing AndAlso Me.team.id = 0 Then
            If Me.location.X < 0 Then
                Me.location.X = 0
            End If
            If Me.location.Y < 0 Then
                Me.location.Y = 0
            End If
            If Me.location.X > world.ArenaSize.Width Then
                Me.location.X = world.ArenaSize.Width
            End If
            If Me.location.Y > world.ArenaSize.Height Then
                Me.location.Y = world.ArenaSize.Height
            End If
        Else
            If Me.location.X < -100 OrElse Me.location.Y < -100 OrElse Me.location.X > world.ArenaSize.Width + 100 OrElse Me.location.Y > world.ArenaSize.Height + 100 Then
                If Me.bot_ship = False Then Me.integrity = 0
            End If
        End If
        ' movement
        If stats.turn = 0 AndAlso stats.speed = 0 Then
            direction = direction + 25 / Me.stats.width
        Else
            Dim new_speed As PointF = Helpers.GetNewPoint(New Point(0, 0), direction, speed)
            Me.speed_vec = New PointF(speed_vec.X * 0.925 + new_speed.X * 0.075, speed_vec.Y * 0.925 + new_speed.Y * 0.075)
            Me.location.X = Me.location.X + speed_vec.X
            Me.location.Y = Me.location.Y + speed_vec.Y
        End If
        '===' Armes '==='
        If Me.cold_deflector_charge <= 0 Then
            For Each AWeapon As Weapon In weapons
                If AWeapon.Bar = 0 Then
                    AWeapon.Load = AWeapon.Load + 1
                    If AWeapon.Load >= AWeapon.stats.loadtime Then
                        AWeapon.Load = 0
                        AWeapon.Bar = AWeapon.stats.salvo
                    End If
                End If
            Next
        End If
        '===' Shield '==='
        If stats.shield > 0 Then
            shield = shield + (stats.shield_regeneration / 100)
            If shield > stats.shield Then shield = stats.shield
            Dim point_min = Math.Max(0, shield * 32 / stats.shield)
            For i = 0 To SHIELD_POINTS - 1
                ShieldPoints(i) -= 4
                If ShieldPoints(i) < point_min Then
                    ShieldPoints(i) = point_min
                End If
            Next
        End If
        '===' Deflector '==='
        If Me.cold_deflector_charge > 0 Then
            Me.cold_deflector_charge *= 0.99
            Me.cold_deflector_charge -= 1
            If world.ticks Mod 2 Then
                Me.integrity -= 1
            End If
        End If
        If deflectors_loaded < stats.deflectors Then
            deflector_loading -= 1
            If deflector_loading <= 0 Then
                deflectors_loaded += 1
                deflector_loading = stats.deflectors_cooldown
            End If
        Else
            deflector_loading = stats.deflectors_cooldown
        End If
        '===' Vie '==='
        If world.ticks Mod 40 = 0 Then
            If Not Me.team Is Nothing Then
                Me.integrity = Me.integrity + stats.repair
                If Me.integrity > Me.stats.integrity Then Me.integrity = Me.stats.integrity
            End If
        End If
        '===' Upgrades '==='
        If Not Upgrading Is Nothing Then
            If UpProgress < Upgrading.delay Then
                UpProgress = UpProgress + 1
                If MainForm.cheats_enabled Then
                    UpProgress = UpProgress + 99
                End If
            Else
                If Upgrading.name.Contains("Pointvortex") Then
                    Console.WriteLine()
                End If
                If Upgrading.upgrade_slots_requiered > 0 Then
                    Ups.Add(Upgrading)
                End If
                'Appliquation debugage 'TODO: NOW: test without this
                'Dim spliter() As String = Upgrading.Effect.Split(" ")
                ' For Each aspli As String In spliter
                'Me.ApplyUpgradeEffect(aspli, True)
                'Next
                'actualisation vaisseau
                Me.ApplyUpgradeFirstTime(Upgrading)
                Me.ResetStats()
                Me.ApplyUpgrades()
                Upgrading = Nothing
                UpProgress = 0
            End If
        End If
        '===' Autre '==='

    End Sub
    Public Sub TurnToQA(ByVal qa As Single)
        While qa > 360
            qa -= 360
        End While
        While qa < 0
            qa += 360
        End While
        While direction > 360
            direction = direction - 360
        End While
        While direction < 0
            direction = direction + 360
        End While
        If Helpers.GetAngleDiff(direction, qa) < stats.turn Then
            direction = qa
            Exit Sub
        Else
            If qa > 180 Then
                If direction > qa - 180 AndAlso direction < qa Then
                    direction = direction + stats.turn
                Else
                    direction = direction - stats.turn
                End If
            Else
                If direction < qa + 180 AndAlso direction > qa Then
                    direction = direction - stats.turn
                Else
                    direction = direction + stats.turn
                End If
            End If
        End If

    End Sub
    Sub TakeDamages(ByVal Amount As Integer, Optional ByRef From As Shoot = Nothing)
        If Me.deflectors_loaded > 0 Then
            Me.deflectors_loaded -= 1
            Return
        End If
        If Me.shield > 0 Then
            Me.shield = Me.shield - Amount
            Amount = Amount - (Amount * stats.shield_opacity / 100)
            If Not From Is Nothing Then
                Dim angle_ship_shoot_rel As Double = Helpers.NormalizeAngleUnsigned(Helpers.GetAngle(location.X, location.Y, From.Coo.X, From.Coo.Y) - direction)
                Dim shield_ptn_index As Integer = (angle_ship_shoot_rel * 16 / 360)
                ShieldPoints(shield_ptn_index Mod 16) = 255
                If ShieldPoints((shield_ptn_index + 1) Mod 16) < 128 Then ShieldPoints((shield_ptn_index + 1) Mod 16) = 128
                If ShieldPoints((shield_ptn_index + 15) Mod 16) < 128 Then ShieldPoints((shield_ptn_index + 15) Mod 16) = 128
            End If
        End If
        If Me.stats.cold_deflector AndAlso Me.cold_deflector_charge < Me.integrity * 4 Then
            Me.cold_deflector_charge += Amount
            Amount /= 8
        End If
        If Not From Is Nothing AndAlso Not From.Team Is Nothing Then
            If Me.stats.sprite = "Star" Then
                From.Team.resources.Antimatter += Amount / 8
            ElseIf Me.stats.sprite = "Asteroid" Then
                If Me.integrity > Amount Then
                    From.Team.resources.Metal += Amount
                ElseIf Me.integrity > 0 Then
                    From.Team.resources.Metal += Me.integrity
                End If
            End If
        End If
        If Amount < 0 Then Return
        Me.integrity -= Amount
        If Me.integrity < 0 Then Me.integrity = 0
    End Sub


    Public Sub IA(rnd_num As Integer)
        Dim QA As Single
        Dim NeedSpeed As Boolean = False
        ' remove destroyed target
        If Not Me.target Is Nothing AndAlso Me.target.IsDestroyed() Then
            Me.target = Nothing
        End If
        '===' Fin de poursuite '==='
        If Me.behavior = BehaviorMode.Folow AndAlso target Is Nothing Then
            Me.behavior = BehaviorMode.Stand
        End If
        ' no team mean drifting
        If Me.team Is Nothing Then
            Me.behavior = BehaviorMode.Drift
        End If
        '===' Auto-Activation '==='
        If rnd_num < 100 Then
            Me.agressivity += 0.05
        End If
        If Me.bot_ship AndAlso Me.behavior <> BehaviorMode.Drift AndAlso Not Me.team Is Nothing Then
            If Me.target Is Nothing Then
                Dim nearest_ship As Ship = Me.GetClosestShip(Me.agressivity, 1.0, 0.1)
                If Not nearest_ship Is Nothing Then
                    Me.target = nearest_ship
                    Me.behavior = BehaviorMode.Folow
                End If
            Else
                If rnd_num < 6 OrElse (Not Me.team Is Me.target.team AndAlso rnd_num < 100) Then 'chance to change target
                    Me.target = Nothing
                    Me.behavior = BehaviorMode.Stand
                End If
            End If
        End If
        '===' Execution '==='
        Select Case behavior
            Case BehaviorMode.Mine
                Me.AllowMining = True
                If Not Me.target Is Nothing Then
                    ' has mining target already
                    QA = Helpers.GetQA(Me.location.X, Me.location.Y, Me.target.location.X, Me.target.location.Y)
                    Dim optimal_range As Double = 50 : If weapons.Count > 0 Then optimal_range = Me.weapons(0).stats.range * 0.5
                    Dim rel_dist As Double = Helpers.Distance(Me.location, Me.target.location) - (Me.target.stats.width / 2)
                    If rel_dist <= optimal_range Then
                        ' turn if too close
                        If Helpers.GetAngleDiff(Me.direction, QA) < 120 Then
                            QA = QA + 180
                        End If
                        NeedSpeed = True
                    Else
                        NeedSpeed = Helpers.GetAngleDiff(Me.direction, QA) < 90
                    End If
                    If Helpers.Distance(Me.TargetPTN, Me.target.location) > world.ArenaSize.Width / 8 Then
                        ' abort target if too far away from mining point
                        Me.target = Nothing
                    End If
                Else
                    ' no current mining target
                    Dim max_mining_distance As Integer = world.ArenaSize.Width / 8
                    Dim mining_target As Ship = Me.GetClosestShip(0.0, 1.0)
                    If Not mining_target Is Nothing AndAlso Helpers.Distance(Me.TargetPTN, mining_target.location) > max_mining_distance Then
                        mining_target = Nothing
                    End If
                    Me.target = mining_target
                    If Me.target Is Nothing Then
                        QA = Helpers.GetQA(Me.location.X, Me.location.Y, Me.TargetPTN.X, Me.TargetPTN.Y)
                        NeedSpeed = True
                    End If
                End If
            Case BehaviorMode.Folow
                If Not Me.target Is Nothing Then
                    Dim forseen_location As PointF = Me.ForseeLocation(Me.target)
                    'world.Effects.Add(New Effect With {.Type = "Cible", .Coo = forseen_location, .Direction = 45, .speed = 0})
                    QA = Helpers.GetQA(Me.location.X, Me.location.Y, forseen_location.X, forseen_location.Y)
                    Dim rel_forseen_dist As Double = Helpers.Distance(Me.location, forseen_location) - (Me.target.stats.width / 2)
                    Dim optimal_range As Double = 50 : If weapons.Count > 0 Then optimal_range = (Me.weapons(0).stats.range * Me.weapons(0).stats.range / rel_forseen_dist) * 0.8 ' TODO: instead of this factor, just use the forseen location of the target
                    If Helpers.Distance(Me.location, Me.target.location) <= optimal_range Then
                        If Helpers.GetAngleDiff(Me.direction, QA) < 112 Then
                            QA = QA + 180
                            NeedSpeed = Helpers.GetAngleDiff(Me.direction, QA - 180) > 45
                        Else
                            NeedSpeed = Helpers.GetAngleDiff(Me.direction, QA) > 90
                        End If
                    Else
                        If Me.target.stats.speed * 1.1 >= Me.stats.speed Then
                            Dim angle_to_target As Double = Helpers.GetQA(Me.location.X, Me.location.Y, target.location.X, target.location.Y)
                            If Helpers.GetAngleDiff(angle_to_target, QA) > 90 Then
                                QA = angle_to_target
                            End If
                        End If
                        NeedSpeed = Helpers.GetAngleDiff(Me.direction, QA) < 90
                    End If
                    'NeedSpeed = True
                End If
            Case BehaviorMode.Stand
                QA = direction
                NeedSpeed = False
            Case BehaviorMode.Drift
                QA = direction
                NeedSpeed = True
            Case BehaviorMode.GoToPoint
                QA = Helpers.GetQA(Me.location.X, Me.location.Y, Me.TargetPTN.X, Me.TargetPTN.Y)
                If Helpers.Distance(Me.location.X, Me.location.Y, Me.TargetPTN.X, Me.TargetPTN.Y) <= 50 Then
                    Me.behavior = BehaviorMode.Stand
                End If
                NeedSpeed = True
        End Select
        '===' Appliquation '==='
        TurnToQA(QA)
        If NeedSpeed Then
            Me.speed = Me.speed + Me.stats.turn / 20
        Else
            Me.speed = Me.speed - Me.stats.turn / 20
        End If
        If Me.speed > Me.stats.speed Then
            Me.speed -= 1
            If Me.speed < Me.stats.speed Then Me.speed = Me.stats.speed
        End If
        If Me.speed < 0 Then Me.speed = 0
        IAFire()
    End Sub 'AIAIAIAIAI
    Public Sub IAFire()
        '===' Tirer '==='
        If weapons.Count > 0 AndAlso (fram Mod 2 = 0) Then
            For Each AWeap As Weapon In weapons
                If AWeap.Bar > 0 Then
                    Dim closest_score As Double = Double.MaxValue
                    Dim weapon_targeted_ship As Ship = Nothing 'Pas de cible
                    Dim weapon_location_x As Integer = (Math.Sin(2 * Math.PI * (AWeap.Loc + direction) / 360) * (stats.width / 2)) + location.X
                    Dim weapon_location_y As Integer = (Math.Cos(2 * Math.PI * (AWeap.Loc + direction) / 360) * (stats.width / 2)) + location.Y
                    ' target in range
                    If Not Me.target Is Nothing Then
                        If Me.team Is Nothing OrElse Not Me.team.IsFriendWith(Me.target.team) Then
                            Dim dist As Double = Helpers.Distance(Me.location, AWeap.ForseeShootingLocation(Me.target)) - (Me.target.stats.width / 2)
                            If dist < AWeap.stats.range * 0.9 Then
                                weapon_targeted_ship = Me.target
                            End If
                        End If
                    End If
                    ' target not in range, find another
                    If weapon_targeted_ship Is Nothing Then
                        For Each OVessel As Ship In world.Ships
                            If OVessel Is Me Then
                                Continue For
                            End If
                            If Not Me.AllowMining AndAlso (OVessel.stats.default_weapons.Count = 0 OrElse AWeap.stats.power = 0) Then
                                Continue For
                            End If
                            If Helpers.Distance(Me.location, OVessel.location) < AWeap.stats.range Then
                                If Me.team Is Nothing OrElse Not Me.team.IsFriendWith(OVessel.team) Then
                                    'Dim dist As Integer = Helpers.Distance(ToX, ToY, OVessel.location.X, OVessel.location.Y) - OVessel.stats.width / 2
                                    Dim score As Double = Helpers.Distance(Me.location, AWeap.ForseeShootingLocation(OVessel)) - (OVessel.stats.width / 2)
                                    If score < AWeap.stats.range * 0.9 Then
                                        If score < AWeap.stats.range Then
                                            If Me.team Is Nothing OrElse Not OVessel.team Is Nothing AndAlso Not Me.team.IsFriendWith(OVessel.team) Then
                                                score /= 8
                                            End If
                                            If Me.target Is OVessel Then
                                                score /= 4
                                            End If
                                        End If
                                        If score < closest_score Then
                                            closest_score = score
                                            weapon_targeted_ship = OVessel
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                    ' shooting if in range
                    If Not weapon_targeted_ship Is Nothing Then
                        Dim weapon_target_point As PointF = AWeap.ForseeShootingLocation(weapon_targeted_ship)
                        Dim QA As Integer = Helpers.GetQA(weapon_location_x, weapon_location_y, weapon_target_point.X, weapon_target_point.Y)
                        AWeap.Fire(QA, New Point(weapon_location_x, weapon_location_y), Me)
                        If (AWeap.stats.special And Weapon.SpecialBits.SelfExplode) <> 0 Then
                            Me.integrity = -2048
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    ' get updates to display
    Public Shared Function ListedUpgrades(ships As List(Of Ship))
        Dim met_upgrades As List(Of Upgrade) = New List(Of Upgrade)
        For Each upgrade As Upgrade In Upgrade.upgrades
            If Ship.CountShipsListingUpgrade(ships, upgrade) = ships.Count() Then
                met_upgrades.Add(upgrade)
            End If
        Next
        Return met_upgrades
    End Function

    ' get all possible upgrades
    Public Function ConditionsMetUpgrades() As List(Of Upgrade)
        Dim met_upgrades As List(Of Upgrade) = New List(Of Upgrade)
        For Each upgrade As Upgrade In Upgrade.upgrades
            If IsUpgradeCompatible(upgrade) OrElse Me.Ups.Contains(upgrade) Then
                met_upgrades.Add(upgrade)
            End If
        Next
        Return met_upgrades
    End Function
    ' get all possible upgrades
    Public Function AvailableUpgrades() As List(Of Upgrade)
        Dim possible_upgrades As List(Of Upgrade) = New List(Of Upgrade)
        For Each upgrade As Upgrade In Upgrade.upgrades
            If CanUpgrade(upgrade) AndAlso (Not Me.bot_ship OrElse Not upgrade.not_for_bots) Then
                possible_upgrades.Add(upgrade)
            End If
        Next
        Return possible_upgrades
    End Function
    ' return true if the upgrade is available
    Public Function CanUpgrade(upgrade As Upgrade) As Boolean
        If MainForm.cheats_enabled Then Return True
        If Not Me.Upgrading Is Nothing Then Return False
        If Me.upgrade_slots - Me.Ups.Count < upgrade.upgrade_slots_requiered Then Return False
        If upgrade.not_for_bots AndAlso Me.bot_ship Then Return False
        If Me.HaveUp(upgrade) Then Return False
        Return IsUpgradeCompatible(upgrade)
    End Function
    ' return true if the upgrade have its conditions met
    Public Function IsUpgradeCompatible(upgrade As Upgrade) As Boolean
        If MainForm.cheats_enabled Then Return True
        If upgrade Is Me.Upgrading Then Return True
        If Not upgrade.spawned_ship Is Nothing Then
            If Not Me.stats.crafts.Contains(upgrade.spawned_ship) Then Return False
        End If
        Dim conditions_strings() As String = upgrade.need.Split(" ")
        For Each a_condition As String In conditions_strings
            If Not IsUpgradeConditionMet(a_condition) Then
                Return False
            End If
        Next
        Return True
    End Function
    Public Shared Function CountShipsListingUpgrade(ships As List(Of Ship), upgrade As Upgrade) As Integer
        Dim count As Integer = 0
        For Each ship As Ship In ships
            If ship.IsUpgradeCompatible(upgrade) OrElse ship.Ups.Contains(upgrade) Then
                count += 1
            End If
        Next
        Return count
    End Function
    Public Shared Function CountShipsBuyableNowUpgrade(ships As List(Of Ship), upgrade As Upgrade) As Integer
        Dim count As Integer = 0
        For Each ship As Ship In ships
            If ship.Upgrading Is Nothing Then
                If ship.IsUpgradeCompatible(upgrade) AndAlso Not ship.Ups.Contains(upgrade) AndAlso ship.upgrade_slots - ship.Ups.Count - upgrade.upgrade_slots_requiered >= 0 Then
                    count += 1
                End If
            End If
        Next
        Return count
    End Function
    Public Shared Function CountShipsHavingUpgrade(ships As List(Of Ship), upgrade As Upgrade) As Integer
        Dim count As Integer = 0
        For Each ship As Ship In ships
            If ship.Ups.Contains(upgrade) Then
                count += 1
            End If
        Next
        Return count
    End Function
    ' test a single upgrade condition
    Public Function IsUpgradeConditionMet(ByVal chain As String) As Boolean
        Dim Spliter() As String = chain.Split(":")
        Select Case Spliter(0)
            Case "" 'Debug
                Return True
            Case "?W"
                If weapons.Count > 0 Then Return True
            Case "?S"
                If stats.speed > 0 Then Return True
            Case "?Base"
                If Me.stats.name.Contains("Station") Then Return True
            Case "?NotStation"
                If Not Me.stats.name.Contains("Station") Then Return True
            Case "+Lvl"
                If Me.stats.level >= Spliter(1) Then Return True
            Case "+Speed" 'vitesse
                If Me.stats.speed >= Spliter(1) Then Return True
            Case "-Speed"
                If Me.stats.speed <= Spliter(1) Then Return True
            Case "+Life" 'Resistance
                If Me.stats.integrity >= Spliter(1) Then Return True
            Case "-Life" '
                If Me.stats.integrity <= Spliter(1) Then Return True
            Case "?Up"
                If Me.HaveUp(Upgrade.UpgradeFromName(Spliter(1))) Then Return True
            Case "?Type" 'Type
                If Me.stats.sprite = Spliter(1) Then Return True
            Case "?Wtype" 'armement
                If Me.weapons(0).stats.sprite = Spliter(1) Then Return True
            Case "?MS"
                If Me.team Is Nothing OrElse world.CountTeamShips(team) < Me.team.ship_count_limit Then Return True
            Case Else
                Throw New Exception("Erreur : " & chain & " (invalid condition)")
        End Select
        Return False
    End Function

    ' get the minimum loading progress of an upgrade for a list of ship, or int.MaxValue if not being upgraded
    Public Shared Function MinUpgradeProgress(ships As List(Of Ship), upgrade As Upgrade) As Integer
        Dim min As Integer = Int32.MaxValue
        For Each ship As Ship In ships
            If ship.Upgrading Is upgrade Then
                min = Math.Min(min, ship.UpProgress)
            End If
        Next
        Return min
    End Function

    ' apply all upgrades effects this ship have
    Public Sub ApplyUpgrades()
        For Each AUp As Upgrade In Me.Ups
            Dim spliter() As String = AUp.effect.Split(" ")
            For Each Aspli As String In spliter
                ApplyUpgradeEffect(Aspli, False)
            Next
        Next
    End Sub
    ' apply a single upgrade effects to this ship
    Public Sub ApplyUpgradeFirstTime(ByRef upgrade As Upgrade)
        Dim spliter() As String = upgrade.effect.Split(" ")
        For Each Aspli As String In spliter
            ApplyUpgradeEffect(Aspli, True)
        Next
    End Sub
    ' apply an upgrade effect
    Public Sub ApplyUpgradeEffect(ByVal Chain As String, ByVal first_application As Boolean)
        Dim Spliter() As String = Chain.Split(":")
        Select Case Spliter(0)
            Case ""
            Case "!C"
                Me.color = Color.FromName(Spliter(1))
                If Me.stats.sprite = "Station" Then
                    Me.team.color = Me.color
                End If
            Case "!Jump"
                Me.speed = Convert.ToInt32(Spliter(1))
            Case "!Agility"
                Me.stats.turn += Spliter(1)
            Case "!Teleport"
                world.Effects.Add(New Effect With {.Type = "Teleported", .Coo = Me.location, .Direction = 0, .speed = 0})
                Dim tp_dst As PointF = Me.TargetPTN
                If Not Me.target Is Nothing Then
                    tp_dst = Me.target.location
                End If
                Me.location = New PointF(tp_dst.X + world.gameplay_random.Next(-512, 512), tp_dst.Y + world.gameplay_random.Next(-512, 512))
                world.Effects.Add(New Effect With {.Type = "Teleported", .Coo = Me.location, .Direction = 0, .speed = 0})
            Case "!Upsbonus"
                If first_application Then Me.team.upgrade_slots_bonus += Spliter(1) 'FN
            Case "!Maxships"
                If first_application Then Me.team.ship_count_limit += Spliter(1) 'FN
            Case "!Shield"
                Me.stats.shield += Spliter(1)
                If first_application Then Me.ResetShieldPoint()
            Case "!Deflector"
                Me.stats.deflectors += Spliter(1)
            Case "!HotDeflector"
                Me.stats.hot_deflector += Spliter(1)
            Case "!ColdDeflector"
                Me.stats.cold_deflector = (Convert.ToInt32(Spliter(1)) = 1)
            Case "%Shield"
                Me.stats.shield += (Me.stats.shield * (Spliter(1) / 100))
                If first_application Then Me.ResetShieldPoint()
            Case "!Shieldop"
                Me.stats.shield_opacity += Spliter(1)
                If first_application Then Me.ResetShieldPoint()
            Case "%Shieldreg"
                Me.stats.shield_regeneration += (Me.stats.shield_regeneration * (Spliter(1) / 100))
                If first_application Then Me.ResetShieldPoint()
            Case "!UpsMax"
                Me.upgrade_slots += Spliter(1)
            Case "!Fix"
                Me.integrity += Me.stats.integrity * (Spliter(1) / 100.0)
            Case "!FixSFull"
                Me.shield = Me.stats.shield
            Case "%Speed"
                Me.stats.speed += (Me.stats.speed * (Spliter(1) / 100))
                Me.stats.turn += (Me.stats.turn * (Spliter(1) / 100))
            Case "%Life"
                Me.stats.integrity += (Me.stats.integrity * (Spliter(1) / 100))
                If first_application Then Me.integrity += (Me.stats.integrity * (Spliter(1) / 100)) 'FN
            Case "!Regen"
                Me.stats.repair += Convert.ToInt32(Spliter(1)) 'FN
            Case "!Type"
                If first_application Then Me.SetStats(Spliter(1))' : Me.stats.sprite = Spliter(1)
            Case "%Wloadmax"
                'For Each AW As Weapon In weapons
                'AW.stats.loadtime += AW.stats.loadtime * (Spliter(1) / 100)
                'Next
                If Me.weapons.Count <> 0 Then
                    Me.weapons(0).stats.loadtime += Me.weapons(0).stats.loadtime * (Helpers.ToDouble(Spliter(1)) / 100.0)
                End If
            Case "%Wbar"
                'For Each AW As Weapon In weapons
                'AW.stats.salvo += AW.stats.salvo * (Spliter(1) / 100)
                'Next
                If Me.weapons.Count <> 0 Then
                    Me.weapons(0).stats.salvo += Me.weapons(0).stats.salvo * (Helpers.ToDouble(Spliter(1)) / 100.0)
                    If first_application Then
                        Me.weapons(0).Bar = 0
                        Me.weapons(0).Load = 0
                    End If
                End If
            Case "%Wpower"
                'For Each AW As Weapon In weapons
                'AW.stats.power += AW.stats.power * (Spliter(1) / 100)
                'Next
                If Me.weapons.Count <> 0 Then
                    Me.weapons(0).stats.power += Me.weapons(0).stats.power * (Helpers.ToDouble(Spliter(1)) / 100.0)
                End If
            Case "%Wrange"
                'For Each AW As Weapon In weapons
                'AW.stats.range += AW.stats.range * (Spliter(1) / 100)
                'Next
                If Me.weapons.Count <> 0 Then
                    Me.weapons(0).stats.range += Me.weapons(0).stats.range * (Helpers.ToDouble(Spliter(1)) / 100.0)
                End If
            Case "%Wcelerity"
                'For Each AW As Weapon In weapons
                'AW.stats.celerity += AW.stats.celerity * (Spliter(1) / 100)
                'Next
                If Me.weapons.Count <> 0 Then
                    Me.weapons(0).stats.celerity += Me.weapons(0).stats.celerity * (Helpers.ToDouble(Spliter(1)) / 100.0)
                End If
            Case "!Sum"
                If first_application Then world.Ships.Add(New Ship(world, Me.team, Spliter(1)) With {.location = New Point(Me.location.X + world.gameplay_random.Next(-10, 11), Me.location.Y + world.gameplay_random.Next(-10, 11))})
                world.Ships(world.Ships.Count - 1).direction = Me.direction
                If world.Ships(world.Ships.Count - 1).weapons.Count() > 0 AndAlso (world.Ships(world.Ships.Count - 1).weapons(0).stats.special And Weapon.SpecialBits.SelfExplode) <> 0 Then
                    If Me.team Is Nothing OrElse Me.target Is Nothing OrElse Me.target.team Is Nothing OrElse Not Me.team.IsFriendWith(Me.target.team) Then
                        world.Ships(world.Ships.Count - 1).target = Me.target
                    Else
                        world.Ships(world.Ships.Count - 1).target = Nothing
                    End If
                    world.Ships(world.Ships.Count - 1).behavior = BehaviorMode.Folow
                    world.Ships(world.Ships.Count - 1).agressivity = Me.agressivity * 100
                    world.Ships(world.Ships.Count - 1).bot_ship = True
                Else
                    world.Ships(world.Ships.Count - 1).behavior = BehaviorMode.Folow
                    world.Ships(world.Ships.Count - 1).target = Me
                End If
            Case "!Ascend"
                If first_application AndAlso Me.team.id = 0 Then
                    MainForm.has_ascended = True
                    MainForm.help = True
                End If
            Case "!Suicide"
                Me.last_damager_team = Me.team
                Me.stats.repair = 0
                Me.integrity = -2048
            Case "!Free"
                Me.SetTeam(Me.world.Teams(world.gameplay_random.Next(0, Me.world.Teams.Count())))
            Case "!Cheats"
                MainForm.cheats_enabled = Not MainForm.cheats_enabled
            Case Else
                Throw New Exception("Erreur : " & Chain & " (invalid effect)")
        End Select
    End Sub
    Public Ups As New List(Of Upgrade)
    Public Upgrading As Upgrade
    Public UpProgress As Integer
    Public upgrade_slots As Integer = -1

    Public Function HaveUp(ByRef upgrade As Upgrade) As Boolean
        Return Me.Ups.Contains(upgrade)
    End Function


    Public Function IsDestroyed() As Boolean
        Return Me.integrity <= 0
    End Function

    Public Function GetClosestShip(Optional enemies As Double = 2.0, Optional neutrals As Double = 1.0, Optional allies As Double = 0.0) As Ship
        Const max_distance As Double = Double.PositiveInfinity
        ' maximum square distance
        Dim closest_distance_sq As Double = Double.PositiveInfinity
        If max_distance < Math.Sqrt(Double.MaxValue) Then
            closest_distance_sq = max_distance * max_distance
        End If
        ' mods are applied to square distance so they should be sqare too
        enemies *= enemies
        neutrals *= neutrals
        allies *= allies
        ' find the ship
        Dim closest_ship As Ship = Nothing
        For Each other_ship As Ship In Me.world.Ships
            If Not Me Is other_ship Then
                Dim distance_sq As Double
                ' relationship priority
                If other_ship.team Is Nothing OrElse Me.team Is Nothing Then
                    distance_sq = Helpers.DistanceSQ(Me.location, other_ship.location) / neutrals
                Else
                    If Me.team.IsFriendWith(other_ship.team) Then
                        distance_sq = Helpers.DistanceSQ(Me.location, other_ship.location) / allies
                    Else
                        distance_sq = Helpers.DistanceSQ(Me.location, other_ship.location) / enemies
                    End If
                End If
                ' test if closer
                If distance_sq < closest_distance_sq Then
                    closest_ship = other_ship
                    closest_distance_sq = distance_sq
                End If
            End If
        Next
        Return closest_ship
    End Function

    ' calculat point to aim to reach a moving target
    Public Function ForseeLocation(target_ship As Ship) As PointF
        Dim dist As Double = Helpers.Distance(Me.location, target_ship.location)
        Dim time As Double = dist / Math.Max(1.0, Me.stats.speed)
        Return New PointF(target_ship.location.X + target_ship.speed_vec.X * time, target_ship.location.Y + target_ship.speed_vec.Y * time)
    End Function

End Class
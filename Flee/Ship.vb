﻿Public Class Ship : Public selected As Boolean

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
    Public TargetPTN As New Point(0, 0)

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
    Public uid As ULong = ULong.MaxValue - 1
    Public team As Team = Nothing
    Public color As Color = Color.White
    Public fram As UShort = 0

    'state
    Public integrity As Integer = 20
    Public position As New PointF(5000, 5000)
    Public speed_vec As New PointF()
    Public direction As Single = 0
    Public speed As Single = 0
    Public cold_deflector_charge As Integer = -1
    Public deflectors_loaded As Integer = 0
    Public deflector_loading As Integer = 0
    Public shield As Single = 0
    Public weapons As New List(Of Weapon)
    Public last_damager_team As Team = Nothing

    ' creation
    Public Sub New(ByRef world As World)
        Me.world = world
        ResetShieldPoint()
    End Sub
    Public Sub New(ByRef world As World, ship_class As String)
        Me.world = world
        Me.uid = Helpers.GetNextUniqueID()
        SetStats(ship_class)
        ResetShieldPoint()
    End Sub
    Public Sub New(ByRef world As World, team As Team, ship_class As String)
        Me.world = world
        Me.uid = Helpers.GetNextUniqueID()
        Me.SetTeam(team)
        SetStats(ship_class)
        ResetShieldPoint()
    End Sub
    Public Sub SetStats(ship_class As String)
        SetStats(ShipStats.classes(ship_class))
    End Sub
    Public Sub SetStats(stats As ShipStats)
        If Not Me.base_stats Is stats Then
            Me.base_stats = stats
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
                Case "Asteroide"
                    behavior = BehaviorMode.Drift : target = Me : color = Color.FromArgb(64, 64, 48)
                Case "Meteoroide"
                    behavior = BehaviorMode.Drift : target = Me : color = Color.FromArgb(80, 48, 80)
                Case "Comet"
                    behavior = BehaviorMode.Drift : target = Me : color = Color.FromArgb(0, 100, 0)
                Case "Star"
                    behavior = BehaviorMode.Drift : target = Me : color = Color.FromArgb(255, 255, 220)
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
            If Me.base_stats.cold_deflector Then Me.cold_deflector_charge = 0 Else Me.cold_deflector_charge = -1
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
            uid = Helpers.GetNextUniqueID()
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
            If Me.position.X < 0 Then
                Me.position.X = 0
            End If
            If Me.position.Y < 0 Then
                Me.position.Y = 0
            End If
            If Me.position.X > world.ArenaSize.Width Then
                Me.position.X = world.ArenaSize.Width
            End If
            If Me.position.Y > world.ArenaSize.Height Then
                Me.position.Y = world.ArenaSize.Height
            End If
        Else
            If Me.position.X < -100 OrElse Me.position.Y < -100 OrElse Me.position.X > world.ArenaSize.Width + 100 OrElse Me.position.Y > world.ArenaSize.Height + 100 Then
                If Me.bot_ship = False Then Me.integrity = 0
            End If
        End If
        '===' Déplacement '==='
        If stats.turn = 0 AndAlso stats.speed = 0 Then
            direction = direction + 1
        Else
            speed_vec = Helpers.GetNewPoint(New Point(0, 0), direction, speed)
            Me.position.X = Me.position.X + speed_vec.X
            Me.position.Y = Me.position.Y + speed_vec.Y
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
            Me.integrity -= 1
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
            If UpProgress < Upgrading.Time Then
                UpProgress = UpProgress + 1
                If MainForm.DebugMode Then
                    UpProgress = UpProgress + 99
                End If
            Else
                If Upgrading.Name.Contains("Pointvortex") Then
                    Console.WriteLine()
                End If
                If Upgrading.Install Then
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
        If GetAngDif(direction, qa) < stats.turn Then
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
    Public Function GetAngDif(ByVal ang1 As Single, ByVal ang2 As Single) As Single
        While ang1 >= 360
            ang1 = ang1 - 360
        End While
        While ang1 < 0
            ang1 = ang1 + 360
        End While
        While ang2 >= 360
            ang2 = ang2 - 360
        End While
        While ang2 < 0
            ang2 = ang2 + 360
        End While
        Return Math.Abs(ang1 - ang2)
    End Function
    Sub TakeDamages(ByVal Amount As Integer, Optional ByRef From As Shoot = Nothing)
        If Me.deflectors_loaded > 0 Then
            Me.deflectors_loaded -= 1
            Return
        End If
        If Me.shield > 0 Then
            Me.shield = Me.shield - Amount
            Amount = Amount - (Amount * stats.shield_opacity / 100)
            If Not From Is Nothing Then
                Dim angle_ship_shoot_rel As Double = Helpers.NormalizeAngleUnsigned(Helpers.GetAngle(position.X, position.Y, From.Coo.X, From.Coo.Y) - direction)
                Dim shield_ptn_index As Integer = (angle_ship_shoot_rel * 16 / 360)
                ShieldPoints(shield_ptn_index Mod 16) = 255
                If ShieldPoints((shield_ptn_index + 1) Mod 16) < 128 Then ShieldPoints((shield_ptn_index + 1) Mod 16) = 128
                If ShieldPoints((shield_ptn_index + 15) Mod 16) < 128 Then ShieldPoints((shield_ptn_index + 15) Mod 16) = 128
            End If
        End If
        If Me.cold_deflector_charge >= 0 AndAlso Me.cold_deflector_charge < Me.integrity * 2 Then
            Me.cold_deflector_charge += Amount / 2
            Amount /= 6
        End If
        If Not From Is Nothing AndAlso Not From.Team Is Nothing Then
            If Me.stats.sprite = "Star" Then
                From.Team.resources.Antimatter += Amount / 8
            Else
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
        'Remove destroyed targets
        If Not Me.target Is Nothing AndAlso Me.target.IsDestroyed() Then
            Me.target = Nothing
        End If
        '===' Fin de poursuite '==='
        If Me.behavior <> BehaviorMode.Drift AndAlso target Is Nothing Then
            target = Nothing
            If Me.behavior <> BehaviorMode.Mine Then
                Me.behavior = BehaviorMode.Stand
            End If
        End If
        '===' Derelict are alway drifting '==='
        If Me.team Is Nothing Then
            Me.behavior = BehaviorMode.Drift
        End If
        '===' Auto-Activation '==='
        If Me.bot_ship AndAlso Me.behavior <> BehaviorMode.Drift Then
            Dim NearVal As Integer = Integer.MaxValue : Dim NearUID As Ship = Nothing
            If Me.target Is Nothing Then
                For Each oShip As Ship In world.Ships
                    If (Not Me.team.IsFriendWith(oShip.team)) AndAlso (rnd_num < 6700 OrElse Not oShip.team Is Nothing) Then
                        Dim dist As Integer = Helpers.GetDistance(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y)
                        If dist < NearVal Then
                            NearVal = dist
                            NearUID = oShip
                        End If
                    End If
                Next
                If Not NearUID Is Nothing Then
                    Me.target = NearUID
                    Me.behavior = BehaviorMode.Folow
                End If
            Else
                If rnd_num < 6 Then 'chance to change target
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
                    Dim oShip As Ship = Me.target
                    QA = Helpers.GetQA(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y)
                    Dim d As Integer = 50 : If weapons.Count > 0 Then d = Me.weapons(0).stats.range / 2
                    If Helpers.GetDistance(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y) <= d Then
                        QA = QA + 180
                    End If
                    NeedSpeed = True
                    If Helpers.GetDistance(Me.TargetPTN.X, Me.TargetPTN.Y, oShip.position.X, oShip.position.Y) > world.ArenaSize.Width / 8 Then
                        Me.target = Nothing
                    End If
                Else
                    Dim NearVal As Integer = world.ArenaSize.Width / 8 : Dim NearUID As Ship = Nothing
                    For Each oShip As Ship In world.Ships
                        If oShip.team Is Nothing Then
                            Dim dist_me As Integer = Helpers.GetDistance(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y)
                            Dim dist_target As Integer = Helpers.GetDistance(Me.TargetPTN.X, Me.TargetPTN.Y, oShip.position.X, oShip.position.Y)
                            If dist_target < world.ArenaSize.Width / 8 AndAlso dist_me < NearVal Then
                                NearVal = dist_me
                                NearUID = oShip
                            End If
                        End If
                    Next
                    Me.target = NearUID
                    QA = Helpers.GetQA(Me.position.X, Me.position.Y, Me.TargetPTN.X, Me.TargetPTN.Y)
                    NeedSpeed = True
                End If
            Case BehaviorMode.Stand
                QA = direction
                NeedSpeed = False
            Case BehaviorMode.Drift
                QA = direction
                NeedSpeed = True
            Case BehaviorMode.Folow
                If Not Me.target Is Nothing Then
                    NeedSpeed = True
                    Dim oShip As Ship = Me.target
                    QA = Helpers.GetQA(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y)
                    Dim d As Integer = 50 : If weapons.Count > 0 Then d = Me.weapons(0).stats.range / 2
                    If Helpers.GetDistance(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y) <= d Then
                        If Me.stats.speed > 4 OrElse Me.stats.width < 35 OrElse Me.speed < 1.0 Then
                            NeedSpeed = True
                        Else
                            NeedSpeed = False
                        End If
                        QA = QA + 180
                    End If
                End If
            Case BehaviorMode.GoToPoint
                QA = Helpers.GetQA(Me.position.X, Me.position.Y, Me.TargetPTN.X, Me.TargetPTN.Y)
                If Helpers.GetDistance(Me.position.X, Me.position.Y, Me.TargetPTN.X, Me.TargetPTN.Y) <= 50 Then
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
                    Dim record As Integer = 100000 : Dim recorded As Ship = Nothing 'Pas de cible
                    Dim ToX As Integer = (Math.Sin(2 * Math.PI * (AWeap.Loc + direction) / 360) * (stats.width / 2)) + position.X
                    Dim ToY As Integer = (Math.Cos(2 * Math.PI * (AWeap.Loc + direction) / 360) * (stats.width / 2)) + position.Y
                    For Each OVessel As Ship In world.Ships
                        If OVessel Is Me Then
                            Continue For
                        End If
                        If Not Me.AllowMining AndAlso (OVessel.stats.sprite = "Asteroide" OrElse OVessel.stats.sprite = "Meteoroide") Then
                            Continue For
                        End If
                        If Me.team Is Nothing OrElse Not Me.team.IsFriendWith(OVessel.team) Then
                            Dim dist As Integer = Helpers.GetDistance(ToX, ToY, OVessel.position.X, OVessel.position.Y) - OVessel.stats.width / 2
                            If dist < Me.weapons(0).stats.range Then
                                If Me.team Is Nothing OrElse Not OVessel.team Is Nothing AndAlso Not Me.team.IsFriendWith(OVessel.team) Then
                                    dist /= 4
                                End If
                                If Me.target Is OVessel Then
                                    dist /= 2
                                End If
                            End If
                            If dist < record Then
                                record = dist
                                recorded = OVessel
                            End If
                        End If
                    Next
                    If Not recorded Is Nothing Then
                        Dim oShip As Ship = recorded
                        record = Helpers.GetDistance(ToX, ToY, oShip.position.X, oShip.position.Y) - oShip.stats.width / 2
                        If record < AWeap.stats.range Then
                            Dim NewPoint As PointF = Helpers.GetNewPoint(oShip.position, oShip.direction, oShip.speed * (record / AWeap.stats.celerity) * 0.9)
                            Dim QA As Integer = Helpers.GetQA(ToX, ToY, NewPoint.X, NewPoint.Y)
                            AWeap.Fire(QA, New Point(ToX, ToY), Me)
                            If (AWeap.stats.special And Weapon.SpecialBits.SelfExplode) <> 0 OrElse (AWeap.stats.special And Weapon.SpecialBits.SelfExplode) <> 0 Then
                                Me.integrity = -2048
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    ' get all possible upgrades
    Public Function ConditionsMetUpgrades()
        Dim met_upgrades As List(Of Upgrade) = New List(Of Upgrade)
        For Each upgrade As Upgrade In Upgrade.Upgrades
            If IsUpgradeCompatible(upgrade) Then
                met_upgrades.Add(upgrade)
            End If
        Next
        Return met_upgrades
    End Function
    ' get all possible upgrades
    Public Function AvailableUpgrades()
        Dim possible_upgrades As List(Of Upgrade) = New List(Of Upgrade)
        For Each upgrade As Upgrade In Upgrade.Upgrades
            If CanUpgrade(upgrade) Then
                possible_upgrades.Add(upgrade)
            End If
        Next
        Return possible_upgrades
    End Function
    ' return true if the upgrade is available
    Public Function CanUpgrade(upgrade As Upgrade) As Boolean
        If MainForm.DebugMode Then Return True
        If Not Me.Upgrading Is Nothing Then Return False
        If upgrade.Install AndAlso Me.Ups.Count >= upgrade_slots Then Return False
        If upgrade.not_for_bots AndAlso Me.bot_ship Then Return False
        If Me.HaveUp(upgrade) Then Return False
        Return IsUpgradeCompatible(upgrade)
    End Function
    ' return true if the upgrade have its conditions met
    Public Function IsUpgradeCompatible(upgrade As Upgrade) As Boolean
        If MainForm.DebugMode Then Return True
        If upgrade Is Me.Upgrading Then Return True
        Dim conditions_strings() As String = upgrade.Need.Split(" ")
        For Each a_condition As String In conditions_strings
            If Not IsUpgradeConditionMet(a_condition) Then
                Return False
            End If
        Next
        Return True
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
                If Me.uid = MainForm.MAIN_BASE Then Return True
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

    ' apply all upgrades effects this ship have
    Public Sub ApplyUpgrades()
        For Each AUp As Upgrade In Me.Ups
            Dim spliter() As String = AUp.Effect.Split(" ")
            For Each Aspli As String In spliter
                ApplyUpgradeEffect(Aspli, False)
            Next
        Next
    End Sub
    ' apply a single upgrade effects to this ship
    Public Sub ApplyUpgradeFirstTime(ByRef upgrade As Upgrade)
        Dim spliter() As String = upgrade.Effect.Split(" ")
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
                Dim tp_dst As PointF = Me.TargetPTN
                If Not Me.target Is Nothing Then
                    tp_dst = Me.target.position
                End If
                Me.position = tp_dst + New Point(world.Rand.Next(-512, 512), world.Rand.Next(-512, 512))
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
                Me.stats.cold_deflector = Spliter(1)
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
                Me.integrity += Me.integrity * (Spliter(1) / 100.0)
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
                If first_application Then world.Ships.Add(New Ship(world, Me.team, Spliter(1)) With {.position = New Point(Me.position.X + world.Rand.Next(-10, 11), Me.position.Y + world.Rand.Next(-10, 11))})
                world.Ships(world.Ships.Count - 1).direction = Me.direction
                If world.Ships(world.Ships.Count - 1).weapons.Count() > 0 AndAlso (world.Ships(world.Ships.Count - 1).weapons(0).stats.special And Weapon.SpecialBits.SelfExplode) <> 0 Then
                    world.Ships(world.Ships.Count - 1).behavior = BehaviorMode.Folow
                    world.Ships(world.Ships.Count - 1).target = Me.target
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
                'Me.TakeDamages(7777777)
                Me.SetTeam(Me.world.Teams(world.Rand.Next(0, Me.world.Teams.Count())))
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

    ' Import/Export




End Class
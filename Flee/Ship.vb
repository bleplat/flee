Public Class Ship : Public selected As Boolean
    Public world As World

    'IA
    Public Auto As Boolean = True
    Public Behavior As String = "Stand"
    Public Target As String = "" : Public TargetPTN As New Point(0, 0)
    Public Vision As Integer = 50000
    Public AllowMining As Boolean = True

    ' shield effect
    Public Const SHIELD_POINTS As Integer = 16
    Public ShieldPoints(SHIELD_POINTS - 1) As Integer
    Public Sub ResetShieldPoint()
        For shield_ptn_value = 0 To SHIELD_POINTS - 1
            ShieldPoints(shield_ptn_value) = 128
        Next
    End Sub

    'main
    Private base_stats As ShipStats = Nothing
    Public stats As ShipStats = Nothing
    Public uid As String = -1
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
    Public Sub SetStats(ship_class As String)
        SetStats(ShipStats.classes(ship_class))
    End Sub
    Public Sub SetStats(stats As ShipStats)
        If Not Me.base_stats Is stats Then
            Me.base_stats = stats
            ' Reset Weapons
            Me.weapons.Clear()
            For Each gun_name As String In Me.base_stats.default_weapons
                Me.weapons.Add(New Weapon(Me, gun_name))
            Next
            ResetStats()
        End If
    End Sub
    Public Sub SetTeam(team As Team)
        If Not Me.team Is team Then
            Me.team = team
            Me.color = team.color
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
        If ShipStats.classes.ContainsKey(SType) Then
            If Me.base_stats Is Nothing Then
                Me.SetStats(SType)
            End If
            Me.SetTeam(STeam)
        Else
            weapons.Clear()
            If stats Is Nothing Then
                stats = New ShipStats(SType)
            End If
            Me.cold_deflector_charge = -1
            Me.stats.deflectors = 0
            Me.stats.hot_deflector = 0
            Me.stats.sprite = SType
            Select Case SType
                'Start ships
                Case "Station"
                    stats.width = 100 : stats.turn = 0 : stats.speed = 0 : stats.integrity = 4000 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 9 : If IsNew Then UpsMax = 99
                Case "Colonizer"
                    stats.width = 55 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 185 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 9 : If IsNew Then UpsMax = 8
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Basic-Gun")))
                    weapons.Add(New Weapon(Me, -135, GunStats.classes("Light-Machine-Gun")))
                    weapons.Add(New Weapon(Me, 135, GunStats.classes("Light-Machine-Gun")))
                Case "MiniColonizer"
                    stats.width = 55 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 150 : stats.shield = 200 : stats.shield_opacity = 75 : stats.shield_regeneration = 10 : stats.level = 0 : If IsNew Then UpsMax = 4
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Standard-Artillery")))
                    weapons.Add(New Weapon(Me, -135, GunStats.classes("Light-Machine-Gun")))
                    weapons.Add(New Weapon(Me, 135, GunStats.classes("Light-Machine-Gun")))
                Case "Ambassador"
                    stats.width = 50 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 175 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 9 : If IsNew Then UpsMax = 32
                    weapons.Add(New Weapon(Me, 45, GunStats.classes("Basic-Gun")))
                    weapons.Add(New Weapon(Me, -45, GunStats.classes("Basic-Gun")))
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Light-Machine-Gun")))
                Case "Pusher"
                    stats.width = 25 : stats.turn = 3.8 : stats.speed = 4.5 : stats.integrity = 95 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 1 : If IsNew Then UpsMax = 4
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Pusher")))
                'Legendary
                Case "Legend_I"
                    stats.width = 45 : stats.turn = 1.8 : stats.speed = 3.2 : stats.integrity = 170 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 9 : If IsNew Then UpsMax = 16
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Standard-Artillery")))
                    weapons.Add(New Weapon(Me, -135, GunStats.classes("Light-Bombs")))
                    weapons.Add(New Weapon(Me, 135, GunStats.classes("Light-Bombs")))
                Case "Legend_K"
                    stats.width = 45 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 220 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 9 : If IsNew Then UpsMax = 16
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Shuriken-Bombs")))
                    weapons.Add(New Weapon(Me, -135, GunStats.classes("Shuriken-Bombs")))
                    weapons.Add(New Weapon(Me, 135, GunStats.classes("Shuriken-Bombs")))
                Case "Legend_L"
                    stats.width = 45 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 240 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 9 : If IsNew Then UpsMax = 16
                    weapons.Add(New Weapon(Me, 135, GunStats.classes("Beam-4")))
                Case "Legend_U"
                    stats.width = 45 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 200 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 9 : If IsNew Then UpsMax = 16
                    weapons.Add(New Weapon(Me, 90, GunStats.classes("Pusher")))
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Pusher")))
                Case "Legend_Y"
                    stats.width = 50 : stats.turn = 1.8 : stats.speed = 3.2 : stats.integrity = 180 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 5 : stats.level = 9 : If IsNew Then UpsMax = 16
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Machine-Gun")))
                    weapons.Add(New Weapon(Me, -135, GunStats.classes("Machine-Gun")))
                    weapons.Add(New Weapon(Me, 135, GunStats.classes("Machine-Gun")))
                Case "Civil_A"
                    stats.width = 60 : stats.turn = 1.4 : stats.speed = 2.2 : stats.integrity = 320 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 5 : stats.deflectors = 6 : stats.deflectors_cooldown = 32 : stats.level = 1 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Lazers")))
                    weapons.Add(New Weapon(Me, 90, GunStats.classes("Lazers")))
                'Craftable/Upgradable
                Case "Cargo"
                    stats.width = 40 : stats.turn = 2.0 : stats.speed = 3.2 : stats.integrity = 70 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 0 : If IsNew Then UpsMax = 4
                    weapons.Add(New Weapon(Me, 90, GunStats.classes("Mining-Gun")))
                Case "Simpleship"
                    stats.width = 40 : stats.turn = 2.0 : stats.speed = 2.8 : stats.integrity = 90 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 1 : If IsNew Then UpsMax = 10
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Basic-Gun")))
                Case "Scout"
                    stats.width = 35 : stats.turn = 2.5 : stats.speed = 3.5 : stats.integrity = 105 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 2
                    weapons.Add(New Weapon(Me, -45, GunStats.classes("Machine-Gun")))
                    weapons.Add(New Weapon(Me, 45, GunStats.classes("Machine-Gun")))
                Case "Bomber"
                    stats.width = 40 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 210 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 2
                    weapons.Add(New Weapon(Me, 45, GunStats.classes("Heavy-Bombs")))
                Case "PartialBomber"
                    stats.width = 40 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 190 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 1 : If IsNew Then UpsMax = 8
                    weapons.Add(New Weapon(Me, 45, GunStats.classes("Heavy-Bombs")))
                Case "Artillery"
                    stats.width = 35 : stats.turn = 1.8 : stats.speed = 2.8 : stats.integrity = 140 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 2
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Standard-Artillery")))
                Case "Dronner"
                    stats.width = 40 : stats.turn = 1.5 : stats.speed = 3.1 : stats.integrity = 135 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 2
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Basic-Gun")))
                'Small ships
                Case "Hunter"
                    stats.width = 25 : stats.turn = 2.8 : stats.speed = 3.5 : stats.integrity = 60 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 1 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Machine-Gun")))
                Case "Harass"
                    stats.width = 25 : stats.turn = 2.3 : stats.speed = 3.3 : stats.integrity = 75 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 1 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Bombs")))
                'Drones
                Case "Drone"
                    stats.width = 25 : stats.turn = 4.5 : stats.speed = 3.5 : stats.integrity = 75 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 5 : stats.level = 1 : If IsNew Then UpsMax = 2
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Basic-Gun")))
                Case "Explorer"
                    stats.width = 15 : stats.turn = 8.5 : stats.speed = 2.8 : stats.integrity = 10 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 1 : stats.level = 0 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Mining-Gun")))
                Case "Combat_Drone_1"
                    stats.width = 14 : stats.turn = 3.5 : stats.speed = 3.5 : stats.integrity = 35 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 1 : stats.level = 0 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Light-Machine-Gun")))
                    weapons.Add(New Weapon(Me, -180, GunStats.classes("Light-Machine-Gun")))
                Case "Combat_Drone_2"
                    stats.width = 14 : stats.turn = 5.5 : stats.speed = 3.5 : stats.integrity = 45 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 1 : stats.level = 0 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Machine-Gun")))
                Case "Combat_Drone_3"
                    stats.width = 14 : stats.turn = 1.5 : stats.speed = 3.5 : stats.integrity = 50 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 1 : stats.level = 0 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Basic-Gun")))
                'NPC
                Case "Sacred"
                    stats.width = 30 : stats.turn = 2.5 : stats.speed = 3.5 : stats.integrity = 85 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 9 : If IsNew Then UpsMax = 2
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Bombs")))
                Case "Strange"
                    stats.width = 44 : stats.turn = 1.2 : stats.speed = 2.8 : stats.integrity = 185 : stats.shield = 25 : stats.shield_opacity = 25 : stats.shield_regeneration = 25 : stats.level = 2 : If IsNew Then UpsMax = 2
                    weapons.Add(New Weapon(Me, 135, GunStats.classes("Beam-6")))
                Case "Yerka"
                    stats.width = 25 : stats.turn = 4.2 : stats.speed = 4 : stats.integrity = 75 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 1 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 270, GunStats.classes("Machine-Gun")))
                Case "Kastou"
                    stats.width = 45 : stats.turn = 2.2 : stats.speed = 3 : stats.integrity = 225 : stats.shield = 100 : stats.shield_opacity = 100 : stats.shield_regeneration = 20 : stats.level = 4 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Heavy-Bombs")))
                Case "Crusher"
                    stats.width = 75 : stats.turn = 0.4 : stats.speed = 2.2 : stats.integrity = 665 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 5 : stats.level = 5 : If IsNew Then UpsMax = 2
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Intercepting-Artillery")))
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Longrange-Bombs")))
                    weapons.Add(New Weapon(Me, +90, GunStats.classes("Longrange-Bombs")))
                Case "Loneboss"
                    stats.width = 40 : stats.turn = 1.5 : stats.speed = 3.2 : stats.integrity = 520 : stats.repair = 4 : stats.shield = 0 : stats.shield_opacity = 25 : stats.shield_regeneration = 5 : stats.deflectors = 4 : stats.hot_deflector = 55 : stats.level = 4 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, -135, GunStats.classes("Machine-Gun")))
                    weapons.Add(New Weapon(Me, 135, GunStats.classes("Machine-Gun")))
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Rail-Gun")))
                Case "Bugs"
                    stats.width = 200 : stats.turn = 2.5 : stats.speed = 3.2 : stats.integrity = 3400 : stats.repair = 2 : stats.shield = 0 : stats.shield_opacity = 25 : stats.shield_regeneration = 5 : cold_deflector_charge = 1 : stats.hot_deflector = 8 : stats.level = 4 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, -45, GunStats.classes("Heavy-Bio-Gun")))
                    weapons.Add(New Weapon(Me, +45, GunStats.classes("Heavy-Bio-Gun")))
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Bio-Gun")))
                Case "Finalizer"
                    stats.width = 60 : stats.turn = 16 : stats.speed = 14 : stats.integrity = 740 : stats.shield = 1800 : stats.shield_opacity = 100 : stats.shield_regeneration = 350 : stats.level = 99 : If IsNew Then UpsMax = 42
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Finalizer")))
                    weapons.Add(New Weapon(Me, 90, GunStats.classes("Finalizer")))
                Case "Ori"
                    stats.width = 100 : stats.turn = 1.6 : stats.speed = 3.8 : stats.integrity = 880 : stats.shield = 2200 : stats.shield_opacity = 100 : stats.shield_regeneration = 380 : stats.level = 99 : If IsNew Then UpsMax = 2
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Basic-Gun")))
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Ori")))
                    'Converter
                Case "Converter"
                    stats.width = 55 : stats.turn = 0.8 : stats.speed = 1.2 : stats.integrity = 360 : stats.repair = 3 : stats.shield = 1200 : stats.shield_opacity = 100 : stats.shield_regeneration = 30 : stats.level = 4 : If IsNew Then UpsMax = 2
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Lazers")))
                    weapons.Add(New Weapon(Me, -45, GunStats.classes("Lazers")))
                    weapons.Add(New Weapon(Me, +45, GunStats.classes("Lazers")))
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Lazers")))
                    weapons.Add(New Weapon(Me, 90, GunStats.classes("Lazers")))
                    weapons.Add(New Weapon(Me, -135, GunStats.classes("Lazers")))
                    weapons.Add(New Weapon(Me, +135, GunStats.classes("Lazers")))
                Case "Converter_A"
                    stats.width = 45 : stats.turn = 6.5 : stats.speed = 3.5 : stats.integrity = 125 : stats.repair = 0 : stats.shield = 350 : stats.shield_opacity = 65 : stats.shield_regeneration = 0 : stats.level = 4 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, -45, GunStats.classes("Lazers")))
                    weapons.Add(New Weapon(Me, +45, GunStats.classes("Lazers")))
                Case "Converter_B"
                    stats.width = 45 : stats.turn = 1.5 : stats.speed = 2.8 : stats.integrity = 145 : stats.repair = 0 : stats.shield = 350 : stats.shield_opacity = 65 : stats.shield_regeneration = 0 : stats.level = 4 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Heavy-Bombs")))
                    'Purger
                Case "Purger_Dronner"
                    stats.width = 50 : stats.turn = 1.2 : stats.speed = 2.8 : stats.integrity = 355 : stats.shield = 300 : stats.shield_opacity = 75 : stats.shield_regeneration = 20 : stats.level = 3 : If IsNew Then UpsMax = 16
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Heavy-Bombs")))
                    weapons.Add(New Weapon(Me, 90, GunStats.classes("Heavy-Bombs")))
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Lazers")))
                Case "Purger_Drone_1"
                    stats.width = 18 : stats.turn = 4.6 : stats.speed = 3.8 : stats.integrity = 8 : stats.repair = 0 : stats.shield = 8 : stats.shield_opacity = 75 : stats.shield_regeneration = 2 : stats.level = 0 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Basic-Gun")))
                Case "Purger_Drone_2"
                    stats.width = 18 : stats.turn = 7.5 : stats.speed = 3.8 : stats.integrity = 8 : stats.repair = 0 : stats.shield = 8 : stats.shield_opacity = 85 : stats.shield_regeneration = 2 : stats.level = 0 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Basic-Gun")))
                Case "Purger_Drone_3"
                    stats.width = 18 : stats.turn = 2.8 : stats.speed = 3.8 : stats.integrity = 8 : stats.repair = 0 : stats.shield = 8 : stats.shield_opacity = 100 : stats.shield_regeneration = 2 : stats.level = 0 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Light-Basic-Gun")))
                    'Turrets
                Case "Outpost"
                    stats.width = 40 : stats.turn = 0.0 : stats.speed = 0.0 : stats.integrity = 190 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 0 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Intercepting-Gun")))
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Intercepting-Gun")))
                    weapons.Add(New Weapon(Me, +90, GunStats.classes("Intercepting-Gun")))
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Intercepting-Gun")))
                Case "BomberFactory"
                    stats.width = 70 : stats.turn = 0.0 : stats.speed = 0.0 : stats.integrity = 450 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 0 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Intercepting-Gun")))
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Intercepting-Gun")))
                    weapons.Add(New Weapon(Me, +90, GunStats.classes("Intercepting-Gun")))
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Intercepting-Gun")))
                Case "Defense"
                    stats.width = 25 : stats.turn = 0.0 : stats.speed = 0.0 : stats.integrity = 170 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 0 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Shuriken-Bombs")))
                    weapons.Add(New Weapon(Me, -120, GunStats.classes("Shuriken-Bombs")))
                    weapons.Add(New Weapon(Me, +120, GunStats.classes("Shuriken-Bombs")))
                Case "Pointvortex"
                    stats.width = 25 : stats.turn = 0.0 : stats.speed = 0.0 : stats.integrity = 130 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 10 : stats.level = 0 : If IsNew Then UpsMax = 1
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Intercepting-Artillery")))
                'Wild
                Case "Asteroide"
                    stats.width = 50 : stats.turn = 1 : stats.speed = 0.9 : stats.integrity = world.Rand.Next(1000, 10000) : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 0 : stats.level = 1
                    Behavior = "Drift" : Target = uid : color = Color.FromArgb(64, 64, 48)
                Case "Meteoroide"
                    stats.width = 30 : stats.turn = 1 : stats.speed = 2.0 + (world.Rand.Next(0, 100) / 100.0) : stats.integrity = 4 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 0 : stats.level = 1
                    Behavior = "Drift" : Target = uid : color = Color.FromArgb(80, 48, 80)
                Case "Comet"
                    stats.width = 25 : stats.turn = 3.0 : stats.speed = 12.0 : stats.integrity = 2 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 5 : stats.level = 1
                    Behavior = "Drift" : Target = uid : color = Color.FromArgb(0, 100, 0)
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Explode-On-Contact")))
                Case "Star"
                    stats.width = 95 : stats.turn = 1.0 : stats.speed = 0.0 : stats.integrity = 1073741823 : stats.level = 1
                    Behavior = "Drift" : Target = uid : color = Color.FromArgb(255, 255, 220)
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Star-Waves")))
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Star-Waves")))
                    weapons.Add(New Weapon(Me, +90, GunStats.classes("Star-Waves")))
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Star-Waves")))
                'Other
                Case "MSL"
                    stats.width = 15 : stats.turn = 3.5 : stats.speed = 8 : stats.integrity = 8 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 0 : stats.level = 0 : stats.repair = 0 : If IsNew Then UpsMax = 0
                    weapons.Add(New Weapon(Me, -90, GunStats.classes("Explode-On-Contact")))
                'Other
                Case "Nuke"
                    stats.width = 40 : stats.turn = 2.5 : stats.speed = 3 : stats.integrity = 72 : stats.shield = 0 : stats.shield_opacity = 0 : stats.shield_regeneration = 0 : stats.level = 0 : stats.repair = 0 : If IsNew Then UpsMax = 0
                    AllowMining = False
                    weapons.Add(New Weapon(Me, 0, GunStats.classes("Explode-On-Contact")))
                    weapons.Add(New Weapon(Me, 180, GunStats.classes("Mining-Gun")))
            End Select
            team = STeam

            ' TODO: REMOVE:
            Me.TempForceUpdateStats()
            'fin 
        End If
        ' Force colors
        Select Case SType
            Case "Asteroide"
                Behavior = "Drift" : Target = uid : color = Color.FromArgb(64, 64, 48)
            Case "Meteoroide"
                Behavior = "Drift" : Target = uid : color = Color.FromArgb(80, 48, 80)
            Case "Comet"
                Behavior = "Drift" : Target = uid : color = Color.FromArgb(0, 100, 0)
            Case "Star"
                Behavior = "Drift" : Target = uid : color = Color.FromArgb(255, 255, 220)
        End Select
        ' Ship just created TODO: NOW: move to constructor
        If IsNew Then
            Me.Upgrading = Nothing
            uid = Helpers.GetNextUniqueID()
            ' state
            integrity = stats.integrity
            shield = stats.shield
            If STeam Is Nothing OrElse STeam.id = 0 Then
                Auto = False
            Else
                Auto = True
            End If
            If Not STeam Is Nothing AndAlso Behavior <> "Drift" Then
                color = STeam.color
            End If
            If Not team Is Nothing AndAlso UpsMax > 0 Then
                UpsMax += team.UpsBonus
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
                If Me.Auto = False Then Me.integrity = 0
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
                If Upgrading.Install Then
                    Ups.Add(Upgrading)
                End If
                'Appliquation debugage
                Dim spliter() As String = Upgrading.Effect.Split(" ")
                For Each aspli As String In spliter
                    Me.InterUpgrade(aspli, True)
                Next
                'actualisation vaisseau
                Me.SetType(Me.stats.sprite, Me.team)
                Me.ResetStats()
                Me.ApplyUps("")
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
                From.Team.resources.Antimatter += Amount / 16
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
        '===' Fin de poursuite '==='
        If Me.Behavior <> "Drift" AndAlso world.GetShipByUID(Target) Is Nothing Then
            Target = ""
            If Me.Behavior <> "Mine" Then
                Me.Behavior = "Stand"
            End If
        End If
        '===' Derelict are alway drifting '==='
        If Me.team Is Nothing Then
            Me.Behavior = "Drift"
        End If
        '===' Auto-Activation '==='
        If Me.Auto AndAlso Me.Behavior <> "Drift" Then
            Dim NearVal As Integer = Vision : Dim NearUID As String = ""
            If Me.Target = "" Then
                For Each oShip As Ship In world.Ships
                    If (Not Me.team.IsFriendWith(oShip.team)) AndAlso (rnd_num < 6700 OrElse Not oShip.team Is Nothing) Then
                        Dim dist As Integer = Helpers.GetDistance(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y)
                        If dist < NearVal Then
                            NearVal = dist
                            NearUID = oShip.uid
                        End If
                    End If
                Next
                If NearUID <> "" Then
                    Me.Target = NearUID
                    Me.Behavior = "Fight"
                End If
            Else
                If rnd_num < 6 Then
                    Me.Target = ""
                End If
            End If
        End If
        '===' Execution '==='
        Select Case Behavior
            Case "Mine"
                Me.AllowMining = True
                If Me.Target <> "" Then
                    Dim oShip As Ship = world.GetShipByUID(Me.Target)
                    QA = Helpers.GetQA(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y)
                    Dim d As Integer = 50 : If weapons.Count > 0 Then d = Me.weapons(0).stats.range / 2
                    If Helpers.GetDistance(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y) <= d Then
                        QA = QA + 180
                    End If
                    NeedSpeed = True
                    If Helpers.GetDistance(Me.TargetPTN.X, Me.TargetPTN.Y, oShip.position.X, oShip.position.Y) > world.ArenaSize.Width / 8 Then
                        Me.Target = ""
                    End If
                Else
                    Dim NearVal As Integer = world.ArenaSize.Width / 8 : Dim NearUID As String = ""
                    For Each oShip As Ship In world.Ships
                        If oShip.team Is Nothing Then
                            Dim dist_me As Integer = Helpers.GetDistance(Me.position.X, Me.position.Y, oShip.position.X, oShip.position.Y)
                            Dim dist_target As Integer = Helpers.GetDistance(Me.TargetPTN.X, Me.TargetPTN.Y, oShip.position.X, oShip.position.Y)
                            If dist_target < world.ArenaSize.Width / 8 AndAlso dist_me < NearVal Then
                                NearVal = dist_me
                                NearUID = oShip.uid
                            End If
                        End If
                    Next
                    Me.Target = NearUID
                    QA = Helpers.GetQA(Me.position.X, Me.position.Y, Me.TargetPTN.X, Me.TargetPTN.Y)
                    NeedSpeed = True
                End If
            Case "Stand"
                QA = direction
                NeedSpeed = False
            Case "Drift"
                QA = direction
                NeedSpeed = True
            Case "Fight"
                If Me.Target <> "" Then
                    NeedSpeed = True
                    Dim oShip As Ship = world.GetShipByUID(Me.Target)
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
            Case "Goto"
                QA = Helpers.GetQA(Me.position.X, Me.position.Y, Me.TargetPTN.X, Me.TargetPTN.Y)
                If Helpers.GetDistance(Me.position.X, Me.position.Y, Me.TargetPTN.X, Me.TargetPTN.Y) <= 50 Then
                    Me.Behavior = "Stand"
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
    End Sub 'IAIAIAIAIA
    Public Sub IAFire()
        '===' Tirer '==='
        If weapons.Count > 0 AndAlso (fram Mod 2 = 0) Then
            For Each AWeap As Weapon In weapons
                If AWeap.Bar > 0 Then
                    Dim record As Integer = 100000 : Dim recorded As String = "" 'Pas de cible
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
                            Dim dist As Integer = Helpers.GetDistance(ToX, ToY, OVessel.position.X, OVessel.position.Y)
                            If dist < Me.weapons(0).stats.range Then
                                If Me.team Is Nothing OrElse Not OVessel.team Is Nothing AndAlso Not Me.team.IsFriendWith(OVessel.team) Then
                                    dist /= 4
                                End If
                                If Me.Target = OVessel.uid Then
                                    dist /= 2
                                End If
                            End If
                            If dist < record Then
                                record = dist
                                recorded = OVessel.uid
                            End If
                        End If
                    Next
                    If recorded <> "" Then
                        Dim oShip As Ship = world.GetShipByUID(recorded)
                        record = Helpers.GetDistance(ToX, ToY, oShip.position.X, oShip.position.Y)
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



    Public Sub ApplyUps(ByVal NewUp As String)
        For Each AUp As Upgrade In Me.Ups
            Dim spliter() As String = AUp.Effect.Split(" ")
            For Each Aspli As String In spliter
                If AUp.Name = NewUp Then
                    InterUpgrade(Aspli, True)
                Else
                    InterUpgrade(Aspli, False)
                End If
            Next
        Next
    End Sub
    ' return true is condition succeed
    Public Function InterUpgrade(ByVal Chain As String, ByVal FN As Boolean) As Boolean
        Dim Spliter() As String = Chain.Split(":")
        Select Case Spliter(0)
            Case "!C"
                Me.color = Color.FromName(Spliter(1))
                If Me.stats.sprite = "Station" Then
                    Me.team.color = Me.color
                End If
            Case "?W"
                If weapons.Count > 0 Then Return True
            Case "?S"
                If stats.speed > 0 Then Return True
            Case "?Base"
                If Me.uid = MainForm.MAIN_BASE Then Return True
            Case "!Jump"
                Me.speed = Convert.ToInt32(Spliter(1))



            Case "!Agility"
                Me.stats.turn += Spliter(1)
            Case "!Teleport"
                Dim target_ship As Ship = world.GetShipByUID(Me.Target)
                If Not target_ship Is Nothing Then
                    Me.position = target_ship.position + New Point(world.Rand.Next(-512, 512), world.Rand.Next(-512, 512))
                End If
                Me.position = Me.TargetPTN + New Point(world.Rand.Next(-512, 512), world.Rand.Next(-512, 512))
            Case "!Upsbonus"
                If FN Then Me.team.UpsBonus += Spliter(1) 'FN
            Case "!Maxships"
                If FN Then Me.team.MaxShips += Spliter(1) 'FN
            Case "+Shield"
                If Me.stats.shield >= Spliter(1) Then Return True
            Case "!Shield"
                Me.stats.shield += Spliter(1)
            Case "!Deflector"
                Me.stats.deflectors += Spliter(1)
            Case "!HotDeflector"
                Me.stats.hot_deflector += Spliter(1)
            Case "!ColdDeflector"
                Me.stats.cold_deflector = Spliter(1)
            Case "%Shield"
                Me.stats.shield += (Me.stats.shield * (Spliter(1) / 100))
            Case "!Shieldop"
                Me.stats.shield_opacity += Spliter(1)
            Case "%Shieldreg"
                Me.stats.shield_regeneration += (Me.stats.shield_regeneration * (Spliter(1) / 100))
            Case "!UpsMax"
                Me.UpsMax += Spliter(1)
            Case "!Fix"
                Me.integrity += Me.integrity * (Spliter(1) / 100.0)
            Case "!FixSFull"
                Me.shield = Me.stats.shield
            Case "+Lvl"
                If Me.stats.level >= Spliter(1) Then Return True
            Case "+Speed" 'vitesse
                If Me.stats.speed >= Spliter(1) Then Return True
            Case "-Speed"
                If Me.stats.speed <= Spliter(1) Then Return True
            Case "%Speed"
                Me.stats.speed += (Me.stats.speed * (Spliter(1) / 100))
                Me.stats.turn += (Me.stats.turn * (Spliter(1) / 100))
            Case "+Life" 'Resistance
                If Me.stats.integrity >= Spliter(1) Then Return True
            Case "-Life" '
                If Me.stats.integrity <= Spliter(1) Then Return True
            Case "%Life"
                Me.stats.integrity += (Me.stats.integrity * (Spliter(1) / 100))
                If FN Then Me.stats.integrity += (Me.stats.integrity * (Spliter(1) / 100)) 'FN
            Case "!Regen"
                If FN Then Me.stats.repair += Spliter(1) 'FN
            Case "?Up"
                If Me.HaveUp(Spliter(1)) Then Return True
            Case "?Type" 'Type
                If Me.stats.sprite = Spliter(1) Then Return True
            Case "!Type"
                If FN Then Me.stats.sprite = Spliter(1)
            Case "?Wtype" 'armement
                If Me.weapons(0).stats.sprite = Spliter(1) Then Return True
            Case "%Wloadmax"
                For Each AW As Weapon In weapons
                    AW.stats.loadtime += AW.stats.loadtime * (Spliter(1) / 100)
                Next
            Case "%Wbar"
                For Each AW As Weapon In weapons
                    AW.stats.salvo += AW.stats.salvo * (Spliter(1) / 100)
                Next
            Case "%Wpower"
                For Each AW As Weapon In weapons
                    AW.stats.power += AW.stats.power * (Spliter(1) / 100)
                Next
            Case "%Wrange"
                For Each AW As Weapon In weapons
                    AW.stats.range += AW.stats.range * (Spliter(1) / 100)
                Next
            Case "%Wcelerity"
                For Each AW As Weapon In weapons
                    AW.stats.celerity += AW.stats.celerity * (Spliter(1) / 100)
                Next
            Case "" 'Debug
                Return True
            Case "?MS"
                If Me.team Is Nothing OrElse world.CountTeamShips(team) < Me.team.MaxShips Then Return True
            Case "!Sum"
                If FN Then world.Ships.Add(New Ship(world) With {.position = New Point(Me.position.X + world.Rand.Next(-10, 11), Me.position.Y + world.Rand.Next(-10, 11))}) : world.Ships(world.Ships.Count - 1).direction = Me.direction : world.Ships(world.Ships.Count - 1).SetType(Spliter(1), Me.team, True)
                If world.Ships(world.Ships.Count - 1).stats.sprite <> "MSL" Then
                    world.Ships(world.Ships.Count - 1).Behavior = "Fight"
                    world.Ships(world.Ships.Count - 1).Target = Me.uid
                End If
            Case "!Ascend"
                If Me.team.id = 0 Then
                    MainForm.has_ascended = True
                    MainForm.help = True
                End If
            Case "!Suicide"
                Me.TakeDamages(7777777)
                '---' UPGRADES SHIPS Au dessus '---'
            Case Else
                MsgBox("Erreur : " & Chain & " (Refusé)", MsgBoxStyle.Critical)
                MainForm.play = False
                MainForm.Ticker.Enabled = False
        End Select
        If MainForm.DebugMode Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Ups As New List(Of Upgrade)
    Public Upgrading As Upgrade : Public UpProgress As Integer
    Public UpsMax As Integer = 10

    Public Function HaveUp(ByVal Upstr As String) As Boolean
        For Each AUp As Upgrade In Me.Ups
            If AUp.Name = Upstr Then
                Return True
            End If
        Next
        Return False
    End Function

    ' Import/Export




End Class
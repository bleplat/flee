Public Class Ship : Public selected As Boolean
    Public world As World
    Public Sub New(ByRef world As World)
        Me.world = world
        For shield_ptn_value = 0 To SHIELD_POINTS - 1
            ShieldPoints(shield_ptn_value) = 0
        Next
    End Sub
    '===' Variable '==='
    'Apparence
    Public fram As UShort = 0
    Public Color As Color = Color.White
    'identitée
    Public UID As String = -1
    Public Team As Team = Nothing
    Public Lvl As Integer = 1
    'Jeu
    Public Coo As New PointF(5000, 5000) : Public Vec As New PointF()
    Public Direction As Single = 0 : Public Speed As Single = 0
    Public Life As Integer = 20 : Public LifeReg As Integer = 1
    Public Shield As Single = 0
    Public last_damager_team As Team = Nothing
    'IA
    Public Auto As Boolean = True
    Public Behavior As String = "Stand"
    Public Target As String = "" : Public TargetPTN As New Point(0, 0)
    Public AllowMining As Boolean = True
    ' bouclier
    Public Const SHIELD_POINTS As Integer = 16
    Public ShieldPoints(SHIELD_POINTS - 1) As Integer
    '===' Lié au Type '==='
    Public Type As String = "Station"
    Public W As UShort = 100
    Public LifeMax As Integer = 20
    Public ShieldMax As Integer = 0 : Public ShieldReg As Integer = 10 : Public ShieldOp As Short = 20 'Pourcentages
    Public DeflectorCount As Integer = 0 : Public DeflectorCountMax As Integer = 0 : Public DeflectorCooldownMax As Integer = 128 : Public DeflectorCooldown As Integer = 0 'Pourcentages
    Public HotDeflector As Integer = 0
    Public ColdDeflector As Integer = -1
    Public Agility As Single = 1 : Public SpeedMax As Single = 2
    Public Vision As Integer = 50000
    Public Weapons As New List(Of Weapon)
    '

    Public Sub SetType(ByVal SType As String, ByVal STeam As Team, Optional ByVal IsNew As Boolean = False)
        Weapons.Clear()
        Me.ColdDeflector = -1
        Me.DeflectorCountMax = 0
        Me.HotDeflector = 0
        Type = SType
        Select Case SType
                'Start ships
            Case "Station"
                W = 100 : Agility = 0 : SpeedMax = 0 : LifeMax = 4000 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 9 : If IsNew Then UpsMax = 99
            Case "Colonizer"
                W = 55 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 185 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 9 : If IsNew Then UpsMax = 8
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 250, .Celerity = 10, .Power = 30, .LoadMax = 85, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "A", .Range = 180, .Celerity = 17, .Power = 2, .LoadMax = 4, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "A", .Range = 180, .Celerity = 17, .Power = 2, .LoadMax = 4, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 135, .Type = "C", .Range = 250, .Celerity = 10, .Power = 30, .LoadMax = 75, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -135, .Type = "C", .Range = 250, .Celerity = 10, .Power = 30, .LoadMax = 75, .BarMax = 1})
            Case "Ambassador"
                W = 50 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 175 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 9 : If IsNew Then UpsMax = 32
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "B", .Range = 200, .Celerity = 11, .Power = 10, .LoadMax = 25, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "B", .Range = 200, .Celerity = 11, .Power = 10, .LoadMax = 30, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "B", .Range = 200, .Celerity = 11, .Power = 10, .LoadMax = 30, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "B", .Range = 200, .Celerity = 11, .Power = 10, .LoadMax = 35, .BarMax = 1})
            Case "Pusher"
                W = 25 : Agility = 3.8 : SpeedMax = 4.5 : LifeMax = 95 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 1 : If IsNew Then UpsMax = 4
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "Q", .Range = 160, .Celerity = 10, .Power = 25, .LoadMax = 25, .BarMax = 1})
                'Legendary
            Case "Legend_I"
                W = 45 : Agility = 1.8 : SpeedMax = 3.2 : LifeMax = 170 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 9 : If IsNew Then UpsMax = 16
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "D", .Range = 350, .Celerity = 10, .Power = 50, .LoadMax = 75, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 135, .Type = "C", .Range = 250, .Celerity = 10, .Power = 30, .LoadMax = 75, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -135, .Type = "C", .Range = 250, .Celerity = 10, .Power = 30, .LoadMax = 75, .BarMax = 1})
            Case "Legend_K"
                W = 45 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 220 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 9 : If IsNew Then UpsMax = 16
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "E", .Range = 350, .Celerity = 8, .Power = 15, .LoadMax = 50, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -135, .Type = "E", .Range = 350, .Celerity = 8, .Power = 15, .LoadMax = 50, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 135, .Type = "E", .Range = 350, .Celerity = 8, .Power = 15, .LoadMax = 50, .BarMax = 1})
            Case "Legend_L"
                W = 45 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 240 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 9 : If IsNew Then UpsMax = 16
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "G", .Range = 250, .Celerity = 14, .Power = 16, .LoadMax = 50, .BarMax = 4})
            Case "Legend_U"
                W = 45 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 200 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 9 : If IsNew Then UpsMax = 16
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "Q", .Range = 250, .Celerity = 10, .Power = 15, .LoadMax = 25, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "Q", .Range = 250, .Celerity = 10, .Power = 15, .LoadMax = 25, .BarMax = 1})
            Case "Legend_Y"
                W = 50 : Agility = 1.8 : SpeedMax = 3.2 : LifeMax = 180 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 5 : Lvl = 9 : If IsNew Then UpsMax = 16
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "A", .Range = 250, .Celerity = 17, .Power = 4, .LoadMax = 4, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -135, .Type = "A", .Range = 180, .Celerity = 17, .Power = 3, .LoadMax = 4, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 135, .Type = "A", .Range = 180, .Celerity = 17, .Power = 3, .LoadMax = 4, .BarMax = 1})
            Case "Civil_A"
                W = 60 : Agility = 1.4 : SpeedMax = 2.2 : LifeMax = 320 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 5 : DeflectorCountMax = 6 : DeflectorCooldownMax = 32 : Lvl = 1 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "I", .Range = 200, .Celerity = 17, .Power = 4, .LoadMax = 15, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "I", .Range = 200, .Celerity = 17, .Power = 4, .LoadMax = 15, .BarMax = 1})
                'Craftable/Upgradable
            Case "Cargo"
                W = 40 : Agility = 2.0 : SpeedMax = 3.2 : LifeMax = 70 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 0 : If IsNew Then UpsMax = 4
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "A", .Range = 250, .Celerity = 16, .Power = 5, .LoadMax = 10, .BarMax = 1})
            Case "Simpleship"
                W = 40 : Agility = 2.0 : SpeedMax = 2.8 : LifeMax = 90 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 1 : If IsNew Then UpsMax = 10
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "B", .Range = 220, .Celerity = 10, .Power = 15, .LoadMax = 30, .BarMax = 1})
            Case "Scout"
                W = 35 : Agility = 2.5 : SpeedMax = 3.5 : LifeMax = 105 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 2
                Weapons.Add(New Weapon(Me) With {.Loc = -45, .Type = "A", .Range = 220, .Celerity = 16, .Power = 5, .LoadMax = 15, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 45, .Type = "A", .Range = 220, .Celerity = 16, .Power = 5, .LoadMax = 15, .BarMax = 1})
            Case "Bomber"
                W = 40 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 210 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 250, .Celerity = 9, .Power = 50, .LoadMax = 55, .BarMax = 1})
            Case "PartialBomber"
                W = 40 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 190 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 1 : If IsNew Then UpsMax = 8
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 250, .Celerity = 9, .Power = 50, .LoadMax = 55, .BarMax = 1})
            Case "Artillery"
                W = 35 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 140 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "D", .Range = 370, .Celerity = 12, .Power = 55, .LoadMax = 70, .BarMax = 1})
            Case "Dronner"
                W = 40 : Agility = 1.5 : SpeedMax = 3.1 : LifeMax = 135 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "B", .Range = 220, .Celerity = 10, .Power = 15, .LoadMax = 30, .BarMax = 1})
                'Small ships
            Case "Hunter"
                W = 25 : Agility = 2.8 : SpeedMax = 3.5 : LifeMax = 60 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 1 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 270, .Type = "A", .Range = 115, .Celerity = 16, .Power = 6, .LoadMax = 6, .BarMax = 1})
            Case "Harass"
                W = 25 : Agility = 2.3 : SpeedMax = 3.3 : LifeMax = 75 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 1 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 200, .Celerity = 10, .Power = 50, .LoadMax = 50, .BarMax = 1})
                'Drones
            Case "Drone"
                W = 25 : Agility = 4.5 : SpeedMax = 3.5 : LifeMax = 75 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 5 : Lvl = 1 : If IsNew Then UpsMax = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "B", .Range = 200, .Celerity = 10, .Power = 10, .LoadMax = 30, .BarMax = 1})
            Case "Explorer"
                W = 15 : Agility = 8.5 : SpeedMax = 2.8 : LifeMax = 10 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 1 : Lvl = 0 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 270, .Type = "A", .Range = 90, .Celerity = 14, .Power = 9, .LoadMax = 7, .BarMax = 1})
            Case "Combat_Drone_1"
                W = 14 : Agility = 3.5 : SpeedMax = 3.5 : LifeMax = 35 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 1 : Lvl = 0 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = 270, .Type = "A", .Range = 160, .Celerity = 16, .Power = 1, .LoadMax = 3, .BarMax = 1})
            Case "Combat_Drone_2"
                W = 14 : Agility = 5.5 : SpeedMax = 3.5 : LifeMax = 45 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 1 : Lvl = 0 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = 270, .Type = "A", .Range = 160, .Celerity = 16, .Power = 2, .LoadMax = 8, .BarMax = 1})
            Case "Combat_Drone_3"
                W = 14 : Agility = 1.5 : SpeedMax = 3.5 : LifeMax = 50 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 1 : Lvl = 0 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "B", .Range = 170, .Celerity = 11, .Power = 10, .LoadMax = 25, .BarMax = 1})
                'NPC
            Case "Sacred"
                W = 30 : Agility = 2.5 : SpeedMax = 3.5 : LifeMax = 85 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 9 : If IsNew Then UpsMax = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 320, .Celerity = 8, .Power = 20, .LoadMax = 20, .BarMax = 1})
            Case "Strange"
                W = 44 : Agility = 1.2 : SpeedMax = 2.8 : LifeMax = 185 : ShieldMax = 25 : ShieldOp = 25 : ShieldReg = 25 : Lvl = 2 : If IsNew Then UpsMax = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "G", .Range = 250, .Celerity = 14, .Power = 16, .LoadMax = 50, .BarMax = 6})
            Case "MiniColonizer"
                W = 55 : Agility = 1.8 : SpeedMax = 2.8 : LifeMax = 150 : ShieldMax = 200 : ShieldOp = 75 : ShieldReg = 10 : Lvl = 0 : If IsNew Then UpsMax = 4
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 320, .Celerity = 16, .Power = 8, .LoadMax = 65, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "A", .Range = 200, .Celerity = 17, .Power = 2, .LoadMax = 8, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "A", .Range = 200, .Celerity = 17, .Power = 2, .LoadMax = 8, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 135, .Type = "A", .Range = 200, .Celerity = 17, .Power = 2, .LoadMax = 8, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -135, .Type = "A", .Range = 200, .Celerity = 17, .Power = 2, .LoadMax = 8, .BarMax = 1})
            Case "Yerka"
                W = 25 : Agility = 4.2 : SpeedMax = 4 : LifeMax = 75 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 1 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 270, .Type = "A", .Range = 175, .Celerity = 18, .Power = 6, .LoadMax = 8, .BarMax = 1})
            Case "Kastou"
                W = 45 : Agility = 2.2 : SpeedMax = 3 : LifeMax = 225 : ShieldMax = 100 : ShieldOp = 100 : ShieldReg = 20 : Lvl = 4 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 250, .Celerity = 10, .Power = 50, .LoadMax = 50, .BarMax = 1})
            Case "Crusher"
                W = 75 : Agility = 0.4 : SpeedMax = 2.2 : LifeMax = 665 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 5 : Lvl = 5 : If IsNew Then UpsMax = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "D", .Range = 440, .Celerity = 14, .Power = 26, .LoadMax = 55, .BarMax = 2})
                Weapons.Add(New Weapon(Me) With {.Loc = +90, .Type = "C", .Range = 340, .Celerity = 6, .Power = 15, .LoadMax = 22, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "C", .Range = 340, .Celerity = 6, .Power = 15, .LoadMax = 20, .BarMax = 1})
            Case "Loneboss"
                W = 40 : Agility = 1.5 : SpeedMax = 3.2 : LifeMax = 520 : LifeReg = 4 : ShieldMax = 0 : ShieldOp = 25 : ShieldReg = 5 : DeflectorCountMax = 4 : HotDeflector = 55 : Lvl = 4 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 135, .Type = "A", .Range = 200, .Celerity = 17, .Power = 2, .LoadMax = 8, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -135, .Type = "A", .Range = 200, .Celerity = 18, .Power = 2, .LoadMax = 8, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "F", .Range = 360, .Celerity = 16, .Power = 32, .LoadMax = 65, .BarMax = 1})
            Case "Bugs"
                W = 200 : Agility = 2.5 : SpeedMax = 3.2 : LifeMax = 3400 : LifeReg = 2 : ShieldMax = 0 : ShieldOp = 25 : ShieldReg = 5 : ColdDeflector = 1 : HotDeflector = 8 : Lvl = 4 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = -45, .Type = "R", .Range = 510, .Celerity = 12, .Power = 24, .LoadMax = 32, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 45, .Type = "R", .Range = 510, .Celerity = 12, .Power = 24, .LoadMax = 32, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "R", .Range = 310, .Celerity = 24, .Power = 16, .LoadMax = 24, .BarMax = 1})
            Case "Finalizer"
                W = 60 : Agility = 16 : SpeedMax = 14 : LifeMax = 740 : ShieldMax = 1800 : ShieldOp = 100 : ShieldReg = 350 : Lvl = 99 : If IsNew Then UpsMax = 42
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "D", .Range = 860, .Celerity = 22, .Power = 64, .LoadMax = 32, .BarMax = 3})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "D", .Range = 860, .Celerity = 22, .Power = 64, .LoadMax = 32, .BarMax = 3})
            Case "Ori"
                W = 100 : Agility = 1.6 : SpeedMax = 3.8 : LifeMax = 880 : ShieldMax = 2200 : ShieldOp = 100 : ShieldReg = 380 : Lvl = 99 : If IsNew Then UpsMax = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 460, .Celerity = 20, .Power = 55, .LoadMax = 40, .BarMax = 7})
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "B", .Range = 320, .Celerity = 16, .Power = 8, .LoadMax = 8, .BarMax = 1})
                    'Converter
            Case "Converter"
                W = 55 : Agility = 0.8 : SpeedMax = 1.2 : LifeMax = 360 : LifeReg = 3 : ShieldMax = 1200 : ShieldOp = 100 : ShieldReg = 30 : Lvl = 4 : If IsNew Then UpsMax = 2
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "I", .Range = 200, .Celerity = 17, .Power = 3, .LoadMax = 14, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 45, .Type = "I", .Range = 200, .Celerity = 17, .Power = 3, .LoadMax = 15, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -45, .Type = "I", .Range = 200, .Celerity = 17, .Power = 3, .LoadMax = 15, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "I", .Range = 200, .Celerity = 17, .Power = 3, .LoadMax = 16, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "I", .Range = 200, .Celerity = 17, .Power = 3, .LoadMax = 16, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 135, .Type = "I", .Range = 200, .Celerity = 17, .Power = 3, .LoadMax = 17, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -135, .Type = "I", .Range = 200, .Celerity = 17, .Power = 3, .LoadMax = 17, .BarMax = 1})
            Case "Converter_A"
                W = 45 : Agility = 6.5 : SpeedMax = 3.5 : LifeMax = 125 : LifeReg = 0 : ShieldMax = 350 : ShieldOp = 65 : ShieldReg = 0 : Lvl = 4 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 45, .Type = "I", .Range = 200, .Celerity = 17, .Power = 4, .LoadMax = 15, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -45, .Type = "I", .Range = 200, .Celerity = 17, .Power = 4, .LoadMax = 15, .BarMax = 1})
            Case "Converter_B"
                W = 45 : Agility = 1.5 : SpeedMax = 2.8 : LifeMax = 145 : LifeReg = 0 : ShieldMax = 350 : ShieldOp = 65 : ShieldReg = 0 : Lvl = 4 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "D", .Range = 380, .Celerity = 14, .Power = 35, .LoadMax = 40, .BarMax = 1})
                    'Purger
            Case "Purger_Dronner"
                W = 50 : Agility = 1.2 : SpeedMax = 2.8 : LifeMax = 355 : ShieldMax = 300 : ShieldOp = 75 : ShieldReg = 20 : Lvl = 3 : If IsNew Then UpsMax = 16
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "D", .Range = 320, .Celerity = 10, .Power = 25, .LoadMax = 45, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "D", .Range = 320, .Celerity = 10, .Power = 25, .LoadMax = 45, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "I", .Range = 170, .Celerity = 16, .Power = 2, .LoadMax = 4, .BarMax = 1})
            Case "Purger_Drone_1"
                W = 18 : Agility = 4.6 : SpeedMax = 3.8 : LifeMax = 8 : LifeReg = 0 : ShieldMax = 8 : ShieldOp = 75 : ShieldReg = 2 : Lvl = 0 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "I", .Range = 160, .Celerity = 16, .Power = 2, .LoadMax = 4, .BarMax = 1})
            Case "Purger_Drone_2"
                W = 18 : Agility = 7.5 : SpeedMax = 3.8 : LifeMax = 8 : LifeReg = 0 : ShieldMax = 8 : ShieldOp = 85 : ShieldReg = 2 : Lvl = 0 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "I", .Range = 160, .Celerity = 16, .Power = 2, .LoadMax = 6, .BarMax = 1})
            Case "Purger_Drone_3"
                W = 18 : Agility = 2.8 : SpeedMax = 3.8 : LifeMax = 8 : LifeReg = 0 : ShieldMax = 8 : ShieldOp = 100 : ShieldReg = 2 : Lvl = 0 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "B", .Range = 170, .Celerity = 11, .Power = 12, .LoadMax = 16, .BarMax = 1})
                    'Turrets
            Case "Outpost"
                W = 40 : Agility = 0.0 : SpeedMax = 0.0 : LifeMax = 190 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 0 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "A", .Range = 320, .Celerity = 18, .Power = 1, .LoadMax = 12, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "A", .Range = 320, .Celerity = 18, .Power = 1, .LoadMax = 12, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "A", .Range = 320, .Celerity = 18, .Power = 1, .LoadMax = 12, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "A", .Range = 320, .Celerity = 18, .Power = 1, .LoadMax = 12, .BarMax = 1})
            Case "BomberFactory"
                W = 70 : Agility = 0.0 : SpeedMax = 0.0 : LifeMax = 450 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 0 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "A", .Range = 320, .Celerity = 18, .Power = 1, .LoadMax = 24, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "A", .Range = 320, .Celerity = 18, .Power = 1, .LoadMax = 24, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "A", .Range = 320, .Celerity = 18, .Power = 1, .LoadMax = 24, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "A", .Range = 320, .Celerity = 18, .Power = 1, .LoadMax = 24, .BarMax = 1})
            Case "Defense"
                W = 25 : Agility = 0.0 : SpeedMax = 0.0 : LifeMax = 170 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 0 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "E", .Range = 360, .Celerity = 8, .Power = 15, .LoadMax = 45, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 120, .Type = "E", .Range = 360, .Celerity = 8, .Power = 15, .LoadMax = 45, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = -120, .Type = "E", .Range = 360, .Celerity = 8, .Power = 15, .LoadMax = 45, .BarMax = 1})
            Case "Pointvortex"
                W = 25 : Agility = 0.0 : SpeedMax = 0.0 : LifeMax = 130 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 10 : Lvl = 0 : If IsNew Then UpsMax = 1
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "C", .Range = 520, .Celerity = 16, .Power = 45, .LoadMax = 35, .BarMax = 1})
                'Wild
            Case "Asteroide"
                W = 50 : Agility = 1 : SpeedMax = 0.9 : LifeMax = world.Rand.Next(1000, 10000) : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 0 : Lvl = 1
                Behavior = "Drift" : Target = UID : Color = Color.FromArgb(64, 64, 48)
            Case "Meteoroide"
                W = 30 : Agility = 1 : SpeedMax = 2.0 + (world.Rand.Next(0, 100) / 100.0) : LifeMax = 4 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 0 : Lvl = 1
                Behavior = "Drift" : Target = UID : Color = Color.FromArgb(80, 48, 80)
            Case "Comet"
                W = 25 : Agility = 3.0 : SpeedMax = 12.0 : LifeMax = 2 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 5 : Lvl = 1
                Behavior = "Drift" : Target = UID : Color = Color.FromArgb(0, 100, 0)
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "Z", .Range = 48, .Celerity = 8, .Power = 75, .LoadMax = 20, .BarMax = 1})
            Case "Star"
                W = 95 : Agility = 1.0 : SpeedMax = 0.0 : LifeMax = 1073741823 : Lvl = 1
                Behavior = "Drift" : Target = UID : Color = Color.FromArgb(255, 255, 220)
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "C", .Range = 210, .Celerity = 4, .Power = 1, .LoadMax = 75, .BarMax = 7})
                Weapons.Add(New Weapon(Me) With {.Loc = 90, .Type = "C", .Range = 210, .Celerity = 4, .Power = 1, .LoadMax = 75, .BarMax = 7})
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "C", .Range = 210, .Celerity = 4, .Power = 1, .LoadMax = 75, .BarMax = 7})
                Weapons.Add(New Weapon(Me) With {.Loc = -90, .Type = "C", .Range = 210, .Celerity = 4, .Power = 1, .LoadMax = 75, .BarMax = 7})
                'Other
            Case "MSL"
                W = 15 : Agility = 3.5 : SpeedMax = 8 : LifeMax = 8 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 0 : Lvl = 0 : LifeReg = 0 : If IsNew Then UpsMax = 0
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "Z", .Range = 32, .Celerity = 8, .Power = 50, .LoadMax = 20, .BarMax = 1})
                'Other
            Case "Nuke"
                W = 40 : Agility = 2.5 : SpeedMax = 3 : LifeMax = 72 : ShieldMax = 0 : ShieldOp = 0 : ShieldReg = 0 : Lvl = 0 : LifeReg = 0 : If IsNew Then UpsMax = 0
                AllowMining = False
                Weapons.Add(New Weapon(Me) With {.Loc = 0, .Type = "Z", .Range = 48, .Celerity = 8, .Power = 50, .LoadMax = 20, .BarMax = 1})
                Weapons.Add(New Weapon(Me) With {.Loc = 180, .Type = "A", .Range = 160, .Celerity = 17, .Power = 1, .LoadMax = 2, .BarMax = 1})
        End Select
        Team = STeam
        If IsNew Then
            Me.Upgrading = Nothing
            UID = Helpers.GetNextUniqueID
            Life = LifeMax
            Shield = ShieldMax
            If STeam Is Nothing OrElse STeam.id = 0 Then
                Auto = False
            Else
                Auto = True
            End If
            If Not STeam Is Nothing And Behavior <> "Drift" Then
                Color = STeam.color
            End If
            If Not Team Is Nothing AndAlso UpsMax > 0 Then
                UpsMax += Team.UpsBonus
            End If
        End If
        'fin 
    End Sub
    Public Sub Check()
        '===' Fram '==='
        fram = fram + 1
        If fram > 7 Then fram = 0
        '===' Bordures '==='
        If Not Me.Team Is Nothing AndAlso Me.Team.id = 0 Then
            If Me.Coo.X < 0 Then
                Me.Coo.X = 0
            End If
            If Me.Coo.Y < 0 Then
                Me.Coo.Y = 0
            End If
            If Me.Coo.X > world.ArenaSize.Width Then
                Me.Coo.X = world.ArenaSize.Width
            End If
            If Me.Coo.Y > world.ArenaSize.Height Then
                Me.Coo.Y = world.ArenaSize.Height
            End If
        Else
            If Me.Coo.X < -100 Or Me.Coo.Y < -100 Or Me.Coo.X > world.ArenaSize.Width + 100 Or Me.Coo.Y > world.ArenaSize.Height + 100 Then
                If Me.Auto = False Then Me.Life = 0
            End If
        End If
        '===' Déplacement '==='
        If Agility = 0 And SpeedMax = 0 Then
            Direction = Direction + 1
        Else
            Vec = Helpers.GetNewPoint(New Point(0, 0), Direction, Speed)
            Me.Coo.X = Me.Coo.X + Vec.X
            Me.Coo.Y = Me.Coo.Y + Vec.Y
        End If
        '===' Armes '==='
        If Me.ColdDeflector <= 0 Then
            For Each AWeapon As Weapon In Weapons
                If AWeapon.Bar = 0 Then
                    AWeapon.Load = AWeapon.Load + 1
                    If AWeapon.Load >= AWeapon.LoadMax Then
                        AWeapon.Load = 0
                        AWeapon.Bar = AWeapon.BarMax
                    End If
                End If
            Next
        End If
        '===' Shield '==='
        If ShieldMax > 0 Then
            Shield = Shield + (ShieldReg / 100)
            If Shield > ShieldMax Then Shield = ShieldMax
            Dim point_min = Math.Max(0, Shield * 32 / ShieldMax)
            For i = 0 To SHIELD_POINTS - 1
                ShieldPoints(i) -= 4
                If ShieldPoints(i) < point_min Then
                    ShieldPoints(i) = point_min
                End If
            Next
        End If
        '===' Deflector '==='
        If Me.ColdDeflector > 0 Then
            Me.ColdDeflector *= 0.99
            Me.ColdDeflector -= 1
            Me.Life -= 1
        End If
        If DeflectorCount < DeflectorCountMax Then
            DeflectorCooldown -= 1
            If DeflectorCooldown <= 0 Then
                DeflectorCount += 1
                DeflectorCooldown = DeflectorCooldownMax
            End If
        Else
            DeflectorCooldown = DeflectorCooldownMax
        End If
        '===' Vie '==='
        If world.ticks Mod 40 = 0 Then
            If Not Me.Team Is Nothing Then
                Me.Life = Me.Life + LifeReg
                If Me.Life > Me.LifeMax Then Me.Life = Me.LifeMax
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
                Me.SetType(Me.Type, Me.Team)
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
        While Direction > 360
            Direction = Direction - 360
        End While
        While Direction < 0
            Direction = Direction + 360
        End While
        If GetAngDif(Direction, qa) < Agility Then
            Direction = qa
            Exit Sub
        Else
            If qa > 180 Then
                If Direction > qa - 180 And Direction < qa Then
                    Direction = Direction + Agility
                Else
                    Direction = Direction - Agility
                End If
            Else
                If Direction < qa + 180 And Direction > qa Then
                    Direction = Direction - Agility
                Else
                    Direction = Direction + Agility
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
        If Me.DeflectorCount > 0 Then
            Me.DeflectorCount -= 1
            Return
        End If
        If Me.Shield > 0 Then
            Me.Shield = Me.Shield - Amount
            Amount = Amount - (Amount * ShieldOp / 100)
            If Not From Is Nothing Then
                Dim angle_ship_shoot_rel As Double = Helpers.NormalizeAngleUnsigned(Helpers.GetAngle(Coo.X, Coo.Y, From.Coo.X, From.Coo.Y) - Direction)
                Dim shield_ptn_index As Integer = (angle_ship_shoot_rel * 16 / 360)
                ShieldPoints(shield_ptn_index Mod 16) = 255
                If ShieldPoints((shield_ptn_index + 1) Mod 16) < 128 Then ShieldPoints((shield_ptn_index + 1) Mod 16) = 128
                If ShieldPoints((shield_ptn_index + 15) Mod 16) < 128 Then ShieldPoints((shield_ptn_index + 15) Mod 16) = 128
            End If
        End If
        If Me.ColdDeflector >= 0 And Me.ColdDeflector < Me.Life * 2 Then
            Me.ColdDeflector += Amount / 2
            Amount /= 6
        End If
        If Not From Is Nothing AndAlso Not From.Team Is Nothing Then
            If Me.Type = "Star" Then
                From.Team.resources.ResA += Amount / 16
            Else
                If Me.Life > Amount Then
                    From.Team.resources.ResM += Amount
                ElseIf Me.Life > 0 Then
                    From.Team.resources.ResM += Me.Life
                End If
            End If
        End If
        If Amount < 0 Then Return
        Me.Life = Me.Life - Amount : If Me.Life < 0 Then Me.Life = 0
    End Sub


    Public Sub IA(rnd_num As Integer)
        Dim QA As Single
        Dim NeedSpeed As Boolean = False
        '===' Fin de poursuite '==='
        If Me.Behavior <> "Drift" And world.GetShipByUID(Target) Is Nothing Then
            Target = ""
            If Me.Behavior <> "Mine" Then
                Me.Behavior = "Stand"
            End If
        End If
        '===' Derelict are alway drifting '==='
        If Me.Team Is Nothing Then
            Me.Behavior = "Drift"
        End If
        '===' Auto-Activation '==='
        If Me.Auto And Me.Behavior <> "Drift" Then
            Dim NearVal As Integer = Vision : Dim NearUID As String = ""
            If Me.Target = "" Then
                For Each oShip As Ship In world.Ships
                    If (Not Me.Team.IsFriendWith(oShip.Team)) And (rnd_num < 6700 Or Not oShip.Team Is Nothing) Then
                        Dim dist As Integer = Helpers.GetDistance(Me.Coo.X, Me.Coo.Y, oShip.Coo.X, oShip.Coo.Y)
                        If dist < NearVal Then
                            NearVal = dist
                            NearUID = oShip.UID
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
                    QA = Helpers.GetQA(Me.Coo.X, Me.Coo.Y, oShip.Coo.X, oShip.Coo.Y)
                    Dim d As Integer = 50 : If Weapons.Count > 0 Then d = Me.Weapons(0).Range / 2
                    If Helpers.GetDistance(Me.Coo.X, Me.Coo.Y, oShip.Coo.X, oShip.Coo.Y) <= d Then
                        QA = QA + 180
                    End If
                    NeedSpeed = True
                    If Helpers.GetDistance(Me.TargetPTN.X, Me.TargetPTN.Y, oShip.Coo.X, oShip.Coo.Y) > world.ArenaSize.Width / 8 Then
                        Me.Target = ""
                    End If
                Else
                    Dim NearVal As Integer = world.ArenaSize.Width / 8 : Dim NearUID As String = ""
                    For Each oShip As Ship In world.Ships
                        If oShip.Team Is Nothing Then
                            Dim dist_me As Integer = Helpers.GetDistance(Me.Coo.X, Me.Coo.Y, oShip.Coo.X, oShip.Coo.Y)
                            Dim dist_target As Integer = Helpers.GetDistance(Me.TargetPTN.X, Me.TargetPTN.Y, oShip.Coo.X, oShip.Coo.Y)
                            If dist_target < world.ArenaSize.Width / 8 And dist_me < NearVal Then
                                NearVal = dist_me
                                NearUID = oShip.UID
                            End If
                        End If
                    Next
                    Me.Target = NearUID
                    QA = Helpers.GetQA(Me.Coo.X, Me.Coo.Y, Me.TargetPTN.X, Me.TargetPTN.Y)
                    NeedSpeed = True
                End If
            Case "Stand"
                QA = Direction
                NeedSpeed = False
            Case "Drift"
                QA = Direction
                NeedSpeed = True
            Case "Fight"
                If Me.Target <> "" Then
                    NeedSpeed = True
                    Dim oShip As Ship = world.GetShipByUID(Me.Target)
                    QA = Helpers.GetQA(Me.Coo.X, Me.Coo.Y, oShip.Coo.X, oShip.Coo.Y)
                    Dim d As Integer = 50 : If Weapons.Count > 0 Then d = Me.Weapons(0).Range / 2
                    If Helpers.GetDistance(Me.Coo.X, Me.Coo.Y, oShip.Coo.X, oShip.Coo.Y) <= d Then
                        If Me.SpeedMax > 4 Or Me.W < 35 Or Me.Speed < 1.0 Then
                            NeedSpeed = True
                        Else
                            NeedSpeed = False
                        End If
                        QA = QA + 180
                    End If
                End If
            Case "Goto"
                QA = Helpers.GetQA(Me.Coo.X, Me.Coo.Y, Me.TargetPTN.X, Me.TargetPTN.Y)
                If Helpers.GetDistance(Me.Coo.X, Me.Coo.Y, Me.TargetPTN.X, Me.TargetPTN.Y) <= 50 Then
                    Me.Behavior = "Stand"
                End If
                NeedSpeed = True
        End Select
        '===' Appliquation '==='
        TurnToQA(QA)
        If NeedSpeed Then
            Me.Speed = Me.Speed + Me.Agility / 20
        Else
            Me.Speed = Me.Speed - Me.Agility / 20
        End If
        If Me.Speed > Me.SpeedMax Then
            Me.Speed -= 1
            If Me.Speed < Me.SpeedMax Then Me.Speed = Me.SpeedMax
        End If
        If Me.Speed < 0 Then Me.Speed = 0
        IAFire()
    End Sub 'IAIAIAIAIA
    Public Sub IAFire()
        '===' Tirer '==='
        If Weapons.Count > 0 And (fram Mod 2 = 0) Then
            For Each AWeap As Weapon In Weapons
                If AWeap.Bar > 0 Then
                    Dim record As Integer = 100000 : Dim recorded As String = "" 'Pas de cible
                    Dim ToX As Integer = (Math.Sin(2 * Math.PI * (AWeap.Loc + Direction) / 360) * (W / 2)) + Coo.X
                    Dim ToY As Integer = (Math.Cos(2 * Math.PI * (AWeap.Loc + Direction) / 360) * (W / 2)) + Coo.Y
                    For Each OVessel As Ship In world.Ships
                        If OVessel Is Me Then
                            Continue For
                        End If
                        If Not Me.AllowMining And (OVessel.Type = "Asteroide" Or OVessel.Type = "Meteoroide") Then
                            Continue For
                        End If
                        If Me.Team Is Nothing OrElse Not Me.Team.IsFriendWith(OVessel.Team) Then
                            Dim dist As Integer = Helpers.GetDistance(ToX, ToY, OVessel.Coo.X, OVessel.Coo.Y)
                            If dist < Me.Weapons(0).Range Then
                                If Me.Team Is Nothing OrElse Not OVessel.Team Is Nothing AndAlso Not Me.Team.IsFriendWith(OVessel.Team) Then
                                    dist /= 4
                                End If
                                If Me.Target = OVessel.UID Then
                                    dist /= 2
                                End If
                            End If
                            If dist < record Then
                                record = dist
                                recorded = OVessel.UID
                            End If
                        End If
                    Next
                    If recorded <> "" Then
                        Dim oShip As Ship = world.GetShipByUID(recorded)
                        record = Helpers.GetDistance(ToX, ToY, oShip.Coo.X, oShip.Coo.Y)
                        If record < AWeap.Range Then
                            Dim NewPoint As PointF = Helpers.GetNewPoint(oShip.Coo, oShip.Direction, oShip.Speed * (record / AWeap.Celerity) * 0.9)
                            Dim QA As Integer = Helpers.GetQA(ToX, ToY, NewPoint.X, NewPoint.Y)
                            AWeap.Fire(QA, New Point(ToX, ToY), Me)
                            If AWeap.Type = "Z" Then
                                Me.Life = -2048
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
                    Me.Color = Color.FromName(Spliter(1))
                    If Me.Type = "Station" Then
                        Me.Team.color = Me.Color
                    End If
                Case "?W"
                    If Weapons.Count > 0 Then Return True
                Case "?S"
                    If SpeedMax > 0 Then Return True
                Case "?Base"
                    If Me.UID = MainForm.MAIN_BASE Then Return True
                Case "!Jump"
                    Me.Speed = Convert.ToInt32(Spliter(1))



                Case "!Agility"
                    Me.Agility += Spliter(1)
                Case "!Teleport"
                Dim target_ship As Ship = world.GetShipByUID(Me.Target)
                If Not target_ship Is Nothing Then
                    Me.Coo = target_ship.Coo + New Point(world.Rand.Next(-512, 512), world.Rand.Next(-512, 512))
                End If
                Me.Coo = Me.TargetPTN + New Point(world.Rand.Next(-512, 512), world.Rand.Next(-512, 512))
            Case "!Upsbonus"
                    If FN Then Me.Team.UpsBonus += Spliter(1) 'FN
                Case "!Maxships"
                    If FN Then Me.Team.MaxShips += Spliter(1) 'FN
                Case "+Shield"
                    If Me.ShieldMax >= Spliter(1) Then Return True
                Case "!Shield"
                    Me.ShieldMax = Me.ShieldMax + Spliter(1)
                Case "!Deflector"
                    Me.DeflectorCountMax += Spliter(1)
                Case "!HotDeflector"
                    Me.HotDeflector += Spliter(1)
                Case "!ColdDeflector"
                    Me.ColdDeflector = Spliter(1)
                Case "%Shield"
                    Me.ShieldMax = Me.ShieldMax + (Me.ShieldMax * (Spliter(1) / 100))
                Case "!Shieldop"
                    Me.ShieldOp = Me.ShieldOp + Spliter(1)
                Case "%Shieldreg"
                    Me.ShieldReg = Me.ShieldReg + (Me.ShieldReg * (Spliter(1) / 100))
                Case "!UpsMax"
                    Me.UpsMax += Spliter(1)
                Case "!Fix"
                    Me.Life += Me.LifeMax * (Spliter(1) / 100.0)
                Case "!FixSFull"
                    Me.Shield = Me.ShieldMax
                Case "+Lvl"
                    If Me.Lvl >= Spliter(1) Then Return True
                Case "+Speed" 'vitesse
                    If Me.SpeedMax >= Spliter(1) Then Return True
                Case "-Speed"
                    If Me.SpeedMax <= Spliter(1) Then Return True
                Case "%Speed"
                    Me.SpeedMax = Me.SpeedMax + (Me.SpeedMax * (Spliter(1) / 100))
                    Me.Agility = Me.Agility + (Me.Agility * (Spliter(1) / 100))
                Case "+Life" 'Resistance
                    If Me.LifeMax >= Spliter(1) Then Return True
                Case "-Life" '
                    If Me.LifeMax <= Spliter(1) Then Return True
                Case "%Life"
                    Me.LifeMax = Me.LifeMax + (Me.LifeMax * (Spliter(1) / 100))
                    If FN Then Me.Life = Me.Life + (Me.Life * (Spliter(1) / 100)) 'FN
                Case "!Regen"
                    If FN Then Me.LifeReg = Me.LifeReg + Spliter(1) 'FN
                Case "?Up"
                    If Me.HaveUp(Spliter(1)) Then Return True
                Case "?Type" 'Type
                    If Me.Type = Spliter(1) Then Return True
                Case "!Type"
                    If FN Then Me.Type = Spliter(1)
                Case "?Wtype" 'armement
                    If Me.Weapons(0).Type = Spliter(1) Then Return True
                Case "%Wloadmax"
                    For Each AW As Weapon In Weapons
                        AW.LoadMax = AW.LoadMax + (AW.LoadMax * (Spliter(1) / 100))
                    Next
                Case "%Wbar"
                    For Each AW As Weapon In Weapons
                        AW.BarMax = AW.BarMax + (AW.BarMax * (Spliter(1) / 100))
                    Next
                Case "%Wpower"
                    For Each AW As Weapon In Weapons
                        AW.Power = AW.Power + (AW.Power * (Spliter(1) / 100))
                    Next
                Case "%Wrange"
                    For Each AW As Weapon In Weapons
                        AW.Range = AW.Range + (AW.Range * (Spliter(1) / 100))
                    Next
                Case "%Wcelerity"
                    For Each AW As Weapon In Weapons
                        AW.Celerity = AW.Celerity + (AW.Celerity * (Spliter(1) / 100))
                    Next
                Case "" 'Debug
                    Return True
                Case "?MS"
                If Me.Team Is Nothing OrElse world.CountTeamShips(Team) < Me.Team.MaxShips Then Return True
            Case "!Sum"
                If FN Then world.Ships.Add(New Ship(world) With {.Coo = New Point(Me.Coo.X + world.Rand.Next(-10, 11), Me.Coo.Y + world.Rand.Next(-10, 11))}) : world.Ships(world.Ships.Count - 1).Direction = Me.Direction : world.Ships(world.Ships.Count - 1).SetType(Spliter(1), Me.Team, True)
                If world.Ships(world.Ships.Count - 1).Type <> "MSL" Then
                    world.Ships(world.Ships.Count - 1).Behavior = "Fight"
                    world.Ships(world.Ships.Count - 1).Target = Me.UID
                End If
            Case "!Ascend"
                    If Me.Team.id = 0 Then
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








End Class
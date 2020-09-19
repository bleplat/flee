Public Class Weapon
    Public ship As Ship = Nothing

    ' Special effect flag
    Enum SpecialBits
        Plasma = 1
        Propeled = 2
        Explode = 4
        BioDrops = 8
        SelfExplode = 16
        SelfNuke = 32
        SpreadOrigin = 64
        Rail = 128
        Flak = 256
    End Enum
    Shared Function SpecialFromString(input As String) As Integer
        Dim total As Integer = 0
        Dim elements() As String = input.Split(";")
        For Each element As String In elements
            If element.Length() = 0 Then Continue For
            Select Case element
                Case "Plasma" : total = total Or SpecialBits.Plasma
                Case "Propeled" : total = total Or SpecialBits.Propeled
                Case "Explode" : total = total Or SpecialBits.Explode
                Case "BioDrops" : total = total Or SpecialBits.BioDrops
                Case "SelfExplode" : total = total Or SpecialBits.SelfExplode
                Case "SelfNuke" : total = total Or SpecialBits.SelfNuke
                Case "SpreadOrigin" : total = total Or SpecialBits.SpreadOrigin
                Case "Rail" : total = total Or SpecialBits.Rail
                Case "Flak" : total = total Or SpecialBits.Flak
                Case Else : Throw New Exception("Special doesnt exists: " & element)
            End Select
        Next
        Return total
    End Function
    Shared Function SpecialToString(weapon_effect As Integer) As String
        Dim total As String = ""
        If (total And SpecialBits.Plasma) <> 0 Then total &= "Plasma;"
        If (total And SpecialBits.Propeled) <> 0 Then total &= "Propeled;"
        If (total And SpecialBits.Explode) <> 0 Then total &= "Explode;"
        If (total And SpecialBits.BioDrops) <> 0 Then total &= "BioDrops;"
        If (total And SpecialBits.SelfExplode) <> 0 Then total &= "SelfExplode;"
        If (total And SpecialBits.SelfNuke) <> 0 Then total &= "SelfNuke;"
        If (total And SpecialBits.SpreadOrigin) <> 0 Then total &= "SpreadOrigin;"
        If (total And SpecialBits.Rail) <> 0 Then total &= "Rail;"
        If (total And SpecialBits.Flak) <> 0 Then total &= "Flak;"
        Return total
    End Function

    ' stats
    Private base_stats As GunStats = Nothing
    Public stats As GunStats = Nothing
    Public Loc As Integer = 0

    'state
    Public Load As Integer = 0
    Public Bar As Integer = 0

    ' creation
    Sub New(ship As Ship)
        Me.ship = ship
    End Sub
    Sub New(ship As Ship, angle As Integer, gun_class As GunStats)
        Me.ship = ship
        Me.Loc = angle
        Me.base_stats = gun_class
        Me.ResetStats()
    End Sub
    Sub New(ship As Ship, input As String)
        Me.ship = ship
        Me.FromString(input)
        Me.ResetStats()
    End Sub
    Public Sub ResetStats()
        stats = base_stats.Clone()
    End Sub

    Public Sub Fire(ByVal QA As Single, ByVal PTN As Point, ByVal Launcher As Ship)
        If Bar > 0 Then
            Bar = Bar - 1

            Dim spawn_point As PointF = PTN
            If (Me.base_stats.special And Weapon.SpecialBits.SpreadOrigin) <> 0 Then
                spawn_point = New PointF(PTN.X + ship.world.Rand.Next(-7, 8), PTN.Y + ship.world.Rand.Next(-7, 8))
            End If

            If (Me.base_stats.special And Weapon.SpecialBits.Rail) <> 0 Then
                Dim dispersion = 16
                For i = 0 To dispersion
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = spawn_point, .Type = base_stats.sprite, .Direction = QA, .speed = stats.celerity + (i / 2), .Life = (stats.range / stats.celerity), .Power = stats.power / dispersion, .Team = Launcher.team, .special = Me.base_stats.special})
                Next
            ElseIf (Me.base_stats.special And Weapon.SpecialBits.Flak) <> 0 Then
                Dim dispersion = 16
                For i = -(dispersion / 2) To (dispersion / 2)
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = spawn_point, .Type = base_stats.sprite, .Direction = QA + i * (360 / dispersion / 16), .speed = stats.celerity + ((i + dispersion) Mod 4) / 2.0, .Life = (stats.range / stats.celerity) + ((i + dispersion) Mod 3), .Power = stats.power / dispersion, .Team = Launcher.team, .special = Me.base_stats.special})
                Next
            ElseIf (Me.base_stats.special And Weapon.SpecialBits.SelfExplode) <> 0 OrElse (Me.base_stats.special And Weapon.SpecialBits.SelfNuke) <> 0 Then
                Dim dispersion = 16
                For i = -(dispersion / 2) To (dispersion / 2)
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = spawn_point, .Type = base_stats.sprite, .Direction = QA + i * (360 / dispersion), .speed = stats.celerity, .Life = (stats.range / stats.celerity), .Power = stats.power / 4 / dispersion, .Team = Launcher.team, .special = Me.base_stats.special})
                Next
                ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = spawn_point, .Type = base_stats.sprite, .Direction = QA, .speed = stats.celerity, .Life = (stats.range / stats.celerity), .Power = stats.power, .Team = Launcher.team, .special = Me.base_stats.special})
            Else
                ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = spawn_point, .Type = base_stats.sprite, .Direction = QA, .speed = stats.celerity, .Life = (stats.range / stats.celerity), .Power = stats.power, .Team = Launcher.team, .special = Me.base_stats.special})
            End If

        End If
    End Sub

    ' Import/Export
    Public Function ToString() As String
        Return (Me.Loc.ToString() & ";" & Me.stats.name)
    End Function
    Public Sub FromString(input As String)
        Dim parts() As String = input.Split(";")
        Me.Loc = Convert.ToInt32(parts(0))
        Me.base_stats = GunStats.classes(parts(1))
        Me.ResetStats()
    End Sub


End Class
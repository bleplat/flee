Public Class Weapon
    Public ship As Ship

    Sub New(ship As Ship)
        Me.ship = ship
    End Sub

    Public Sub Fire(ByVal QA As Single, ByVal PTN As Point, ByVal Launcher As Ship)
        If Bar > 0 Then
            Bar = Bar - 1
            Select Case Type
                Case "A" 'Simple Shoot 1
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "B" 'Simple Shoot 2
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "C" 'Plasma Shoot
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "D" 'Plasma Shoot 2
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "E" 'Plasma lightning
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "F" 'Rail Gun
                    Dim dispersion = 16
                    For i = 0 To dispersion
                        ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity + (i / 2), .Life = (Range / Celerity), .Power = Power / dispersion, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                    Next
                Case "G" 'Lazer
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "H" '
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "I" '
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = New PointF(PTN.X + ship.world.Rand.Next(-7, 8), PTN.Y + ship.world.Rand.Next(-7, 8)), .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "Q" 'Thrower
                    Dim dispersion = 16
                    For i = -(dispersion / 2) To (dispersion / 2)
                        ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA + i * (360 / dispersion / 16), .speed = Celerity + ((i + dispersion) Mod 4) / 2.0, .Life = (Range / Celerity) + ((i + dispersion) Mod 3), .Power = Power / dispersion, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                    Next
                Case "R"
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA + ship.world.Rand.Next(-128, 128) / 8, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})

                Case "Z"
                    Dim dispersion = 16
                    For i = -(dispersion / 2) To (dispersion / 2)
                        ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA + i * (360 / dispersion), .speed = Celerity, .Life = (Range / Celerity), .Power = Power / 4 / dispersion, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                    Next
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})

                Case Else
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = Type, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
            End Select
        End If
    End Sub
    Public Loc As Integer = 0
    Public Type As String = "A"
    Public Celerity As Integer = 10
    Public Power As Integer = 5
    Public Load As Integer = 0 : Public LoadMax As Integer = 50
    Public Bar As Integer = 0 : Public BarMax As Integer = 1
    Public Range As Integer = 500
End Class
Public Class Weapon
    Public ship As Ship = Nothing

    ' class
    Public gun_class As GunClass = Nothing

    ' properties
    Public Loc As Integer = 0
    Public Power As Integer = 5
    Public Celerity As Integer = 10
    Public Load As Integer = 0 : Public LoadMax As Integer = 50
    Public Bar As Integer = 0 : Public BarMax As Integer = 1
    Public Range As Integer = 500

    Sub New(ship As Ship)
        Me.ship = ship
    End Sub
    Sub New(ship As Ship, angle As Integer, gun_class As GunClass)
        Me.ship = ship
        Me.Loc = angle
        Me.gun_class = gun_class
        Me.ResetGunFromClass()
    End Sub
    Public Sub ResetGunFromClass()
        Me.Range = gun_class.range
        Me.Power = gun_class.power
        Me.Celerity = gun_class.celerity
        Me.LoadMax = gun_class.loadtime
        Me.BarMax = gun_class.salvo
    End Sub

    Public Sub Fire(ByVal QA As Single, ByVal PTN As Point, ByVal Launcher As Ship)
        If Bar > 0 Then
            Bar = Bar - 1
            Select Case gun_class.sprite
                Case "A" 'Simple Shoot 1
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "B" 'Simple Shoot 2
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "C" 'Plasma Shoot
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "D" 'Plasma Shoot 2
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "E" 'Plasma lightning
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "F" 'Rail Gun
                    Dim dispersion = 16
                    For i = 0 To dispersion
                        ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity + (i / 2), .Life = (Range / Celerity), .Power = Power / dispersion, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                    Next
                Case "G" 'Lazer
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "H" '
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "I" '
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = New PointF(PTN.X + ship.world.Rand.Next(-7, 8), PTN.Y + ship.world.Rand.Next(-7, 8)), .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                Case "Q" 'Thrower
                    Dim dispersion = 16
                    For i = -(dispersion / 2) To (dispersion / 2)
                        ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA + i * (360 / dispersion / 16), .speed = Celerity + ((i + dispersion) Mod 4) / 2.0, .Life = (Range / Celerity) + ((i + dispersion) Mod 3), .Power = Power / dispersion, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                    Next
                Case "R"
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA + ship.world.Rand.Next(-128, 128) / 8, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})

                Case "Z"
                    Dim dispersion = 16
                    For i = -(dispersion / 2) To (dispersion / 2)
                        ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA + i * (360 / dispersion), .speed = Celerity, .Life = (Range / Celerity), .Power = Power / 4 / dispersion, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
                    Next
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})

                Case Else
                    ship.world.Shoots.Add(New Shoot(ship.world) With {.Coo = PTN, .Type = gun_class.sprite, .Direction = QA, .speed = Celerity, .Life = (Range / Celerity), .Power = Power, .Team = Launcher.Team, .DispMode = Shoot.DisplayMode.Simple})
            End Select
        End If
    End Sub


End Class
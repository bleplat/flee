Public Class Shoot
    Sub New(ByRef world As World)
        Me.world = world
    End Sub


    Public Sub Check()
        '===' Fram '==='
        fram = fram + 1
        If fram > 7 Then fram = 0
        '===' Déplacement '==='
        Vec = Helpers.GetNewPoint(New Point(0, 0), Direction, speed)
        Me.Coo.X = Me.Coo.X + Vec.X
        Me.Coo.Y = Me.Coo.Y + Vec.Y
        '===' Life '==='
        Life = Life - 1
        '===' Spécial '==='
        Select Case Type
            Case "C"
                world.Effects.Add(New Effect With {.Type = "Plasma", .Coo = Coo, .Direction = world.Rand.Next(0, 360), .Life = 6, .speed = 1})
            Case "D"
                world.Shoots.Add(New Shoot(Me.world) With {.Type = "A", .Coo = Coo, .Direction = world.Rand.Next(0, 360), .Life = 5, .speed = 2, .Power = 1, .Team = Me.Team})
            Case "R"
                world.Shoots.Add(New Shoot(Me.world) With {.Type = "B", .Coo = Coo, .Direction = Direction + world.Rand.Next(-90, 90), .Life = 8, .speed = world.Rand.Next(4, 8), .Power = 2, .Team = Me.Team})
                world.Shoots.Add(New Shoot(Me.world) With {.Type = "B", .Coo = Coo, .Direction = world.Rand.Next(0, 360), .Life = 8, .speed = world.Rand.Next(6, 10), .Power = Power / 2, .Team = Me.Team})
        End Select
    End Sub
    Public world As World = Nothing
    'Primaire
    Public fram As UShort = 0
    Public Type As String = "Default"
    Public Team As Team = Nothing
    Public Coo As New PointF : Public Vec As New PointF()
    Public Direction As Single = 0
    Public speed As Single = 0
    Public Life As Integer = 8
    'Secondaire
    Public Power As Integer = 10
    Public DispMode As DisplayMode = DisplayMode.Simple

    Enum DisplayMode
        Simple
    End Enum
End Class
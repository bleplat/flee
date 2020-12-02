Public Class Effect
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
    End Sub
    'Primaire
    Public fram As UShort = 0
    Public sprite_y As UShort = 0
    Public Type As String = "Default"
    Public Coo As New PointF : Public Vec As New PointF()
    Public Direction As Single = 0
    Public speed As Single = 0
    Public Life As Integer = 8
End Class
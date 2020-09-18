Public Class Upgrade2

	Public Sub New(name As String)
		Me.name = name
	End Sub

	' Upgrade Name
	Public name As String = "Default"
	' Upgrade Sprite icon
	Public sprite As String = "Ups"
	Public coords As Point = New Point(0, 0)
	' Dependencies
	Public cost As MaterialSet = New MaterialSet(0, 0, 0, 0)
	Public ships As Integer = 0 ' > 0 if this summon ships, checked against max ships
	Public requieres As New List(Of String)

	' Import/Export
	Public Function ToString() As String
		Dim total As String = ""
		total += "UPGRADE " & name & "\n"
		total += "sprite=" & sprite & "\n"
		total += "coords=" & coords.X.ToString() & ";" & coords.Y.ToString() & "\n"
		total += "cost=" & cost.ToString() & "\n"
		total += "ships=" & ships.ToString() & "\n"
		Return total
	End Function
	Public Sub SetProperty(name As String, value As String)
		Select Case name
			Case "sprite" : Me.sprite = value
			Case "coords" : Me.coords = Helpers.PointFromString(value)
			Case "cost" : Me.cost.LoadFromString(value)
			Case "ships" : Me.ships = Convert.ToInt32(value)
			Case "requiere" : Me.ships = Convert.ToInt32(value)
		End Select
	End Sub

End Class

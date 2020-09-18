Public Class GunClass

	' shared
	Public Shared classes As New Dictionary(Of String, GunClass)

	' identity and appearence
	Public name As String = "Default-Gun"
	Public sprite As String = "MSL"
	Public effect As Integer = 0
	Public desc As String = ""

	' stats
	Public range As Integer = 180
	Public celerity As Integer = 16
	Public power As Integer = 0
	Public loadtime As Integer = 15
	Public salvo As Integer = 1

	' constructor
	Public Sub New(name As String)
		Me.name = name
	End Sub

	' Import/Export
	Public Function ToString() As String
		Dim total As String = "gun " & Me.name & vbLf
		If desc <> "" Then
			total &= vbTab & "desc=" & Me.desc & vbLf
		End If
		total &= vbTab & "gun " & Me.name & vbLf
		total &= vbTab & "sprite=" & Me.sprite & vbLf
		If effect <> 0 Then
			total &= vbTab & "effect=" & Me.effect.ToString() & vbLf
		End If
		total &= vbTab & "range=" & Me.range.ToString() & vbLf
		total &= vbTab & "celerity=" & Me.celerity.ToString() & vbLf
		total &= vbTab & "power=" & Me.power.ToString() & vbLf
		total &= vbTab & "loadtime=" & Me.loadtime.ToString() & vbLf
		If salvo <> 1 Then
			total &= vbTab & "salvo=" & Me.salvo.ToString() & vbLf
		End If
		Return total
	End Function
	Public Sub SetProperty(name As String, value As String)
		Select Case name
			Case "desc" : Me.desc = value
			Case "sprite" : Me.sprite = value
			Case "effect" : Me.effect = Convert.ToInt32(value)
			Case "range" : Me.range = Convert.ToInt32(value)
			Case "celerity" : Me.celerity = Convert.ToInt32(value)
			Case "power" : Me.power = Convert.ToInt32(value)
			Case "loadtime" : Me.loadtime = Convert.ToInt32(value)
			Case "salvo" : Me.salvo = Convert.ToInt32(value)
			Case Else : Throw New Exception("Property " & name & " is not part of gun class!")
		End Select
	End Sub

End Class

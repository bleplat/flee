Imports System.Globalization

Public Class ShipStats

	' shared
	Public Shared classes As New Dictionary(Of String, ShipStats)
	Public Shared Function DumpClasses() As String
		Dim total As String = ""
		For Each stats As ShipStats In classes.Values
			total += stats.ToString()
			total += vbLf
		Next
		Return total
	End Function

	' identity
	Public name As String = "Default"
	Public desc As String = Nothing

	' visual
	Public sprite As String = "Plasma"
	Public level As Integer = 0
	Public width As Integer = -1

	' stats (base)
	Public integrity As Integer = 0
	Public repair As Integer = 0
	Public speed As Double = 0.0
	Public turn As Double = 0.0
	Public default_weapons As New List(Of String)

	' stats (shields)
	Public shield As Integer = 0
	Public shield_regeneration As Integer = 10
	Public shield_opacity As Double = 0

	' stats (deflectors)
	Public deflectors As Integer = 0
	Public deflectors_cooldown As Integer = 64
	Public hot_deflector As Double = 0
	Public cold_deflector As Boolean = 0

	' crafting
	Public crafts As New List(Of String)
	Public cost As MaterialSet = New MaterialSet(0, 0, 0, 0)
	Public complexity = 0

	' upgrades
	Public native_upgrades As New List(Of String)

	' constructor
	Public Sub New(name As String)
		Me.name = name
		SetSprite(name)
	End Sub
	Public Sub SetSprite(sprite As String)
		Me.sprite = sprite
		Dim try_bmp As Bitmap = Helpers.GetSprite(Me.sprite, -1, -1, Nothing)
		If Not try_bmp Is Nothing Then
			Me.width = try_bmp.Width / 8 - 2
			Me.level = Math.Sqrt(Me.width)
			Me.integrity = (Me.width * Me.width) / 12
			If Me.width >= 25 Then Me.repair = 1
			Me.cost.Metal = Me.width * 2 + Me.level * 10
			If Me.width >= 20 Then Me.cost.Crystal = 1
		End If
	End Sub

	' Import/Export
	Public Sub SetProperty(name As String, value As String)
		Select Case name
			Case "desc" : Me.desc = value
			Case "sprite"
				SetSprite(value)
			Case "level" : Me.level = Convert.ToInt32(value)
			Case "width" : Me.width = Convert.ToInt32(value)
			Case "integrity" : Me.integrity = Convert.ToInt32(value)
			Case "repair" : Me.repair = Convert.ToInt32(value)
			Case "speed"
				Me.speed = Helpers.ToDouble(value)
				If turn = 0 Then Me.turn = speed
			Case "turn" : Me.turn = Helpers.ToDouble(value)
			Case "weapon" : default_weapons.Add(value)
			Case "shield"
				Me.shield = value
				If Me.shield_opacity = 0 Then Me.shield_opacity = 25
			Case "shield_regeneration" : Me.shield_regeneration = Helpers.ToDouble(value)
			Case "shield_opacity" : Me.shield_opacity = Helpers.ToDouble(value)
			Case "deflectors" : Me.deflectors = Convert.ToInt32(value)
			Case "deflectors_cooldown" : Me.deflectors_cooldown = Convert.ToInt32(value)
			Case "hot_deflector" : Me.hot_deflector = Helpers.ToDouble(value)
			Case "cold_deflector" : Me.cold_deflector = Convert.ToInt32(value)
			Case "craft" : Me.crafts.Add(value)
			Case "native_upgrade" : Me.native_upgrades.Add(value)
			Case "cost"
				Me.cost = New MaterialSet(value)
				If Me.complexity = 0 Then Me.complexity = Me.width * 5 + Me.cost.Metal / 8 + Me.cost.Crystal * 15 + Me.cost.Antimatter / 4 + Me.cost.Fissile * 100
			Case "complexity" : Me.complexity = Convert.ToInt32(value)
			Case Else : Throw New Exception("'" & name & "' is not a valid ship property")
		End Select
	End Sub
	Public Function ToString() As String
		Dim total As String = "ship " & Me.name & vbLf
		If Me.sprite <> Me.name Then
			total &= vbTab & "sprite=" & Me.sprite & vbLf
		End If
		total &= vbTab & "level=" & Me.level.ToString() & vbLf
		If Me.width <> Helpers.GetSprite(Me.sprite, -1, -1, Nothing).Width / 8 - 2 Then
			total &= vbTab & "width=" & Me.width.ToString() & vbLf
		End If
		total &= vbTab & "integrity=" & Me.integrity.ToString() & vbLf
		If speed <> 0.0 Then
			total &= vbTab & "speed=" & Helpers.ToString(speed) & vbLf
		End If
		If turn <> speed Then
			total &= vbTab & "turn=" & Helpers.ToString(Me.turn) & vbLf
		End If
		For Each item As String In default_weapons
			total &= vbTab & "weapon=" & item & vbLf
		Next
		'weapons
		If shield > 0 Then
			total &= vbTab & "shield=" & Me.shield.ToString() & vbLf
			If shield_regeneration <> 10 Then
				total &= vbTab & "shield_regeneration=" & Helpers.ToString(Me.shield_regeneration) & vbLf
			End If
			If shield_opacity <> 25 Then
				total &= vbTab & "shield_opacity=" & Helpers.ToString(Me.shield_opacity) & vbLf
			End If
		End If
		If deflectors > 0 Then
			total &= vbTab & "deflectors=" & Me.deflectors.ToString() & vbLf
			If deflectors_cooldown <> 64 Then
				total &= vbTab & "deflectors_cooldown=" & Me.deflectors_cooldown.ToString() & vbLf
			End If
		End If
		If hot_deflector Then
			total &= vbTab & "hot_deflector=" & Helpers.ToString(Me.hot_deflector) & vbLf
		End If
		If cold_deflector Then
			total &= vbTab & "cold_deflector=" & Convert.ToInt32(Me.cold_deflector).ToString() & vbLf
		End If
		For Each item As String In crafts
			total &= vbTab & "craft=" & item & vbLf
		Next
		For Each item As String In native_upgrades
			total &= vbTab & "native_upgrade=" & item & vbLf
		Next
		total &= vbTab & "cost=" & Me.cost.ToString() & vbLf
		total &= vbTab & "complexity=" & Me.complexity.ToString() & vbLf
		Return total
	End Function
	Public Function Clone() As ShipStats
		Return DirectCast(Me.MemberwiseClone(), ShipStats)
	End Function

End Class

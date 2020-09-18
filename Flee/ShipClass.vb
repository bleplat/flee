Public Class ShipClass
	' identity
	Public name As String = "Default"
	' visual
	Public sprite As String = "Station"
	Public level As Integer = 0
	Public width As Integer = -1
	' stats (base)
	Public integrity As Integer = 0
	Public repair As Integer = 0
	Public speed As Double = 0.0
	Public turn As Double = 0.0
	Public weapon_slots As New List(Of Integer)
	Public weapons As New List(Of String)
	' stats (shields)
	Public shield As Integer = 0
	Public shield_regeneration As Integer = 10
	Public shield_opacity As Integer = 25
	' stats (deflectors)
	Public deflectors As Integer = 0
	Public deflectors_cooldown As Integer = 128
	Public hot_deflector As Integer = 0
	Public cold_deflector As Boolean = 0
	' crafting
	Public craft As New List(Of ShipClass)
	Public cost As MaterialSet = New MaterialSet(0, 0, 0, 0)
	Public complexity = 400
	' settings
	Public Sub SetProperty(name As String, value As String)
		Select Case name
			Case "sprite" : name = value
			Case "level" : name = Convert.ToInt32(value)
			Case "width" : name = Convert.ToInt32(value)
			Case "integrity" : name = Convert.ToInt32(value)
			Case "repair" : name = Convert.ToInt32(value)
			Case "speed" : name = Convert.ToDouble(value)
			Case "turn" : name = Convert.ToDouble(value)
			Case "weapon_slot" : weapon_slots.Add(Convert.ToInt32(value))
			Case "weapon" : weapons.Add(value)
			Case "shield" : name = value
			Case "shield_regeneration" : name = value
			Case "shield_opacity" : name = value
			Case "deflectors" : name = value
			Case "deflectors_cooldown" : name = value
			Case "hot_deflector" : name = value
			Case "cold_deflector" : name = value
			Case "name" : name = value
		End Select
	End Sub
End Class

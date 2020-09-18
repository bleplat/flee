Public Class MaterialSet

	' Materials
	Public Metal As Long = 0 ' TODO: Resname "metals"
	Public Crystal As Long = 0 ' TODO: Resname "crystals"
	Public Fissile As Long = 0 ' TODO: Resname "fissile"
	Public Antimatter As Long = 0 ' TODO: Resname "antimatter"

	' Constructor
	Sub New()
		' Default
	End Sub
	Sub New(ResM As Long, ResC As Long, ResU As Long, ResA As Long)
		Me.Metal = ResM
		Me.Crystal = ResC
		Me.Fissile = ResU
		Me.Antimatter = ResA
	End Sub
	Sub New(input As String)
		Me.LoadFromString(input)
	End Sub

	' Test if this material set contains at least some requierements
	Public Function HasEnough(ByRef requierement As MaterialSet) As Boolean
		Return (Metal >= requierement.Metal AndAlso Crystal >= requierement.Crystal AndAlso Fissile >= requierement.Fissile AndAlso Antimatter >= requierement.Antimatter)
	End Function

	' Remove materials from this set
	Public Sub Deplete(ByRef depletion As MaterialSet)
		Metal -= depletion.Metal
		Crystal -= depletion.Crystal
		Fissile -= depletion.Fissile
		Antimatter -= depletion.Antimatter
	End Sub

	' Add materials to this set
	Public Sub Add(ByRef addition As MaterialSet)
		Metal += addition.Metal
		Crystal += addition.Crystal
		Fissile += addition.Fissile
		Antimatter += addition.Antimatter
	End Sub
	Public Sub Add(ResM As Long, ResC As Long, ResU As Long, ResA As Long)
		Me.Metal += ResM
		Me.Crystal += ResC
		Me.Fissile += ResU
		Me.Antimatter += ResA
	End Sub

	' Import / Export
	Public Function ToString() As String
		Return (Metal & ";" & Crystal & ";" & Antimatter & ";" & Fissile)
	End Function
	Public Sub LoadFromString(input As String)
		Dim inputs(5) As String
		inputs = input.Split(";")
		Metal = Convert.ToInt64(inputs(0))
		Crystal = Convert.ToInt64(inputs(1))
		Antimatter = Convert.ToInt64(inputs(2))
		Fissile = Convert.ToInt64(inputs(3))
	End Sub

End Class

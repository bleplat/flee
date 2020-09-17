Public Class MaterialSet

	' Materials
	Public ResM As Long = 0 ' TODO: Resname "metals"
	Public ResC As Long = 0 ' TODO: Resname "crystals"
	Public ResU As Long = 0 ' TODO: Resname "fissile"
	Public ResA As Long = 0 ' TODO: Resname "antimatter"

	' Constructor
	Sub New()
		' Default
	End Sub
	Sub New(ResM As Long, ResC As Long, ResU As Long, ResA As Long)
		Me.ResM = ResM
		Me.ResC = ResC
		Me.ResU = ResU
		Me.ResA = ResA
	End Sub

	' Test if this material set contains at least some requierements
	Public Function HasEnough(ByRef requierement As MaterialSet) As Boolean
		Return (ResM >= requierement.ResM AndAlso ResC >= requierement.ResC AndAlso ResU >= requierement.ResU AndAlso ResA >= requierement.ResA)
	End Function

	' Remove materials from this set
	Public Sub Deplete(ByRef depletion As MaterialSet)
		ResM -= depletion.ResM
		ResC -= depletion.ResC
		ResU -= depletion.ResU
		ResA -= depletion.ResA
	End Sub

	' Add materials to this set
	Public Sub Add(ByRef addition As MaterialSet)
		ResM += addition.ResM
		ResC += addition.ResC
		ResU += addition.ResU
		ResA += addition.ResA
	End Sub
	Public Sub Add(ResM As Long, ResC As Long, ResU As Long, ResA As Long)
		Me.ResM += ResM
		Me.ResC += ResC
		Me.ResU += ResU
		Me.ResA += ResA
	End Sub

End Class

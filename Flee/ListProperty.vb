Public Class ListProperty

	Public name As String
	Public value As String

	Public Sub New(line As String)
		If line(0) = "\t" OrElse line(0) = " " Then
			Throw New Exception("Malformed property: " + line)
		End If
		Dim tuple() As String = line.Split("=")
		Me.name = tuple(0)
		Me.value = tuple(1)
	End Sub

End Class

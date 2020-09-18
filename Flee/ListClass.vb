Public Class ListClass

	Public type As String
	Public name As String
	Public properties As List(Of ListProperty) = New List(Of ListProperty)

	Public Sub New(header As String, lines As List(Of String))
		Dim header_parts() As String = header.Split(" ")
		Me.type = header_parts(0)
		Me.name = header_parts(1)
		For Each line As String In lines
			If line.Length() = 0 OrElse line(0) = "#" Then
				Continue For
			End If
			If Not line.Contains("=") Then
				Throw New Exception("Invalid property: " & line)
			End If
			properties.Add(New ListProperty(line))
		Next
	End Sub

End Class

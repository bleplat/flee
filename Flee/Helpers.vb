Imports System.Globalization
Imports System.IO

Public NotInheritable Class Helpers

	Public Shared ReadOnly INVALID_UID As ULong = ULong.MaxValue - 1
	Private Shared last_uid As ULong = 0
	Public Shared Function GetNextUniqueID() As ULong
		last_uid = last_uid + 1
		Return last_uid
	End Function

	Public Shared Function GetQA(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Double
		'Dim calc As Double = ((y2 - y1) / (x2 - x1))
		'Dim QA As Double = Math.Atan(calc) * 360
		Dim QA As Double = Math.Atan2(x1 - x2, y1 - y2)
		QA = QA * (180 / Math.PI)
		QA = QA - 180
		If QA < 0 Then QA = QA + 360
		If QA >= 360 Then QA = QA - 360
		Return QA
	End Function

	Public Shared Function Distance(ByRef p1 As PointF, ByRef p2 As PointF) As Double
		Return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y))
	End Function
	Public Shared Function Distance(ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single) As Double
		Return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))
	End Function
	Public Shared Function DistanceSQ(ByRef p1 As PointF, ByRef p2 As PointF) As Double
		Return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y)
	End Function
	Public Shared Function DistanceSQ(ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single) As Double
		Return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)
	End Function

	Public Shared Function GetNewPoint(ByVal AncPoint As PointF, ByVal Dir As Single, ByVal speed As Single) As PointF
		Dim Tox As Single = Math.Sin(2 * Math.PI * Dir / 360) * speed + AncPoint.X
		Dim Toy As Single = Math.Cos(2 * Math.PI * Dir / 360) * speed + AncPoint.Y
		AncPoint.X = Tox
		AncPoint.Y = Toy
		Return AncPoint
	End Function

	Public Shared Function Modulo(a As Double, n As Double) As Double
		Return (a Mod n + n) Mod n
	End Function

	Public Shared Function GetAngleDiff(a1 As Double, a2 As Double) As Double
		Return Math.Abs(Modulo((a2 - a1 + 180), 360) - 180)
	End Function

	Private Shared bitmaps As New Dictionary(Of String, Bitmap)
	Public Shared Function GetSprite(ByVal img_name As String, ByVal x As Integer, ByVal y As Integer, Optional ByVal Scolor As Color = Nothing) As Bitmap
		Dim bmp As Bitmap = Nothing
		Dim full_img_name As String
		If Scolor = Nothing Then
			full_img_name = img_name & x & y
		Else
			full_img_name = img_name & x & y & Scolor.ToString()
		End If
		' Find already loaded image
		If bitmaps.TryGetValue(full_img_name, bmp) Then
			Return (bmp)
		End If
		' Image is not in cache, get from file
		' Non sprited image
		If x = -1 AndAlso y = -1 Then
			' Base image requested
			Dim file_name As String = "./sprites/" & img_name & ".bmp"
			bmp = New Bitmap(Image.FromFile(file_name)) ' My.Resources.ResourceManager.GetObject(img_name, My.Resources.Culture)
		Else
			' Get base image
			bmp = GetSprite(img_name, -1, -1, Nothing)
			' New colored image from non-colored image
			Dim ItW As Integer = bmp.Width / 8
			bmp = bmp.Clone(New Rectangle(New Point(x * ItW + 1, y * ItW + 1), New Size(ItW - 2, ItW - 2)), Imaging.PixelFormat.DontCare)
			' Coloring
			If Scolor <> Nothing Then
				Dim SSC As Color = Scolor
				Dim SRed, SGreen, SBlue As Integer : Dim red, green, blue As Integer
				SRed = SSC.R
				SGreen = SSC.G
				SBlue = SSC.B
				With bmp
					For i As Integer = 0 To .Width - 1
						For j As Integer = 0 To .Height - 1
							red = Int(.GetPixel(i, j).R)
							green = Int(.GetPixel(i, j).G)
							blue = Int(.GetPixel(i, j).B)
							If red = green AndAlso green = blue Then
								.SetPixel(i, j, Color.FromArgb((red * SRed) / 255, (green * SGreen) / 255, (blue * SBlue) / 255))
							End If
						Next
					Next
				End With
			End If
		End If
		' Final processing
		bmp.MakeTransparent(Color.Black)
		' Caching
		bitmaps.Add(full_img_name, bmp)
		' Returning
		Return bmp
	End Function

	Public Shared Function getSColor(ByVal Scolor As String) As Color
		Return Color.FromName(Scolor)
	End Function

	Public Shared Function GetRect(ByRef PT1 As Point, ByRef PT2 As Point) As Rectangle
		Dim NR As New Rectangle
		If PT1.X < PT2.X Then
			NR.X = PT1.X
			NR.Width = PT2.X - PT1.X
		Else
			NR.X = PT2.X
			NR.Width = PT1.X - PT2.X
		End If
		If PT1.Y < PT2.Y Then
			NR.Y = PT1.Y
			NR.Height = PT2.Y - PT1.Y
		Else
			NR.Y = PT2.Y
			NR.Height = PT1.Y - PT2.Y
		End If
		Return NR
	End Function

	Public Shared Function ImproveColor(color As Color) As Color
		Dim R As Integer = color.R
		Dim G As Integer = color.G
		Dim B As Integer = color.B
		If R <= G AndAlso R <= B Then
			R -= 48
			G += 48
			B += 48
		End If
		If G <= R AndAlso G <= B Then
			R += 48
			G -= 48
			B += 48
		End If
		If B <= R AndAlso B <= G Then
			R += 48
			G += 48
			B -= 48
		End If
		R = Math.Min(255, Math.Max(0, R))
		G = Math.Min(255, Math.Max(0, G))
		B = Math.Min(255, Math.Max(0, B))
		Return Color.FromArgb(R, G, B)
	End Function

	Shared Function NormalizeAngleUnsigned(angle As Double) As Double
		While (angle < 0)
			angle += 360
		End While
		While (angle >= 360)
			angle -= 360
		End While
		Return angle
	End Function
	Shared Function GetAngle(x1 As Double, y1 As Double, x2 As Double, y2 As Double) As Integer
		If (x1 = x2 AndAlso y1 = y2) Then Return -1
		Dim QA As Double = Math.Atan2(x2 - x1, y2 - y1) * 180.0 / Math.PI
		QA = NormalizeAngleUnsigned(QA)
		Return QA
	End Function

	Shared Function PointFromString(input As String) As Point
		Dim components(2) As String
		components = input.Split(";")
		Return New Point(Convert.ToInt32(components(0)), Convert.ToInt32(components(1)))
	End Function

	Shared Function GetIndentation(line As String) As Integer
		Dim count As Integer = 0
		For Each c As Char In line
			If c = "\t" OrElse c = " " Then
				count += 1
			Else
				Exit For
			End If
		Next
		Return count
	End Function

	Public Shared Sub LoadLists()
		Dim list_classes As List(Of ListClass) = New List(Of ListClass)
		list_classes.AddRange(Helpers.GetListsFromFile("./lists/weapons.txt"))
		list_classes.AddRange(Helpers.GetListsFromFile("./lists/derelict_ships.txt"))
		list_classes.AddRange(Helpers.GetListsFromFile("./lists/static_ships.txt"))
		list_classes.AddRange(Helpers.GetListsFromFile("./lists/human_ships.txt"))
		list_classes.AddRange(Helpers.GetListsFromFile("./lists/alien_ships.txt"))
		list_classes.AddRange(Helpers.GetListsFromFile("./lists/boss_ships.txt"))
		Helpers.LoadLists(list_classes)
	End Sub

	Public Shared Sub LoadLists(list_classes As List(Of ListClass))
		For Each a_class As ListClass In list_classes
			Select Case a_class.type
				Case "gun"
					If Not GunStats.classes.ContainsKey(a_class.name) Then
						GunStats.classes(a_class.name) = New GunStats(a_class.name)
					End If
					For Each prop As ListProperty In a_class.properties
						GunStats.classes(a_class.name).SetProperty(prop.name, prop.value)
					Next
				Case "ship"
					If Not ShipStats.classes.ContainsKey(a_class.name) Then
						ShipStats.classes(a_class.name) = New ShipStats(a_class.name)
					End If
					For Each prop As ListProperty In a_class.properties
						ShipStats.classes(a_class.name).SetProperty(prop.name, prop.value)
					Next
				Case Else : Throw New Exception("Unknown class type: " & a_class.type)
			End Select
		Next
	End Sub

	Public Shared Function GetListsFromFile(filename As String) As List(Of ListClass)
		Return Helpers.GetListsFromRaw(File.ReadAllText(filename).Replace(vbCr & vbLf, vbLf))
	End Function
	Public Shared Function GetListsFromRaw(data As String) As List(Of ListClass)
		' use as LoadList(File.ReadAllText("./lists/weapons.txt").Replace(vbCr & vbLf, vbLf))
		data &= vbLf
		Dim list_classes As New List(Of ListClass)
		Dim lines() As String = data.Split(vbLf)
		Dim header As String = ""
		Dim paragraph As List(Of String) = New List(Of String)
		For Each line As String In lines ' reading lines one by one
			If header.Length() > 0 Then
				If line.Length() > 0 AndAlso (line(0) = vbTab OrElse line(0) = " ") Then
					paragraph.Add(line.Substring(1, line.Length() - 1))
				Else
					list_classes.Add(New ListClass(header, paragraph))
					header = ""
					paragraph.Clear()
				End If
			End If
			If header.Length() = 0 Then
				If line.Length() = 0 OrElse line(0) = "#" Then
					Continue For
				ElseIf line(0) = " " OrElse line(0) = vbTab Then
					Throw New Exception("Malformed line: " + line)
				Else
					header = line
				End If
			End If
		Next
		Return list_classes
	End Function

	'locale indpendent double conversions
	Public Shared Function ToDouble(s As String) As Double
		'Static culture As CultureInfo = CultureInfo.CreateSpecificCulture("en-US")
		Static format As NumberFormatInfo = New NumberFormatInfo() With {.NegativeSign = "-", .NumberDecimalSeparator = "."}
		Return Double.Parse(s.Replace(",", "."), format)
	End Function
	Public Shared Function ToString(d As Double) As String
		Static culture As CultureInfo = CultureInfo.CreateSpecificCulture("en-US")
		Return d.ToString("0.00", culture)
	End Function





	Public Shared Function RandomStationName(rand As Random) As String
		Dim station_names As List(Of String) = New List(Of String)
		For Each ship_class_name As String In ShipStats.classes.Keys
			If ship_class_name.Contains("Station") Then
				station_names.Add(ship_class_name)
			End If
		Next
		Return station_names(rand.Next(0, station_names.Count()))
	End Function
	Public Shared Function RandomTurretName(rand As Random) As String
		Select Case rand.Next(0, 3)
			Case 0 : Return "Outpost"
			Case 1 : Return "Defense"
			Case 2 : Return "Pointvortex"
		End Select
	End Function


	Public Shared Function GetSpawnUpgrades(ship As Ship) As List(Of String)
		Dim upgrades As List(Of String) = New List(Of String)
		For Each craft As String In ship.stats.crafts
			For Each a_up As Upgrade In Upgrade.Upgrades
				If a_up.Name = ("Build_" & craft) Then
					upgrades.Add(a_up.Name)
				End If
			Next
		Next
		Return upgrades
	End Function
	Public Shared Function GetRandomSpawnUpgrade(rand As Random, ship As Ship) As String
		Dim upgrades As List(Of String) = GetSpawnUpgrades(ship)
		If upgrades.Count() = 0 Then
			Return Nothing
		End If
		Return upgrades(rand.Next(0, upgrades.Count))
	End Function

End Class

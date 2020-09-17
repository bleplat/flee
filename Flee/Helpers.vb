Public NotInheritable Class Helpers

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

    Public Shared Function GetDistance(ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single) As Double
        Return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))
    End Function

    Public Shared Function GetNewPoint(ByVal AncPoint As PointF, ByVal Dir As Single, ByVal speed As Single) As PointF
        Dim Tox As Single = Math.Sin(2 * Math.PI * Dir / 360) * speed + AncPoint.X
        Dim Toy As Single = Math.Cos(2 * Math.PI * Dir / 360) * speed + AncPoint.Y
        AncPoint.X = Tox
        AncPoint.Y = Toy
        Return AncPoint
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
        If R <= G And R <= B Then
            R -= 48
            G += 48
            B += 48
        End If
        If G <= R And G <= B Then
            R += 48
            G -= 48
            B += 48
        End If
        If B <= R And B <= G Then
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
        If (x1 = x2 And y1 = y2) Then Return -1
        Dim QA As Double = Math.Atan2(x2 - x1, y2 - y1) * 180.0 / Math.PI
        QA = NormalizeAngleUnsigned(QA)
        Return QA
    End Function


End Class

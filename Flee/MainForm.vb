Imports System.Drawing.Drawing2D

Public Class MainForm
    Public Shared DebugMode As Boolean = False
    Public Const MAIN_BASE As String = "MainBase"

    Dim See As New Point(4700, 4700)

    '===' Stats '==='
    Public Shared play As Boolean = True
    Public Shared timelapse As Boolean = False
    Public Shared timelapsev2 As Boolean = False
    Public Shared target_identification As Boolean = False
    Public Shared help As Boolean = True
    Public Shared has_ascended As Boolean = False

    '===' Salutations ! '==='
    Const Pi As Single = 3.14159265
    Dim DrawBMP As New Bitmap(600, 600)
    Dim G As Graphics

    ' world
    Dim world As World = Nothing
    Dim player_team As Team = Nothing

    Private Sub MainForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        G = Graphics.FromImage(DrawBMP)
        G.CompositingQuality = CompositingQuality.HighSpeed
        G.InterpolationMode = InterpolationMode.Bicubic
        If My.Application.CommandLineArgs.Count > 1 Then
            SeedTextBox.Text = My.Application.CommandLineArgs(1)
        Else
            SeedTextBox.Text = (New Random().Next()).ToString()
        End If
        Upgrade.LoadRegUpgrades()
        Me.Width = Me.Width + 1
        My.Computer.Audio.Play("sounds/PhilippWeigl-SubdivisionOfTheMasses.wav", AudioPlayMode.BackgroundLoop)
    End Sub
    Private Sub BeginButton_Click(sender As Object, e As EventArgs) Handles StartPlayingButton.Click
        Dim Seed As Integer
        Try
            Seed = Convert.ToInt32(SeedTextBox.Text)
        Catch ex As Exception
            Return
        End Try

        StartPlayingButton.Enabled = False
        MenuPanel.Visible = False

        world = New World(Seed)
        player_team = world.Teams(0)
        Me.Text = "Flee - Seed: " & SeedTextBox.Text

        ' Place camera on player
        See = New Point(world.Ships(0).Coo.X - 200, world.Ships(0).Coo.Y - 200)

        Ticker.Enabled = True
    End Sub


    '===' Boucle '==='
    Private Sub Ticker_Tick(sender As System.Object, e As System.EventArgs) Handles Ticker.Tick
        Dim start_time As DateTime = DateTime.Now

        If play Then
            For i = 1 To IIf(timelapse, 8, 1) * IIf(timelapsev2, 16, 1)
                world.Tick()
            Next
        End If
        CheckRightPanel()
        KeysCheck()
        SelectCheck()
        drawUpgrades()
        DrawAll()
        'debugstr += vbNewLine + "DrawAll: " + (DateTime.Now - start_time).ToString() : start_time = DateTime.Now
    End Sub
    Sub KeysCheck()
        For Each AKey As String In KeyList
            Select Case AKey
                Case "Up"
                    See.Y = See.Y - 50
                    CheckSee()
                Case "Down"
                    See.Y = See.Y + 50
                    CheckSee()
                Case "Left"
                    See.X = See.X - 50
                    CheckSee()
                Case "Right"
                    See.X = See.X + 50
                    CheckSee()
            End Select
        Next
    End Sub
    Sub DrawAll()
        If world.NuclearEffect <= 0 Then
            G.Clear(Color.Black)
        Else
            G.Clear(Color.FromArgb(world.NuclearEffect, world.NuclearEffect, world.NuclearEffect))
            world.NuclearEffect -= 2
        End If
        MiniG.FillRectangle(New SolidBrush(Color.FromArgb(25, 0, 0, 0)), 0, 0, 200, 200)
        '===' Minimap '==='
        MiniG.DrawRectangle(Pens.White, New Rectangle(New Point(See.X / world.ArenaSize.Width * MiniBMP.Width, See.Y / world.ArenaSize.Height * MiniBMP.Height), New Size(DrawBMP.Width * MiniBMP.Width / world.ArenaSize.Width, DrawBMP.Height * MiniBMP.Height / world.ArenaSize.Height)))

        '===' Ships '==='
        For Each AShip As Ship In world.Ships
            ' Minimap '
            Dim W As Integer = AShip.W / 30
            If W < 2 Then W = 2
            If W > 5 Then W = 5
            Dim mini_color = AShip.Color
            If target_identification Then
                If AShip.Team Is Nothing Or AShip.Type = "Comet" Then
                    mini_color = AShip.Color
                ElseIf AShip.Team Is player_team Then
                    mini_color = Color.Lime
                ElseIf AShip.Team.IsFriendWith(player_team) Then
                    mini_color = Color.Cyan
                Else
                    mini_color = Color.Red
                End If
            End If
            MiniG.FillRectangle(New SolidBrush(mini_color), New Rectangle(AShip.Coo.X / world.ArenaSize.Width * MiniBMP.Width - (W / 2), AShip.Coo.Y / world.ArenaSize.Height * MiniBMP.Height - (W / 2), W, W))
            ' Main screen '
            If AShip.Coo.X + (AShip.W / 2) > See.X And AShip.Coo.X - (AShip.W / 2) < See.X + DrawBox.Width And AShip.Coo.Y + (AShip.W / 2) > See.Y And AShip.Coo.Y - (AShip.W / 2) < See.Y + DrawBox.Height Then
                Dim img As New Bitmap(Helpers.GetSprite(AShip.Type, AShip.fram, 0, mini_color)) 'image
                Dim center As New Point(AShip.Coo.X - See.X, AShip.Coo.Y - See.Y) 'centre
                Dim AddD As Integer = 0 : If AShip.Team Is Nothing Then AddD = world.ticks Mod 360
                Dim MonM As Matrix = New Matrix : MonM.RotateAt(-AShip.Direction + 180 + AddD, center) 'rotation
                G.Transform = MonM 'affectation
                G.DrawImage(img, New Point(center.X - img.Size.Width / 2, center.Y - img.Size.Width / 2)) 'dessin
                G.ResetTransform() 'reset
            End If
        Next

        '===' Shoots '==='
        For Each AShoot As Shoot In world.Shoots
            'If AShoot.Coo.X > See.X And AShoot.Coo.X < See.X + DrawBox.Width And AShoot.Coo.Y > See.Y And AShoot.Coo.Y < See.Y + DrawBox.Height Then
            Dim col As Color
            If AShoot.Team Is Nothing Then
                col = Color.White
            Else
                col = AShoot.Team.color
            End If
            Dim img As New Bitmap(Helpers.GetSprite(AShoot.Type, AShoot.fram, 0, col)) 'image
            Dim center As New Point(AShoot.Coo.X - See.X, AShoot.Coo.Y - See.Y) 'centre
            Dim MonM As Matrix = New Matrix : MonM.RotateAt(-AShoot.Direction + 180, center) 'rotation
            G.Transform = MonM 'affectation
            G.DrawImage(img, New Point(center.X - img.Size.Width / 2, center.Y - img.Size.Width / 2)) 'dessin
            G.ResetTransform() 'reset
            'End If
        Next
        '===' Effets '==='
        For Each AEffect As Effect In world.Effects
            If AEffect.Coo.X > See.X And AEffect.Coo.X < See.X + DrawBox.Width And AEffect.Coo.Y > See.Y And AEffect.Coo.Y < See.Y + DrawBox.Height Then
                Dim img As New Bitmap(Helpers.GetSprite(AEffect.Type, AEffect.fram, 0)) 'image
                Dim center As New Point(AEffect.Coo.X - See.X, AEffect.Coo.Y - See.Y) 'centre
                Dim MonM As Matrix = New Matrix : MonM.RotateAt(-AEffect.Direction + 180, center) 'rotation
                G.Transform = MonM 'affectation
                G.DrawImage(img, New Point(center.X - img.Size.Width / 2, center.Y - img.Size.Width / 2)) 'dessin
                G.ResetTransform() 'reset
            End If
        Next
        '===' Séléction '==='
        If SelectStarted Then
            Dim NR As Rectangle = Helpers.GetRect(SelectPTN1, SelectPTN2)
            NR.X = NR.X - See.X
            NR.Y = NR.Y - See.Y
            G.DrawRectangle(Pens.White, NR)
        End If
        '===' Ships Special '==='
        For Each AShip As Ship In world.Ships
            If AShip.Coo.X + (AShip.W / 2) > See.X And AShip.Coo.X - (AShip.W / 2) < See.X + DrawBox.Width And AShip.Coo.Y + (AShip.W / 2) > See.Y And AShip.Coo.Y - (AShip.W / 2) < See.Y + DrawBox.Height Then
                If Not AShip.Team Is Nothing AndAlso AShip.Behavior <> "Drift" And AShip.Type <> "MSL" Then
                    ' Selection '
                    Dim drawrect As Rectangle = New Rectangle(New Point(AShip.Coo.X - AShip.W / 2 - See.X, AShip.Coo.Y - AShip.W / 2 - See.Y), New Size(AShip.W, AShip.W)) 'zone de dessin
                    If False AndAlso target_identification Then ' disabled
                        If AShip.Team.id = 0 Then
                            G.DrawRectangle(Pens.Lime, drawrect)
                        ElseIf AShip.Team.IsFriendWith(player_team) Then
                            G.DrawRectangle(Pens.Blue, drawrect)
                        Else
                            G.DrawRectangle(Pens.Red, drawrect)
                        End If
                    End If
                    If AShip.selected Then
                        G.DrawRectangle(New Pen(AShip.Team.color), drawrect)
                    End If
                    ' shields
                    If AShip.ShieldMax >= 1 Then
                        Dim shields_ptns(16 - 1) As PointF
                        Dim shields_colors(16 - 1) As Color
                        For i = 0 To shields_ptns.Length - 1
                            shields_ptns(i) = Helpers.GetNewPoint(New PointF(drawrect.X + drawrect.Width / 2, drawrect.Y + drawrect.Height / 2), i * 360 / 16 + AShip.Direction, drawrect.Width / 2 + 5)
                            Dim c_charge = AShip.Shield * 255 / AShip.ShieldMax
                            Dim c_nocharge = 255 - c_charge
                            Dim c_speed = (AShip.ShieldReg - 10) * 255 / 30
                            Dim c_op = AShip.ShieldOp * 255 / 100
                            Dim c_max = AShip.ShieldMax
                            Dim c_i = AShip.ShieldPoints(i)
                            Dim c_p = Math.Max(1, AShip.ShieldPoints(i))
                            shields_colors(i) = Color.FromArgb(c_i, Math.Min(255, Math.Max(0, (c_op * c_i + c_nocharge * (255 - c_i)) / c_p)), Math.Min(255, Math.Max(0, (c_charge * c_i + c_max * (255 - c_i)) / c_p)), Math.Min(255, Math.Max(0, (c_speed * c_i + c_charge * (255 - c_i)) / c_p)))
                        Next
                        Dim shieldsbrush As PathGradientBrush = New PathGradientBrush(shields_ptns)
                        shieldsbrush.SurroundColors = shields_colors
                        shieldsbrush.CenterColor = Color.FromArgb(0, 0, 0, 0)
                        G.FillEllipse(shieldsbrush, drawrect)
                    End If
                    'life   'New Pen(getSColor(AShip.Color))
                    If AShip.LifeMax > 20 Then
                        G.DrawRectangle(Pens.DimGray, New Rectangle(New Point(AShip.Coo.X - AShip.W / 2 - See.X, AShip.Coo.Y + AShip.W / 2 + 5 - See.Y), New Size(AShip.W, 1)))
                        G.DrawRectangle(New Pen(AShip.Team.color), New Rectangle(New Point(AShip.Coo.X - AShip.W / 2 - See.X, AShip.Coo.Y + AShip.W / 2 + 5 - See.Y), New Size(AShip.Life / AShip.LifeMax * AShip.W, 1)))
                        G.DrawString(AShip.Life & "/" & AShip.LifeMax, Me.Font, New SolidBrush(AShip.Team.color), New Point(AShip.Coo.X - AShip.W / 2 - See.X, AShip.Coo.Y + AShip.W / 2 + 7 - See.Y))
                        If AShip.DeflectorCountMax > 0 Then
                            If AShip.DeflectorCount = AShip.DeflectorCountMax Then
                                G.DrawString(AShip.DeflectorCount & "/" & AShip.DeflectorCountMax, Me.Font, New SolidBrush(Color.Gray), New Point(AShip.Coo.X - AShip.W / 2 - See.X, AShip.Coo.Y + AShip.W / 2 + 7 + 7 - See.Y))
                            Else
                                G.DrawString(AShip.DeflectorCount & "/" & AShip.DeflectorCountMax & " <- " & AShip.DeflectorCooldown, Me.Font, New SolidBrush(Color.Gray), New Point(AShip.Coo.X - AShip.W / 2 - See.X, AShip.Coo.Y + AShip.W / 2 + 7 + 7 - See.Y))
                            End If
                        End If
                    End If
                End If
            End If
        Next
        '===' Infos '==='
        G.DrawString("Ships : " & world.CountTeamShips(player_team) & " / " & player_team.MaxShips & " Max.", New Font("Consolas", 10), Brushes.Lime, New Point(0, 0))
        If Not play Then
            G.DrawString("PAUSE", New Font("Consolas", 16), Brushes.White, New Point(0, DrawBMP.Height - 32))
        ElseIf timelapse Then
            G.DrawString("TIMELAPSE", New Font("Consolas", 16), Brushes.White, New Point(0, DrawBMP.Height - 32))
        End If
        '===' Help '==='
        If help Then
            Dim help_str As String = ""
            If Not has_ascended Then
                help_str += "CONTROLS:" & vbNewLine
                help_str += "Arrows               - move camera" & vbNewLine
                help_str += "Left Click           - select unit" & vbNewLine
                help_str += "Right Click          - (for selected units) move, follow or attack" & vbNewLine
                help_str += "Double Right Click   - (for selected units) mine there" & vbNewLine
                help_str += "Right Click (on you) - (for unit you click on, if selected) ignore asteroids" & vbNewLine
                help_str += "Space                - pause" & vbNewLine
                help_str += "M                    - timelapse" & vbNewLine
                help_str += "I                    - combat mode (show all enemies in red)" & vbNewLine
                help_str += "H                    - show/hide this help" & vbNewLine
                help_str += vbNewLine
                help_str += "The galaxy went into chaos. Find a way to escape." & vbNewLine
            Else
                help_str += "Congratulations!" & vbNewLine
                help_str += "Your people just accessed another level of existence." & vbNewLine
                help_str += "Your ships remain under your control, but your mind is now immortal." & vbNewLine
                help_str += "You can keep fighting, but your goal have been reached." & vbNewLine
                help_str += vbNewLine
                help_str += "You won." & vbNewLine
            End If
            G.DrawString(help_str, New Font("Consolas", 8), Brushes.Gray, New Point(0, DrawBMP.Height - 256))
        End If

        MiniBox.Image = MiniBMP
        DrawBox.Image = DrawBMP
    End Sub





    '===' Mini-Map '==='
    Dim MiniBMP As New Bitmap(200, 200)
    Dim MiniG As Graphics = Graphics.FromImage(MiniBMP)
    Dim MiniMDown As Boolean = False
    Private Sub MiniBox_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles MiniBox.MouseDown
        MiniMDown = True
        See.X = (e.X / sender.width * world.ArenaSize.Width) - DrawBMP.Width / 2
        See.Y = (e.Y / sender.height * world.ArenaSize.Height) - DrawBMP.Height / 2
        CheckSee()
    End Sub
    Private Sub MiniBox_MouseUp(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles MiniBox.MouseUp
        MiniMDown = False
    End Sub
    Private Sub MiniBox_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles MiniBox.MouseMove
        If MiniMDown Then
            See.X = (e.X / sender.width * world.ArenaSize.Width) - DrawBMP.Width / 2
            See.Y = (e.Y / sender.height * world.ArenaSize.Height) - DrawBMP.Height / 2
            CheckSee()
        End If
    End Sub
    Sub CheckSee()
        If See.X < 0 Then
            See.X = 0
        End If
        If See.Y < 0 Then
            See.Y = 0
        End If
        If See.X > world.ArenaSize.Width - DrawBMP.Width Then
            See.X = world.ArenaSize.Width - DrawBMP.Width
        End If
        If See.Y > world.ArenaSize.Height - DrawBMP.Height Then
            See.Y = world.ArenaSize.Height - DrawBMP.Height
        End If
    End Sub


    '===' Touches '==='
    Public KeyList As New List(Of String)
    Private Sub MainForm_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.I Then
            target_identification = Not target_identification
        End If
        If e.KeyData = Keys.H Then
            help = Not help
        End If
        If e.KeyData = Keys.Space Then
            If play Then
                play = False
            Else
                play = True
            End If
        End If
        If e.KeyData = Keys.M Then
            timelapse = Not timelapse
        End If
        If e.KeyData = Keys.P Then
            timelapsev2 = Not timelapsev2
        End If
        If e.KeyData = Keys.F12 Then
            If DebugMode Then
                DebugMode = False
            Else
                DebugMode = True
            End If
        End If
        If Not KeyList.Contains(e.KeyData.ToString) Then
            KeyList.Add(e.KeyData.ToString)
        End If
    End Sub
    Private Sub MainForm_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        KeyList.Remove(e.KeyData.ToString)
    End Sub


    '===' Clics '==='
    Private Sub DrawBox_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles DrawBox.MouseDown
        SelectPTN2 = New Point((e.X * 600 / DrawBox.Width) + See.X, (e.Y * 600 / DrawBox.Height) + See.Y)
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                SelectStarted = True
                SelectPTN1 = New Point((e.X * 600 / DrawBox.Width) + See.X, (e.Y * 600 / DrawBox.Height) + See.Y)
            Case Windows.Forms.MouseButtons.Middle

            Case Windows.Forms.MouseButtons.Right

        End Select
    End Sub
    Private Sub DrawBox_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles DrawBox.MouseMove
        SelectPTN2 = New Point((e.X * 600 / DrawBox.Width) + See.X, (e.Y * 600 / DrawBox.Height) + See.Y)
    End Sub
    Private Sub DrawBox_MouseUp(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles DrawBox.MouseUp
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                SelectStarted = False
                SelectPTN2 = New Point((e.X * 600 / DrawBox.Width) + See.X, (e.Y * 600 / DrawBox.Height) + See.Y)
                SelectInSquare()
            Case Windows.Forms.MouseButtons.Middle

            Case Windows.Forms.MouseButtons.Right
                SelectOrder()
        End Select
    End Sub
    Dim SelectCount As Integer = 0
    Dim SelectPTN1 As New Point(0, 0)
    Dim SelectPTN2 As New Point(0, 0)
    Dim SelectStarted As Boolean = False
    Sub SelectCheck()
        SelectCount = 0
        For Each AShip As Ship In world.Ships
            If AShip.selected Then
                SelectCount = SelectCount + 1
            End If
        Next
    End Sub
    Public Sub SelectInSquare()
        Dim SS As Rectangle = Helpers.GetRect(SelectPTN1, SelectPTN2)
        For Each aship As Ship In world.Ships
            aship.selected = False
            If aship.Team Is player_team Or DebugMode Then 'Si mode debug ou equipe correcte
                If aship.Coo.X + aship.W / 2 > SS.X Then
                    If aship.Coo.X - aship.W / 2 < SS.X + SS.Width Then
                        If aship.Coo.Y + aship.W / 2 > SS.Y Then
                            If aship.Coo.Y - aship.W / 2 < SS.Y + SS.Height Then
                                aship.selected = True
                                LastSShipSelect = aship.UID
                            End If
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    Public Sub SelectOrder()
        If SelectCount = 0 Then
            Exit Sub
        End If
        Dim oUID As String = ""
        '===' Recherche '==='
        For Each AShip As Ship In world.Ships
            If AShip.Coo.X + AShip.W / 2 > SelectPTN2.X Then
                If AShip.Coo.X - AShip.W / 2 < SelectPTN2.X Then
                    If AShip.Coo.Y + AShip.W / 2 > SelectPTN2.Y Then
                        If AShip.Coo.Y - AShip.W / 2 < SelectPTN2.Y Then
                            oUID = AShip.UID
                        End If
                    End If
                End If
            End If
        Next
        '===' Validation '==='
        If oUID = "" Then
            For Each AShip As Ship In world.Ships
                If AShip.selected Then
                    If AShip.TargetPTN = SelectPTN2 Then
                        world.Effects.Add(New Effect With {.Type = "Patrouille", .Coo = SelectPTN2})
                        AShip.Behavior = "Mine"
                        AShip.TargetPTN = SelectPTN2
                        AShip.Target = ""
                    Else
                        world.Effects.Add(New Effect With {.Type = "Fleche", .Coo = SelectPTN2})
                        AShip.Behavior = "Goto"
                        AShip.TargetPTN = SelectPTN2
                        AShip.Target = AShip.UID
                    End If
                End If
            Next
        Else
            For Each AShip As Ship In world.Ships
                If AShip.selected Then
                    If AShip.UID = oUID Then
                        world.Effects.Add(New Effect With {.Type = "Cible2", .Coo = SelectPTN2})
                        AShip.AllowMining = False
                    Else
                        world.Effects.Add(New Effect With {.Type = "Cible", .Coo = SelectPTN2})
                        AShip.Behavior = "Fight"
                        AShip.Target = oUID
                    End If
                End If
            Next
        End If
    End Sub
    Private Sub PictureBox2_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox2.Click, PictureBox5.Click, PictureBox6.Click
        If DebugMode Then
            player_team.resources = New MaterialSet(999999, 999, 9999, 999999)
        End If
    End Sub

    Private Sub MainForm_Resize(sender As System.Object, e As System.EventArgs) Handles MyBase.Resize
        DrawBox.Width = DrawBox.Height
        PanelRes.Left = DrawBox.Width + DrawBox.Left
        SShipPanel.Left = DrawBox.Width + DrawBox.Left
        MiniBox.Left = DrawBox.Width + DrawBox.Left
    End Sub


    Private Sub RandomizeButton_Click(sender As Object, e As EventArgs) Handles RandomizeButton.Click
        SeedTextBox.Text = (New Random()).Next().ToString()
    End Sub






    '===' UPGRADES '==='

    Public ListedUps As New List(Of Upgrade)

    '===' SHIP PANEL & Cie '==='

    Public LastSShipSelect As String = ""
    Public LastShipPaneload As String = ""
    Sub CheckRightPanel()
        MetalTextBox.Text = player_team.resources.ResM
        CristalTextBox.Text = player_team.resources.ResC
        UraniumTextBox.Text = player_team.resources.ResU
        AntimatterTextBox.Text = player_team.resources.ResA
        If SelectCount = 1 Then
            SelectSShip(LastSShipSelect)
        Else
            SelectSShip(Nothing)
        End If
    End Sub
    Sub SelectSShip(ByVal UID As String)
        If UID Is Nothing Then
            SShipPanel.Visible = False
            Exit Sub
        End If
        Dim AShip As Ship = world.GetShipByUID(UID)
        If AShip Is Nothing Then
            SShipPanel.Visible = False
            Exit Sub
        End If
        If SShipPanel.Visible = False Then
            SShipPanel.Visible = True
        End If
        LastShipPaneload = UID

        '===' Afficher infos '==='
        SShipImageBox.Image = Helpers.GetSprite(AShip.Type, 0, 0, AShip.Color)
        SShipTypeBox.Text = AShip.Type
        SShipUpsMax.Text = AShip.Ups.Count & " / " & AShip.UpsMax
        AllowMiningBox.Visible = Not AShip.AllowMining

        '===' Upgrades '==='
        If True Then
            Try
                ListedUps.Clear()
                For Each AUp As Upgrade In Upgrade.Upgrades
                    Dim ok As Boolean = True
                    Dim Spliter() As String = AUp.Need.Split(" ")
                    If Not AShip.HaveUp(AUp.Name) Then
                        For Each ac As String In Spliter
                            If AShip.InterUpgrade(ac, False) = False And (AShip.Upgrading Is Nothing OrElse AUp.Name <> AShip.Upgrading.Name) Then
                                ok = False
                            End If
                        Next
                    End If
                    If ok Then
                        ListedUps.Add(AUp)
                    End If
                Next
            Catch
            End Try
        End If
    End Sub

    Dim UpX As Integer = -1 : Dim UpY As Integer = -1
    Private Sub UpgradesBox_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles UpgradesBox.MouseMove
        UpX = e.X \ 25
        UpY = e.Y \ 25
        UpgradeDetails.Top = UpgradesBox.Location.Y + e.Y
    End Sub
    Dim PBMP As New Bitmap(200, 400)
    Dim PG As Graphics = Graphics.FromImage(PBMP)
    Sub drawUpgrades()
        Dim Aship As Ship = world.GetShipByUID(LastShipPaneload)
        If SShipPanel.Visible = False Or Aship Is Nothing Then
            Exit Sub
        End If
        PG.Clear(Color.Black)
        Dim x As Integer = 0 : Dim y As Integer = 0
        Dim udV As Boolean = False
        For Each AUp As Upgrade In ListedUps
            If x = UpX And y = UpY Then
                PG.FillRectangle(Brushes.DimGray, x * 25, y * 25, 25, 25)
                udV = True
                UpName.Text = AUp.Name
                UpDesc.Text = AUp.Desc
                ' prices
                PriceC.Text = AUp.cost.ResC
                PriceM.Text = AUp.cost.ResM
                PriceU.Text = AUp.cost.ResU
                PriceA.Text = AUp.cost.ResA
                ' invisible resources
                PriceM.Visible = (AUp.cost.ResM <> 0)
                PriceMIcon.Visible = (AUp.cost.ResM <> 0)
                PriceC.Visible = (AUp.cost.ResC <> 0)
                PriceCIcon.Visible = (AUp.cost.ResC <> 0)
                PriceU.Visible = (AUp.cost.ResU <> 0)
                PriceUIcon.Visible = (AUp.cost.ResU <> 0)
                PriceA.Visible = (AUp.cost.ResA <> 0)
                PriceAIcon.Visible = (AUp.cost.ResA <> 0)
            End If
            If Aship.HaveUp(AUp.Name) Then
                PG.DrawRectangle(New Pen(Brushes.White, 2), x * 25 + 1, y * 25 + 1, 24 - 1, 24 - 1)
            ElseIf (Not Aship.Upgrading Is Nothing) AndAlso Aship.Upgrading.Name = AUp.Name Then
                PG.DrawRectangle(New Pen(Brushes.Yellow, 2), x * 25, y * 25, 24, 24)
                Dim ph As Integer = Aship.UpProgress / AUp.Time * 25
                PG.FillRectangle(Brushes.White, x * 25, y * 25 + 25 - ph, 25, ph)
            ElseIf Aship.Ups.Count >= Aship.UpsMax And AUp.Install Then
                PG.DrawRectangle(Pens.DarkBlue, x * 25, y * 25, 24, 24)
            ElseIf Aship.Team Is Nothing OrElse Not Aship.Team.resources.HasEnough(AUp.cost) Then
                PG.DrawRectangle(Pens.DarkRed, x * 25, y * 25, 24, 24)
            ElseIf Not AUp.Install Then
                PG.DrawRectangle(Pens.Cyan, x * 25, y * 25, 24, 24)
            Else
                PG.DrawRectangle(Pens.DarkGreen, x * 25, y * 25, 24, 24)
            End If
            PG.DrawImage(Helpers.GetSprite(AUp.File, AUp.PTN.X, AUp.PTN.Y), New Rectangle(New Point(x * 25, y * 25), New Size(25, 25)))

            'item suivant
            x = x + 1
            If x >= 8 Then
                x = 0
                y = y + 1
            End If
        Next
        'infos
        If udV Then
            UpgradeDetails.Visible = True
        Else
            UpgradeDetails.Visible = False
        End If
        UpgradesBox.Image = PBMP
    End Sub
    Private Sub UpgradesBox_Click(sender As System.Object, e As System.EventArgs) Handles UpgradesBox.Click
        Dim AShip As Ship = world.GetShipByUID(LastShipPaneload)
        Dim x As Integer = 0 : Dim y As Integer = 0
        For Each AUp As Upgrade In ListedUps
            If x = UpX And y = UpY Then
                If (Not AShip.Upgrading Is Nothing) Or (AShip.Ups.Count >= AShip.UpsMax And AUp.Install) Then
                    Exit Sub
                End If
                If AShip.Team Is Nothing OrElse AShip.Team.resources.HasEnough(AUp.cost) Then
                    If Not AShip.HaveUp(AUp.Name) Then
                        '===' achat '==='
                        If Not AShip.Team Is Nothing Then AShip.Team.resources.Deplete(AUp.cost)
                        AShip.Upgrading = AUp
                        Exit Sub
                    End If
                End If
            End If
            'item suivant
            x = x + 1
            If x >= 8 Then
                x = 0
                y = y + 1
            End If
        Next

    End Sub
    Private Sub UpgradesBox_MouseLeave(sender As System.Object, e As System.EventArgs) Handles UpgradesBox.MouseLeave
        UpgradeDetails.Visible = False
        UpX = -1
        UpY = -1
    End Sub
End Class

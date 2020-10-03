Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Threading

Public Class MainForm

	Enum PlayState
		Paused
		Playing
		Timelapse
	End Enum


	Public Shared cheats_enabled As Boolean = False
	Public Const MAIN_BASE As ULong = ULong.MaxValue

	Dim See As New Point(4700, 4700)

	'===' Stats '==='
	Public Shared play_state As PlayState = PlayState.Playing
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
	Public player_team As Team = Nothing

	Private Sub MainForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		' culture
		'Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
		' Commandline
		If My.Application.CommandLineArgs.Count > 1 Then
			SeedTextBox.Text = My.Application.CommandLineArgs(1)
		Else
			SeedTextBox.Text = (New Random().Next()).ToString()
		End If
		' Graphics settings
		G = Graphics.FromImage(DrawBMP)
		G.CompositingQuality = CompositingQuality.HighSpeed
		G.InterpolationMode = InterpolationMode.Bicubic
		Me.Width = Me.Width + 1
		' Play the music if it's available
		Try
			My.Computer.Audio.Play("sounds/PhilippWeigl-SubdivisionOfTheMasses.wav", AudioPlayMode.BackgroundLoop)
		Catch ex As Exception
			Me.Text = "Flee - Music was not found AndAlso therefore not loaded!"
		End Try
		' Load lists
		Upgrade.LoadRegUpgrades()
		Helpers.LoadLists()
		Upgrade.LoadBuildUpgrades()

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
		See = New Point(world.Ships(0).location.X - 200, world.Ships(0).location.Y - 200)

		Ticker.Enabled = True
	End Sub


	'===' Boucle '==='
	Private Sub Ticker_Tick(sender As System.Object, e As System.EventArgs) Handles Ticker.Tick
		Dim start_time As DateTime = DateTime.Now

		If play_state <> PlayState.Paused Then
			For i = 1 To IIf(play_state = PlayState.Timelapse, 8, 1) * IIf(timelapsev2, 16, 1)
				world.Tick()
			Next
		End If
		CheckRightPanel()
		KeysCheck()
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
			Dim W As Integer = AShip.stats.width / 30
			If W < 2 Then W = 2
			If W > 5 Then W = 5
			Dim mini_color = AShip.color
			If target_identification Then
				If AShip.team Is Nothing OrElse AShip.stats.sprite = "Comet" Then
					mini_color = AShip.color
				ElseIf AShip.team Is player_team Then
					mini_color = Color.Lime
				ElseIf AShip.team.IsFriendWith(player_team) Then
					mini_color = Color.Cyan
				Else
					mini_color = Color.Red
				End If
			End If
			MiniG.FillRectangle(New SolidBrush(mini_color), New Rectangle(AShip.location.X / world.ArenaSize.Width * MiniBMP.Width - (W / 2), AShip.location.Y / world.ArenaSize.Height * MiniBMP.Height - (W / 2), W, W))
			' Main screen '
			If AShip.location.X + (AShip.stats.width / 2) > See.X AndAlso AShip.location.X - (AShip.stats.width / 2) < See.X + DrawBox.Width AndAlso AShip.location.Y + (AShip.stats.width / 2) > See.Y AndAlso AShip.location.Y - (AShip.stats.width / 2) < See.Y + DrawBox.Height Then
				Dim img As New Bitmap(Helpers.GetSprite(AShip.stats.sprite, AShip.fram, 0, mini_color)) 'image
				Dim center As New Point(AShip.location.X - See.X, AShip.location.Y - See.Y) 'centre
				Dim AddD As Integer = 0 : If AShip.team Is Nothing AndAlso AShip.stats.turn = 0 Then AddD = world.ticks Mod 360
				Dim MonM As Matrix = New Matrix : MonM.RotateAt(-AShip.direction + 180 + AddD, center) 'rotation
				G.Transform = MonM 'affectation
				G.DrawImage(img, New Point(center.X - img.Size.Width / 2, center.Y - img.Size.Width / 2)) 'dessin
				G.ResetTransform() 'reset
			End If
		Next

		'===' Shoots '==='
		For Each AShoot As Shoot In world.Shoots
			'If AShoot.Coo.X > See.X AndAlso AShoot.Coo.X < See.X + DrawBox.Width AndAlso AShoot.Coo.Y > See.Y AndAlso AShoot.Coo.Y < See.Y + DrawBox.Height Then
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
			If AEffect.Coo.X > See.X AndAlso AEffect.Coo.X < See.X + DrawBox.Width AndAlso AEffect.Coo.Y > See.Y AndAlso AEffect.Coo.Y < See.Y + DrawBox.Height Then
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
			If AShip.location.X + (AShip.stats.width / 2) > See.X AndAlso AShip.location.X - (AShip.stats.width / 2) < See.X + DrawBox.Width AndAlso AShip.location.Y + (AShip.stats.width / 2) > See.Y AndAlso AShip.location.Y - (AShip.stats.width / 2) < See.Y + DrawBox.Height Then
				If Not AShip.team Is Nothing AndAlso AShip.behavior <> Ship.BehaviorMode.Drift AndAlso AShip.stats.sprite <> "MSL" Then
					' Selection '
					Dim drawrect As Rectangle = New Rectangle(New Point(AShip.location.X - AShip.stats.width / 2 - See.X, AShip.location.Y - AShip.stats.width / 2 - See.Y), New Size(AShip.stats.width, AShip.stats.width)) 'zone de dessin
					If False AndAlso target_identification Then ' disabled
						If AShip.team.id = 0 Then
							G.DrawRectangle(Pens.Lime, drawrect)
						ElseIf AShip.team.IsFriendWith(player_team) Then
							G.DrawRectangle(Pens.Blue, drawrect)
						Else
							G.DrawRectangle(Pens.Red, drawrect)
						End If
					End If
					If selected_ships.Contains(AShip) Then
						G.DrawRectangle(New Pen(AShip.team.color), drawrect)
					End If
					' shields
					If AShip.stats.shield >= 1 Then
						Dim shields_ptns(16 - 1) As PointF
						Dim shields_colors(16 - 1) As Color
						For i = 0 To shields_ptns.Length - 1
							shields_ptns(i) = Helpers.GetNewPoint(New PointF(drawrect.X + drawrect.Width / 2, drawrect.Y + drawrect.Height / 2), i * 360 / 16 + AShip.direction, drawrect.Width / 2 + 5)
							'Dim c_charge = AShip.shield * 255 / AShip.stats.shield
							'Dim c_nocharge = 255 - c_charge
							'Dim c_speed = (AShip.stats.shield_regeneration - 10) * 255 / 30
							'Dim c_op = AShip.stats.shield_opacity * 255 / 100
							'Dim c_max = AShip.stats.shield
							'Dim c_i = AShip.ShieldPoints(i)
							'Dim c_p = Math.Max(1, AShip.ShieldPoints(i))
							'shields_colors(i) = Color.FromArgb(c_i, Math.Min(255, Math.Max(0, (c_op * c_i + c_nocharge * (255 - c_i)) / c_p)), Math.Min(255, Math.Max(0, (c_charge * c_i + c_max * (255 - c_i)) / c_p)), Math.Min(255, Math.Max(0, (c_speed * c_i + c_charge * (255 - c_i)) / c_p)))

							Dim f_alpha As Double = AShip.ShieldPoints(i) / 256.0
							Dim f_red_0 As Double = 1.0 - (AShip.shield / AShip.stats.shield) / 2.0
							Dim f_red_1 As Double = 1.0 - (AShip.shield / AShip.stats.shield)
							Dim f_green_0 As Double = (AShip.stats.shield_opacity - 25) / 75.0 + AShip.stats.shield_regeneration / 40.0
							Dim f_green_1 As Double = f_green_0 * Math.Sqrt(Math.Max(0, AShip.shield / AShip.stats.shield))
							Dim f_blue_0 As Double = (Math.Sqrt(AShip.stats.shield) * 20.0) / 400.0
							Dim f_blue_1 As Double = f_blue_0 * (AShip.shield / AShip.stats.shield)
							shields_colors(i) = Color.FromArgb(Math.Min(255, f_alpha * 255 * 2), Math.Min(255, Math.Max(0, (1.0 - f_alpha) * f_red_0 * 256 + (f_alpha) * f_red_1 * 256)), Math.Min(255, Math.Max(0, (1.0 - f_alpha) * f_green_0 * 256 + (f_alpha) * f_green_1 * 256)), Math.Min(255, Math.Max(0, (1.0 - f_alpha) * f_blue_0 * 256 + (f_alpha) * f_blue_1 * 256)))
						Next
						Dim shieldsbrush As PathGradientBrush = New PathGradientBrush(shields_ptns)
						shieldsbrush.SurroundColors = shields_colors
						shieldsbrush.CenterColor = Color.FromArgb(0, 0, 0, 0)
						G.FillEllipse(shieldsbrush, drawrect)
					End If
					'life   'New Pen(getSColor(AShip.Color))
					If AShip.stats.integrity > 20 Then
						G.DrawRectangle(Pens.DimGray, New Rectangle(New Point(AShip.location.X - AShip.stats.width / 2 - See.X, AShip.location.Y + AShip.stats.width / 2 + 5 - See.Y), New Size(AShip.stats.width, 1)))
						G.DrawRectangle(New Pen(AShip.team.color), New Rectangle(New Point(AShip.location.X - AShip.stats.width / 2 - See.X, AShip.location.Y + AShip.stats.width / 2 + 5 - See.Y), New Size(AShip.integrity / AShip.stats.integrity * AShip.stats.width, 1)))
						G.DrawString(AShip.integrity & "/" & AShip.stats.integrity, Me.Font, New SolidBrush(AShip.team.color), New Point(AShip.location.X - AShip.stats.width / 2 - See.X, AShip.location.Y + AShip.stats.width / 2 + 7 - See.Y))
						If AShip.stats.deflectors > 0 Then
							If AShip.deflectors_loaded = AShip.stats.deflectors Then
								G.DrawString(AShip.deflectors_loaded & "/" & AShip.stats.deflectors, Me.Font, New SolidBrush(Color.Gray), New Point(AShip.location.X - AShip.stats.width / 2 - See.X, AShip.location.Y + AShip.stats.width / 2 + 7 + 7 - See.Y))
							Else
								G.DrawString(AShip.deflectors_loaded & "/" & AShip.stats.deflectors & " <- " & AShip.deflector_loading, Me.Font, New SolidBrush(Color.Gray), New Point(AShip.location.X - AShip.stats.width / 2 - See.X, AShip.location.Y + AShip.stats.width / 2 + 7 + 7 - See.Y))
							End If
						End If
					End If
				End If
			End If
		Next
		'===' Infos '==='
		G.DrawString("Ships : " & world.CountTeamShips(player_team) & " / " & player_team.ship_count_limit & " Max.", New Font("Consolas", 10), Brushes.Lime, New Point(0, 0))
		If play_state = PlayState.Paused Then
			G.DrawString("PAUSE", New Font("Consolas", 16), Brushes.White, New Point(0, DrawBMP.Height - 32))
		ElseIf play_state = PlayState.Timelapse Then
			G.DrawString("TIMELAPSE", New Font("Consolas", 16), Brushes.White, New Point(0, DrawBMP.Height - 32))
		End If
		'===' Help '==='
		If help Then
			Dim help_str As String = ""
			If Not has_ascended Then
				help_str += "The galaxy went into chaos. Find a way to escape." & vbNewLine
				help_str += vbNewLine
				help_str += "Use the arrows, or click the minimap to move the camera." & vbNewLine
				help_str += "Press SPACE to pause the game." & vbNewLine
				help_str += "Press M to accelerate time." & vbNewLine
				help_str += "Press I to display all allies in blue and all enemies in red." & vbNewLine
				help_str += vbNewLine
				help_str += "Click or draw a sqare to select units." & vbNewLine
				help_str += "Right click to order them to move, folow an ally or attack an enemy." & vbNewLine
				help_str += "Double right click to order your ships to mine nearby asteroids." & vbNewLine
				help_str += "Right click onto your selected ship itself to order it to only attack enemies." & vbNewLine
				help_str += vbNewLine
				help_str += "Press H to hide this text." & vbNewLine
			Else
				help_str += "Congratulations!" & vbNewLine
				help_str += "Your people just accessed another level of existence." & vbNewLine
				help_str += "Your ships remain under your control, but your mind is now immortal." & vbNewLine
				help_str += "You can keep fighting, but your goal have been reached." & vbNewLine
				help_str += vbNewLine
				help_str += "You won." & vbNewLine
			End If
			G.DrawString(help_str, New Font("Consolas", 10), Brushes.Cyan, New Point(0, DrawBMP.Height - 256))
		End If

		MiniBox.Image = MiniBMP
		DrawBox.Image = DrawBMP
	End Sub





	'===' Mini-Map '==='
	Dim MiniBMP As New Bitmap(200, 200)
	Dim MiniG As Graphics = Graphics.FromImage(MiniBMP)
	Dim MiniMDown As Boolean = False
	Private Sub MiniBox_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles MiniBox.MouseDown
		If MenuPanel.Visible Then
			Return
		End If
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
			If play_state <> PlayState.Paused Then
				play_state = PlayState.Paused
			Else
				play_state = PlayState.Playing
			End If
		End If
		If e.KeyData = Keys.M Then
			If play_state <> PlayState.Timelapse Then
				play_state = PlayState.Timelapse
			Else
				play_state = PlayState.Playing
			End If
		End If
		If e.KeyData = Keys.P Then
			timelapsev2 = Not timelapsev2
		End If
		If e.KeyData = Keys.F12 Then
			cheats_enabled = Not cheats_enabled
		End If
		If e.KeyData = Keys.F7 Then
			Dim total As String = ShipStats.DumpClasses()
			Clipboard.SetText(total)
		End If
		If e.KeyData = Keys.F8 Then
			For Each a_ship As Ship In world.Ships
				a_ship.agressivity = 1000.0
			Next
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
		SelectPTN2 = New Point((e.X * DrawBMP.Width / DrawBox.Width) + See.X, (e.Y * DrawBMP.Height / DrawBox.Height) + See.Y)
		Select Case e.Button
			Case Windows.Forms.MouseButtons.Left
				SelectStarted = True
				SelectPTN1 = New Point((e.X * DrawBMP.Width / DrawBox.Width) + See.X, (e.Y * DrawBMP.Height / DrawBox.Height) + See.Y)
			Case Windows.Forms.MouseButtons.Middle

			Case Windows.Forms.MouseButtons.Right

		End Select
	End Sub
	Private Sub DrawBox_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles DrawBox.MouseMove
		SelectPTN2 = New Point((e.X * DrawBMP.Width / DrawBox.Width) + See.X, (e.Y * DrawBMP.Height / DrawBox.Height) + See.Y)
	End Sub
	Private Sub DrawBox_MouseUp(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles DrawBox.MouseUp
		Select Case e.Button
			Case Windows.Forms.MouseButtons.Left
				SelectStarted = False
				SelectPTN2 = New Point((e.X * DrawBMP.Width / DrawBox.Width) + See.X, (e.Y * DrawBMP.Height / DrawBox.Height) + See.Y)
				SelectInSquare()
			Case Windows.Forms.MouseButtons.Middle

			Case Windows.Forms.MouseButtons.Right
				SelectOrder()
		End Select
	End Sub
	Dim SelectPTN1 As New Point(0, 0)
	Dim SelectPTN2 As New Point(0, 0)
	Dim SelectStarted As Boolean = False
	Public Sub SelectInSquare()
		Dim SS As Rectangle = Helpers.GetRect(SelectPTN1, SelectPTN2)
		If MenuPanel.Visible Then
			Return
		End If
		If Not ModifierKeys.HasFlag(Keys.Control) Then
			selected_ships.Clear()
		End If
		For Each aship As Ship In world.Ships
			If aship.team Is player_team OrElse cheats_enabled Then 'Si mode debug ou equipe correcte
				If aship.location.X + aship.stats.width / 2 > SS.X Then
					If aship.location.X - aship.stats.width / 2 < SS.X + SS.Width Then
						If aship.location.Y + aship.stats.width / 2 > SS.Y Then
							If aship.location.Y - aship.stats.width / 2 < SS.Y + SS.Height Then
								If Not selected_ships.Contains(aship) Then
									selected_ships.Add(aship)
									If Not aship.team Is Nothing Then
										player_team = aship.team
									End If
									If SelectPTN1 = SelectPTN2 Then
										Return
									End If
								End If
							End If
						End If
					End If
				End If
			End If
		Next
	End Sub
	Public Sub SelectOrder()
		If selected_ships.Count() = 0 Then
			Exit Sub
		End If
		Dim target_ship As Ship = Nothing
		' disable bots
		For Each ship As Ship In selected_ships
			ship.bot_ship = False
			If ship.stats.name.Contains("Station") Then
				If Not ship.team Is Nothing Then
					ship.team.bot_team = False
				End If
			End If
		Next
		'===' Recherche '==='
		For Each AShip As Ship In world.Ships
			If AShip.location.X + AShip.stats.width / 2 > SelectPTN2.X Then
				If AShip.location.X - AShip.stats.width / 2 < SelectPTN2.X Then
					If AShip.location.Y + AShip.stats.width / 2 > SelectPTN2.Y Then
						If AShip.location.Y - AShip.stats.width / 2 < SelectPTN2.Y Then
							target_ship = AShip
						End If
					End If
				End If
			End If
		Next
		'===' Validation '==='
		If target_ship Is Nothing Then
			For Each AShip As Ship In selected_ships
				If AShip.TargetPTN = SelectPTN2 Then
					world.Effects.Add(New Effect With {.Type = "Patrouille", .Coo = SelectPTN2})
					AShip.behavior = Ship.BehaviorMode.Mine
					AShip.TargetPTN = SelectPTN2
					AShip.target = Nothing
					If AShip.stats.name.Contains("Station") Then
						If Not AShip.team Is Nothing AndAlso Not player_team Is AShip.team Then
							AShip.team.bot_team = True
						End If
					End If
				Else
					world.Effects.Add(New Effect With {.Type = "Fleche", .Coo = SelectPTN2})
					AShip.behavior = Ship.BehaviorMode.GoToPoint
					AShip.TargetPTN = SelectPTN2
					AShip.target = Nothing
				End If
			Next
		Else
			For Each AShip As Ship In selected_ships
				If AShip Is target_ship Then
					world.Effects.Add(New Effect With {.Type = "Cible2", .Coo = SelectPTN2})
					AShip.AllowMining = False
				Else
					world.Effects.Add(New Effect With {.Type = "Cible", .Coo = SelectPTN2})
					AShip.behavior = Ship.BehaviorMode.Folow
					AShip.target = target_ship
				End If
			Next
		End If
	End Sub
	Private Sub PictureBox2_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox2.Click, PictureBox5.Click, PictureBox6.Click
		If cheats_enabled Then
			Dim team As Team = player_team
			If selected_ships.Count() > 0 AndAlso Not selected_ships(0).team Is Nothing Then
				team = selected_ships(0).team
			End If
			team.resources = New MaterialSet(999999, 999, 9999, 999999)
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



	' Selection and right panel
	Public listed_upgrades As List(Of Upgrade) = New List(Of Upgrade)
	Public selected_ships As List(Of Ship) = New List(Of Ship)

	Public Sub CheckRightPanel()
		update_displayed_materials()
		verify_selected_ships_existence()
		update_selected_ships_details()
	End Sub
	Public Sub update_displayed_materials()
		Dim selected_team As Team = player_team
		If selected_ships.Count > 0 AndAlso Not selected_ships(0).team Is Nothing Then
			selected_team = selected_ships(0).team
		End If
		MetalTextBox.Text = selected_team.resources.Metal
		CristalTextBox.Text = selected_team.resources.Crystal
		UraniumTextBox.Text = selected_team.resources.Fissile
		AntimatterTextBox.Text = selected_team.resources.Antimatter
	End Sub
	Sub verify_selected_ships_existence()
		For index = selected_ships.Count - 1 To 0 Step -1
			If Not world.Ships.Contains(selected_ships(index)) Then
				selected_ships.RemoveAt(index)
			End If
		Next
	End Sub
	Sub update_selected_ships_details()
		' panel visibility
		If selected_ships.Count = 0 Then
			SShipPanel.Visible = False
			Exit Sub
		End If
		If SShipPanel.Visible = False Then
			SShipPanel.Visible = True
		End If
		' ship details
		If selected_ships.Count = 1 Then
			SShipImageBox.Image = Helpers.GetSprite(selected_ships(0).stats.sprite, 0, 0, selected_ships(0).color)
			SShipTypeBox.Text = selected_ships(0).stats.name
			SShipUpsMax.Text = selected_ships(0).Ups.Count & " / " & selected_ships(0).upgrade_slots
			AllowMiningBox.Visible = Not selected_ships(0).AllowMining
		Else
			SShipImageBox.Image = My.Resources.Fleet
			SShipTypeBox.Text = selected_ships.Count().ToString() & " units"
			SShipUpsMax.Text = "- / -"
			AllowMiningBox.Visible = False
		End If
		' upgrade list
		listed_upgrades = Ship.ListedUpgrades(selected_ships)
	End Sub

	Dim UpX As Integer = -1 : Dim UpY As Integer = -1
	Private Sub UpgradesBox_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles UpgradesBox.MouseMove
		UpX = e.X \ 25
		UpY = e.Y \ 25
		UpgradeDetails.Top = UpgradesBox.Location.Y + e.Y
	End Sub
	Dim PBMP As New Bitmap(200, 600)
	Dim PG As Graphics = Graphics.FromImage(PBMP)
	Sub drawUpgrades()
		If SShipPanel.Visible = False OrElse selected_ships.Count() = 0 Then
			Exit Sub
		End If
		PG.Clear(Color.Black)
		Dim x As Integer = 0 : Dim y As Integer = 0
		Dim udV As Boolean = False
		For Each AUp As Upgrade In listed_upgrades
			Dim ships_upgradable As Integer = Ship.CountShipsBuyableNowUpgrade(selected_ships, AUp)
			Dim ships_installed As Integer = Ship.CountShipsHavingUpgrade(selected_ships, AUp)
			Dim min_progress As Integer = Ship.MinUpgradeProgress(selected_ships, AUp)
			If x = UpX AndAlso y = UpY Then
				PG.FillRectangle(Brushes.DimGray, x * 25, y * 25, 25, 25)
				udV = True
				If selected_ships.Count() > 1 Then
					UpName.Text = AUp.name & " (" & ships_upgradable.ToString() & ")"
				Else
					UpName.Text = AUp.name
				End If
				UpDesc.Text = AUp.desc
				' prices
				PriceC.Text = AUp.cost.Crystal * Math.Max(1, ships_upgradable)
				PriceM.Text = AUp.cost.Metal * Math.Max(1, ships_upgradable)
				PriceU.Text = AUp.cost.Fissile * Math.Max(1, ships_upgradable)
				PriceA.Text = AUp.cost.Antimatter * Math.Max(1, ships_upgradable)
				' invisible resources
				PriceM.Visible = (AUp.cost.Metal <> 0)
				PriceMIcon.Visible = (AUp.cost.Metal <> 0)
				PriceC.Visible = (AUp.cost.Crystal <> 0)
				PriceCIcon.Visible = (AUp.cost.Crystal <> 0)
				PriceU.Visible = (AUp.cost.Fissile <> 0)
				PriceUIcon.Visible = (AUp.cost.Fissile <> 0)
				PriceA.Visible = (AUp.cost.Antimatter <> 0)
				PriceAIcon.Visible = (AUp.cost.Antimatter <> 0)
			End If
			If ships_installed = selected_ships.Count() Then
				' already installed
				PG.DrawRectangle(New Pen(Brushes.White, 2), x * 25 + 1, y * 25 + 1, 24 - 1, 24 - 1)
			ElseIf min_progress < Int32.MaxValue Then
				' being installing
				PG.DrawRectangle(New Pen(Brushes.Yellow, 2), x * 25, y * 25, 24, 24)
				Dim ph As Integer = min_progress / Math.Max(1, AUp.delay) * 25
				PG.FillRectangle(Brushes.White, x * 25, y * 25 + 25 - ph, 25, ph)
			ElseIf ships_upgradable = 0 Then
				' no update slot remaining
				If ships_installed Then
					PG.DrawRectangle(New Pen(Brushes.LightGray), x * 25, y * 25, 24, 24)
				Else
					PG.DrawRectangle(Pens.DimGray, x * 25, y * 25, 24, 24)
				End If
			ElseIf selected_ships(0).team Is Nothing OrElse Not selected_ships(0).team.resources.HasEnough(AUp.cost.MultipliedBy(ships_upgradable)) Then
				' cannot afford all
				If AUp.upgrade_slots_requiered > 0 Then
					If selected_ships(0).team.resources.HasEnough(AUp.cost) Then
						' can afford at least one
						PG.DrawRectangle(Pens.DarkOrange, x * 25, y * 25, 24, 24)
					Else
						' cannot even afford one
						PG.DrawRectangle(Pens.DarkRed, x * 25, y * 25, 24, 24)
					End If
				Else
					If selected_ships(0).team.resources.HasEnough(AUp.cost) Then
						' can afford at least one
						PG.DrawRectangle(Pens.PaleGoldenrod, x * 25, y * 25, 24, 24)
					Else
						' cannot even afford one
						PG.DrawRectangle(Pens.PaleVioletRed, x * 25, y * 25, 24, 24)
					End If
				End If
			Else
				'can afford
				If AUp.upgrade_slots_requiered = 0 Then
					PG.DrawRectangle(Pens.PaleGreen, x * 25, y * 25, 24, 24)
				Else
					PG.DrawRectangle(Pens.DarkGreen, x * 25, y * 25, 24, 24)
				End If
			End If
			PG.DrawImage(Helpers.GetSprite(AUp.file, AUp.frame_coords.X, AUp.frame_coords.Y), New Rectangle(New Point(x * 25, y * 25), New Size(25, 25)))

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
		If MenuPanel.Visible Then
			Return
		End If
		Dim x As Integer = 0 : Dim y As Integer = 0
		For Each AUp As Upgrade In listed_upgrades
			If x = UpX AndAlso y = UpY Then
				For Each ship As Ship In selected_ships
					If ((ship.CanUpgrade(AUp) AndAlso ship.team Is player_team)) OrElse MainForm.cheats_enabled Then
						If ship.team Is Nothing OrElse ship.team.resources.HasEnough(AUp.cost) OrElse MainForm.cheats_enabled Then
							If Not ship.team Is Nothing Then ship.team.resources.Deplete(AUp.cost)
							ship.Upgrading = AUp
						End If
					End If
				Next
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

	Private Sub DrawBox_SizeChanged(sender As Object, e As EventArgs) Handles DrawBox.SizeChanged
		If DrawBox.Size.Width > 0 AndAlso DrawBox.Size.Height > 0 Then
			DrawBMP = New Bitmap(DrawBox.Size.Width, DrawBox.Size.Height)
			G = Graphics.FromImage(DrawBMP)
		End If
	End Sub
End Class

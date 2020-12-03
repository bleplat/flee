using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public partial class MainForm {
		public MainForm() {
			MiniG = Graphics.FromImage(MiniBMP);
			PG = Graphics.FromImage(PBMP);
			InitializeComponent();
			_MiniBox.Name = "MiniBox";
			_RandomizeButton.Name = "RandomizeButton";
			_StartPlayingButton.Name = "StartPlayingButton";
			_UpgradesBox.Name = "UpgradesBox";
			_PictureBox5.Name = "PictureBox5";
			_PictureBox6.Name = "PictureBox6";
			_PictureBox2.Name = "PictureBox2";
			_DrawBox.Name = "DrawBox";
		}

		public enum PlayState {
			Paused,
			Playing,
			Timelapse
		}

		public static bool cheats_enabled = false;
		public const ulong MAIN_BASE = ulong.MaxValue;
		private Point See = new Point(4700, 4700);

		// ===' Stats '==='
		public static PlayState play_state = PlayState.Playing;
		public static bool timelapsev2 = false;
		public static bool target_identification = false;
		public static bool help = true;
		public static bool has_ascended = false;

		// ===' Salutations ! '==='
		private Bitmap DrawBMP = new Bitmap(600, 600);
		private Graphics G;

		// world
		private World world = null;
		public Team player_team = null;

		private void MainForm_Load(object sender, EventArgs e) {
			// Commandline
			if (My.MyProject.Application.CommandLineArgs.Count > 1) SeedTextBox.Text = My.MyProject.Application.CommandLineArgs[1];
			else SeedTextBox.Text = new Random().Next().ToString();
			// Graphics settings
			G = Graphics.FromImage(DrawBMP);
			G.CompositingQuality = CompositingQuality.HighSpeed;
			G.InterpolationMode = InterpolationMode.Bilinear;
			Width = Width + 1;
			// Play the music if it's available
			try {
				My.MyProject.Computer.Audio.Play("sounds/PhilippWeigl-SubdivisionOfTheMasses.wav", AudioPlayMode.BackgroundLoop);
			} catch (Exception ex) {
				Text = "Flee - Music was not found AndAlso therefore not loaded!";
			}
			// Load lists
			Upgrade.LoadRegUpgrades();
			Helpers.LoadLists();
			Upgrade.LoadBuildUpgrades();
		}

		private void BeginButton_Click(object sender, EventArgs e) {
			// get the seed, checking that it's correct
			int Seed;
			try {
				Seed = Convert.ToInt32(SeedTextBox.Text);
			} catch (Exception ex) {
				return;
			}

			Text = "Flee - Seed: " + SeedTextBox.Text;
			// close menu
			StartPlayingButton.Enabled = false;
			MenuPanel.Visible = false;
			// create the world
			world = new World(Seed);
			player_team = world.Teams[0];
			// Place camera on player
			See = new Point((int)(world.Ships[0].location.X - 200f), (int)(world.Ships[0].location.Y - 200f));
			// finaly enable timer
			Ticker.Enabled = true;
		}


		// ===' Boucle '==='
		private void Ticker_Tick(object sender, EventArgs e) {
			var start_time = DateTime.Now;
			if (play_state != PlayState.Paused) for (int i = 1, loopTo = (int)(((play_state == PlayState.Timelapse) ? 8 : 1) * (timelapsev2 ? 16 : 1)); i <= loopTo; i++)
					world.Tick();

			CheckRightPanel();
			KeysCheck();
			drawUpgrades();
			DrawAll();
			// debugstr += vbNewLine + "DrawAll: " + (DateTime.Now - start_time).ToString() : start_time = DateTime.Now
		}

		public void KeysCheck() {
			foreach (string AKey in KeyList) switch (AKey ?? "") {
					case "Up": {
							See.Y = See.Y - 50;
							CheckSee();
							break;
						}

					case "Down": {
							See.Y = See.Y + 50;
							CheckSee();
							break;
						}

					case "Left": {
							See.X = See.X - 50;
							CheckSee();
							break;
						}

					case "Right": {
							See.X = See.X + 50;
							CheckSee();
							break;
						}
				}
		}

		public void DrawAll() {
			if (world.NuclearEffect <= 0) G.Clear(Color.Black);
			else {
				G.Clear(Color.FromArgb(world.NuclearEffect, world.NuclearEffect, world.NuclearEffect));
				world.NuclearEffect -= 2;
			}

			MiniG.FillRectangle(new SolidBrush(Color.FromArgb(25, 0, 0, 0)), 0, 0, 200, 200);
			// ===' Minimap '==='
			MiniG.DrawRectangle(Pens.White, new Rectangle(new Point((int)(See.X / (double)world.ArenaSize.Width * MiniBMP.Width), (int)(See.Y / (double)world.ArenaSize.Height * MiniBMP.Height)), new Size((int)(DrawBMP.Width * MiniBMP.Width / (double)world.ArenaSize.Width), (int)(DrawBMP.Height * MiniBMP.Height / (double)world.ArenaSize.Height))));

			// ===' Ships '==='
			foreach (Ship AShip in world.Ships) {
				// Minimap '
				int W = (int)(AShip.stats.width / 30d);
				if (W < 2)
					W = 2;
				if (W > 5)
					W = 5;
				var mini_color = AShip.color;
				if (target_identification) if (AShip.team is null || AShip.stats.sprite == "Comet") mini_color = AShip.color;
					else if (ReferenceEquals(AShip.team, player_team)) mini_color = Color.Lime;
					else if (AShip.team.IsFriendWith(player_team)) mini_color = Color.Cyan;
					else mini_color = Color.Red;

				MiniG.FillRectangle(new SolidBrush(mini_color), new Rectangle((int)(AShip.location.X / world.ArenaSize.Width * MiniBMP.Width - W / 2d), (int)(AShip.location.Y / world.ArenaSize.Height * MiniBMP.Height - W / 2d), W, W));
				// Main screen '
				if (AShip.location.X + AShip.stats.width / 2d > See.X && AShip.location.X - AShip.stats.width / 2d < See.X + DrawBox.Width && AShip.location.Y + AShip.stats.width / 2d > See.Y && AShip.location.Y - AShip.stats.width / 2d < See.Y + DrawBox.Height) {
					var img = Helpers.GetSprite(AShip.stats.sprite, AShip.fram, 0, mini_color); // image
					var center = new Point((int)(AShip.location.X - See.X), (int)(AShip.location.Y - See.Y)); // centre
					int AddD = 0;
					if (AShip.team is null && AShip.stats.turn == 0d)
						AddD = world.ticks % 360;
					var MonM = new Matrix();
					MonM.RotateAt(-AShip.direction + 180f + AddD, center); // rotation
					G.Transform = MonM; // affectation
					G.DrawImage(img, new Point((int)(center.X - img.Size.Width / 2d), (int)(center.Y - img.Size.Width / 2d))); // dessin
					G.ResetTransform(); // reset
				}
			}

			// ===' Shoots '==='
			foreach (Shoot AShoot in world.Shoots) {
				// If AShoot.Coo.X > See.X AndAlso AShoot.Coo.X < See.X + DrawBox.Width AndAlso AShoot.Coo.Y > See.Y AndAlso AShoot.Coo.Y < See.Y + DrawBox.Height Then
				Color col;
				if (AShoot.Team is null) col = Color.White;
				else col = AShoot.Team.color;

				var img = new Bitmap(Helpers.GetSprite(AShoot.Type, AShoot.fram, 0, col)); // image
				var center = new Point((int)(AShoot.Coo.X - See.X), (int)(AShoot.Coo.Y - See.Y)); // centre
				var MonM = new Matrix();
				MonM.RotateAt(-AShoot.Direction + 180f, center); // rotation
				G.Transform = MonM; // affectation
				G.DrawImage(img, new Point((int)(center.X - img.Size.Width / 2d), (int)(center.Y - img.Size.Width / 2d))); // dessin
				G.ResetTransform(); // reset
									// End If
			}
			// ===' Effets '==='
			foreach (Effect AEffect in world.Effects) if (AEffect.Coo.X > See.X && AEffect.Coo.X < See.X + DrawBox.Width && AEffect.Coo.Y > See.Y && AEffect.Coo.Y < See.Y + DrawBox.Height) {
					var img = new Bitmap(Helpers.GetSprite(AEffect.Type, AEffect.fram, AEffect.sprite_y)); // image
					var center = new Point((int)(AEffect.Coo.X - See.X), (int)(AEffect.Coo.Y - See.Y)); // centre
					var MonM = new Matrix();
					MonM.RotateAt(-AEffect.Direction + 180f, center); // rotation
					G.Transform = MonM; // affectation
					G.DrawImage(img, new Point((int)(center.X - img.Size.Width / 2d), (int)(center.Y - img.Size.Width / 2d))); // dessin
					G.ResetTransform(); // reset
				}
			// ===' Séléction '==='
			if (SelectStarted) {
				var NR = Helpers.GetRect(ref SelectPTN1, ref SelectPTN2);
				NR.X = NR.X - See.X;
				NR.Y = NR.Y - See.Y;
				G.DrawRectangle(Pens.White, NR);
			}
			// ===' Ships Special '==='
			foreach (Ship AShip in world.Ships) if (AShip.location.X + AShip.stats.width / 2d > See.X && AShip.location.X - AShip.stats.width / 2d < See.X + DrawBox.Width && AShip.location.Y + AShip.stats.width / 2d > See.Y && AShip.location.Y - AShip.stats.width / 2d < See.Y + DrawBox.Height) if (AShip.team is object && AShip.behavior != Ship.BehaviorMode.Drift && AShip.stats.sprite != "MSL") {
						// Selection '
						var drawrect = new Rectangle(new Point((int)(AShip.location.X - AShip.stats.width / 2d - See.X), (int)(AShip.location.Y - AShip.stats.width / 2d - See.Y)), new Size(AShip.stats.width, AShip.stats.width)); // zone de dessin
						if (false && target_identification) if (AShip.team.id == 0) G.DrawRectangle(Pens.Lime, drawrect);
							else if (AShip.team.IsFriendWith(player_team)) G.DrawRectangle(Pens.Blue, drawrect);
							else G.DrawRectangle(Pens.Red, drawrect);

						if (selected_ships.Contains(AShip)) G.DrawRectangle(new Pen(AShip.team.color), drawrect);
						// shields
						if (AShip.stats.shield >= 1) {
							var shields_ptns = new PointF[16];
							var shields_colors = new Color[16];
							for (int i = 0, loopTo = shields_ptns.Length - 1; i <= loopTo; i++) {
								shields_ptns[i] = Helpers.GetNewPoint(new PointF((float)(drawrect.X + drawrect.Width / 2d), (float)(drawrect.Y + drawrect.Height / 2d)), (float)(i * 360 / 16d + AShip.direction), (float)(drawrect.Width / 2d + drawrect.Width / 4d + 16d)); // border location
																																																																			  // Dim c_charge = AShip.shield * 255 / AShip.stats.shield
																																																																			  // Dim c_nocharge = 255 - c_charge
																																																																			  // Dim c_speed = (AShip.stats.shield_regeneration - 10) * 255 / 30
																																																																			  // Dim c_op = AShip.stats.shield_opacity * 255 / 100
																																																																			  // Dim c_max = AShip.stats.shield
																																																																			  // Dim c_i = AShip.ShieldPoints(i)
																																																																			  // Dim c_p = Math.Max(1, AShip.ShieldPoints(i))
																																																																			  // shields_colors(i) = Color.FromArgb(c_i, Math.Min(255, Math.Max(0, (c_op * c_i + c_nocharge * (255 - c_i)) / c_p)), Math.Min(255, Math.Max(0, (c_charge * c_i + c_max * (255 - c_i)) / c_p)), Math.Min(255, Math.Max(0, (c_speed * c_i + c_charge * (255 - c_i)) / c_p)))

								double f_alpha = AShip.ShieldPoints[i] / 256.0d;
								double f_red_0 = 1.0d - AShip.shield / AShip.stats.shield / 2.0d;
								double f_red_1 = 1.0d - AShip.shield / AShip.stats.shield;
								double f_green_0 = (AShip.stats.shield_opacity - 25d) / 75.0d + AShip.stats.shield_regeneration / 40.0d;
								double f_green_1 = f_green_0 * Math.Sqrt(Math.Max(0f, AShip.shield / AShip.stats.shield));
								double f_blue_0 = Math.Sqrt(AShip.stats.shield) * 20.0d / 400.0d;
								double f_blue_1 = f_blue_0 * (AShip.shield / AShip.stats.shield);
								shields_colors[i] = Color.FromArgb((int)Math.Min(255d, f_alpha * 255d * 2d), (int)Math.Min(255d, Math.Max(0d, (1.0d - f_alpha) * f_red_0 * 256d + f_alpha * f_red_1 * 256d)), (int)Math.Min(255d, Math.Max(0d, (1.0d - f_alpha) * f_green_0 * 256d + f_alpha * f_green_1 * 256d)), (int)Math.Min(255d, Math.Max(0d, (1.0d - f_alpha) * f_blue_0 * 256d + f_alpha * f_blue_1 * 256d)));
							}

							var shieldsbrush = new PathGradientBrush(shields_ptns);
							shieldsbrush.SurroundColors = shields_colors;
							shieldsbrush.CenterColor = Color.FromArgb(0, 0, 0, 0);
							G.FillEllipse(shieldsbrush, new Rectangle(new Point((int)(drawrect.X - drawrect.Width / 16d - 4d), (int)(drawrect.Y - drawrect.Height / 16d - 4d)), new Size((int)(drawrect.Width + drawrect.Width / 8d + 8d), (int)(drawrect.Height + drawrect.Height / 8d + 8d))));
						}
						// life   'New Pen(getSColor(AShip.Color))
						if (AShip.stats.integrity > 20) {
							G.DrawRectangle(Pens.DimGray, new Rectangle(new Point((int)(AShip.location.X - AShip.stats.width / 2d - See.X), (int)(AShip.location.Y + AShip.stats.width / 2d + 5d - See.Y)), new Size(AShip.stats.width, 1)));
							G.DrawRectangle(new Pen(AShip.team.color), new Rectangle(new Point((int)(AShip.location.X - AShip.stats.width / 2d - See.X), (int)(AShip.location.Y + AShip.stats.width / 2d + 5d - See.Y)), new Size((int)(AShip.integrity / (double)AShip.stats.integrity * AShip.stats.width), 1)));
							G.DrawString(AShip.integrity + "/" + AShip.stats.integrity, Font, new SolidBrush(AShip.team.color), new Point((int)(AShip.location.X - AShip.stats.width / 2d - See.X), (int)(AShip.location.Y + AShip.stats.width / 2d + 7d - See.Y)));
							if (AShip.stats.deflectors > 0) if (AShip.deflectors_loaded == AShip.stats.deflectors) G.DrawString(AShip.deflectors_loaded + "/" + AShip.stats.deflectors, Font, new SolidBrush(Color.Gray), new Point((int)(AShip.location.X - AShip.stats.width / 2d - See.X), (int)(AShip.location.Y + AShip.stats.width / 2d + 7d + 7d - See.Y)));
								else G.DrawString(AShip.deflectors_loaded + "/" + AShip.stats.deflectors + " <- " + AShip.deflector_loading, Font, new SolidBrush(Color.Gray), new Point((int)(AShip.location.X - AShip.stats.width / 2d - See.X), (int)(AShip.location.Y + AShip.stats.width / 2d + 7d + 7d - See.Y)));
						}
					}
			// ===' Infos '==='
			G.DrawString("Ships : " + world.CountTeamShips(player_team) + " / " + player_team.ship_count_limit + " Max.", new Font("Consolas", 10f), Brushes.Lime, new Point(0, 0));
			if (play_state == PlayState.Paused) G.DrawString("PAUSE", new Font("Consolas", 16f), Brushes.White, new Point(0, DrawBMP.Height - 32));
			else if (play_state == PlayState.Timelapse) G.DrawString("TIMELAPSE", new Font("Consolas", 16f), Brushes.White, new Point(0, DrawBMP.Height - 32));
			// ===' Help '==='
			if (help) {
				string help_str = "";
				if (!has_ascended) {
					help_str += "The galaxy went into chaos. Find a way to escape." + Constants.vbNewLine;
					help_str += Constants.vbNewLine;
					help_str += "Use the arrows, or click the minimap to move the camera." + Constants.vbNewLine;
					help_str += "Press SPACE to pause the game." + Constants.vbNewLine;
					help_str += "Press M to accelerate time." + Constants.vbNewLine;
					help_str += "Press I to display all allies in blue and all enemies in red." + Constants.vbNewLine;
					help_str += Constants.vbNewLine;
					help_str += "Click or draw a sqare to select units." + Constants.vbNewLine;
					help_str += "Right click to order them to move, folow an ally or attack an enemy." + Constants.vbNewLine;
					help_str += "Double right click to order your ships to mine nearby asteroids." + Constants.vbNewLine;
					help_str += "Right click onto your selected ship itself to order it to only attack enemies." + Constants.vbNewLine;
					help_str += Constants.vbNewLine;
					help_str += "Press H to hide this text." + Constants.vbNewLine;
				} else {
					help_str += "Congratulations!" + Constants.vbNewLine;
					help_str += "Your people just accessed another level of existence." + Constants.vbNewLine;
					help_str += "Your ships remain under your control, but your mind is now immortal." + Constants.vbNewLine;
					help_str += "You can keep fighting, but your goal have been reached." + Constants.vbNewLine;
					help_str += Constants.vbNewLine;
					help_str += "You won." + Constants.vbNewLine;
				}

				G.DrawString(help_str, new Font("Consolas", 10f), Brushes.Cyan, new Point(0, DrawBMP.Height - 256));
			}

			MiniBox.Image = MiniBMP;
			DrawBox.Image = DrawBMP;
		}





		// ===' Mini-Map '==='
		private Bitmap MiniBMP = new Bitmap(200, 200);
		private Graphics MiniG;
		private bool MiniMDown = false;

		private void MiniBox_MouseDown(object sender, MouseEventArgs e) {
			if (MenuPanel.Visible) return;

			MiniMDown = true;
			See.X = (int)(((e.X / MiniBox.Width) * world.ArenaSize.Width) - DrawBMP.Width / 2d);
			See.Y = (int)(((e.Y / MiniBox.Height) * world.ArenaSize.Height) - DrawBMP.Height / 2d);
			CheckSee();
		}

		private void MiniBox_MouseUp(object sender, MouseEventArgs e) {
			MiniMDown = false;
		}

		private void MiniBox_MouseMove(object sender, MouseEventArgs e) {
			if (MiniMDown) {
				See.X = (int)(((e.X / MiniBox.Width) * world.ArenaSize.Width) - DrawBMP.Width / 2d);
				See.Y = (int)(((e.Y / MiniBox.Height) * world.ArenaSize.Height) - DrawBMP.Height / 2d);
				CheckSee();
			}
		}

		public void CheckSee() {
			if (See.X < 0) See.X = 0;

			if (See.Y < 0) See.Y = 0;

			if (See.X > world.ArenaSize.Width - DrawBMP.Width) See.X = world.ArenaSize.Width - DrawBMP.Width;

			if (See.Y > world.ArenaSize.Height - DrawBMP.Height) See.Y = world.ArenaSize.Height - DrawBMP.Height;
		}


		// ===' Touches '==='
		public List<string> KeyList = new List<string>();

		private void MainForm_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyData == Keys.I) target_identification = !target_identification;

			if (e.KeyData == Keys.H) help = !help;

			if (e.KeyData == Keys.Space) if (play_state != PlayState.Paused) play_state = PlayState.Paused;
				else play_state = PlayState.Playing;

			if (e.KeyData == Keys.M) if (play_state != PlayState.Timelapse) play_state = PlayState.Timelapse;
				else play_state = PlayState.Playing;

			if (e.KeyData == Keys.P) timelapsev2 = !timelapsev2;

			if (e.KeyData == Keys.F12) cheats_enabled = !cheats_enabled;

			if (e.KeyData == Keys.F7) {
				string total = ShipStats.DumpClasses();
				Clipboard.SetText(total);
			}

			if (e.KeyData == Keys.F8) foreach (Ship a_ship in world.Ships)
					a_ship.agressivity = 1000.0d;

			if (!KeyList.Contains(e.KeyData.ToString())) KeyList.Add(e.KeyData.ToString());
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			KeyList.Remove(e.KeyData.ToString());
		}


		// ===' Clics '==='
		private void DrawBox_MouseDown(object sender, MouseEventArgs e) {
			SelectPTN2 = new Point((int)(e.X * DrawBMP.Width / (double)DrawBox.Width + See.X), (int)(e.Y * DrawBMP.Height / (double)DrawBox.Height + See.Y));
			switch (e.Button) {
				case MouseButtons.Left: {
						SelectStarted = true;
						SelectPTN1 = new Point((int)(e.X * DrawBMP.Width / (double)DrawBox.Width + See.X), (int)(e.Y * DrawBMP.Height / (double)DrawBox.Height + See.Y));
						break;
					}

				case MouseButtons.Middle: {
						break;
					}

				case MouseButtons.Right: {
						break;
					}
			}
		}

		private void DrawBox_MouseMove(object sender, MouseEventArgs e) {
			SelectPTN2 = new Point((int)(e.X * DrawBMP.Width / (double)DrawBox.Width + See.X), (int)(e.Y * DrawBMP.Height / (double)DrawBox.Height + See.Y));
		}

		private void DrawBox_MouseUp(object sender, MouseEventArgs e) {
			switch (e.Button) {
				case MouseButtons.Left: {
						SelectStarted = false;
						SelectPTN2 = new Point((int)(e.X * DrawBMP.Width / (double)DrawBox.Width + See.X), (int)(e.Y * DrawBMP.Height / (double)DrawBox.Height + See.Y));
						SelectInSquare();
						break;
					}

				case MouseButtons.Middle: {
						break;
					}

				case MouseButtons.Right: {
						SelectOrder();
						break;
					}
			}
		}

		private Point SelectPTN1 = new Point(0, 0);
		private Point SelectPTN2 = new Point(0, 0);
		private bool SelectStarted = false;

		public void SelectInSquare() {
			var SS = Helpers.GetRect(ref SelectPTN1, ref SelectPTN2);
			if (MenuPanel.Visible) return;

			if (!ModifierKeys.HasFlag(Keys.Control)) selected_ships.Clear();

			foreach (Ship aship in world.Ships) if (ReferenceEquals(aship.team, player_team) || cheats_enabled) if (aship.location.X + aship.stats.width / 2d > SS.X) if (aship.location.X - aship.stats.width / 2d < SS.X + SS.Width) if (aship.location.Y + aship.stats.width / 2d > SS.Y) if (aship.location.Y - aship.stats.width / 2d < SS.Y + SS.Height) if (!selected_ships.Contains(aship)) {
										selected_ships.Add(aship);
										if (aship.team is object) player_team = aship.team;

										if (SelectPTN1 == SelectPTN2) return;
									}
		}

		public void SelectOrder() {
			if (selected_ships.Count == 0) return;

			Ship target_ship = null;
			// disable bots
			foreach (Ship ship in selected_ships) {
				ship.bot_ship = false;
				if (ship.stats.name.Contains("Station")) if (ship.team is object) ship.team.bot_team = false;
			}
			// ===' Recherche '==='
			foreach (Ship AShip in world.Ships) if (AShip.location.X + AShip.stats.width / 2d > SelectPTN2.X) if (AShip.location.X - AShip.stats.width / 2d < SelectPTN2.X) if (AShip.location.Y + AShip.stats.width / 2d > SelectPTN2.Y) if (AShip.location.Y - AShip.stats.width / 2d < SelectPTN2.Y) target_ship = AShip;
			// ===' Validation '==='
			if (target_ship is null) foreach (Ship AShip in selected_ships) if (AShip.TargetPTN == SelectPTN2) {
						world.Effects.Add(new Effect() { Type = "EFF_Mine", Coo = SelectPTN2 });
						AShip.behavior = Ship.BehaviorMode.Mine;
						AShip.TargetPTN = SelectPTN2;
						AShip.target = null;
						if (AShip.stats.name.Contains("Station")) if (AShip.team is object && !ReferenceEquals(player_team, AShip.team)) AShip.team.bot_team = true;
					} else {
						world.Effects.Add(new Effect() { Type = "EFF_Goto", Coo = SelectPTN2 });
						AShip.behavior = Ship.BehaviorMode.GoToPoint;
						AShip.TargetPTN = SelectPTN2;
						AShip.target = null;
					}
			else foreach (Ship AShip in selected_ships) if (ReferenceEquals(AShip, target_ship)) {
						world.Effects.Add(new Effect() { Type = "EFF_OrderDefend", Coo = SelectPTN2 });
						AShip.AllowMining = false;
					} else {
						AShip.behavior = Ship.BehaviorMode.Folow;
						AShip.target = target_ship;
						if (AShip.team is object && AShip.team.IsFriendWith(target_ship.team)) world.Effects.Add(new Effect() { Type = "EFF_Assist", Coo = SelectPTN2, Direction = 180f });
						else world.Effects.Add(new Effect() { Type = "EFF_OrderTarget", Coo = SelectPTN2 });
					}
		}

		private void PictureBox2_Click(object sender, EventArgs e) {
			if (cheats_enabled) {
				var team = player_team;
				if (selected_ships.Count > 0 && selected_ships[0].team is object) team = selected_ships[0].team;

				team.resources = new MaterialSet(999999L, 999L, 9999L, 999999L);
			}
		}

		private void MainForm_Resize(object sender, EventArgs e) {
			DrawBox.Width = DrawBox.Height;
			PanelRes.Left = DrawBox.Width + DrawBox.Left;
			SShipPanel.Left = DrawBox.Width + DrawBox.Left;
			MiniBox.Left = DrawBox.Width + DrawBox.Left;
		}

		private void RandomizeButton_Click(object sender, EventArgs e) {
			SeedTextBox.Text = new Random().Next().ToString();
		}



		// Selection and right panel
		public List<Upgrade> listed_upgrades = new List<Upgrade>();
		public List<Ship> selected_ships = new List<Ship>();

		public void CheckRightPanel() {
			update_displayed_materials();
			verify_selected_ships_existence();
			update_selected_ships_details();
		}

		public void update_displayed_materials() {
			var selected_team = player_team;
			if (selected_ships.Count > 0 && selected_ships[0].team is object) selected_team = selected_ships[0].team;

			MetalTextBox.Text = selected_team.resources.Metal.ToString();
			CristalTextBox.Text = selected_team.resources.Crystal.ToString();
			UraniumTextBox.Text = selected_team.resources.Fissile.ToString();
			AntimatterTextBox.Text = selected_team.resources.Antimatter.ToString();
		}

		public void verify_selected_ships_existence() {
			for (int index = selected_ships.Count - 1; index >= 0; index -= 1) if (!world.Ships.Contains(selected_ships[index])) selected_ships.RemoveAt(index);
		}

		public void update_selected_ships_details() {
			// panel visibility
			if (selected_ships.Count == 0) {
				SShipPanel.Visible = false;
				return;
			}

			if (SShipPanel.Visible == false) SShipPanel.Visible = true;
			// ship details
			if (selected_ships.Count == 1) {
				SShipImageBox.Image = Helpers.GetSprite(selected_ships[0].stats.sprite, 0, 0, selected_ships[0].color);
				SShipTypeBox.Text = selected_ships[0].stats.name;
				SShipUpsMax.Text = selected_ships[0].Ups.Count + " / " + selected_ships[0].upgrade_slots;
				AllowMiningBox.Visible = !selected_ships[0].AllowMining;
			} else {
				SShipImageBox.Image = My.Resources.Resources.Fleet;
				SShipTypeBox.Text = selected_ships.Count.ToString() + " units";
				SShipUpsMax.Text = "- / -";
				AllowMiningBox.Visible = false;
			}
			// upgrade list
			listed_upgrades = (List<Upgrade>)Ship.ListedUpgrades(selected_ships);
		}

		private int UpX = -1;
		private int UpY = -1;

		private void UpgradesBox_MouseMove(object sender, MouseEventArgs e) {
			UpX = e.X / 25;
			UpY = e.Y / 25;
			UpgradeDetails.Top = UpgradesBox.Location.Y + e.Y;
		}

		private Bitmap PBMP = new Bitmap(200, 600);
		private Graphics PG;

		public void drawUpgrades() {
			if (SShipPanel.Visible == false || selected_ships.Count == 0) return;

			PG.Clear(Color.Black);
			int x = 0;
			int y = 0;
			bool udV = false;
			foreach (Upgrade AUp in listed_upgrades) {
				int ships_upgradable = Ship.CountShipsBuyableNowUpgrade(selected_ships, AUp);
				int ships_installed = Ship.CountShipsHavingUpgrade(selected_ships, AUp);
				int min_progress = Ship.MinUpgradeProgress(selected_ships, AUp);
				if (x == UpX && y == UpY) {
					PG.FillRectangle(Brushes.DimGray, x * 25, y * 25, 25, 25);
					udV = true;
					if (selected_ships.Count > 1) UpName.Text = AUp.name + " (" + ships_upgradable.ToString() + ")";
					else UpName.Text = AUp.name;

					UpDesc.Text = AUp.desc;
					// prices
					PriceC.Text = (AUp.cost.Crystal * Math.Max(1, ships_upgradable)).ToString();
					PriceM.Text = (AUp.cost.Metal * Math.Max(1, ships_upgradable)).ToString();
					PriceU.Text = (AUp.cost.Fissile * Math.Max(1, ships_upgradable)).ToString();
					PriceA.Text = (AUp.cost.Antimatter * Math.Max(1, ships_upgradable)).ToString();
					// invisible resources
					PriceM.Visible = AUp.cost.Metal != 0L;
					PriceMIcon.Visible = AUp.cost.Metal != 0L;
					PriceC.Visible = AUp.cost.Crystal != 0L;
					PriceCIcon.Visible = AUp.cost.Crystal != 0L;
					PriceU.Visible = AUp.cost.Fissile != 0L;
					PriceUIcon.Visible = AUp.cost.Fissile != 0L;
					PriceA.Visible = AUp.cost.Antimatter != 0L;
					PriceAIcon.Visible = AUp.cost.Antimatter != 0L;
				}

				bool localHasEnough() { var argrequierement = AUp.cost.MultipliedBy(ships_upgradable); var ret = selected_ships[0].team.resources.HasEnough(ref argrequierement); return ret; }

				if (ships_installed == selected_ships.Count)                // already installed
					PG.DrawRectangle(new Pen(Brushes.White, 2f), x * 25 + 1, y * 25 + 1, 24 - 1, 24 - 1);
				else if (min_progress < int.MaxValue) {
					// being installing
					PG.DrawRectangle(new Pen(Brushes.Yellow, 2f), x * 25, y * 25, 24, 24);
					int ph = (int)(min_progress / Math.Max(1m, AUp.delay) * 25m);
					PG.FillRectangle(Brushes.White, x * 25, y * 25 + 25 - ph, 25, ph);
				} else if (ships_upgradable == 0)               // no update slot remaining
					if (ships_installed != 0)
						PG.DrawRectangle(new Pen(Brushes.LightGray), x * 25, y * 25, 24, 24);
					else PG.DrawRectangle(Pens.DimGray, x * 25, y * 25, 24, 24);
				else if (selected_ships[0].team is null || !localHasEnough())               // cannot afford all
					if (AUp.upgrade_slots_requiered > 0) if (selected_ships[0].team.resources.HasEnough(ref AUp.cost))                      // can afford at least one
							PG.DrawRectangle(Pens.DarkOrange, x * 25, y * 25, 24, 24);
						else                        // cannot even afford one
							PG.DrawRectangle(Pens.DarkRed, x * 25, y * 25, 24, 24);
					else if (selected_ships[0].team.resources.HasEnough(ref AUp.cost))                      // can afford at least one
						PG.DrawRectangle(Pens.PaleGoldenrod, x * 25, y * 25, 24, 24);
					else                        // cannot even afford one
						PG.DrawRectangle(Pens.PaleVioletRed, x * 25, y * 25, 24, 24);
				else if (AUp.upgrade_slots_requiered == 0) PG.DrawRectangle(Pens.PaleGreen, x * 25, y * 25, 24, 24);
				else PG.DrawRectangle(Pens.DarkGreen, x * 25, y * 25, 24, 24);

				PG.DrawImage(Helpers.GetSprite(AUp.file, AUp.frame_coords.X, AUp.frame_coords.Y), new Rectangle(new Point(x * 25, y * 25), new Size(25, 25)));

				// item suivant
				x = x + 1;
				if (x >= 8) {
					x = 0;
					y = y + 1;
				}
			}
			// infos
			if (udV) UpgradeDetails.Visible = true;
			else UpgradeDetails.Visible = false;

			UpgradesBox.Image = PBMP;
		}

		private void UpgradesBox_Click(object sender, EventArgs e) {
			if (MenuPanel.Visible) return;

			int x = 0;
			int y = 0;
			foreach (Upgrade AUp in listed_upgrades) {
				if (x == UpX && y == UpY) foreach (Ship ship in selected_ships) if (ship.CanUpgrade(AUp) && ReferenceEquals(ship.team, player_team) || cheats_enabled) if (ship.team is null || ship.team.resources.HasEnough(ref AUp.cost) || cheats_enabled) {
								if (ship.team is object)
									ship.team.resources.Deplete(ref AUp.cost);
								ship.Upgrading = AUp;
							}
				// item suivant
				x = x + 1;
				if (x >= 8) {
					x = 0;
					y = y + 1;
				}
			}
		}

		private void UpgradesBox_MouseLeave(object sender, EventArgs e) {
			UpgradeDetails.Visible = false;
			UpX = -1;
			UpY = -1;
		}

		private void DrawBox_SizeChanged(object sender, EventArgs e) {
			if (DrawBox.Size.Width > 0 && DrawBox.Size.Height > 0) {
				DrawBMP = new Bitmap(DrawBox.Size.Width, DrawBox.Size.Height);
				G = Graphics.FromImage(DrawBMP);
			}
		}
	}
}
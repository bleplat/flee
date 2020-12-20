using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public partial class GameForm {
		
		/* Game */
		Game game;

		/* Drawing */
		bool form_ready = false;
		public static bool target_identification = false;
		public static bool help = true;
		private Point See = new Point(4700, 4700);
		public void InitDrawing() {
			// nothing
		}
		TextureBrush _background_brush = null;
		TextureBrush GetBackgroundBrush() {
			if (_background_brush == null) {
				Bitmap background_bmp = new Bitmap("data/sprites/background.png");
				background_bmp = new Bitmap(background_bmp).Clone(new Rectangle(0, 0, background_bmp.Width, background_bmp.Height), Helpers.GetScreenPixelFormat());
				_background_brush = new TextureBrush(background_bmp, WrapMode.Tile);
			}
			return (_background_brush);
		}

		/* Construction */
		public GameForm() {
			InitializeComponent();
			game = new Game(this);
			InitDrawing();
			// VS authored shit:
			_MiniBox.Name = "MiniBox";
			_RandomizeButton.Name = "RandomizeButton";
			_StartPlayingButton.Name = "StartPlayingButton";
			_UpgradesBox.Name = "UpgradesBox";
			_DrawBox.Name = "DrawBox";
		}
		private void MainForm_Load(object sender, EventArgs e) {
			// Commandline
			if (My.MyProject.Application.CommandLineArgs.Count > 1)
				SeedTextBox.Text = My.MyProject.Application.CommandLineArgs[1];
			else
				SeedTextBox.Text = new Random().Next().ToString();
			// Play the music if it's available
			try {
				My.MyProject.Computer.Audio.Play("data/musics/PhilippWeigl-SubdivisionOfTheMasses.wav", AudioPlayMode.BackgroundLoop);
			} catch (FileNotFoundException) {
				Text = "Flee - Music not found!";
			}
			// Load lists
			Loader.Load();
			// 
			form_ready = true;
			this.Invalidate();
		}

		/* Begin Game */
		void SetUISettingsFromMenu() {
			// No longer relevant
		}
		void SetGameSettingsFromMenu() {
			// Seed
			try {
				game.seed = Convert.ToInt32(SeedTextBox.Text);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			// Timer
			game.tick_duration_ms = checkBoxFPS.Checked ? 25 : 33;
			Ticker.Interval = game.tick_duration_ms;
			// multiplayer
			game.is_multiplayer = checkBoxLAN.Checked;
			game.is_host = true;
		}
		void SetMenuVisible(bool visible) {
			StartPlayingButton.Enabled = visible;
			MenuPanel.Visible = visible;
		}
		private void BeginButton_Click(object sender, EventArgs e) {
			SetUISettingsFromMenu();
			SetGameSettingsFromMenu();
			game.StartSingleplayer();
			// Window Title
			this.Text = "Flee - Seed: " + SeedTextBox.Text;
			// close menu
			SetMenuVisible(false);
			// Place camera on player
			See = new Point((int)(game.world.ships[game.world.ships.Count - 1].location.X - this.Width / 2), (int)(game.world.ships[game.world.ships.Count - 1].location.Y - this.Height / 2));
			// finaly enable timer
			Ticker.Enabled = true; // TODO: have ticker enabled since the begining
		}

		/* Loop */
		private void Ticker_Tick(object sender, EventArgs e) {
			game.Tick();
			CheckPressedKeys();
			if (game.play_state == PlayState.Timelapse)
				CheckPressedKeys(); // Expected to run more often when timelapsing;
			CheckSidePanels();
			DrawAll();
		}

		/* Key Controls */
		public void CheckPressedKeys() {
			foreach (string key in pressed_keys) {
				if (key == "Up") {
					See.Y = See.Y - 50;
					ClampCameraLocationToArena();
				}
				if (key == "Down") {
					See.Y = See.Y + 50;
					ClampCameraLocationToArena();
				}
				if (key == "Left") {
					See.X = See.X - 50;
					ClampCameraLocationToArena();
				}
				if (key == "Right") {
					See.X = See.X + 50;
					ClampCameraLocationToArena();
				}
			}
		}

		/* Drawing */
		public string DebugString() {
			return (""
					+ "ships: " + game.world.ships.Count + "\r\n"
					+ "effects: " + game.world.effects.Count + "\r\n"
					+ "shoots: " + game.world.shoots.Count + "\r\n"
					+ "ships[0, 0]: " + game.world.sectors[0, 0].ships.Count + "\r\n"
					+ "ships[0, 1]: " + game.world.sectors[0, 1].ships.Count + "\r\n"
					+ "ships[1, 1]: " + game.world.sectors[1, 1].ships.Count + "\r\n"
					+ "shoots[0, 0]: " + game.world.sectors[0, 0].shoots.Count + "\r\n"
					+ "shoots[0, 1]: " + game.world.sectors[0, 1].shoots.Count + "\r\n"
					+ "shoots[1, 1]: " + game.world.sectors[1, 1].shoots.Count + "\r\n"
					);
		}
		public void UpdateUpgradeInfo(Upgrade up, int buy_amount) {
			// title
			if (selected_ships.Count > 1)
				UpName.Text = up.name.Replace("_", " ") + " (" + buy_amount.ToString() + ")";
			else
				UpName.Text = up.name.Replace("_", " ");

			UpDesc.Text = up.desc;

			// prices
			PriceC.Text = (up.cost.Crystal * buy_amount).ToString();
			PriceM.Text = (up.cost.Metal * buy_amount).ToString();
			PriceU.Text = (up.cost.Fissile * buy_amount).ToString();
			PriceA.Text = (up.cost.Starfuel * buy_amount).ToString();
			PriceSlots.Text = (up.required_upgrade_slots).ToString();

			// resources visibility
			PriceM.Visible = up.cost.Metal != 0;
			PriceMIcon.Visible = up.cost.Metal != 0;
			PriceC.Visible = up.cost.Crystal != 0;
			PriceCIcon.Visible = up.cost.Crystal != 0;
			PriceU.Visible = up.cost.Fissile != 0;
			PriceUIcon.Visible = up.cost.Fissile != 0;
			PriceA.Visible = up.cost.Starfuel != 0;
			PriceAIcon.Visible = up.cost.Starfuel != 0;
			PriceSlots.Visible = up.install;
			PriceSlotsIcon.Visible = up.install;
		}
		public void DrawUpgrades(Graphics g) {
			bool udV = false;
			int upgrade_columns = UpgradesBox.Width / 25; //  (int)g.VisibleClipBounds.Width / 25;
			if (SShipPanel.Visible == false || selected_ships.Count == 0)
				return;
			g.Clear(Color.Black);
			int x = 0;
			int y = 0;
			foreach (Upgrade AUp in listed_upgrades) {
				int ships_upgradable = Ship.CountShipsBuyableNowUpgrade(selected_ships, AUp);
				int ships_installed = Ship.CountShipsHavingUpgrade(selected_ships, AUp);
				int min_progress = Ship.MinUpgradeProgress(selected_ships, AUp);
				if (x == UpX && y == UpY) {
					g.FillRectangle(Brushes.DimGray, x * 25, y * 25, 25, 25);
					UpdateUpgradeInfo(AUp, Math.Max(1, ships_upgradable));
					udV = true;
				}

				bool localHasEnough() {
					var argrequierement = AUp.cost.MultipliedBy(ships_upgradable);
					var ret = selected_ships[0].team.resources.HasEnough(ref argrequierement);
					return ret;
				}

				if (ships_installed == selected_ships.Count)                // already installed
					g.DrawRectangle(new Pen(Brushes.White, 2f), x * 25 + 1, y * 25 + 1, 24 - 1, 24 - 1);
				else if (min_progress < int.MaxValue) {
					// installing
					g.DrawRectangle(new Pen(Brushes.Yellow, 2f), x * 25, y * 25, 24, 24);
					int ph = (int)(min_progress / Math.Max(1m, AUp.time) * 25m);
					g.FillRectangle(Brushes.White, x * 25, y * 25 + 25 - ph, 25, ph);
				} else if (ships_upgradable == 0)               // no update slot remaining
					if (ships_installed != 0)
						g.DrawRectangle(new Pen(Brushes.LightGray), x * 25, y * 25, 24, 24);
					else
						g.DrawRectangle(Pens.DimGray, x * 25, y * 25, 24, 24);
				else if (!localHasEnough())               // cannot afford all
					if (AUp.install)
						if (selected_ships[0].team.resources.HasEnough(ref AUp.cost))                      // can afford at least one
							g.DrawRectangle(Pens.DarkOrange, x * 25, y * 25, 24, 24);
						else                        // cannot even afford one
							g.DrawRectangle(Pens.DarkRed, x * 25, y * 25, 24, 24);
					else if (selected_ships[0].team.resources.HasEnough(ref AUp.cost))                      // can afford at least one
						g.DrawRectangle(Pens.PaleGoldenrod, x * 25, y * 25, 24, 24);
					else                        // cannot even afford one
						g.DrawRectangle(Pens.PaleVioletRed, x * 25, y * 25, 24, 24);
				else if (!AUp.install)
					g.DrawRectangle(Pens.PaleGreen, x * 25, y * 25, 24, 24);
				else
					g.DrawRectangle(Pens.DarkGreen, x * 25, y * 25, 24, 24);

				g.DrawImage(AUp.sprite_array.GetSprite(AUp.sprite_coords.X, AUp.sprite_coords.Y), new Rectangle(new Point(x * 25, y * 25), new Size(25, 25))) ;

				// item suivant
				x = x + 1;
				if (x >= upgrade_columns) {
					x = 0;
					y = y + 1;
				}
			}
			// upgrade details visibility
			if (udV)
				UpgradeDetails.Visible = true;
			else
				UpgradeDetails.Visible = false;
		}
		private void _UpgradesBox_Paint(object sender, PaintEventArgs e) {
			if (form_ready && game is object && game.world is object)
				DrawUpgrades(e.Graphics);
			else
				base.OnPaint(e);
		}
		public void DrawMinimap(Graphics g) {
			// Transparent clear
			g.FillRectangle(new SolidBrush(Color.FromArgb(24, 0, 0, 0)), 0, 0, 200, 200);
			// Visible area rectangle
			g.DrawRectangle(Pens.White, new Rectangle(new Point((int)(See.X / (double)game.world.ArenaSize.Width * g.VisibleClipBounds.Width), (int)(See.Y / (double)game.world.ArenaSize.Height * g.VisibleClipBounds.Height)), new Size((int)(this.Width * g.VisibleClipBounds.Width / (double)game.world.ArenaSize.Width), (int)(this.Height * g.VisibleClipBounds.Height / (double)game.world.ArenaSize.Height))));
			// Engagements
			if (game.player_team is object) {
				foreach (Engagement engagement in game.player_team.engagements) {
					Point center_mm = new Point((int)(engagement.location.X * g.VisibleClipBounds.Width / game.world.ArenaSize.Width), (int)(engagement.location.Y * g.VisibleClipBounds.Height / game.world.ArenaSize.Height));
					int warn_dist = engagement.timeout / 3;
					Matrix m = new Matrix();
					m.RotateAt((float)engagement.timeout * 0.33f, center_mm);
					g.Transform = m;
					g.DrawLine(new Pen(Color.FromArgb(engagement.timeout, 255, 0, 0), 1), new Point(center_mm.X - warn_dist, center_mm.Y), new Point(center_mm.X + warn_dist, center_mm.Y));
					g.DrawLine(new Pen(Color.FromArgb(engagement.timeout, 255, 0, 0), 1), new Point(center_mm.X, center_mm.Y - warn_dist), new Point(center_mm.X, center_mm.Y + warn_dist));
					g.ResetTransform();
				}
			}
			// Ships
			foreach (Ship AShip in game.world.ships) {
				// Minimap '
				int W = (int)(AShip.stats.width / 30d);
				if (W < 2)
					W = 2;
				if (W > 5)
					W = 5;
				var mini_color = AShip.color;
				if (target_identification)
					if (AShip.team.affinity == AffinityEnum.Wilderness)
						mini_color = Color.Black;
					else if (AShip.team.affinity == AffinityEnum.Hostile)
						mini_color = Color.Red;
					else if (ReferenceEquals(AShip.team, game.player_team))
						mini_color = Color.Lime;
					else if (AShip.team.affinity == AffinityEnum.Wilderness || AShip.team.affinity == AffinityEnum.Neutral)
						mini_color = Color.White;
					else if (AShip.team.IsFriendWith(game.player_team))
						mini_color = Color.Cyan;
					else
						mini_color = Color.OrangeRed;
				g.FillRectangle(new SolidBrush(mini_color), new Rectangle((int)(AShip.location.X / game.world.ArenaSize.Width * g.VisibleClipBounds.Width - W / 2d), (int)(AShip.location.Y / game.world.ArenaSize.Height * g.VisibleClipBounds.Height - W / 2d), W, W));
			}
		}
		private void _MiniBox_Paint(object sender, PaintEventArgs e) {
			if (form_ready && game is object && game.world is object)
				DrawMinimap(e.Graphics);
			else
				base.OnPaint(e);
		}
		Size background_size = default;
		Size GetBackgroundSize() {
			if (background_size == default) {
				background_size = new Size(GetBackgroundBrush().Image.Width, GetBackgroundBrush().Image.Height);
			}
			return (background_size);
		}
		public void DrawMain(Graphics g) {
			// Background
			if (!checkBoxEnableBackground.Checked) { // disable background image 
				g.Clear(Color.Black);
			} else {
				GetBackgroundBrush().TranslateTransform(-See.X / (game.world.ArenaSize.Width / (GetBackgroundSize().Width - (int)g.VisibleClipBounds.Width)) + game.world.background_offset.X, -See.Y / (game.world.ArenaSize.Height / (GetBackgroundSize().Height - (int)g.VisibleClipBounds.Height)) + game.world.background_offset.Y);
				g.CompositingMode = CompositingMode.SourceCopy;
				g.FillRectangle(GetBackgroundBrush(), g.VisibleClipBounds);
				g.CompositingMode = CompositingMode.SourceOver;
				GetBackgroundBrush().ResetTransform();
			}
			// Nuke effect
			if (game.world.nuke_effect > 0) {
				SolidBrush nuclear_brush = new SolidBrush(Color.FromArgb(game.world.nuke_effect, game.world.nuke_effect, game.world.nuke_effect));
				g.FillRectangle(nuclear_brush, g.VisibleClipBounds);
				game.world.nuke_effect -= 2;
			}
			// ships
			foreach (Ship AShip in game.world.ships) {
				// Main screen '
				if (AShip.location.X + AShip.stats.width / 2d > See.X && AShip.location.X - AShip.stats.width / 2d < See.X + g.VisibleClipBounds.Width && AShip.location.Y + AShip.stats.width / 2d > See.Y && AShip.location.Y - AShip.stats.width / 2d < See.Y + g.VisibleClipBounds.Height) {
					var img = AShip.sprites.GetSprite(AShip.fram, 0);
					PointF center = new PointF(AShip.location.X - See.X, AShip.location.Y - See.Y); // centre
					int AddD = 0;
					if (ReferenceEquals(AShip.team, AShip.world.wilderness_team) && AShip.stats.turn == 0d)
						AddD = (int)(game.world.ticks % 360);
					var MonM = new Matrix();
					MonM.RotateAt(-AShip.direction + 180f + AddD, center); // rotation
					g.Transform = MonM; // affectation
					g.DrawImage(img, new PointF((center.X - img.Size.Width / 2.0f), (center.Y - img.Size.Width / 2.0f))); // dessin
					g.ResetTransform(); // reset
				}
			}
			// shoots
			foreach (Shoot AShoot in game.world.shoots) {
				// TODO: check bounds
				var img = AShoot.sprites.GetSprite(AShoot.fram, AShoot.sprite_y);
				PointF center = new PointF(AShoot.location.X - See.X, AShoot.location.Y - See.Y); // centre
				var MonM = new Matrix();
				MonM.RotateAt(-AShoot.direction + 180f, center); // rotation
				g.Transform = MonM; // affectation
				g.DrawImage(img, new PointF((center.X - img.Size.Width / 2.0f), (center.Y - img.Size.Width / 2.0f))); // dessin
				g.ResetTransform(); // reset
									// End If
			}
			// effects
			foreach (Effect AEffect in game.world.effects) {
				if (AEffect.location.X > See.X && AEffect.location.X < See.X + g.VisibleClipBounds.Width && AEffect.location.Y > See.Y && AEffect.location.Y < See.Y + g.VisibleClipBounds.Height) {
					var img = AEffect.sprites.GetSprite(AEffect.fram, AEffect.sprite_y);
					PointF center = new PointF(AEffect.location.X - See.X, AEffect.location.Y - See.Y); // centre
					var MonM = new Matrix();
					MonM.RotateAt(-AEffect.direction + 180f, center); // rotation
					g.Transform = MonM; // affectation
					g.DrawImage(img, new PointF((center.X - img.Size.Width / 2.0f), (center.Y - img.Size.Width / 2.0f))); // dessin
					g.ResetTransform(); // reset
				}
			}
			// Select rectangle
			if (SelectStarted) {
				var NR = Helpers.MakeRectangle(ref down_mouse_location, ref last_mouse_location);
				NR.X = NR.X - See.X;
				NR.Y = NR.Y - See.Y;
				g.DrawRectangle(Pens.White, NR);
			}
			// ship specials
			foreach (Ship AShip in game.world.ships)
				if (AShip.location.X + AShip.stats.width / 2d > See.X && AShip.location.X - AShip.stats.width / 2d < See.X + g.VisibleClipBounds.Width && AShip.location.Y + AShip.stats.width / 2d > See.Y && AShip.location.Y - AShip.stats.width / 2d < See.Y + g.VisibleClipBounds.Height)
					if (AShip.behavior != Ship.BehaviorMode.Drift && AShip.stats.sprite != "MSL") {
						// selection rectangle
						var drawrect = new Rectangle(new Point((int)(AShip.location.X - AShip.stats.width / 2d - See.X), (int)(AShip.location.Y - AShip.stats.width / 2d - See.Y)), new Size(AShip.stats.width, AShip.stats.width)); // zone de dessin
						// draw rectangle arround allies or enemies																																																 // Target Identification mode
						if (false && target_identification) {
							if (ReferenceEquals(AShip.team, game.player_team))
								g.DrawRectangle(Pens.Lime, drawrect);
							else if (AShip.team.affinity == AffinityEnum.Friendly)
								g.DrawRectangle(Pens.LightGreen, drawrect);
							else if (AShip.team.affinity == AffinityEnum.Dissident)
								g.DrawRectangle(Pens.OrangeRed, drawrect);
							else if (AShip.team.affinity == AffinityEnum.Dissident)
								g.DrawRectangle(Pens.Red, drawrect);
							else
								g.DrawRectangle(Pens.LightYellow, drawrect);
						}
						// draw selection rectangle
						if (selected_ships.Contains(AShip))
							g.DrawRectangle(new Pen(AShip.team.color), drawrect);
						// shields
						if (AShip.stats.shield >= 1) {
							var shields_ptns = new PointF[16];
							var shields_colors = new Color[16];
							for (int i = 0, loopTo = shields_ptns.Length - 1; i <= loopTo; i++) {
								shields_ptns[i] = Helpers.GetNewPoint(new PointF((float)(drawrect.X + drawrect.Width / 2d), (float)(drawrect.Y + drawrect.Height / 2d)), (float)(i * 360 / 16d + AShip.direction), (float)(drawrect.Width / 2d + drawrect.Width / 4d + 16d)); 
								double f_alpha = AShip.ShieldPoints[i] / 256.0d;
								double f_red_0 = 1.0d - AShip.shield / AShip.stats.shield / 2.0d;
								double f_red_1 = 1.0d - AShip.shield / AShip.stats.shield;
								double f_green_0 = (AShip.stats.shield_opacity + AShip.stats.shield_regeneration) / 2.0;
								double f_green_1 = f_green_0 * Math.Sqrt(Math.Max(0f, AShip.shield / AShip.stats.shield));
								double f_blue_0 = Math.Sqrt(AShip.stats.shield) / 375.0f;
								double f_blue_1 = f_blue_0 * (AShip.shield / AShip.stats.shield);
								shields_colors[i] = Color.FromArgb((int)Math.Min(255d, f_alpha * 255d * 2d), (int)Math.Min(255d, Math.Max(0d, (1.0d - f_alpha) * f_red_0 * 256d + f_alpha * f_red_1 * 256d)), (int)Math.Min(255d, Math.Max(0d, (1.0d - f_alpha) * f_green_0 * 256d + f_alpha * f_green_1 * 256d)), (int)Math.Min(255d, Math.Max(0d, (1.0d - f_alpha) * f_blue_0 * 256d + f_alpha * f_blue_1 * 256d)));
							}

							var shieldsbrush = new PathGradientBrush(shields_ptns);
							shieldsbrush.SurroundColors = shields_colors;
							shieldsbrush.CenterColor = Color.FromArgb(0, 0, 0, 0);
							g.FillEllipse(shieldsbrush, new Rectangle(new Point((int)(drawrect.X - drawrect.Width / 16d - 4d), (int)(drawrect.Y - drawrect.Height / 16d - 4d)), new Size((int)(drawrect.Width + drawrect.Width / 8d + 8d), (int)(drawrect.Height + drawrect.Height / 8d + 8d))));
						}
						// life   'New Pen(getSColor(AShip.Color))
						if (AShip.stats.integrity > 20) {
							g.DrawRectangle(Pens.DimGray, new Rectangle(new Point((int)(AShip.location.X - AShip.stats.width / 2 - See.X), (int)(AShip.location.Y + AShip.stats.width / 2 + 5 - See.Y)), new Size(AShip.stats.width, 1)));
							g.DrawRectangle(new Pen(AShip.team.color), new Rectangle(new Point((int)(AShip.location.X - AShip.stats.width / 2d - See.X), (int)(AShip.location.Y + AShip.stats.width / 2 + 5 - See.Y)), new Size((int)(AShip.integrity / (double)AShip.stats.integrity * AShip.stats.width), 1)));
							g.DrawString((int)AShip.integrity + "/" + (int)AShip.stats.integrity, Font, new SolidBrush(AShip.team.color), new Point((int)(AShip.location.X - AShip.stats.width / 2 - See.X), (int)(AShip.location.Y + AShip.stats.width / 2 + 7 - See.Y)));
							if (AShip.stats.shield > 0)
								g.DrawString((int)AShip.shield + "/" + (int)AShip.stats.shield, Font, Brushes.LightGray, new Point((int)(AShip.location.X - AShip.stats.width / 2 - See.X), (int)(AShip.location.Y + AShip.stats.width / 2 + 7 - See.Y + 7)));
							if (AShip.stats.TotalDeflectorsMax() > 0 || AShip.stats.cold_deflectors > 0)
								if (AShip.deflectors == AShip.stats.deflectors)
									g.DrawString(AShip.deflectors + "/" + AShip.stats.TotalDeflectorsMax(), Font, new SolidBrush(Color.DimGray), new Point((int)(AShip.location.X - AShip.stats.width / 2 - See.X), (int)(AShip.location.Y + AShip.stats.width / 2 + 7 + 14 - See.Y)));
								else
									g.DrawString(AShip.deflectors + "/" + AShip.stats.TotalDeflectorsMax() + " <- " + AShip.deflector_cooldown, Font, new SolidBrush(Color.Gray), new Point((int)(AShip.location.X - AShip.stats.width / 2 - See.X), (int)(AShip.location.Y + AShip.stats.width / 2 + 7 + 14 - See.Y)));
							if (AShip.emp_damage > 0.0f)
								g.DrawString(((int)AShip.emp_damage).ToString(), Font, Brushes.Cyan, new Point((int)(AShip.location.X - AShip.stats.width / 2 - See.X), (int)(AShip.location.Y + AShip.stats.width / 2 + 7 - See.Y + 7 + 7 + 7)));
						}
					}
			// text infos
			g.DrawImage(SpriteArray.GetSpriteArray("Upgrades").GetSprite(5, 0), new PointF(0, 0));
			g.DrawString(game.world.CountTeamShips(game.player_team) + " / " + game.player_team.ship_count_limit, new Font("Consolas", 10f), Brushes.Gray, new Point(32, 8));
			// special infos
			if (game.player_team == null || game.player_team.ship_count_approximation == 0)
				g.DrawString("spectator", new Font("Consolas", 7f), Brushes.DarkCyan, new Point(64, 0));
			if (game.player_team != null && game.player_team.cheats_enabled)
				g.DrawString("cheats enabled", new Font("Consolas", 7f), Brushes.DarkRed, new Point(64, 0));
			if (game.play_state == PlayState.Paused)
				g.DrawString("PAUSE", new Font("Consolas", 16f), Brushes.White, new Point(0, (int)g.VisibleClipBounds.Height - 32));
			else if (game.play_state == PlayState.Timelapse)
				g.DrawString("TIMELAPSE", new Font("Consolas", 16f), Brushes.White, new Point(0, (int)g.VisibleClipBounds.Height - 32));
			//g.DrawString(DebugString(), new Font("Consolas", 8.0f), Brushes.Gray, new Point(0, (int)g.VisibleClipBounds.Height / 2));
			if (help) {
				string help_str = "";
				if (!game.player_team.has_ascended) {
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
				g.DrawString(help_str, new Font("Consolas", 10f), Brushes.Cyan, new Point(175, (int)g.VisibleClipBounds.Height - 256));
			}
		}
		private void _DrawBox_Paint(object sender, PaintEventArgs e) {
			if (form_ready && game is object && game.world is object)
				DrawMain(e.Graphics);
			else
				base.OnPaint(e);
		}
		public void DrawAll() {
			WarCriminalLabel.Visible = (game.player_team.affinity == AffinityEnum.Hostile);
			UpgradesBox.Invalidate();
			DrawBox.Invalidate();
			MiniBox.Invalidate();
		}

		/* Camera */
		public void ClampCameraLocationToArena() {
			if (See.X < 0)
				See.X = 0;
			if (See.Y < 0)
				See.Y = 0;
			if (See.X > game.world.ArenaSize.Width - DrawBox.Width)
				See.X = game.world.ArenaSize.Width - DrawBox.Width;
			if (See.Y > game.world.ArenaSize.Height - DrawBox.Height)
				See.Y = game.world.ArenaSize.Height - DrawBox.Height;
		}

		/* Minimap Controls */
		private bool MiniMDown = false;
		private void MiniBox_MouseDown(object sender, MouseEventArgs e) {
			if (MenuPanel.Visible)
				return;
			MiniMDown = true;
			See.X = (int)(((e.X / (float)MiniBox.Width) * game.world.ArenaSize.Width) - DrawBox.Width / 2d);
			See.Y = (int)(((e.Y / (float)MiniBox.Height) * game.world.ArenaSize.Height) - DrawBox.Height / 2d);
			ClampCameraLocationToArena();
		}
		private void MiniBox_MouseUp(object sender, MouseEventArgs e) {
			MiniMDown = false;
		}
		private void MiniBox_MouseMove(object sender, MouseEventArgs e) {
			if (MiniMDown) {
				See.X = (int)(((e.X / (float)MiniBox.Width) * game.world.ArenaSize.Width) - DrawBox.Width / 2d);
				See.Y = (int)(((e.Y / (float)MiniBox.Height) * game.world.ArenaSize.Height) - DrawBox.Height / 2d);
				ClampCameraLocationToArena();
			}
		}

		/* Key Controls */
		public List<string> pressed_keys = new List<string>();
		private void MainForm_KeyDown(object sender, KeyEventArgs e) {
			// Interface switches
			if (e.KeyData == Keys.I)
				target_identification = !target_identification;
			if (e.KeyData == Keys.H)
				help = !help;
			// Pause and timelapse
			if (game.is_host) {
				if (e.KeyData == Keys.Space)
					if (game.play_state != PlayState.Paused)
						game.play_state = PlayState.Paused;
					else
						game.play_state = PlayState.Playing;
			}
			if (!game.is_multiplayer) {
				if (e.KeyData == Keys.M)
					if (game.play_state != PlayState.Timelapse)
						game.play_state = PlayState.Timelapse;
					else
						game.play_state = PlayState.Playing;
			}
			// Cheats
			if (e.KeyData == Keys.F12)
				game.player_team.cheats_enabled = !game.player_team.cheats_enabled;
			// Debug
			if (e.KeyData == Keys.F7) {
				string total = ShipStats.DumpClasses();
				Clipboard.SetText(total);
			}
			if (e.KeyData == Keys.F8)
				foreach (Ship a_ship in game.world.ships)
					a_ship.agressivity = 1000.0f;
			if (!pressed_keys.Contains(e.KeyData.ToString()))
				pressed_keys.Add(e.KeyData.ToString());
		}
		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			pressed_keys.Remove(e.KeyData.ToString());
		}

		/* Mouse Controls */
		private Point down_mouse_location = new Point(0, 0);
		private Point last_mouse_location = new Point(0, 0);
		private bool SelectStarted = false;
		private void DrawBox_MouseDown(object sender, MouseEventArgs e) {
			last_mouse_location = new Point((int)(e.X * DrawBox.Width / (double)DrawBox.Width + See.X), (int)(e.Y * DrawBox.Height / (double)DrawBox.Height + See.Y));
			if (e.Button == MouseButtons.Left) {
				SelectStarted = true;
				down_mouse_location = new Point((int)(e.X * DrawBox.Width / (double)DrawBox.Width + See.X), (int)(e.Y * DrawBox.Height / (double)DrawBox.Height + See.Y));
			}
		}
		private void DrawBox_MouseMove(object sender, MouseEventArgs e) {
			last_mouse_location = new Point((int)(e.X * DrawBox.Width / (double)DrawBox.Width + See.X), (int)(e.Y * DrawBox.Height / (double)DrawBox.Height + See.Y));
		}
		private void DrawBox_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				SelectStarted = false;
				last_mouse_location = new Point((int)(e.X * DrawBox.Width / (double)DrawBox.Width + See.X), (int)(e.Y * DrawBox.Height / (double)DrawBox.Height + See.Y));
				SelectInSquare();
			}
			if (e.Button == MouseButtons.Right) {
				SelectOrder();
			}
		}
		public void SelectInSquare() {
			var SS = Helpers.MakeRectangle(ref down_mouse_location, ref last_mouse_location);
			if (MenuPanel.Visible)
				return;
			if (!ModifierKeys.HasFlag(Keys.Control) || IsSelectionNonControlled())
				selected_ships.Clear();
			foreach (Ship aship in game.world.ships) {
				if (!aship.auto)
					if (ReferenceEquals(aship.team, game.player_team) || game.player_team.cheats_enabled || down_mouse_location == last_mouse_location)
						if (aship.location.X + aship.stats.width / 2d > SS.X)
							if (aship.location.X - aship.stats.width / 2d < SS.X + SS.Width)
								if (aship.location.Y + aship.stats.width / 2d > SS.Y)
									if (aship.location.Y - aship.stats.width / 2d < SS.Y + SS.Height)
										if (!selected_ships.Contains(aship)) {
											selected_ships.Add(aship);
											if (game.player_team.cheats_enabled)
												game.player_team = aship.team;
											if (down_mouse_location == last_mouse_location)
												return;
										}
			}
		}
		public void SelectOrder() {
			if (IsSelectionNonControlled())
				return;
			Ship target_ship = null;
			// disable bots
			foreach (Ship ship in selected_ships) {
				ship.bot_ship = false;
				if ((ship.stats.role & (int)ShipRole.Shipyard) != 0)
					ship.team.bot_team = false;
			}
			// ===' Recherche '==='
			foreach (Ship AShip in game.world.ships)
				if (AShip.location.X + AShip.stats.width / 2d > last_mouse_location.X)
					if (AShip.location.X - AShip.stats.width / 2d < last_mouse_location.X)
						if (AShip.location.Y + AShip.stats.width / 2d > last_mouse_location.Y)
							if (AShip.location.Y - AShip.stats.width / 2d < last_mouse_location.Y)
								target_ship = AShip;
			// ===' Validation '==='
			if (target_ship is null)
				foreach (Ship AShip in selected_ships)
					if (AShip.target_point == last_mouse_location) {
						game.world.effects.Add(new Effect(-1, "EFF_Mine", last_mouse_location));
						AShip.behavior = Ship.BehaviorMode.Mine;
						AShip.target_point = last_mouse_location;
						AShip.target = null;
						if ((AShip.stats.role & (int)ShipRole.Shipyard) != 0)
							if (!ReferenceEquals(game.player_team, AShip.team))
								AShip.team.bot_team = true;
					} else {
						game.world.effects.Add(new Effect(-1, "EFF_Goto", last_mouse_location));
						AShip.behavior = Ship.BehaviorMode.GoToPoint;
						AShip.target_point = last_mouse_location;
						AShip.target = null;
					}
			else
				foreach (Ship AShip in selected_ships)
					if (ReferenceEquals(AShip, target_ship)) {
						game.world.effects.Add(new Effect(-1, "EFF_OrderDefend", last_mouse_location));
						AShip.allow_mining = false;
					} else {
						AShip.behavior = Ship.BehaviorMode.Folow;
						AShip.target = target_ship;
						if (AShip.team.IsFriendWith(target_ship.team))
							game.world.effects.Add(new Effect(-1, "EFF_Assist", last_mouse_location, 180));
						else
							game.world.effects.Add(new Effect(-1, "EFF_OrderTarget", last_mouse_location));
					}
		}

		/* Cheat Resources */
		private void PictureBoxAvailableStarfuel_Click(object sender, EventArgs e) {
			if (game.player_team.cheats_enabled) {
				var team = game.player_team;
				if (selected_ships.Count > 0)
					team = selected_ships[0].team;

				team.resources = new MaterialSet(999999L, 999L, 9999L, 999999L);
			}
		}

		/* Menu */
		private void RandomizeButton_Click(object sender, EventArgs e) {
			SeedTextBox.Text = new Random().Next().ToString();
		}

		/* Materials Panel */
		public void update_displayed_materials() {
			var selected_team = game.player_team;
			if (selected_ships.Count > 0)
				selected_team = selected_ships[0].team;
			MetalTextBox.Text = selected_team.resources.Metal.ToString();
			CristalTextBox.Text = selected_team.resources.Crystal.ToString();
			UraniumTextBox.Text = selected_team.resources.Fissile.ToString();
			AntimatterTextBox.Text = selected_team.resources.Starfuel.ToString();
		}

		/* Selection Panel */
		public List<Ship> selected_ships = new List<Ship>();
		public List<Upgrade> listed_upgrades = new List<Upgrade>();
		public bool IsSelectionNonControlled() {
			return (selected_ships.Count == 0 || (selected_ships.Count >= 1 && !ReferenceEquals(selected_ships[0].team, game.player_team)));
		}
		public void CheckSidePanels() {
			update_displayed_materials();
			verify_selected_ships_existence();
			update_selected_ships_details();
		}
		public void verify_selected_ships_existence() {
			for (int index = selected_ships.Count - 1; index >= 0; index -= 1)
				if (!game.world.ships.Contains(selected_ships[index]))
					selected_ships.RemoveAt(index);
		}
		public void update_selected_ships_details() {
			// panel visibility
			if (selected_ships.Count == 0) {
				SShipPanel.Visible = false;
				return;
			}
			if (SShipPanel.Visible == false)
				SShipPanel.Visible = true;
			// ship details
			if (selected_ships.Count == 1) {
				SShipImageBox.Image = Helpers.GetSprite(selected_ships[0].stats.sprite, 0, 0, selected_ships[0].color);
				SShipTypeBox.Text = selected_ships[0].stats.name.Replace("_", " ");
				SShipUpsMax.Text = selected_ships[0].upgrades.Count + " / " + selected_ships[0].upgrade_slots;
				AllowMiningBox.Visible = !selected_ships[0].allow_mining;
			} else {
				SShipImageBox.Image = My.Resources.Resources.Fleet;
				SShipTypeBox.Text = selected_ships.Count.ToString() + " units";
				SShipUpsMax.Text = "- / -";
				AllowMiningBox.Visible = false;
			}
			// upgrade list
			if (IsSelectionNonControlled())
				listed_upgrades = selected_ships[0].InstalledOrInstallUpgrades();
			else
				listed_upgrades = Ship.ListedUpgrades(selected_ships);
		}
		private int UpX = -1;
		private int UpY = -1;
		private void UpgradesBox_MouseMove(object sender, MouseEventArgs e) {
			UpX = e.X / 25;
			UpY = e.Y / 25;
			UpgradeDetails.Top = UpgradesBox.Location.Y + e.Y;
		}
		private void UpgradesBox_Click(object sender, EventArgs e) {
			if (IsSelectionNonControlled())
				return;
			int upgrade_columns = UpgradesBox.Width / 25;
			if (MenuPanel.Visible)
				return;
			int x = 0;
			int y = 0;
			foreach (Upgrade AUp in listed_upgrades) {
				if (x == UpX && y == UpY)
					foreach (Ship ship in selected_ships)
						if (ship.CanUpgradeFree(AUp) && ReferenceEquals(ship.team, game.player_team) || game.player_team.cheats_enabled)
							if (ship.team.resources.HasEnough(ref AUp.cost) || game.player_team.cheats_enabled) {
								ship.team.resources.Deplete(ref AUp.cost);
								//ship.team.ship_count_approximation += AUp.required_team_slots;
								ship.Upgrading = AUp;
							}
				// next item
				x = x + 1;
				if (x >= upgrade_columns) {
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

	}
}
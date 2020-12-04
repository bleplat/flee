using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class World {

		// Definition
		public Size ArenaSize = new Size(20000, 20000);
		public int Seed;

		// Randoms
		public Random generation_random = null;
		public Random gameplay_random = null;

		// Content
		public List<Team> Teams = new List<Team>();
		public List<Ship> Ships = new List<Ship>();
		public List<Shoot> Shoots = new List<Shoot>();
		public List<Effect> Effects = new List<Effect>();

		// Sectors
		public const int sectors_count_x = 16;
		public const int sectors_count_y = 16;
		public WorldSector[,] sectors = new WorldSector[sectors_count_x, sectors_count_y];
		public void InitSectors() {
			for (int x = 0; x < sectors_count_x; x++) {
				for (int y = 0; y < sectors_count_x; y++) {
					sectors[x, y] = new WorldSector(this, x, y);
				}
			}
			for (int x = 0; x < sectors_count_x; x++) {
				for (int y = 0; y < sectors_count_x; y++) {
					sectors[x, y].InitForeignSectors();
				}
			}
		}
		public void UpdateSectors() {
			foreach (Ship ship in Ships) {
				ship.UpdateSector();
			}
			foreach (Shoot shoot in Shoots) {
				shoot.UpdateSector();
			}
		}
		public void Unspawn(in Ship ship) {
			if (ship.sector is object)
				ship.sector.ships.Remove(ship);
			Ships.Remove(ship);
		}
		public void Unspawn(in Shoot shoot) {
			if (shoot.sector is object)
				shoot.sector.shoots.Remove(shoot);
			Shoots.Remove(shoot);
		}

		// State
		public int ticks = 0;
		public int NuclearEffect = 0;
		public Team boss_team = null;

		public World(int Seed) {
			InitSectors();
			this.Seed = Seed;
			gameplay_random = new Random(Seed);
			generation_random = new Random(Seed);
			InitTeams();
			InitPlayer();
			InitDerelicts();
			InitBots();
		}

		public void Tick() {
			SpawnDerelictsObjects();
			NPCUpgrades();
			CheckAll();
			UpdateSectors(); // Done after movements. This may be innacurate. This could be done per ship/shoot on every movement.
			AutoColide();
			AntiSuperposition();
			AutoUnspawn();
			ticks += 1;
		}

		public void SPAWN_STATION_RANDOMLY(Random from_rand, string main_type, Team team, int spawn_allies) {
			var rand = new Random(from_rand.Next());
			// spawn main station
			if (main_type is null)
				main_type = Helpers.RandomStationName(rand);

			var main_coords = new Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000));
			for (int index = 1, loopTo = (Math.Max(1, Math.Min(8, 2000 / ShipStats.classes[main_type].complexity))); index <= loopTo; index++)
				if (index == 1) {
					var argworld = this;
					Ships.Add(new Ship(ref argworld, team, main_type) { location = main_coords });
				} else {
					var argworld1 = this;
					Ships.Add(new Ship(ref argworld1, team, main_type) { location = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513)) });
				}
			// spawn turrets
			while (spawn_allies > 0) {
				string ally_type = Helpers.RandomTurretName(rand);
				var ally_coords = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513));
				var argworld2 = this;
				Ships.Add(new Ship(ref argworld2, team, ally_type) { location = ally_coords });
				spawn_allies -= 1;
			}
			// spawn additional station
			if (team.bot_team && team.affinity != (int)AffinityEnum.KIND && rand.Next(0, 2) == 0) {
				var argworld1 = this;
				Ships.Add(new Ship(ref argworld1, team, main_type) { location = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513)) });
			}
		}

		private void InitTeams() {
			var rand = new Random(generation_random.Next());
			// player team
			Teams.Add(new Team(this, (int)AffinityEnum.KIND));
			// boss team
			Teams.Add(new Team(this, (int)AffinityEnum.ALOOF));
			boss_team = Teams[Teams.Count - 1];
			// 1 allied NPC team
			Teams.Add(new Team(this, (int)AffinityEnum.KIND));
			// enemy NPC team
			Teams.Add(new Team(this, (int)AffinityEnum.MEAN));
			// random teams
			int npc_team_count = rand.Next(3, 6);
			for (int i = 0, loopTo = npc_team_count - 1; i <= loopTo; i++)
				Teams.Add(new Team(this, default));
		}

		private void InitPlayer() {
			var rand = new Random(generation_random.Next());
			int power = 100;
			PointF origin;
			// Player Team
			var player_team = Teams[0];
			player_team.bot_team = false;
			// Player Ships
			if (Seed.ToString().Contains("777")) {
				origin = new Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000));
				var argworld = this;
				Ships.Add(new Ship(ref argworld, player_team, "DeinsCruiser") { location = new Point((int)origin.X, (int)(origin.Y - 1f)) });
				Ships[Ships.Count - 1].direction = (float)Helpers.GetQA((int)Ships[0].location.X, (int)Ships[0].location.Y, (int)origin.X, (int)origin.Y);
				power -= 35;
			} else if (rand.Next(0, 100) < 85) {
				SPAWN_STATION_RANDOMLY(rand, "Station", player_team, 3);
				origin = Ships[0].location;
				power -= 25;
			} else
				origin = new Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000));

			if (rand.Next(0, 100) < 75) {
				var argworld1 = this;
				Ships.Add(new Ship(ref argworld1, player_team, "Colonizer") { location = new Point((int)origin.X, (int)(origin.Y - 1f)) });
				Ships[Ships.Count - 1].direction = (float)Helpers.GetQA((int)Ships[0].location.X, (int)Ships[0].location.Y, (int)origin.X, (int)origin.Y);
				Ships[Ships.Count - 1].upgrade_slots += rand.Next(4, 10);
				power -= 15;
			}

			if (rand.Next(0, 100) < 75) {
				var argworld2 = this;
				Ships.Add(new Ship(ref argworld2, player_team, "Ambassador") { location = new Point((int)(origin.X + 1f), (int)origin.Y) });
				Ships[Ships.Count - 1].direction = (float)Helpers.GetQA((int)Ships[0].location.X, (int)Ships[0].location.Y, (int)origin.X, (int)origin.Y);
				Ships[Ships.Count - 1].upgrade_slots += rand.Next(4, 10);
				power -= 25;
			}

			while (power > 0) {
				var types = new[] { "MiniColonizer", "MiniColonizer", "Artillery", "Bomber", "Scout", "Simpleship", "Pusher", "Hunter" };
				var argworld3 = this;
				Ships.Add(new Ship(ref argworld3, player_team, types[rand.Next(0, types.Length)]) { location = new Point((int)(origin.X - 1f), (int)origin.Y) });
				Ships[Ships.Count - 1].direction = (float)Helpers.GetQA((int)Ships[0].location.X, (int)Ships[0].location.Y, (int)origin.X, (int)origin.Y);
				Ships[Ships.Count - 1].upgrade_slots += rand.Next(6, 12);
				power -= 15;
			}
		}

		public void InitDerelicts() {
			var rand = new Random(generation_random.Next());
			// Stars
			for (int i = 0, loopTo = rand.Next(1, 3); i <= loopTo; i++) {
				string T = "Star";
				var argworld = this;
				Ships.Add(new Ship(ref argworld, null, T) { location = new Point(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Width)), direction = rand.Next(0, 360) });
			}
			// Asteroids
			for (int i = 1; i <= 25; i++) {
				string T = "Asteroid";
				var location = new PointF(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Height));
				double direction = rand.Next(0, 360);
				for (int j = 0, loopTo1 = rand.Next(0, 4); j <= loopTo1; j++) {
					var argworld1 = this;
					Ships.Add(new Ship(ref argworld1, null, T) { location = new PointF(location.X + rand.Next(-4, 5), location.Y + rand.Next(-4, 5)), direction = (float)direction });
				}
			}
			// Meteoroids
			for (int i = 1; i <= 6; i++) {
				string T = "Meteoroid";
				var location = new PointF(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Height));
				double direction = rand.Next(0, 360);
				for (int j = 0, loopTo2 = rand.Next(4, 12); j <= loopTo2; j++) {
					var argworld2 = this;
					Ships.Add(new Ship(ref argworld2, null, T) { location = new PointF(location.X + rand.Next(-4, 5), location.Y + rand.Next(-4, 5)), direction = (float)direction });
				}
			}
		}

		public void InitBots() {
			var rand = new Random(generation_random.Next());
			foreach (Team team in Teams)
				if (!(team.affinity == (int)AffinityEnum.ALOOF) && team.bot_team)
					SPAWN_STATION_RANDOMLY(rand, null, team, rand.Next(6, 12));
		}

		public void AntiSuperposition() {
			/*
			if (Ships.Count > 1)
				for (int a = 1, loopTo = Ships.Count - 1; a <= loopTo; a++) {
					var Aship = Ships[a];
					for (int b = 0, loopTo1 = a - 1; b <= loopTo1; b++) {
						var Bship = Ships[b];
						if (Aship.location.X + Aship.stats.width > Bship.location.X - Bship.stats.width && Bship.location.X + Bship.stats.width > Aship.location.X - Aship.stats.width && Aship.location.Y + Aship.stats.width > Bship.location.Y - Bship.stats.width && Bship.location.Y + Bship.stats.width > Aship.location.Y - Aship.stats.width) {
							double dist = Helpers.Distance(ref Aship.location, ref Bship.location);
							double rel_dist = dist - (Aship.stats.width / 2d + Bship.stats.width / 2d);
							if (rel_dist < 0d) {
								double z = -1 * rel_dist / (Aship.stats.width / 2d + Bship.stats.width / 2d) * 0.0125d;
								var a_to_b = new PointF(Bship.location.X - Aship.location.X, Bship.location.Y - Aship.location.Y);
								if (Bship.stats.speed != 0d)
									Bship.speed_vec = new PointF((float)(Bship.speed_vec.X + a_to_b.X * z), (float)(Bship.speed_vec.Y + a_to_b.Y * z + 0.001d));
								if (Aship.stats.speed != 0d)
									Aship.speed_vec = new PointF((float)(Aship.speed_vec.X - a_to_b.X * z), (float)(Aship.speed_vec.Y - a_to_b.Y * z));
							}
						}
					}
				}
			*/
			///*
			foreach (Ship Aship in Ships) {
				foreach (Ship Bship in Aship.sector.ships) {
					if (!(Aship == Bship)) {
						if (Aship.location.X + Aship.stats.width > Bship.location.X - Bship.stats.width && Bship.location.X + Bship.stats.width > Aship.location.X - Aship.stats.width && Aship.location.Y + Aship.stats.width > Bship.location.Y - Bship.stats.width && Bship.location.Y + Bship.stats.width > Aship.location.Y - Aship.stats.width) {
							double dist = Helpers.Distance(ref Aship.location, ref Bship.location);
							double rel_dist = dist - (Aship.stats.width / 2d + Bship.stats.width / 2d);
							if (rel_dist < 0d) {
								double z = -1 * rel_dist / (Aship.stats.width / 2d + Bship.stats.width / 2d) * 0.0125d;
								var a_to_b = new PointF(Bship.location.X - Aship.location.X, Bship.location.Y - Aship.location.Y);
								if (Bship.stats.speed != 0 || Aship.stats.speed == 0) {
									Bship.speed_vec = new PointF((float)(Bship.speed_vec.X + a_to_b.X * z), (float)(Bship.speed_vec.Y + a_to_b.Y * z));
									Bship.location.X += 0.001f;
								}
							}
						}
					}
				}
			}
			//*/
		}

		public void CheckAll() {
			// ===' Ships '==='
			for (int i = Ships.Count - 1; i >= 0; i -= 1)
				Ships[i].Check();
			foreach (Ship AShip in Ships)
				AShip.IA(gameplay_random.Next(0, 10000));
			// ===' Shoots '==='
			for (int i = Shoots.Count - 1; i >= 0; i -= 1)
				Shoots[i].Check();
			// ===' Effets '==='
			foreach (Effect AEffect in Effects)
				AEffect.Check();
		}

		public void AutoColide() {
			for (int i_shoot = 0; i_shoot < Shoots.Count; i_shoot += 1) {
				Shoot AShoot = Shoots[i_shoot];
				foreach (Ship AShip in AShoot.sector.EnumerateNearbyShips()) {
					if (!ReferenceEquals(AShoot.Team, AShip.team) && (AShoot.Team is null || !AShoot.Team.IsFriendWith(AShip.team)))
						if (Helpers.Distance(AShoot.location.X, AShoot.location.Y, AShip.location.X, AShip.location.Y) < AShip.stats.width / 2d) {
							AShoot.Life = 0;
							if (AShip.stats.hot_deflector > 0d && gameplay_random.Next(0, 100) < AShip.stats.hot_deflector)
								Effects.Add(new Effect() { Type = "EFF_Deflected2", Coo = AShoot.location, Direction = AShoot.direction, speed = 0f });
							else {
								if (AShip.deflectors_loaded > 0)
									Effects.Add(new Effect() { Type = "EFF_Deflected", Coo = AShoot.location, Direction = AShoot.direction, speed = 0f });
								else if (AShoot.Power < 16)
									Effects.Add(new Effect() { Type = "EFF_Impact0", Coo = AShoot.location, Direction = AShoot.direction, speed = 0f });
								else if (AShoot.Power < 32)
									Effects.Add(new Effect() { Type = "EFF_Impact1", Coo = AShoot.location, Direction = AShoot.direction, speed = 0f, sprite_y = (ushort)gameplay_random.Next(0, 4) });
								else if (AShoot.Power < 48)
									Effects.Add(new Effect() { Type = "EFF_Impact2", Coo = AShoot.location, Direction = AShoot.direction, speed = 0f, sprite_y = (ushort)gameplay_random.Next(0, 4) });
								else
									Effects.Add(new Effect() { Type = "EFF_Impact3", Coo = AShoot.location, Direction = AShoot.direction, speed = 0f, sprite_y = (ushort)gameplay_random.Next(0, 4) });

								AShip.TakeDamages(AShoot.Power, ref AShoot);
								AShip.last_damager_team = AShoot.Team;
								if (AShip.stats.cold_deflector && AShip.cold_deflector_charge < AShip.stats.integrity * 4)
									Effects.Add(new Effect() { Type = "EFF_Deflected3", Coo = AShoot.location, Direction = AShoot.direction, speed = 0f });
							}
						}
				}
			}
		}

		public void AutoUnspawn() {
			// Ships
			if (Ships.Count > 0)
				for (int i = Ships.Count - 1; i >= 0; i -= 1)
					if (Ships[i].integrity <= 0) {
						Effects.Add(new Effect() { Type = "EFF_Destroyed", Coo = Ships[i].location, Direction = 0f, Life = 8, speed = 0f });
						for (int c = 1, loopTo = (int)(Ships[i].stats.width / 8d); c <= loopTo; c++)
							Effects.Add(new Effect() { Type = "EFF_Debris", Coo = Ships[i].location, Direction = gameplay_random.Next(0, 360), Life = gameplay_random.Next(80, 120), speed = gameplay_random.Next(3, 7) });
						if (Ships[i].weapons.Count > 1 && (Ships[i].weapons[0].stats.special & (int)Weapon.SpecialBits.SelfNuke) != 0) {
							NuclearEffect = 255;
							for (int c = 1; c <= 256; c++)
								Effects.Add(new Effect() { Type = "EFF_Destroyed", Coo = Ships[i].location, Direction = gameplay_random.Next(0, 360), Life = 8, speed = gameplay_random.Next(5, 256) });
							int FriendlyFireCount = 0;
							foreach (Ship a_ship in Ships) {
								a_ship.shield = 0f;
								a_ship.deflectors_loaded = 0;
								Shoot argFrom = null;
								a_ship.TakeDamages(8, ref argFrom);
								Shoot argFrom1 = null;
								a_ship.TakeDamages(8, ref argFrom1);
								Shoot argFrom2 = null;
								a_ship.TakeDamages(8, ref argFrom2);
								Shoot argFrom3 = null;
								a_ship.TakeDamages(8, ref argFrom3);
								Shoot argFrom4 = null;
								a_ship.TakeDamages((int)Math.Max(0d, Math.Sqrt(Math.Max(0d, 7000d - Helpers.Distance(Ships[i].location.X, Ships[i].location.Y, a_ship.location.X, a_ship.location.Y)))), ref argFrom4);
								Shoot argFrom5 = null;
								a_ship.TakeDamages(24, ref argFrom5);
								if (Helpers.Distance(Ships[i].location.X, Ships[i].location.Y, a_ship.location.X, a_ship.location.Y) < 5500d) {
									Shoot argFrom6 = null;
									a_ship.TakeDamages(10000, ref argFrom6);
								}

								if (a_ship.team is object && a_ship.team.affinity == (int)AffinityEnum.KIND && a_ship.integrity <= 0 && a_ship.team.id != Ships[i].team.id)
									FriendlyFireCount += 1;

								if (FriendlyFireCount >= 4) {
									Ships[i].team.affinity = (int)AffinityEnum.ALOOF;
									if (Ships[i].team.id == 0)
										My.MyProject.Forms.MainForm.WarCriminalLabel.Visible = true;
								}
							}
						}

						if (Ships[i].last_damager_team is object)
							Ships[i].last_damager_team.resources.AddLoot(ref Ships[i].stats.cost);

						this.Unspawn(Ships[i]);
					}
			// Shoots
			if (Shoots.Count > 0)
				for (int i = Shoots.Count - 1; i >= 0; i -= 1)
					if (Shoots[i].Life <= 0)
						this.Unspawn(Shoots[i]);
			// Effects
			if (Effects.Count > 0)
				for (int i = Effects.Count - 1; i >= 0; i -= 1)
					if (Effects[i].Life <= 0)
						Effects.RemoveAt(i);
		}

		public bool HasTeamWon(Team team) {
			foreach (var aShip in Ships) {
				if (aShip.team is null || aShip.team.id <= 1)
					continue;

				if (aShip.stats.name.Contains("Station") && !aShip.team.IsFriendWith(team))
					return false;
			}

			return true;
		}

		public void SpawnDerelictsObjects() {
			var rand = generation_random;
			if (ticks % 128 == 0) {
				// Spawn Location
				var Spawn = new Point();
				switch (rand.Next(0, 4)) {
				case 0: { // haut
					Spawn = new Point(rand.Next(50, ArenaSize.Width - 50), 0);
					break;
				}

				case 1: { // bas
					Spawn = new Point(rand.Next(50, ArenaSize.Width - 50), ArenaSize.Height);
					break;
				}

				case 2: { // gauche
					Spawn = new Point(0, rand.Next(50, ArenaSize.Height - 50));
					break;
				}

				case 3: { // droite
					Spawn = new Point(ArenaSize.Width, rand.Next(50, ArenaSize.Height - 50));
					break;
				}
				}
				// Spawn Direction
				float dir = 0f;
				dir = (float)(Helpers.GetQA(Spawn.X, Spawn.Y, (int)(ArenaSize.Width / 2d), (int)(ArenaSize.Height / 2d)) + rand.Next(-75, 75));
				// Type AndAlso count
				string Type = "Asteroid";
				Team Team = null;
				var AltTeam = boss_team;
				if (rand.Next(0, 3) == 0)
					AltTeam = Teams[rand.Next(2, Teams.Count)];
				int Count = rand.Next(1, 5);
				if (rand.Next(0, 4) == 0) {
					Type = "Meteoroid";
					Count = rand.Next(6, 12);
				} else if (rand.Next(0, 5) == 0) {
					Type = "Comet";
					Count = 1;
				} else if (rand.Next(0, 30) == 0) {
					Type = "Cargo";
					Count = 1;
				} else if (rand.Next(0, 90) == 0) {
					Type = "Civil_A";
					Team = AltTeam;
					Count = 1;
				} else if (rand.Next(0, 150) == 0) {
					Type = "Purger_Dronner";
					Team = AltTeam;
					Count = 1;
				} else if (rand.Next(0, 175) == 0) {
					Type = "Loneboss";
					Team = AltTeam;
					Count = 1;
				} else if (rand.Next(0, 350) == 0) {
					Type = "Converter";
					Team = AltTeam;
					Count = 1;
				} else if (rand.Next(0, 1250) == 0) {
					Type = "Bugs";
					Team = AltTeam;
					Count = 1;
				}

				if (HasTeamWon(Teams[0]))
					if (rand.Next(0, 2) == 0) {
						var possibles = new List<string>() { "Loneboss", "Civil_A", "Legend_I", "Legend_L", "Legend_K", "Colonizer" };
						if (Teams[0].affinity == (int)AffinityEnum.ALOOF) {
							possibles.Add("Nuke");
							possibles.Add("Ambassador");
						}

						if (MainForm.has_ascended) {
							possibles.Add("Ori");
							possibles.Add("Ori");
							possibles.Add("Ori");
							possibles.Add("Ori");
						}

						Type = possibles[rand.Next(0, possibles.Count)];
						Team = boss_team;
						Count = 1;
					}

				for (int j = 1, loopTo = Count; j <= loopTo; j++) {
					var argworld = this;
					Ships.Add(new Ship(ref argworld, Team, Type) { location = new Point(Spawn.X + rand.Next(-4, 5), Spawn.Y + rand.Next(-4, 5)), direction = dir });
				}
			}
		}

		public void NPCUpgrades() {
			var rand = generation_random;
			if (ticks % 80 == 0) {
				// Count Teams's ships
				foreach (Team a_team in Teams)
					a_team.ship_count_approximation = 0;
				foreach (Ship a_ship in Ships)
					if (a_ship.team is object)
						a_ship.team.ship_count_approximation += 1;
				// Summoning / upgrades
				foreach (Ship a_ship in Ships)
					if (a_ship.team is object && a_ship.bot_ship && a_ship.UpProgress == 0 && a_ship.team.ship_count_approximation < a_ship.team.ship_count_limit) {
						string wished_upgrade = null;
						if (rand.Next(0, 16) < 8)                         // building ships
							if (a_ship.stats.crafts.Count > 0) {
								wished_upgrade = Helpers.GetRandomSpawnUpgrade(rand, a_ship);
								if (wished_upgrade is object)
									Upgrade.ForceUpgradeToShip(a_ship, wished_upgrade);
							} else if (rand.Next(0, 2) == 0) {
								// upgrading
								var PossibleUps = a_ship.AvailableUpgrades();
								if (PossibleUps.Count >= 1)
									a_ship.Upgrading = PossibleUps[rand.Next(0, PossibleUps.Count)];
							} else {
								// boss bases must suicide to not use the max ship counter
								if (a_ship.team.affinity == (int)AffinityEnum.ALOOF && a_ship.stats.turn == 0.0d)
									Upgrade.ForceUpgradeToShip(a_ship, "Suicide");
								// ships equiped with cold deflectors jumps when unable to fire
								if (a_ship.cold_deflector_charge > 24)
									if (a_ship.CanUpgrade(Upgrade.UpgradeFromName("Jump")))
										Upgrade.ForceUpgradeToShip(a_ship, "Jump");
									else if (a_ship.CanUpgrade(Upgrade.UpgradeFromName("Jump_II")))
										Upgrade.ForceUpgradeToShip(a_ship, "Jump_II");
									else if (a_ship.CanUpgrade(Upgrade.UpgradeFromName("Warp")))
										Upgrade.ForceUpgradeToShip(a_ship, "Warp");
								// ship jumping when not in good shape
								if (a_ship.integrity + a_ship.shield < (a_ship.stats.integrity + a_ship.stats.shield) / 3d)
									if (a_ship.CanUpgrade(Upgrade.UpgradeFromName("Jump")))
										Upgrade.ForceUpgradeToShip(a_ship, "Jump");
									else if (a_ship.CanUpgrade(Upgrade.UpgradeFromName("Jump_II")))
										Upgrade.ForceUpgradeToShip(a_ship, "Jump_II");
									else if (a_ship.CanUpgrade(Upgrade.UpgradeFromName("Warp")))
										Upgrade.ForceUpgradeToShip(a_ship, "Warp");
							}
					}
			}
		}

		// ===' UPGRADES SHIPS '==='
		public int CountTeamShips(Team team) {
			int count = 0;
			foreach (Ship aship in Ships)
				if (ReferenceEquals(aship.team, team)) {
					if (aship.stats.speed != 0.0d || !aship.stats.name.Contains("Station"))
						count = count + 1;

					if (aship.Upgrading is object && aship.Upgrading.effect.StartsWith("!Sum"))
						count = count + 1;
				}

			return count;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class World {

		/* Definition */
		public Size ArenaSize = new Size(30000, 30000);
		public int Seed;

		/* Characteristics */
		public Point background_offset = new Point(0, 0);

		/* Randoms */
		public Random generation_random = null;
		public Random gameplay_random = null;

		/* Content */
		public List<Team> teams = new List<Team>();
		public List<Ship> ships = new List<Ship>();
		public List<Shoot> shoots = new List<Shoot>();
		public List<Effect> effects = new List<Effect>();
		public Team wilderness_team = null;
		public Team boss_team = null;

		/* Sectors */
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
			foreach (Ship ship in ships) {
				ship.UpdateSector();
			}
			foreach (Shoot shoot in shoots) {
				shoot.UpdateSector();
			}
		}
		public void Unspawn(in Ship ship) {
			if (ship.sector is object)
				ship.sector.ships.Remove(ship);
			ships.Remove(ship);
		}
		public void Unspawn(in Shoot shoot) {
			if (shoot.sector is object)
				shoot.sector.shoots.Remove(shoot);
			shoots.Remove(shoot);
		}

		/* State */
		public ulong ticks = 0;
		public int nuke_effect = 0;
		public bool is_invaded_by_bosses = false;
		public bool is_invaded_by_ascended = false;

		/* Creation */
		public World(int seed) {
			InitSectors();
			Random rand = InitRandom(seed);
			InitCharacteristics(new Random(rand.Next()));
			InitTeams(new Random(rand.Next()));
			InitDerelicts(new Random(rand.Next()));
			InitBots(new Random(rand.Next()));
		}
		Random InitRandom(int seed) {
			Random rand = new Random(seed);
			this.Seed = seed;
			generation_random = new Random(rand.Next());
			gameplay_random = new Random(rand.Next());
			return (rand);
		}
		void InitCharacteristics(Random rand) {
			background_offset.X = rand.Next(0, 4096);
			background_offset.Y = rand.Next(0, 4096);
		}
		void InitTeams(Random rand) {
			// Wilderness
			teams.Add(new Team(this));
			teams[teams.Count - 1].InitWildernessTeam();
			wilderness_team = teams[teams.Count - 1];
			// Boss
			teams.Add(new Team(this));
			teams[teams.Count - 1].InitBossTeam();
			boss_team = teams[teams.Count - 1];
			// NPCs
			{
				teams.Add(new Team(this, AffinityEnum.Friendly, new Random(rand.Next()))); // at least 1 Friendly
				teams.Add(new Team(this, AffinityEnum.Dissident, new Random(rand.Next()))); // at least 1 Dissident
				teams.Add(new Team(this, AffinityEnum.Hostile, new Random(rand.Next()))); // at least 1 Hostile
				int additional_team_count = rand.Next(2, 5); // 2, 3 or 4 additional teams
				for (int i = 0; i < additional_team_count; i++) {
					teams.Add(new Team(this, RandomNPCAffinity(rand), new Random(rand.Next())));
				}
			}
		}
		AffinityEnum RandomNPCAffinity(Random rand) {
			switch (rand.Next(0, 8)) {
			case 0: return (AffinityEnum.Neutral);
			case 1: return (AffinityEnum.Friendly);
			case 2: return (AffinityEnum.Friendly);
			case 3: return (AffinityEnum.Dissident);
			case 4: return (AffinityEnum.Dissident);
			case 5: return (AffinityEnum.Hostile);
			case 6: return (AffinityEnum.Hostile);
			case 7: return (AffinityEnum.Hostile);
			default: throw new Exception("coding mistake");
			}
		}
		public void InitDerelicts(Random rand) {
			// TODO: use 'role'
			// Stars
			for (int i = 0, loopTo = rand.Next(1, 3); i <= loopTo; i++) {
				string T = "Star";
				var argworld = this;
				ships.Add(new Ship(ref argworld, null, T) { location = new Point(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Width)), direction = rand.Next(0, 360) });
			}
			// Asteroids
			for (int i = 1; i <= 25; i++) {
				string T = "Asteroid";
				var location = new PointF(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Height));
				double direction = rand.Next(0, 360);
				for (int j = 0, loopTo1 = rand.Next(0, 4); j <= loopTo1; j++) {
					var argworld1 = this;
					ships.Add(new Ship(ref argworld1, null, T) { location = new PointF(location.X + rand.Next(-4, 5), location.Y + rand.Next(-4, 5)), direction = (float)direction });
				}
			}
			// Meteoroids
			for (int i = 1; i <= 6; i++) {
				string T = "Meteoroid";
				var location = new PointF(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Height));
				double direction = rand.Next(0, 360);
				for (int j = 0, loopTo2 = rand.Next(4, 12); j <= loopTo2; j++) {
					var argworld2 = this;
					ships.Add(new Ship(ref argworld2, null, T) { location = new PointF(location.X + rand.Next(-4, 5), location.Y + rand.Next(-4, 5)), direction = (float)direction });
				}
			}
		}
		public void InitBots(Random rand) {
			foreach (Team team in teams)
				if (team.bot_team && !ReferenceEquals(team, wilderness_team) && !ReferenceEquals(team, boss_team))
					SpawnStations(new Random(rand.Next()), null, team, rand.Next(6, 12));
		}
		public void SpawnStations(Random rand, string main_type, Team team, int spawn_allies) {
			// spawn main station
			if (main_type is null)
				main_type = Helpers.RandomStationName(rand);
			var main_coords = new Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000));
			for (int index = 1, loopTo = (Math.Max(1, Math.Min(8, 2000 / ShipStats.classes[main_type].complexity))); index <= loopTo; index++)
				if (index == 1) {
					var argworld = this;
					ships.Add(new Ship(ref argworld, team, main_type) { location = main_coords });
				} else {
					var argworld1 = this;
					ships.Add(new Ship(ref argworld1, team, main_type) { location = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513)) });
				}
			// spawn turrets
			while (spawn_allies > 0) {
				string ally_type = Helpers.RandomTurretName(rand);
				var ally_coords = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513));
				var argworld2 = this;
				ships.Add(new Ship(ref argworld2, team, ally_type) { location = ally_coords });
				spawn_allies -= 1;
			}
			// spawn additional station
			if (team.bot_team && team.affinity != AffinityEnum.Friendly && rand.Next(0, 2) == 0) {
				var argworld1 = this;
				ships.Add(new Ship(ref argworld1, team, main_type) { location = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513)) });
			}
		}
		public Team CreateAndSpawnPlayer(AffinityEnum affinity) {
			Random rand = new Random(gameplay_random.Next());
			// Add Team
			Team player_team = new Team(this);
			player_team.InitPlayerTeam(affinity);
			teams.Add(player_team);
			// Add Fleet
			int power = 100;
			PointF origin;
			// Player Ships
			if (Seed.ToString().Contains("777")) {
				origin = new Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000));
				var argworld = this;
				ships.Add(new Ship(ref argworld, player_team, "DeinsCruiser") { location = new Point((int)origin.X, (int)(origin.Y - 1f)) });
				ships[ships.Count - 1].direction = (float)Helpers.GetQA((int)ships[ships.Count - 1].location.X, (int)ships[ships.Count - 1].location.Y, (int)origin.X, (int)origin.Y);
				power -= 25;
			} else if (rand.Next(0, 100) < 85) {
				SpawnStations(rand, "Station", player_team, 3);
				origin = ships[ships.Count - 1].location;
				power -= 25;
			} else
				origin = new Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000));
			if (rand.Next(0, 100) < 75) {
				var argworld1 = this;
				ships.Add(new Ship(ref argworld1, player_team, "Colonizer") { location = new Point((int)origin.X, (int)(origin.Y - 1f)) });
				ships[ships.Count - 1].direction = (float)Helpers.GetQA((int)ships[ships.Count - 1].location.X, (int)ships[ships.Count - 1].location.Y, (int)origin.X, (int)origin.Y);
				ships[ships.Count - 1].upgrade_slots += rand.Next(4, 10);
				power -= 15;
			}
			if (rand.Next(0, 100) < 75) {
				var argworld2 = this;
				ships.Add(new Ship(ref argworld2, player_team, "Ambassador") { location = new Point((int)(origin.X + 1f), (int)origin.Y) });
				ships[ships.Count - 1].direction = (float)Helpers.GetQA((int)ships[ships.Count - 1].location.X, (int)ships[ships.Count - 1].location.Y, (int)origin.X, (int)origin.Y);
				ships[ships.Count - 1].upgrade_slots += rand.Next(4, 10);
				power -= 25;
			}
			while (power > 0) {
				var types = new[] { "MiniColonizer", "MiniColonizer", "Artillery", "Bomber", "Scout", "Simpleship", "Pusher", "Hunter" };
				var argworld3 = this;
				ships.Add(new Ship(ref argworld3, player_team, types[rand.Next(0, types.Length)]) { location = new Point((int)(origin.X - 1f), (int)origin.Y) });
				ships[ships.Count - 1].direction = (float)Helpers.GetQA((int)ships[ships.Count - 1].location.X, (int)ships[ships.Count - 1].location.Y, (int)origin.X, (int)origin.Y);
				ships[ships.Count - 1].upgrade_slots += rand.Next(6, 12);
				power -= 15;
			}
			return (teams[teams.Count - 1]);
		}

		/**
		 * @brief Pushes ships away from each other to avoid them supperposing.
		 */
		public void AntiSuperposition() {
			foreach (Ship Aship in ships) {
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
		}

		public void CheckAll() {
			// ===' Ships '==='
			for (int i = ships.Count - 1; i >= 0; i -= 1)
				ships[i].Check();
			foreach (Ship AShip in ships)
				AShip.IA(gameplay_random.Next(0, 10000));
			// ===' Shoots '==='
			for (int i = shoots.Count - 1; i >= 0; i -= 1)
				shoots[i].Check();
			// ===' Effets '==='
			foreach (Effect AEffect in effects)
				AEffect.Check();
		}

		public void AutoColide() {
			for (int i_shoot = 0; i_shoot < shoots.Count; i_shoot += 1) {
				Shoot AShoot = shoots[i_shoot];
				foreach (Ship AShip in AShoot.sector.EnumerateNearbyShips()) {
					if (!ReferenceEquals(AShoot.Team, AShip.team) && (AShoot.Team is null || !AShoot.Team.IsFriendWith(AShip.team)))
						if (Helpers.Distance(AShoot.location.X, AShoot.location.Y, AShip.location.X, AShip.location.Y) < AShip.stats.width / 2d) {
							if (AShip.team is object && AShoot.Team is object)
								AShip.team.NotifyEngagement(AShip.location);
							AShoot.time_to_live = 0;
							if (AShip.stats.hot_deflector > 0d && gameplay_random.Next(0, 100) < AShip.stats.hot_deflector)
								effects.Add(new Effect(-1, "EFF_Deflected2", AShoot.location, AShoot.direction, AShip.speed_vec, gameplay_random.Next()));
							else {
								if (AShip.deflectors_loaded > 0)
									effects.Add(new Effect(-1, "EFF_Deflected", AShoot.location, AShoot.direction, AShip.speed_vec, gameplay_random.Next()));
								else if (AShoot.Power < 16)
									effects.Add(new Effect(-1, "EFF_Impact0", AShoot.location, AShoot.direction, AShip.speed_vec, gameplay_random.Next()));
								else if (AShoot.Power < 32)
									effects.Add(new Effect(-1, "EFF_Impact1", AShoot.location, AShoot.direction, AShip.speed_vec, gameplay_random.Next()));
								else if (AShoot.Power < 48)
									effects.Add(new Effect(-1, "EFF_Impact2", AShoot.location, AShoot.direction, AShip.speed_vec, gameplay_random.Next()));
								else
									effects.Add(new Effect(-1, "EFF_Impact3", AShoot.location, AShoot.direction, AShip.speed_vec, gameplay_random.Next()));

								AShip.TakeDamages(AShoot.Power, ref AShoot);
								AShip.last_damager_team = AShoot.Team;
								if (AShip.stats.cold_deflector && AShip.cold_deflector_charge < AShip.stats.integrity * 4)
									effects.Add(new Effect(-1, "EFF_Deflected3", AShoot.location, AShoot.direction, AShip.speed_vec, gameplay_random.Next()));
							}
						}
				}
			}
		}

		public void AutoUnspawn() {
			// Ships
			if (ships.Count > 0)
				for (int i = ships.Count - 1; i >= 0; i -= 1)
					if (ships[i].integrity <= 0) {
						effects.Add(new Effect(-1, "EFF_Destroyed", ships[i].location, ships[i].direction, ships[i].speed_vec, gameplay_random.Next()));
						for (int c = 1, loopTo = (int)(ships[i].stats.width / 8d); c <= loopTo; c++)
							effects.Add(new Effect(gameplay_random.Next(64, 192), "EFF_Debris", ships[i].location, gameplay_random.Next(0, 360), gameplay_random.Next(2, 7), gameplay_random.Next()));
						if (ships[i].weapons.Count > 1 && (ships[i].weapons[0].stats.special & (int)Weapon.SpecialBits.SelfNuke) != 0) {
							nuke_effect = 255;
							for (int c = 1; c <= 256; c++)
								effects.Add(new Effect(gameplay_random.Next(64, 128), "EFF_Debris", ships[i].location, gameplay_random.Next(0, 360), gameplay_random.Next(5, 256), gameplay_random.Next()));
							int FriendlyFireCount = 0;
							foreach (Ship a_ship in ships) {
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
								a_ship.TakeDamages((int)Math.Max(0d, Math.Sqrt(Math.Max(0d, 7000d - Helpers.Distance(ships[i].location.X, ships[i].location.Y, a_ship.location.X, a_ship.location.Y)))), ref argFrom4);
								Shoot argFrom5 = null;
								a_ship.TakeDamages(24, ref argFrom5);
								if (Helpers.Distance(ships[i].location.X, ships[i].location.Y, a_ship.location.X, a_ship.location.Y) < 5500d) {
									Shoot argFrom6 = null;
									a_ship.TakeDamages(10000, ref argFrom6);
								}
								
								if (a_ship.team is object && a_ship.team.affinity == ships[i].team.affinity && a_ship.integrity <= 0 && ReferenceEquals(a_ship.team, ships[i].team))
									FriendlyFireCount += 1;

								if (FriendlyFireCount >= 4) 
									ships[i].team.affinity = AffinityEnum.Hostile;
							}
						}

						if (ships[i].last_damager_team is object)
							ships[i].last_damager_team.resources.AddLoot(ref ships[i].stats.cost);

						this.Unspawn(ships[i]);
					}
			// Shoots
			if (shoots.Count > 0)
				for (int i = shoots.Count - 1; i >= 0; i -= 1)
					if (shoots[i].time_to_live <= 0)
						this.Unspawn(shoots[i]);
			// Effects
			if (effects.Count > 0)
				for (int i = effects.Count - 1; i >= 0; i -= 1)
					if (effects[i].time_to_live <= 0)
						effects.RemoveAt(i);
		}

		public bool HasTeamWon(Team team) {
			foreach (var aShip in ships) {
				if (aShip.team is null || (aShip.team.affinity == AffinityEnum.Neutral))
					continue;
				if (aShip.stats.name.Contains("Station") && !aShip.team.IsFriendWith(team))
					return false;
			}
			return true;
		}
		public bool HasAnyTeamAscended() {
			foreach (Team team in teams) {
				if (team.has_ascended)
					return (true);
			}
			return (false);
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
					AltTeam = teams[rand.Next(2, teams.Count)];
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

				if (is_invaded_by_bosses)
					if (rand.Next(0, 2) == 0) {
						var possibles = new List<string>() { "Loneboss", "Civil_A", "Legend_I", "Legend_L", "Legend_K", "Colonizer" };
						if (teams[0].affinity == AffinityEnum.Hostile) {
							possibles.Add("Nuke");
							possibles.Add("Ambassador");
						}

						if (is_invaded_by_ascended) {
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
					ships.Add(new Ship(ref argworld, Team, Type) { location = new Point(Spawn.X + rand.Next(-4, 5), Spawn.Y + rand.Next(-4, 5)), direction = dir });
				}
			}
		}

		public void NPCUpgrades() {
			var rand = generation_random;
			if (ticks % 80 == 0) {
				// Count Teams's ships
				foreach (Team a_team in teams)
					a_team.ship_count_approximation = 0;
				foreach (Ship a_ship in ships)
					if (a_ship.team is object)
						a_ship.team.ship_count_approximation += 1;
				// Summoning / upgrades
				foreach (Ship a_ship in ships)
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
								if (a_ship.team.affinity == AffinityEnum.Hostile && a_ship.stats.turn == 0.0d)
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

		/* Per Tick */
		public void Tick() {
			UpdateStageDifficulty();
			SpawnDerelictsObjects();
			NPCUpgrades();
			CheckAll();
			UpdateSectors(); // Done after movements. This may be innacurate. This could be done per ship/shoot on every movement.
			AutoColide();
			AntiSuperposition();
			AutoUnspawn();
			foreach (Team team in teams)
				team.Tick();
			ticks += 1;
		}

		// ===' UPGRADES SHIPS '==='
		public bool HaveAnyAffinityWon() {
			bool neutral_alive = false;
			bool friendly_alive = false;
			bool dissident_alive = false;
			bool hostile_alive = false;
			foreach (Ship ship in ships) {
				if (ship.team is object && ship.stats.name.Contains("Station")) {
					if (ship.team.affinity == AffinityEnum.Neutral)
						neutral_alive = true;
					if (ship.team.affinity == AffinityEnum.Friendly)
						friendly_alive = true;
					if (ship.team.affinity == AffinityEnum.Dissident)
						dissident_alive = true;
					if (ship.team.affinity == AffinityEnum.Hostile)
						hostile_alive = true;
				}
			}
			if (!dissident_alive && !hostile_alive) // Friendly won
				return (true);
			if (!friendly_alive && !hostile_alive) // Dissident won
				return (true);
			if (!dissident_alive && !friendly_alive && !neutral_alive) // Hostile won
				return (true);
			return (false);
		}
		public void UpdateStageDifficulty() {
			if (HasAnyTeamAscended()) {
				this.is_invaded_by_ascended = true;
				this.is_invaded_by_bosses = true;
			} else if (HaveAnyAffinityWon()) {
				this.is_invaded_by_ascended = false;
				this.is_invaded_by_bosses = true;
			} else {
				this.is_invaded_by_ascended = false;
				this.is_invaded_by_bosses = false;
			}
		}
		public int CountTeamShips(Team team) {
			int count = 0;
			foreach (Ship aship in ships)
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
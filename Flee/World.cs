﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class World {

		/* Definition */
		public Size ArenaSize = new Size(30000, 30000);
		public int Seed;
		public float difficulty = 1.0f;

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
			foreach (Ship ship in ships) 
				UpdateSector(ship);
			foreach (Shoot shoot in shoots) 
				UpdateSector(shoot);
		}
		public void UpdateSector(Ship ship) {
			Point new_sector_coords = ship.ComputeCurrentSectorCoords();
			if (new_sector_coords == ship.sector_coords)
				return;
			if (ship.sector is object)
				ship.sector.ships.Remove(ship);
			ship.sector_coords = new_sector_coords;
			ship.sector = sectors[ship.sector_coords.X, ship.sector_coords.Y];
			ship.sector.ships.Add(ship);
		}
		public void UpdateSector(Shoot shoot) {
			Point new_sector_coords = shoot.ComputeCurrentSectorCoords();
			if (new_sector_coords == shoot.sector_coords)
				return;
			if (shoot.sector is object)
				shoot.sector.shoots.Remove(shoot);
			shoot.sector_coords = new_sector_coords;
			shoot.sector = sectors[shoot.sector_coords.X, shoot.sector_coords.Y];
			shoot.sector.shoots.Add(shoot);
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
		public World(int seed, float difficulty, bool armagedon) {
			this.difficulty = difficulty;
			InitSectors();
			Random rand = InitRandom(seed);
			InitSpecialTeams();
			InitCharacteristics(new Random(rand.Next()));
			if (armagedon)
				for (int i = 0; i < 4; i++)
					InitTeams(new Random(rand.Next()));
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
		void InitSpecialTeams() {
			// Wilderness
			teams.Add(new Team(this));
			teams[teams.Count - 1].InitWildernessTeam();
			wilderness_team = teams[teams.Count - 1];
			// Boss
			teams.Add(new Team(this));
			teams[teams.Count - 1].InitBossTeam();
			boss_team = teams[teams.Count - 1];
		}
		void InitTeams(Random rand) {
			// NPCs
			{
				teams.Add(new Team(this, AffinityEnum.Friendly, new Random(rand.Next()))); // at least 1 Friendly
				teams.Add(new Team(this, AffinityEnum.Dissident, new Random(rand.Next()))); // at least 1 Dissident
				teams[teams.Count - 1].damage_multiplicator = difficulty;
				teams.Add(new Team(this, AffinityEnum.Hostile, new Random(rand.Next()))); // at least 1 Hostile
				teams[teams.Count - 1].damage_multiplicator = difficulty;
				int additional_team_count = rand.Next(2, 5); // 2, 3 or 4 additional teams
				for (int i = 0; i < additional_team_count; i++) {
					teams.Add(new Team(this, RandomNPCAffinity(rand), new Random(rand.Next())));
					teams[teams.Count - 1].damage_multiplicator = difficulty;
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
			// Statics
			for (int i = 0, loopTo = rand.Next(2, 5); i <= loopTo; i++) {
				ShipStats static_type = Loader.RandomShipFromRole(rand, (int)ShipRole.Static);
				var location = new PointF(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Height));
				double direction = rand.Next(0, 360);
				for (int j = 0, loopTo1 = rand.Next(static_type.spawning_amount_min, static_type.spawning_amount_max); j < loopTo1; j++) {
					ships.Add(new Ship(this, this.wilderness_team, static_type.name) { location = new PointF(location.X + rand.Next(-4, 5), location.Y + rand.Next(-4, 5)), direction = (float)direction });
				}
			}
			// Derelicts
			for (int i = 1; i <= 60; i++) {
				ShipStats derelict_type = Loader.RandomShipFromRole(rand, (int)ShipRole.Derelict);
				var location = new PointF(rand.Next(0, ArenaSize.Width), rand.Next(0, ArenaSize.Height));
				double direction = rand.Next(0, 360);
				for (int j = 0, loopTo1 = rand.Next(derelict_type.spawning_amount_min, derelict_type.spawning_amount_max); j < loopTo1; j++) {
					ships.Add(new Ship(this, this.wilderness_team, derelict_type.name) { location = new PointF(location.X + rand.Next(-4, 5), location.Y + rand.Next(-4, 5)), direction = (float)direction });
				}
			}
		}
		public void InitBots(Random rand) {
			foreach (Team team in teams)
				if (team.bot_team && !ReferenceEquals(team, wilderness_team) && !ReferenceEquals(team, boss_team))
					SpawnStations(new Random(rand.Next()), null, team, rand.Next(6, 12));
		}
		public Point SpawnStations(Random rand, ShipStats main_type, Team team, int spawn_allies) {
			// spawn main station
			if (main_type is null)
				main_type = Loader.RandomShipFromRole(rand, (int)ShipRole.Shipyard | (int)ShipRole.NPC);
			var main_coords = new Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000));
			for (int index = 0, loopTo = rand.Next(main_type.spawning_amount_min, main_type.spawning_amount_max); index < loopTo; index++)
				if (index == 0) {
					ships.Add(new Ship(this, team, main_type.name) { location = main_coords });
				} else {
					ships.Add(new Ship(this, team, main_type.name) { location = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513)) });
				}
			// spawn additional station
			if (rand.Next(0, 2) == 0) {
				if (team.bot_team && team.affinity != AffinityEnum.Friendly && team.affinity != AffinityEnum.Neutral) {
					ships.Add(new Ship(this, team, main_type.name) { location = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513)) });
				}
			}
			// spawn turrets
			if (main_type.speed < 0.01) {
				while (spawn_allies > 0) {
					string ally_type = Loader.RandomShipFromRole(rand, (int)ShipRole.Defense).name;
					var ally_coords = new Point(main_coords.X + rand.Next(-512, 513), main_coords.Y + rand.Next(-512, 513));
					ships.Add(new Ship(this, team, ally_type) { location = ally_coords });
					spawn_allies -= 1;
				}
			}
			return (main_coords);
		}
		public Team CreateAndSpawnPlayer(AffinityEnum affinity) {
			Random rand = new Random(gameplay_random.Next());
			// Add Team
			Team player_team = new Team(this);
			player_team.InitPlayerTeam(affinity);
			teams.Add(player_team);
			player_team.damage_multiplicator = (1.0f / difficulty);
			// Player Station
			ShipStats station_type = Loader.RandomShipFromRole(rand, (int)ShipRole.Shipyard | (int)ShipRole.Playable);
			PointF origin = SpawnStations(new Random(rand.Next()), station_type, player_team, 3);
			// new Point(rand.Next(1000, ArenaSize.Width - 1000), rand.Next(1000, ArenaSize.Height - 1000));
			// Player Starters
			for (int i = 0; i < rand.Next(4, 6); i++) {
				string starter_type = Loader.RandomShipFromRole(rand, (int)ShipRole.Starter | (int)ShipRole.Playable).name;
				ships.Add(new Ship(this, player_team, starter_type) { location = new Point((int)(origin.X + (i % 3 - 1)), (int)origin.Y + (i / 2 % 3 - 1)) });
				ships[ships.Count - 1].direction = (float)Helpers.GetQA((int)origin.X, (int)origin.Y, (int)origin.X, (int)origin.Y);
				ships[ships.Count - 1].upgrade_slots += rand.Next(0, 4);
				if (ships[ships.Count - 1].upgrades.Count > ships[ships.Count - 1].upgrade_slots)
					ships[ships.Count - 1].upgrade_slots = ships[ships.Count - 1].upgrades.Count;
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
								PointF a_to_b = new PointF(Bship.location.X - Aship.location.X, Bship.location.Y - Aship.location.Y);
								if (Bship.stats.speed != 0 || Aship.stats.speed == 0) {
									if (Bship.stats.turn == 0.0f && Bship.stats.speed != 0.0f && Aship.stats.turn == 0.0f && Aship.stats.speed != 0.0f) {
										Bship.location.X += a_to_b.X * 0.01f;
										Bship.location.Y += a_to_b.Y * 0.01f;
									}
									else
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
			for (int i = ships.Count - 1; i >= 0; i -= 1)
				ships[i].AI();
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
					if (!ReferenceEquals(AShoot.Team, AShip.team) && (!AShoot.Team.IsFriendWith(AShip.team)))
						if (Helpers.Distance(AShoot.location.X, AShoot.location.Y, AShip.location.X, AShip.location.Y) < AShip.stats.width / 2d) {
							if (AShoot.Team.affinity != AffinityEnum.Wilderness)
								AShip.team.NotifyEngagement(AShip.location);
							AShoot.time_to_live = 0;
							AShip.TakeDamages(AShoot.power, AShoot);
							AShip.last_damager_team = AShoot.Team;
						}
				}
			}
		}

		public void AutoUnspawn() {
			// Ships
			if (ships.Count > 0)
				for (int i = ships.Count - 1; i >= 0; i -= 1)
					if (ships[i].IsDestroyed()) {
						if (ships[i].lifespan > 0 || ships[i].weapons.Count > 0 && (ships[i].weapons[0].stats.special & (int)Weapon.SpecialBits.SelfExplode) != 0) {
							effects.Add(new Effect(-1, "EFF_Destroyed", ships[i].location, ships[i].direction, ships[i].speed_vec, gameplay_random.Next()));
							for (int c = 0, loopTo = ships[i].stats.width / 8 - 1; c < loopTo; c++)
								effects.Add(new Effect(gameplay_random.Next(64, 192), "EFF_Debris", ships[i].location, gameplay_random.Next(0, 360), gameplay_random.Next(2, 7), gameplay_random.Next()));
						}
						if (ships[i].weapons.Count > 1 && (ships[i].weapons[0].stats.special & (int)Weapon.SpecialBits.SelfNuke) != 0) {
							nuke_effect = 255;
							for (int c = 1; c <= 256; c++)
								effects.Add(new Effect(gameplay_random.Next(64, 128), "EFF_Debris", ships[i].location, gameplay_random.Next(0, 360), gameplay_random.Next(5, 256), gameplay_random.Next()));
							int FriendlyFireCount = 0;
							foreach (Ship a_ship in ships) {
								a_ship.shield = 0;
								a_ship.deflectors = -a_ship.stats.cold_deflectors;
								a_ship.TakeDamages(8, null);
								a_ship.TakeDamages(8, null);
								a_ship.TakeDamages(8, null);
								a_ship.TakeDamages(8, null);
								a_ship.TakeDamages((int)Math.Max(0d, Math.Sqrt(Math.Max(0d, 7000d - Helpers.Distance(ships[i].location.X, ships[i].location.Y, a_ship.location.X, a_ship.location.Y)))), null);
								a_ship.TakeDamages(24, null);
								if (Helpers.Distance(ships[i].location.X, ships[i].location.Y, a_ship.location.X, a_ship.location.Y) < 5500d) {
									a_ship.TakeDamages(10000, null);
								}
								if (a_ship.team.affinity == ships[i].team.affinity && a_ship.integrity <= 0 && ReferenceEquals(a_ship.team, ships[i].team))
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
				if (aShip.team.affinity == AffinityEnum.Wilderness || aShip.team.affinity == AffinityEnum.Neutral)
					continue;
				if ((aShip.stats.role & (int)ShipRole.Shipyard) != 0 && !aShip.team.IsFriendWith(team))
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

		public Point RandomBorderSpawnPoint(Random rand) {
				switch (rand.Next(0, 4)) {
				case 0: return (new Point(rand.Next(50, ArenaSize.Width - 50), 0)); // top
				case 1: return (new Point(rand.Next(50, ArenaSize.Width - 50), ArenaSize.Height)); // bottom
				case 2: return (new Point(0, rand.Next(50, ArenaSize.Height - 50))); // left
				case 3: return (new Point(ArenaSize.Width, rand.Next(50, ArenaSize.Height - 50))); // right
				default: throw new Exception("impossible");
				}
		}
		public void SpawnDerelictsAndBosses() {
			if (ticks % 128 == 0) {
				// spawn Location
				Point spawn = RandomBorderSpawnPoint(generation_random);
				// direction
				float dir = 0f;
				dir = (float)(Helpers.GetQA(spawn.X, spawn.Y, (int)(ArenaSize.Width / 2d), (int)(ArenaSize.Height / 2d)) + generation_random.Next(-75, 75));
				// type and team
				ShipStats type;
				Team team = wilderness_team;
				if (is_invaded_by_bosses && generation_random.Next(0, 5) > 0)
					type = Loader.RandomShipFromRole(generation_random, (int)ShipRole.Derelict);
				else if (!is_invaded_by_bosses && generation_random.Next(0, 33) > 0)
					type = Loader.RandomShipFromRole(generation_random, (int)ShipRole.Derelict);
				else {
					type = Loader.RandomShipFromRole(generation_random, (int)ShipRole.Boss);
					team = boss_team;
				}
				// spawning the right amount
				for (int j = 0, loopTo = generation_random.Next(type.spawning_amount_min, type.spawning_amount_max); j < loopTo; j++) {
					ships.Add(new Ship(this, team, type.name) { location = new Point(spawn.X + generation_random.Next(-4, 5), spawn.Y + generation_random.Next(-4, 5)), direction = dir });
				}
			}
		}

		public void UpdateTeamsShipCounts() {
				foreach (Team a_team in teams)
					a_team.ship_count_approximation = 0;
				foreach (Ship a_ship in ships) {
					if (!a_ship.auto && (a_ship.stats.role & (int)ShipRole.Shipyard) == 0)
						a_ship.team.ship_count_approximation += 1;
					if (a_ship.Upgrading != null)
						a_ship.team.ship_count_approximation += a_ship.Upgrading.required_team_slots;
				}
		}
		public void NPCUpgrades() {
			var rand = generation_random;
			if (ticks % 128 == 0) {
				// Count Teams's ships
				UpdateTeamsShipCounts();
				// Summoning / upgrades
				for (int i_ship = ships.Count - 1; i_ship >= 0; i_ship--) {
					Ship a_ship = ships[i_ship];
					if (a_ship.team.affinity != AffinityEnum.Wilderness && a_ship.bot_ship && a_ship.upgrade_progress == 0 && a_ship.team.ship_count_approximation < a_ship.team.ship_count_limit) {
						string wished_upgrade = null;
						if (rand.Next(0, 16) < 8)
							if (rand.Next(0, 2) == 0) { 
								// build ships
								wished_upgrade = Loader.GetRandomSpawnUpgrade(rand, a_ship);
								if (wished_upgrade is object)
									a_ship.UpgradeForFree(wished_upgrade);
							} else if (rand.Next(0, 3) == 0) {
								// upgrading
								var PossibleUps = a_ship.AvailableNotInstalledUpgrades();
								if (PossibleUps.Count >= 1)
									a_ship.Upgrading = PossibleUps[rand.Next(0, PossibleUps.Count)];
							} else {
								// ships equiped with cold deflectors jumps when unable to fire
								if (a_ship.deflectors < 0)
									if (a_ship.CanUpgradeFree("Jump"))
										a_ship.UpgradeForFree("Jump");
									else if (a_ship.CanUpgradeFree("Jump_II"))
										a_ship.UpgradeForFree("Jump_II");
									else if (a_ship.CanUpgradeFree("Warp"))
										a_ship.UpgradeForFree("Warp");
								// ship jumping when not in good shape
								if (a_ship.integrity + a_ship.shield < (a_ship.stats.integrity + a_ship.stats.shield) / 3d)
									if (a_ship.CanUpgradeFree("Jump"))
										a_ship.UpgradeForFree("Jump");
									else if (a_ship.CanUpgradeFree("Jump_II"))
										a_ship.UpgradeForFree("Jump_II");
									else if (a_ship.CanUpgradeFree("Warp"))
										a_ship.UpgradeForFree("Warp");
							}
					}
				}
			}
		}

		/* Per Tick */
		public void Tick() {
			UpdateStageDifficulty();
			SpawnDerelictsAndBosses();
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
				if ((ship.stats.role & (int)ShipRole.Shipyard) != 0) {
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
		public int CountTeamShips(Team team) { // TODO: reverify
			int count = 0;
			foreach (Ship aship in ships) {
				if (ReferenceEquals(aship.team, team)) {
					if (aship.auto)
						continue;
					if ((aship.stats.role & (int)ShipRole.Shipyard) != 0)
						continue;
					count += 1;
					// TODO: count being built ships
				}
			}
			return (count);
		}
	}
}
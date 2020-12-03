using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class Ship : WorldEntity {

		// mode of behavior
		public enum BehaviorMode {
			None,
			Stand,
			Drift,
			Folow,
			Mine,
			GoToPoint
		}

		// IA
		public bool bot_ship = true;
		public bool AllowMining = true;
		public BehaviorMode behavior = BehaviorMode.Stand;
		public Ship target = null;
		public PointF TargetPTN = new PointF(0f, 0f);
		public double agressivity = 1.0d;

		// shield effect
		public const int SHIELD_POINTS = 16;
		public int[] ShieldPoints = new int[16];

		public void ResetShieldPoint() {
			for (int shield_ptn_value = 0; shield_ptn_value <= SHIELD_POINTS - 1; shield_ptn_value++)
				ShieldPoints[shield_ptn_value] = Math.Max(ShieldPoints[shield_ptn_value], 128);
		}

		// main
		private ShipStats base_stats = null;
		public ShipStats stats = null;
		public Team team = null;
		public Color color = Color.White;
		public ushort fram = 0;

		// state
		public int integrity = 20;
		public int cold_deflector_charge = 0;
		public int deflectors_loaded = 0;
		public int deflector_loading = 0;
		public float shield = 0f;
		public List<Weapon> weapons = new List<Weapon>();
		public Team last_damager_team = null;

		// creation
		public Ship(ref World world) : base(ref world) {
			ResetShieldPoint();
			TargetPTN = new PointF(location.X, location.Y);
			UpdateSector();
		}

		public Ship(ref World world, string ship_class) : base(ref world) {
			SetStats(ship_class);
			ResetShieldPoint();
			TargetPTN = new PointF(location.X, location.Y);
			UpdateSector();
		}

		public Ship(ref World world, Team team, string ship_class) : base(ref world) {
			SetTeam(team);
			SetStats(ship_class);
			ResetShieldPoint();
			TargetPTN = new PointF(location.X, location.Y);
			UpdateSector();
		}

		public void SetStats(string ship_class) {
			SetStats(ShipStats.classes[ship_class]);
		}

		public void SetStats(ShipStats stats) {
			if (!ReferenceEquals(base_stats, stats)) {
				base_stats = stats;
				// native upgrades
				if (Ups.Count == 0)
					foreach (string native_upgrade_name in base_stats.native_upgrades)
						Ups.Add(Upgrade.UpgradeFromName(native_upgrade_name));
				// upgrade_slots
				if (upgrade_slots < 0) { // initial value is -1
					upgrade_slots = base_stats.level;
					if (base_stats.repair > 0)
						if (team is object)
							upgrade_slots += team.upgrade_slots_bonus;
				}
				// Reset Weapons
				weapons.Clear();
				foreach (string gun_name in base_stats.default_weapons)
					weapons.Add(new Weapon(this, gun_name));
				// force a color
				switch (base_stats.name ?? "") {
				case "Star": {
					behavior = BehaviorMode.Drift;
					target = null;
					color = Color.FromArgb(255, 255, 220);
					break;
				}

				case "Asteroid": {
					behavior = BehaviorMode.Drift;
					target = null;
					color = Color.FromArgb(64, 64, 48);
					break;
				}

				case "Meteoroid": {
					behavior = BehaviorMode.Drift;
					target = null;
					color = Color.FromArgb(80, 48, 80);
					break;
				}

				case "Comet": {
					behavior = BehaviorMode.Drift;
					target = null;
					color = Color.FromArgb(0, 100, 0);
					break;
				}
				}
				// 
				ResetStats();
			}
		}

		public void SetTeam(Team team) {
			if (!ReferenceEquals(this.team, team)) {
				this.team = team;
				if (this.team is object) {
					bot_ship = this.team.bot_team;
					color = team.color;
				}
			}
		}

		public void ResetStats() {
			if (stats is null) {
				integrity = base_stats.integrity;
				shield = base_stats.shield;
			}

			stats = base_stats.Clone();
			foreach (Weapon weapon in weapons)
				weapon.ResetStats();
		}

		// TODO: Remove
		public void TempForceUpdateStats() {
			if (base_stats is null) {
				base_stats = stats.Clone();
				base_stats.default_weapons.Clear();
				foreach (Weapon a_weapon in weapons)
					base_stats.default_weapons.Add(a_weapon.ToString());
				if (!ShipStats.classes.ContainsKey(base_stats.name))
					ShipStats.classes[base_stats.name] = base_stats.Clone();
			}
		}

		public void SetType(string SType, Team STeam, bool IsNew = false) {
			if (base_stats is null)
				SetStats(SType);

			SetTeam(STeam);
			// Ship just created TODO: NOW: move to constructor
			if (IsNew) {
				Upgrading = null;
				// state
				integrity = stats.integrity;
				shield = stats.shield;
				if (STeam is null || STeam.id == 0)
					bot_ship = false;
				else
					bot_ship = true;

				if (STeam is object && behavior != BehaviorMode.Drift)
					color = STeam.color;

				if (team is object && upgrade_slots > 0)
					upgrade_slots += team.upgrade_slots_bonus;
			}
		}

		public void Check() {
			// ===' Fram '==='
			fram = (ushort)(fram + 1);
			if (fram > 7)
				fram = 0;
			// ===' Bordures '==='
			if (team is object && team.id == 0) {
				if (location.X < 0f)
					location.X = 0f;

				if (location.Y < 0f)
					location.Y = 0f;

				if (location.X > world.ArenaSize.Width)
					location.X = world.ArenaSize.Width;

				if (location.Y > world.ArenaSize.Height)
					location.Y = world.ArenaSize.Height;
			} else if (location.X < -100 || location.Y < -100 || location.X > world.ArenaSize.Width + 100 || location.Y > world.ArenaSize.Height + 100)
				if (bot_ship == false)
					integrity = 0;
			// movement
			if (stats.turn == 0d && stats.speed == 0d)
				direction = (float)(direction + 25d / stats.width);
			else {
				var new_speed = Helpers.GetNewPoint(new Point(0, 0), direction, speed);
				speed_vec = new PointF((float)(speed_vec.X * 0.925d + new_speed.X * 0.075d), (float)(speed_vec.Y * 0.925d + new_speed.Y * 0.075d));
				location.X = location.X + speed_vec.X;
				location.Y = location.Y + speed_vec.Y;
			}
			// ===' Armes '==='
			if (cold_deflector_charge <= 0)
				foreach (Weapon AWeapon in weapons)
					if (AWeapon.Bar == 0) {
						AWeapon.Load = AWeapon.Load + 1;
						if (AWeapon.Load >= AWeapon.stats.loadtime) {
							AWeapon.Load = 0;
							AWeapon.Bar = AWeapon.stats.salvo;
						}
					}
			// ===' Shield '==='
			if (stats.shield > 0) {
				shield = (float)(shield + stats.shield_regeneration / 100d);
				if (shield > stats.shield)
					shield = stats.shield;
				float point_min = Math.Max(0f, shield * 32f / stats.shield);
				for (int i = 0; i <= SHIELD_POINTS - 1; i++) {
					ShieldPoints[i] -= 4;
					if (ShieldPoints[i] < point_min)
						ShieldPoints[i] = (int)point_min;
				}
			}
			// ===' Deflector '==='
			if (cold_deflector_charge > 0) {
				cold_deflector_charge = (int)(cold_deflector_charge * 0.999d);
				if ((world.ticks % 2) != 0) {
					integrity = (int)(integrity - Math.Max(1d, cold_deflector_charge / 16d));
					cold_deflector_charge = (int)(cold_deflector_charge - Math.Max(1d, cold_deflector_charge / 16d));
				}
			}

			if (deflectors_loaded < stats.deflectors) {
				deflector_loading -= 1;
				if (deflector_loading <= 0) {
					deflectors_loaded += 1;
					deflector_loading = stats.deflectors_cooldown;
				}
			} else
				deflector_loading = stats.deflectors_cooldown;
			// ===' Vie '==='
			if (world.ticks % 40 == 0)
				if (team is object) {
					integrity = integrity + stats.repair;
					if (integrity > stats.integrity)
						integrity = stats.integrity;
				}
			// ===' Upgrades '==='
			if (Upgrading is object)
				if (UpProgress < (decimal)Upgrading.delay) {
					UpProgress = UpProgress + 1;
					if (MainForm.cheats_enabled)
						UpProgress = UpProgress + 99;
				} else {
					if (Upgrading.name.Contains("Pointvortex"))
						Console.WriteLine();

					if (Upgrading.upgrade_slots_requiered > 0)
						Ups.Add(Upgrading);
					// Appliquation debugage 'TODO: NOW: test without this
					// Dim spliter() As String = Upgrading.Effect.Split(" ")
					// For Each aspli As String In spliter
					// Me.ApplyUpgradeEffect(aspli, True)
					// Next
					// actualisation vaisseau
					ApplyUpgradeFirstTime(ref Upgrading);
					ResetStats();
					ApplyUpgrades();
					Upgrading = null;
					UpProgress = 0;
				}
			// ===' Autre '==='

		}

		public void TurnToQA(float qa) {
			while (qa > 360f)
				qa -= 360f;
			while (qa < 0f)
				qa += 360f;
			while (direction > 360f)
				direction = direction - 360f;
			while (direction < 0f)
				direction = direction + 360f;
			if (Helpers.GetAngleDiff(direction, qa) < stats.turn) {
				direction = qa;
				return;
			} else if (qa > 180f)
				if (direction > qa - 180f && direction < qa)
					direction = (float)(direction + stats.turn);
				else
					direction = (float)(direction - stats.turn);
			else if (direction < qa + 180f && direction > qa)
				direction = (float)(direction - stats.turn);
			else
				direction = (float)(direction + stats.turn);
		}

		public void TakeDamages(int Amount, [Optional, DefaultParameterValue(null)] ref Shoot From) {
			if (deflectors_loaded > 0) {
				deflectors_loaded -= 1;
				return;
			}

			if (shield > 0f) {
				shield = shield - Amount;
				Amount = (int)(Amount - Amount * stats.shield_opacity / 100d);
				if (From is object) {
					double angle_ship_shoot_rel = Helpers.NormalizeAngleUnsigned(Helpers.GetAngle(location.X, location.Y, From.location.X, From.location.Y) - direction);
					int shield_ptn_index = (int)(angle_ship_shoot_rel * 16d / 360d);
					ShieldPoints[shield_ptn_index % 16] = 255;
					if (ShieldPoints[(shield_ptn_index + 1) % 16] < 128)
						ShieldPoints[(shield_ptn_index + 1) % 16] = 128;
					if (ShieldPoints[(shield_ptn_index + 15) % 16] < 128)
						ShieldPoints[(shield_ptn_index + 15) % 16] = 128;
				}
			}

			if (stats.cold_deflector && cold_deflector_charge < integrity * 4) {
				cold_deflector_charge = (int)(cold_deflector_charge + Amount * 7 / 8d);
				Amount = (int)(Amount / 8d);
			}

			if (From is object && From.Team is object)
				if (stats.sprite == "Star")
					From.Team.resources.Antimatter = (long)(From.Team.resources.Antimatter + Amount / 8d);
				else if (stats.sprite == "Asteroid")
					if (integrity > Amount)
						From.Team.resources.Metal += Amount;
					else if (integrity > 0)
						From.Team.resources.Metal += integrity;

			if (Amount < 0)
				return;
			integrity -= Amount;
			if (integrity < 0)
				integrity = 0;
		}

		public void IA(int rnd_num) {
			var QA = default(float);
			bool NeedSpeed = false;
			// remove destroyed target
			if (target is object && target.IsDestroyed())
				target = null;
			// ===' Fin de poursuite '==='
			if (behavior == BehaviorMode.Folow && target is null)
				behavior = BehaviorMode.Stand;
			// no team mean drifting
			if (team is null)
				behavior = BehaviorMode.Drift;
			// ===' Auto-Activation '==='
			if (rnd_num < 100)
				agressivity += 0.05d;

			if (bot_ship && behavior != BehaviorMode.Drift && team is object)
				if (target is null) {
					var nearest_ship = GetClosestShip(agressivity, 1.0d, 0.1d);
					if (nearest_ship is object) {
						target = nearest_ship;
						behavior = BehaviorMode.Folow;
					}
				} else if (rnd_num < 6 || !ReferenceEquals(team, target.team) && rnd_num < 100) { // chance to change target
					target = null;
					behavior = BehaviorMode.Stand;
				}
			// ===' Execution '==='
			switch (behavior) {
			case BehaviorMode.Mine: {
				AllowMining = true;
				if (target is object) {
					// has mining target already
					QA = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)target.location.X, (int)target.location.Y);
					double optimal_range = 50d;
					if (weapons.Count > 0)
						optimal_range = weapons[0].stats.range * 0.5d;
					double rel_dist = Helpers.Distance(ref location, ref target.location) - target.stats.width / 2d;
					if (rel_dist <= optimal_range) {
						// turn if too close
						if (Helpers.GetAngleDiff(direction, QA) < 120d)
							QA = QA + 180f;

						NeedSpeed = true;
					} else
						NeedSpeed = Helpers.GetAngleDiff(direction, QA) < 90d;

					if (Helpers.Distance(ref TargetPTN, ref target.location) > world.ArenaSize.Width / 8d)                         // abort target if too far away from mining point
						target = null;
				} else {
					// no current mining target
					int max_mining_distance = (int)(world.ArenaSize.Width / 8d);
					var mining_target = GetClosestShip(0.0d, 1.0d);
					if (mining_target is object && Helpers.Distance(ref TargetPTN, ref mining_target.location) > max_mining_distance)
						mining_target = null;

					target = mining_target;
					if (target is null) {
						QA = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)TargetPTN.X, (int)TargetPTN.Y);
						NeedSpeed = true;
					}
				}

				break;
			}

			case BehaviorMode.Folow: {
				if (target is object) {
					var forseen_location = ForseeLocation(target);
					// world.Effects.Add(New Effect With {.Type = "EFF_OrderTarget", .Coo = forseen_location, .Direction = 45, .speed = 0})
					QA = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)forseen_location.X, (int)forseen_location.Y);
					double rel_forseen_dist = Helpers.Distance(ref location, ref forseen_location) - target.stats.width / 2d;
					double optimal_range = 50d;
					if (weapons.Count > 0)
						optimal_range = weapons[0].stats.range * weapons[0].stats.range / rel_forseen_dist * 0.8d; // TODO: instead of this factor, just use the forseen location of the target
					if (Helpers.Distance(ref location, ref target.location) <= optimal_range)
						if (Helpers.GetAngleDiff(direction, QA) < 112d) {
							QA = QA + 180f;
							NeedSpeed = Helpers.GetAngleDiff(direction, QA - 180f) > 45d;
						} else
							NeedSpeed = Helpers.GetAngleDiff(direction, QA) > 90d;
					else {
						if (target.stats.speed * 1.1d >= stats.speed) {
							double angle_to_target = Helpers.GetQA((int)location.X, (int)location.Y, (int)target.location.X, (int)target.location.Y);
							if (Helpers.GetAngleDiff(angle_to_target, QA) > 90d)
								QA = (float)angle_to_target;
						}

						NeedSpeed = Helpers.GetAngleDiff(direction, QA) < 90d;
					}
					// NeedSpeed = True
				}

				break;
			}

			case BehaviorMode.Stand: {
				QA = direction;
				NeedSpeed = false;
				break;
			}

			case BehaviorMode.Drift: {
				QA = direction;
				NeedSpeed = true;
				break;
			}

			case BehaviorMode.GoToPoint: {
				QA = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)TargetPTN.X, (int)TargetPTN.Y);
				if (Helpers.Distance(location.X, location.Y, TargetPTN.X, TargetPTN.Y) <= 50d)
					behavior = BehaviorMode.Stand;

				NeedSpeed = true;
				break;
			}
			}
			// ===' Appliquation '==='
			TurnToQA(QA);
			if (NeedSpeed)
				speed = (float)(speed + stats.turn / 20d);
			else
				speed = (float)(speed - stats.turn / 20d);

			if (speed > stats.speed) {
				speed -= 1f;
				if (speed < stats.speed)
					speed = (float)stats.speed;
			}

			if (speed < 0f)
				speed = 0f;
			IAFire();
		} // AIAIAIAIAI

		public void IAFire() {
			// ===' Tirer '==='
			if (weapons.Count > 0 && fram % 2 == 0)
				foreach (Weapon AWeap in weapons)
					if (AWeap.Bar > 0) {
						double closest_score = double.MaxValue;
						Ship weapon_targeted_ship = null; // Pas de cible
						int weapon_location_x = (int)(Math.Sin(2d * Math.PI * (AWeap.Loc + direction) / 360d) * (stats.width / 2d) + location.X);
						int weapon_location_y = (int)(Math.Cos(2d * Math.PI * (AWeap.Loc + direction) / 360d) * (stats.width / 2d) + location.Y);
						// target in range
						if (target is object)
							if (team is null || !team.IsFriendWith(target.team)) {
								var argp2 = AWeap.ForseeShootingLocation(target);
								double dist = Helpers.Distance(ref location, ref argp2) - target.stats.width / 2d;
								if (dist < AWeap.stats.range * 0.9d)
									weapon_targeted_ship = target;
							}
						// target not in range, find another
						if (weapon_targeted_ship is null)
							foreach (Ship OVessel in this.sector.EnumerateNearbyShips()) {
								if (ReferenceEquals(OVessel, this))
									continue;

								if (!AllowMining && (OVessel.stats.default_weapons.Count == 0 || AWeap.stats.power == 0))
									continue;

								if (Helpers.Distance(ref location, ref OVessel.location) < AWeap.stats.range)
									if (team is null || !team.IsFriendWith(OVessel.team)) {
										// Dim dist As Integer = Helpers.Distance(ToX, ToY, OVessel.location.X, OVessel.location.Y) - OVessel.stats.width / 2
										var argp21 = AWeap.ForseeShootingLocation(OVessel);
										double score = Helpers.Distance(ref location, ref argp21) - OVessel.stats.width / 2d;
										if (score < AWeap.stats.range * 0.9d) {
											if (score < AWeap.stats.range) {
												if (team is null || OVessel.team is object && !team.IsFriendWith(OVessel.team))
													score /= 8d;

												if (ReferenceEquals(target, OVessel))
													score /= 4d;
											}

											if (score < closest_score) {
												closest_score = score;
												weapon_targeted_ship = OVessel;
											}
										}
									}
							}
						// shooting if in range
						if (weapon_targeted_ship is object) {
							var weapon_target_point = AWeap.ForseeShootingLocation(weapon_targeted_ship);
							int QA = (int)Helpers.GetQA(weapon_location_x, weapon_location_y, (int)weapon_target_point.X, (int)weapon_target_point.Y);
							AWeap.Fire(QA, new Point(weapon_location_x, weapon_location_y), this);
							if ((AWeap.stats.special & (int)Weapon.SpecialBits.SelfExplode) != 0)
								integrity = -2048;
						}
					}
		}

		// get updates to display
		public static object ListedUpgrades(List<Ship> ships) {
			var met_upgrades = new List<Upgrade>();
			foreach (Upgrade upgrade in Upgrade.upgrades)
				if (CountShipsListingUpgrade(ships, upgrade) == ships.Count)
					met_upgrades.Add(upgrade);

			return met_upgrades;
		}

		// get all possible upgrades
		public List<Upgrade> ConditionsMetUpgrades() {
			var met_upgrades = new List<Upgrade>();
			foreach (Upgrade upgrade in Upgrade.upgrades)
				if (IsUpgradeCompatible(upgrade) || Ups.Contains(upgrade))
					met_upgrades.Add(upgrade);

			return met_upgrades;
		}
		// get all possible upgrades
		public List<Upgrade> AvailableUpgrades() {
			var possible_upgrades = new List<Upgrade>();
			foreach (Upgrade upgrade in Upgrade.upgrades)
				if (CanUpgrade(upgrade) && (!bot_ship || !upgrade.not_for_bots))
					possible_upgrades.Add(upgrade);

			return possible_upgrades;
		}
		// return true if the upgrade is available
		public bool CanUpgrade(Upgrade upgrade) {
			if (MainForm.cheats_enabled)
				return true;
			if (Upgrading is object)
				return false;
			if (upgrade_slots - Ups.Count < upgrade.upgrade_slots_requiered)
				return false;
			if (upgrade.not_for_bots && bot_ship)
				return false;
			if (HaveUp(ref upgrade))
				return false;
			return IsUpgradeCompatible(upgrade);
		}
		// return true if the upgrade have its conditions met
		public bool IsUpgradeCompatible(Upgrade upgrade) {
			if (MainForm.cheats_enabled)
				return true;
			if (ReferenceEquals(upgrade, Upgrading))
				return true;
			if (upgrade.spawned_ship is object)
				if (!stats.crafts.Contains(upgrade.spawned_ship))
					return false;

			var conditions_strings = upgrade.need.Split(' ');
			foreach (string a_condition in conditions_strings)
				if (!IsUpgradeConditionMet(a_condition))
					return false;

			return true;
		}

		public static int CountShipsListingUpgrade(List<Ship> ships, Upgrade upgrade) {
			int count = 0;
			foreach (Ship ship in ships)
				if (ship.IsUpgradeCompatible(upgrade) || ship.Ups.Contains(upgrade))
					count += 1;

			return count;
		}

		public static int CountShipsBuyableNowUpgrade(List<Ship> ships, Upgrade upgrade) {
			int count = 0;
			foreach (Ship ship in ships)
				if (ship.Upgrading is null)
					if (ship.IsUpgradeCompatible(upgrade) && !ship.Ups.Contains(upgrade) && ship.upgrade_slots - ship.Ups.Count - upgrade.upgrade_slots_requiered >= 0)
						count += 1;

			return count;
		}

		public static int CountShipsHavingUpgrade(List<Ship> ships, Upgrade upgrade) {
			int count = 0;
			foreach (Ship ship in ships)
				if (ship.Ups.Contains(upgrade))
					count += 1;

			return count;
		}
		// test a single upgrade condition
		public bool IsUpgradeConditionMet(string chain) {
			var Spliter = chain.Split(':');
			switch (Spliter[0] ?? "") { // Debug
			case var @case when @case == "": {
				return true;
			}

			case "?W": {
				if (weapons.Count > 0)
					return true;
				break;
			}

			case "?S": {
				if (stats.speed > 0d)
					return true;
				break;
			}

			case "?Base": {
				if (stats.name.Contains("Station"))
					return true;
				break;
			}

			case "?NotStation": {
				if (!stats.name.Contains("Station"))
					return true;
				break;
			}

			case "+Lvl": {
				if (stats.level >= Helpers.ToDouble(Spliter[1]))
					return true;
				break;
			}

			case "+Speed": { // vitesse
				if (stats.speed >= Helpers.ToDouble(Spliter[1]))
					return true;
				break;
			}

			case "-Speed": {
				if (stats.speed <= Helpers.ToDouble(Spliter[1]))
					return true;
				break;
			}

			case "+Life": { // Resistance
				if (stats.integrity >= Helpers.ToDouble(Spliter[1]))
					return true;
				break;
			}

			case "-Life": { // 
				if (stats.integrity <= Helpers.ToDouble(Spliter[1]))
					return true;
				break;
			}

			case "?Up": {
				var argupgrade = Upgrade.UpgradeFromName(Spliter[1]);
				if (HaveUp(ref argupgrade))
					return true;
				break;
			}

			case "?Type": { // Type
				if ((stats.sprite ?? "") == (Spliter[1] ?? ""))
					return true;
				break;
			}

			case "?Wtype": { // armement
				if ((weapons[0].stats.sprite ?? "") == (Spliter[1] ?? ""))
					return true;
				break;
			}

			case "?MS": {
				if (team is null || world.CountTeamShips(team) < team.ship_count_limit)
					return true;
				break;
			}

			default: {
				throw new Exception("Erreur : " + chain + " (invalid condition)");
				break;
			}
			}

			return false;
		}

		// get the minimum loading progress of an upgrade for a list of ship, or int.MaxValue if not being upgraded
		public static int MinUpgradeProgress(List<Ship> ships, Upgrade upgrade) {
			int min = int.MaxValue;
			foreach (Ship ship in ships)
				if (ReferenceEquals(ship.Upgrading, upgrade))
					min = Math.Min(min, ship.UpProgress);

			return min;
		}

		// apply all upgrades effects this ship have
		public void ApplyUpgrades() {
			foreach (Upgrade AUp in Ups) {
				var spliter = AUp.effect.Split(' ');
				foreach (string Aspli in spliter)
					ApplyUpgradeEffect(Aspli, false);
			}
		}
		// apply a single upgrade effects to this ship
		public void ApplyUpgradeFirstTime(ref Upgrade upgrade) {
			var spliter = upgrade.effect.Split(' ');
			foreach (string Aspli in spliter)
				ApplyUpgradeEffect(Aspli, true);
		}
		// apply an upgrade effect
		public void ApplyUpgradeEffect(string Chain, bool first_application) {
			var Spliter = Chain.Split(':');
			switch (Spliter[0] ?? "") {
			case var @case when @case == "": {
				break;
			}

			case "!C": {
				color = Color.FromName(Spliter[1]);
				if (stats.sprite == "Station")
					team.color = color;

				break;
			}

			case "!Jump": {
				world.Effects.Add(new Effect() { Type = "EFF_Jumped", Coo = location, Direction = 0f, speed = 0f });
				speed = Convert.ToInt32(Spliter[1]);
				break;
			}

			case "!Agility": {
				stats.turn += Helpers.ToDouble(Spliter[1]);
				break;
			}

			case "!Teleport": {
				world.Effects.Add(new Effect() { Type = "EFF_Teleported", Coo = location, Direction = 0f, speed = 0f });
				var tp_dst = TargetPTN;
				if (target is object)
					tp_dst = target.location;

				location = new PointF(tp_dst.X + world.gameplay_random.Next(-512, 512), tp_dst.Y + world.gameplay_random.Next(-512, 512));
				world.Effects.Add(new Effect() { Type = "EFF_Teleported", Coo = location, Direction = 0f, speed = 0f });
				break;
			}

			case "!Upsbonus": {
				if (first_application)
					team.upgrade_slots_bonus = (ushort)(team.upgrade_slots_bonus + Helpers.ToDouble(Spliter[1])); // FN
				break;
			}

			case "!Maxships": {
				if (first_application)
					team.ship_count_limit = (ushort)(team.ship_count_limit + Helpers.ToDouble(Spliter[1])); // FN
				break;
			}

			case "!Shield": {
				stats.shield = (int)(stats.shield + Helpers.ToDouble(Spliter[1]));
				if (first_application)
					ResetShieldPoint();
				break;
			}

			case "!Deflector": {
				stats.deflectors = (int)(stats.deflectors + Helpers.ToDouble(Spliter[1]));
				break;
			}

			case "!HotDeflector": {
				stats.hot_deflector += Helpers.ToDouble(Spliter[1]);
				break;
			}

			case "!ColdDeflector": {
				stats.cold_deflector = Convert.ToInt32(Spliter[1]) == 1;
				break;
			}

			case "%Shield": {
				stats.shield = (int)(stats.shield + stats.shield * (Helpers.ToDouble(Spliter[1]) / 100d));
				if (first_application)
					ResetShieldPoint();
				break;
			}

			case "!Shieldop": {
				stats.shield_opacity += Helpers.ToDouble(Spliter[1]);
				if (first_application)
					ResetShieldPoint();
				break;
			}

			case "%Shieldreg": {
				stats.shield_regeneration = (int)(stats.shield_regeneration + stats.shield_regeneration * (Helpers.ToDouble(Spliter[1]) / 100d));
				if (first_application)
					ResetShieldPoint();
				break;
			}

			case "!UpsMax": {
				upgrade_slots = (int)(upgrade_slots + Helpers.ToDouble(Spliter[1]));
				break;
			}

			case "!Fix": {
				integrity = (int)(integrity + stats.integrity * (Helpers.ToDouble(Spliter[1]) / 100.0d));
				break;
			}

			case "!FixSFull": {
				shield = stats.shield;
				break;
			}

			case "%Speed": {
				stats.speed += stats.speed * (Helpers.ToDouble(Spliter[1]) / 100d);
				stats.turn += stats.turn * (Helpers.ToDouble(Spliter[1]) / 100d);
				break;
			}

			case "%Life": {
				stats.integrity = (int)(stats.integrity + stats.integrity * (Helpers.ToDouble(Spliter[1]) / 100d));
				if (first_application)
					integrity = (int)(integrity + stats.integrity * (Helpers.ToDouble(Spliter[1]) / 100d)); // FN
				break;
			}

			case "!Regen": {
				stats.repair += Convert.ToInt32(Spliter[1]); // FN
				break;
			}

			case "!Type": {
				if (first_application)
					SetStats(Spliter[1]);// : Me.stats.sprite = Spliter(1)
				break;
			}

			case "%Wloadmax": {
				// For Each AW As Weapon In weapons
				// AW.stats.loadtime += AW.stats.loadtime * (Spliter(1) / 100)
				// Next
				if (weapons.Count != 0)
					weapons[0].stats.loadtime = (int)(weapons[0].stats.loadtime + weapons[0].stats.loadtime * (Helpers.ToDouble(Spliter[1]) / 100.0d));

				break;
			}

			case "%Wbar": {
				// For Each AW As Weapon In weapons
				// AW.stats.salvo += AW.stats.salvo * (Spliter(1) / 100)
				// Next
				if (weapons.Count != 0) {
					weapons[0].stats.salvo = (int)(weapons[0].stats.salvo + weapons[0].stats.salvo * (Helpers.ToDouble(Spliter[1]) / 100.0d));
					if (first_application) {
						weapons[0].Bar = 0;
						weapons[0].Load = 0;
					}
				}

				break;
			}

			case "%Wpower": {
				// For Each AW As Weapon In weapons
				// AW.stats.power += AW.stats.power * (Spliter(1) / 100)
				// Next
				if (weapons.Count != 0)
					weapons[0].stats.power = (int)(weapons[0].stats.power + weapons[0].stats.power * (Helpers.ToDouble(Spliter[1]) / 100.0d));

				break;
			}

			case "%Wrange": {
				// For Each AW As Weapon In weapons
				// AW.stats.range += AW.stats.range * (Spliter(1) / 100)
				// Next
				if (weapons.Count != 0)
					weapons[0].stats.range = (int)(weapons[0].stats.range + weapons[0].stats.range * (Helpers.ToDouble(Spliter[1]) / 100.0d));

				break;
			}

			case "%Wcelerity": {
				// For Each AW As Weapon In weapons
				// AW.stats.celerity += AW.stats.celerity * (Spliter(1) / 100)
				// Next
				if (weapons.Count != 0)
					weapons[0].stats.celerity = (int)(weapons[0].stats.celerity + weapons[0].stats.celerity * (Helpers.ToDouble(Spliter[1]) / 100.0d));

				break;
			}

			case "!Sum": {
				if (first_application)
					world.Ships.Add(new Ship(ref world, team, Spliter[1]) { location = new Point((int)(location.X + world.gameplay_random.Next(-10, 11)), (int)(location.Y + world.gameplay_random.Next(-10, 11))) });
				world.Ships[world.Ships.Count - 1].direction = direction;
				if (world.Ships[world.Ships.Count - 1].weapons.Count > 0 && (world.Ships[world.Ships.Count - 1].weapons[0].stats.special & (int)Weapon.SpecialBits.SelfExplode) != 0) {
					if (team is null || target is null || target.team is null || !team.IsFriendWith(target.team))
						world.Ships[world.Ships.Count - 1].target = target;
					else
						world.Ships[world.Ships.Count - 1].target = null;

					world.Ships[world.Ships.Count - 1].behavior = BehaviorMode.Folow;
					world.Ships[world.Ships.Count - 1].agressivity = agressivity * 100d;
					world.Ships[world.Ships.Count - 1].bot_ship = true;
				} else {
					world.Ships[world.Ships.Count - 1].behavior = BehaviorMode.Folow;
					world.Ships[world.Ships.Count - 1].target = this;
				}

				break;
			}

			case "!Ascend": {
				if (first_application && team.id == 0) {
					MainForm.has_ascended = true;
					MainForm.help = true;
				}

				break;
			}

			case "!Suicide": {
				last_damager_team = team;
				stats.repair = 0;
				integrity = -2048;
				break;
			}

			case "!Free": {
				SetTeam(world.Teams[world.gameplay_random.Next(0, world.Teams.Count)]);
				break;
			}

			case "!Cheats": {
				MainForm.cheats_enabled = !MainForm.cheats_enabled;
				break;
			}

			default: {
				throw new Exception("Erreur : " + Chain + " (invalid effect)");
				break;
			}
			}
		}

		public List<Upgrade> Ups = new List<Upgrade>();
		public Upgrade Upgrading;
		public int UpProgress;
		public int upgrade_slots = -1;

		public bool HaveUp(ref Upgrade upgrade) {
			return Ups.Contains(upgrade);
		}

		public bool IsDestroyed() {
			return integrity <= 0;
		}

		public Ship GetClosestShip(double enemies = 2.0d, double neutrals = 1.0d, double allies = 0.0d) {
			const double max_distance = double.PositiveInfinity;
			// maximum square distance
			double closest_distance_sq = double.PositiveInfinity;
			if (max_distance < Math.Sqrt(double.MaxValue))
				closest_distance_sq = max_distance * max_distance;
			// mods are applied to square distance so they should be sqare too
			enemies *= enemies;
			neutrals *= neutrals;
			allies *= allies;
			// find the ship
			Ship closest_ship = null;
			foreach (Ship other_ship in world.Ships)
				if (!ReferenceEquals(this, other_ship)) {
					double distance_sq;
					// relationship priority
					if (other_ship.team is null || team is null)
						distance_sq = Helpers.DistanceSQ(ref location, ref other_ship.location) / neutrals;
					else if (team.IsFriendWith(other_ship.team))
						distance_sq = Helpers.DistanceSQ(ref location, ref other_ship.location) / allies;
					else
						distance_sq = Helpers.DistanceSQ(ref location, ref other_ship.location) / enemies;
					// test if closer
					if (distance_sq < closest_distance_sq) {
						closest_ship = other_ship;
						closest_distance_sq = distance_sq;
					}
				}

			return closest_ship;
		}

		// calculat point to aim to reach a moving target
		public PointF ForseeLocation(Ship target_ship) {
			double dist = Helpers.Distance(ref location, ref target_ship.location);
			double time = dist / Math.Max(1.0d, stats.speed);
			return new PointF((float)(target_ship.location.X + target_ship.speed_vec.X * time), (float)(target_ship.location.Y + target_ship.speed_vec.Y * time));
		}

		public override void UpdateSector() {
			Point new_sector_coords = ComputeCurrentSectorCoords();
			if (new_sector_coords == sector_coords)
				return;
			if (sector is object)
				sector.ships.Remove(this); // < Remove ship/shoot from ships/shoots here >
			sector_coords = new_sector_coords;
			sector = world.sectors[sector_coords.X, sector_coords.Y];
			sector.ships.Add(this); // < Add ship/shoot to ships/shoots here >
		}
	}
}
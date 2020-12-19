using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class Ship : WorldEntity {

		// mode of behavior
		public enum BehaviorMode {
			None,		// No order
			Stand,		// Order to stay here
			Drift,		// Order to move forward indefinitely
			Folow,		// Order to move to a ship
			Mine,		// Order to mine nearby resources
			GoToPoint	// Order to move to a point
		}

		/* identity */
		private ShipStats base_stats = null;
		public ShipStats stats = null;
		public Team team = null;
		public Color color = Color.White;
		public SpriteArray sprites = null;
		public int fram = 0;

		/* state */
		public float integrity = 20;
		public int deflectors = 0;
		public int deflector_cooldown = 0;
		public float shield = 0f;
		public List<Weapon> weapons = new List<Weapon>();
		public Team last_damager_team = null;
		public float emp_damage = 0;

		/* Automated control */
		public bool auto = false; // for in-team auto objects (cf missiles)
		public bool bot_ship = true;
		public float agressivity = 1.0f;

		/* AI */
		public bool allow_mining = true;
		public BehaviorMode behavior = BehaviorMode.Stand;
		public Ship target = null;								// ship targeted by the order
		public PointF target_point = new PointF(0f, 0f);		// point targeted by the order

		/* Shield effect */
		public const int SHIELD_POINTS = 16;
		public int[] ShieldPoints = new int[16];
		public void BrightShield() {
			for (int shield_ptn_value = 0; shield_ptn_value <= SHIELD_POINTS - 1; shield_ptn_value++)
				ShieldPoints[shield_ptn_value] = Math.Max(ShieldPoints[shield_ptn_value], 128);
		}

		/* Uprades */
		public List<Upgrade> upgrades = new List<Upgrade>();
		public Upgrade Upgrading = null;
		public int upgrade_progress;
		public int upgrade_slots = -1;

		/* Construction */
		public Ship(World world, Team team, string ship_class) : base(ref world) {
			this.fram = Helpers.rand.Next(0, 256);
			if (team == null) // TODO: NOW: remove this
				throw new Exception ("team was null");
			SetTeam(team);
			SetStats(ship_class);
			ApplyUpgrades();
			BrightShield();
			target_point = new PointF(location.X, location.Y);
			UpdateSector();
		}

		public void SetStats(string ship_class) {
			SetStats(ShipStats.classes[ship_class]);
		}

		public void SetStats(ShipStats stats) {
			if (!ReferenceEquals(base_stats, stats)) {
				base_stats = stats;
				// native upgrades
				if (upgrades.Count == 0)
					foreach (string native_upgrade_name in base_stats.native_upgrades)
						upgrades.Add(Upgrade.upgrades[native_upgrade_name]);
				// upgrade_slots
				if (upgrade_slots < 0) { // initial value is -1
					upgrade_slots = base_stats.level;
					if (base_stats.repair > 0)
						upgrade_slots += team.upgrade_slots_bonus;
				}
				// Reset Weapons
				weapons.Clear();
				foreach (string gun_name in base_stats.default_weapons)
					weapons.Add(new Weapon(this, gun_name));
				// force a color
				if (this.team.affinity == AffinityEnum.Wilderness) {
					behavior = BehaviorMode.Drift;
					target = null;
					color = Color.FromArgb(64, 64, 48);
					if ((base_stats.role & (int)ShipRole.Static) != 0 && (base_stats.role & (int)ShipRole.Mine) != 0) {
						color = Color.FromArgb(255, 255, 255);
					} else if ((base_stats.role & (int)ShipRole.Static) != 0) {
						color = Color.FromArgb(32, 32, 32);
					} else if (base_stats.cost.Fissile > 0) {
						color = Color.FromArgb(0, 100, 0);
					} else if (base_stats.cost.Crystal * 100 > base_stats.cost.Metal) {
						color = Color.FromArgb(80, 48, 80);	
					} else if (base_stats.cost.Starfuel > 0) {
						color = Color.FromArgb(80, 80, 48);
					}
				}
				// 
				ResetStats();
				// sprite
				UpdateSprite();
			}
		}
		public void UpdateSprite() {
			this.sprites = SpriteArray.GetSpriteArray(stats.sprite, this.color);
		}

		public void SetTeam(Team team) {
			if (!ReferenceEquals(this.team, team)) {
				this.team = team;
				bot_ship = this.team.bot_team;
				color = team.color;
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

		public void Check() {
			// frame
			fram = (fram + 1) % sprites.count_x;
			// world borders
			if (!team.bot_team) {
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
			if (stats.turn == 0d && stats.speed == 0d) {
				direction = (float)(direction + 25.0f / stats.width);
				speed_vec = new PointF((float)(speed_vec.X * 0.925), (float)(speed_vec.Y * 0.925));
			} else {
				var new_speed = Helpers.GetNewPoint(new Point(0, 0), direction, speed);
				speed_vec = new PointF((float)(speed_vec.X * 0.925 + new_speed.X * 0.075), (float)(speed_vec.Y * 0.925 + new_speed.Y * 0.075));
			}
			location.X = location.X + speed_vec.X;
			location.Y = location.Y + speed_vec.Y;
			// reloadings dependent on emp_damage
			if (emp_damage < this.stats.width) {
				// integrity repairs
				if (world.ticks % 32 == 0 && this.deflectors >= 0) {
					if (team.affinity != AffinityEnum.Wilderness) {
						integrity += stats.repair;
					}
				}
				// shields
				if (stats.shield > 0 && this.deflectors >= 0) {
					shield += (float)(stats.shield * stats.shield_regeneration);
					int point_min = (int)Math.Max(0f, shield * 32f / stats.shield);
					for (int i = 0; i <= SHIELD_POINTS - 1; i++) {
						ShieldPoints[i] -= 4;
						if (ShieldPoints[i] < point_min)
							ShieldPoints[i] = (int)point_min;
					}
				}
				// weapons
				if (this.deflectors >= 0) {
					foreach (Weapon AWeapon in weapons) {
						if (AWeapon.Bar == 0) {
							if ((AWeapon.stats.special & (int)Weapon.SpecialBits.ReloadRNG) != 0)
								AWeapon.Load += +1;
							AWeapon.Load += +1;
							if (AWeapon.Load >= AWeapon.stats.loadtime) {
								AWeapon.Load = 0;
								AWeapon.Bar = AWeapon.stats.salvo;
							}
						}
					}
				}
				// upgrading
				if (Upgrading != null) {
					if (upgrade_progress < Upgrading.time) {
						upgrade_progress = upgrade_progress + 1;
						if (team.cheats_enabled)
							upgrade_progress = upgrade_progress + 99;
					} else {
						if (Upgrading.required_upgrade_slots > 0)
							upgrades.Add(Upgrading);
						// actualisation vaisseau
						ResetStats();
						ApplyUpgrades();
						Upgrading.ApplyOnceEffects(this);
						Upgrading = null;
						upgrade_progress = 0;
					}
				}
				// deflectors
				if (deflectors >= stats.deflectors)
					deflector_cooldown = stats.deflectors_cooldown;
				else {
					deflector_cooldown -= 1;
					if (deflector_cooldown <= 0) {
						deflectors += 1;
					if (deflectors < 0)
						deflectors = 0;
						deflector_cooldown = stats.deflectors_cooldown;
					}
				}
			}
			// emp damage decrease
			if (stats.cold_deflectors >= 0) {
				this.emp_damage -= 0.05f;
				this.emp_damage *= 0.99f;
				if (this.emp_damage < 0)
					this.emp_damage = 0;
			}
			if (this.emp_damage > stats.width)
				world.effects.Add(new Effect(-1, "EFF_emped", new PointF(this.location.X + world.gameplay_random.Next(-this.stats.width, this.stats.width) / 2, this.location.Y + world.gameplay_random.Next(-this.stats.width, this.stats.width) / 2), world.gameplay_random.Next(0, 360), this.stats.width / 16.0f));
			// stats bounds
			if (integrity > stats.integrity)
				integrity = stats.integrity;
			if (shield > stats.shield)
				shield = stats.shield;
			if (deflectors > stats.TotalDeflectorsMax())
				deflectors = stats.TotalDeflectorsMax();
			// autos should not live forever
			if (this.auto && world.ticks % 128 == 0)
				integrity -= 1;
		}

		public void TurnTowardDirection(float qa) {
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

		public void TakeDamages(float Amount, Shoot From = null) {
			// deflectors
			if (deflectors > -stats.cold_deflectors) {
				if (From != null) {
					if (deflectors > stats.deflectors)
						this.world.effects.Add(new Effect(-1, "EFF_hot_deflected", From.location, From.direction, this.speed_vec));
					else if (deflectors >= 0)
						this.world.effects.Add(new Effect(-1, "EFF_deflected", From.location, From.direction, this.speed_vec));
					else
						this.world.effects.Add(new Effect(-1, "EFF_cold_deflected", From.location, From.direction, this.speed_vec));
				}
				deflectors -= 1;
				return;
			}
			// EMP capability
			if (From != null && From.emp_power > 0) {
				if (this.shield > 0)
					this.emp_damage *= (1.0f - this.stats.shield_opacity * Math.Max(0.0f, this.shield / this.stats.shield) / 2.0f);
				this.emp_damage += From.emp_power;
				double angle_ship_shoot_rel = Helpers.NormalizeAngleUnsigned(Helpers.GetAngle(location.X, location.Y, From.location.X, From.location.Y) - direction);
				int shield_ptn_index = (int)(angle_ship_shoot_rel * 16d / 360d);
				ShieldPoints[shield_ptn_index % 16] = 255;
			}
			// shield
			if (shield > 0f) {
				shield = shield - Amount;
				Amount = (int)(Amount - Amount * stats.shield_opacity);
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
			// Wilderness mining
			if (team.affinity == AffinityEnum.Wilderness && From != null  && (this.stats.role & (int)ShipRole.Mine) != 0) {
				From.Team.resources.Metal += (long)(this.stats.cost.Metal * Amount / 1024.0);
				if (world.gameplay_random.Next(0, 100) < this.stats.cost.Crystal)
					From.Team.resources.Crystal += 1;
				From.Team.resources.Starfuel += (long)(this.stats.cost.Starfuel * Amount / 1024.0);
				if (world.gameplay_random.Next(0, 100) < this.stats.cost.Fissile)
					From.Team.resources.Fissile += 1;
			}
			// integrity hit
			if (Amount > 0.0) {
				// effect
				if (From != null) {
					if (From.power < 16)
						world.effects.Add(new Effect(-1, "EFF_Impact0", From.location, From.direction, this.speed_vec, world.gameplay_random.Next()));
					else if (From.power < 32)
						world.effects.Add(new Effect(-1, "EFF_Impact1", From.location, From.direction, this.speed_vec, world.gameplay_random.Next()));
					else if (From.power < 48)
						world.effects.Add(new Effect(-1, "EFF_Impact2", From.location, From.direction, this.speed_vec, world.gameplay_random.Next()));
					else
						world.effects.Add(new Effect(-1, "EFF_Impact3", From.location, From.direction, this.speed_vec, world.gameplay_random.Next()));
				}
				// damages
				integrity -= Amount;
				if (integrity < 0)
					integrity = 0;
			}
		}

		/* AI */
		public void AISanitize() {
			if (target is object && target.IsDestroyed())
				target = null;
			if (behavior == BehaviorMode.Folow && target is null)
				behavior = BehaviorMode.Stand;
			if (ReferenceEquals(team, world.wilderness_team))
				behavior = BehaviorMode.Drift;
		}
		public void AI() {
			float		required_direction = 0.0f;
			bool		require_speed = false;

			AISanitize();
			// AI cant operate when EMP disabled
			if (this.emp_damage > this.stats.width) {
				if (!ReferenceEquals(team, world.wilderness_team))
					this.speed = 0;
				return;
			}
			if (bot_ship || auto) {
				// Randomly increase ship's agressivity
				if (world.gameplay_random.Next(0, 1000) < 1)
					agressivity += 0.05f;
				// Choose a target ship
				if (behavior != BehaviorMode.Drift && team.affinity != AffinityEnum.Wilderness)
					if (target is null) {
						var nearest_ship = GetClosestShip(agressivity, 1.0d, 0.1d);
						if (nearest_ship is object) {
							target = nearest_ship;
							behavior = BehaviorMode.Folow;
						}
					} else if (world.gameplay_random.Next(0, 1000) < 2 || !ReferenceEquals(team, target.team) && world.gameplay_random.Next(0, 1000) < 10) { // chance to change target
						target = null;
						behavior = BehaviorMode.Stand;
					}
			}
			// folow behavior
			switch (behavior) {
			case BehaviorMode.Mine: {
				allow_mining = true;
				if (target is object) {
					AITowardTargetMining(ref required_direction, ref require_speed);
				} else {
					// no current mining target
					int max_mining_distance = (int)(world.ArenaSize.Width / 8d);
					var mining_target = GetClosestShip(0.0d, 1.0d);
					if (mining_target is object && Helpers.Distance(ref target_point, ref mining_target.location) > max_mining_distance)
						mining_target = null;
					target = mining_target;
					if (target is null)
						AITowardTargetPoint(ref required_direction, ref require_speed);
				}
				break;
			}
			case BehaviorMode.Folow: {
				if (target is object) {
					if (this.team.IsFriendWith(this.target.team))
						AITowardTargetEscort(ref required_direction, ref require_speed);
					else
						AITowardTargetAgressive(ref required_direction, ref require_speed);
				}
				break;
			}
			case BehaviorMode.Stand: {
				required_direction = direction;
				require_speed = false;
				break;
			}
			case BehaviorMode.Drift: {
				required_direction = direction;
				require_speed = true;
				break;
			}
			case BehaviorMode.GoToPoint: {
				AITowardTargetPoint(ref required_direction, ref require_speed);
				break;
			}
			}
			// apply choices
			AIApplyNow(required_direction, require_speed);
			// weapons
			AIFire();
		}
		void AITowardTargetPoint(ref float required_direction, ref bool require_speed) {
				required_direction = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)target_point.X, (int)target_point.Y);
				if (Helpers.Distance(location.X, location.Y, target_point.X, target_point.Y) <= 50d)
					behavior = BehaviorMode.Stand;
				require_speed = true;
		}
		void AITowardTargetAgressive(ref float required_direction, ref bool require_speed) {
			var forseen_location = ForseeLocation(target);
			// world.Effects.Add(New Effect With {.Type = "EFF_OrderTarget", .Coo = forseen_location, .Direction = 45, .speed = 0})
			required_direction = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)forseen_location.X, (int)forseen_location.Y);
			double rel_forseen_dist = Helpers.Distance(ref location, ref forseen_location) - target.stats.width / 2d;
			double optimal_range = 100.0f;
			if (weapons.Count > 0)
				optimal_range = weapons[0].stats.range * weapons[0].stats.range / rel_forseen_dist * 0.8d;
			if (Helpers.Distance(ref location, ref target.location) <= optimal_range)
				if (Helpers.GetAngleDiff(direction, required_direction) < 112d) {
					required_direction = required_direction + 180f;
					require_speed = Helpers.GetAngleDiff(direction, required_direction - 180f) > 45d;
				} else
					require_speed = Helpers.GetAngleDiff(direction, required_direction) > 90d;
			else {
				if (target.stats.speed * 1.1d >= stats.speed) {
					double angle_to_target = Helpers.GetQA((int)location.X, (int)location.Y, (int)target.location.X, (int)target.location.Y);
					if (Helpers.GetAngleDiff(angle_to_target, required_direction) > 90d)
						required_direction = (float)angle_to_target;
				}
				require_speed = Helpers.GetAngleDiff(direction, required_direction) < 90d;
			}
		}
		void AITowardTargetEscort(ref float required_direction, ref bool require_speed) {
			var forseen_location = ForseeLocation(target);
			// world.Effects.Add(New Effect With {.Type = "EFF_OrderTarget", .Coo = forseen_location, .Direction = 45, .speed = 0})
			required_direction = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)forseen_location.X, (int)forseen_location.Y);
			double rel_forseen_dist = Helpers.Distance(ref location, ref forseen_location) - target.stats.width / 2d;
			double optimal_range = target.stats.width * 4.0f + 10.0f;
			if (target.weapons.Count > 0)
				optimal_range = target.weapons[0].stats.range * target.weapons[0].stats.range / rel_forseen_dist * 0.8d;
			// decrease optimal range for ships with high range
			if (weapons.Count > 0)
				optimal_range *= Math.Min(1.0f, Math.Max(0.2f, 150.0f / weapons[0].stats.range));
			double dist = Helpers.Distance(ref location, ref target.location);
			if (dist <= optimal_range * 0.6) {
				if (Helpers.GetAngleDiff(direction, required_direction) < 112d) {
					required_direction = required_direction + 180f;
					require_speed = Helpers.GetAngleDiff(direction, required_direction - 180f) > 45d;
				} else
					require_speed = Helpers.GetAngleDiff(direction, required_direction) > 90d;
			} else if (dist <= optimal_range * 1.1) {
				if (target.stats.speed == 0.0f && target.stats.turn == 0.0f)
					require_speed = true;
				else
					require_speed = (this.speed < target.speed);
				required_direction = target.direction;
			} else {
				if (target.stats.speed * 1.1d >= stats.speed) {
					double angle_to_target = Helpers.GetQA((int)location.X, (int)location.Y, (int)target.location.X, (int)target.location.Y);
					if (Helpers.GetAngleDiff(angle_to_target, required_direction) > 90d)
						required_direction = (float)angle_to_target;
				}
				require_speed = Helpers.GetAngleDiff(direction, required_direction) < 90d;
			}
		}
		void AITowardTargetMining(ref float required_direction, ref bool require_speed) {
			var forseen_location = ForseeLocation(target);
			// world.Effects.Add(New Effect With {.Type = "EFF_OrderTarget", .Coo = forseen_location, .Direction = 45, .speed = 0})
			required_direction = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)forseen_location.X, (int)forseen_location.Y);
			double rel_forseen_dist = Helpers.Distance(ref location, ref forseen_location) - target.stats.width / 2d;
			double optimal_range = 120.0f;
			if (weapons.Count > 0)
				optimal_range = weapons[0].stats.range * weapons[0].stats.range / rel_forseen_dist * 0.8d;
			double dist = Helpers.Distance(ref location, ref target.location);
			if (dist <= optimal_range * 0.7) {
				if (Helpers.GetAngleDiff(direction, required_direction) < 112d) {
					required_direction = required_direction + 180f;
					require_speed = Helpers.GetAngleDiff(direction, required_direction - 180f) > 45d;
				} else
					require_speed = Helpers.GetAngleDiff(direction, required_direction) > 90d;
			} else if (dist <= optimal_range * 1.2) {
				required_direction = target.direction;
				require_speed = (this.speed <= target.speed);
			} else {
				if (target.stats.speed * 1.1d >= stats.speed) {
					double angle_to_target = Helpers.GetQA((int)location.X, (int)location.Y, (int)target.location.X, (int)target.location.Y);
					if (Helpers.GetAngleDiff(angle_to_target, required_direction) > 90d)
						required_direction = (float)angle_to_target;
				}
				require_speed = Helpers.GetAngleDiff(direction, required_direction) < 90d;
			}
		}
		public void AIApplyNow(float required_direction, bool require_speed) {
			TurnTowardDirection(required_direction);
			if (require_speed)
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
		}
		public void AIFire() {
			// ===' Tirer '==='
			if (weapons.Count > 0 && fram % 2 == 0)
				foreach (Weapon AWeap in weapons)
					if (AWeap.Bar > 0) {
						double closest_score = double.MaxValue;
						Ship weapon_targeted_ship = null; // Pas de cible
						int weapon_location_x = (int)(Math.Sin(2d * Math.PI * (AWeap.Loc + direction) / 360d) * (stats.width / 2d) + location.X);
						int weapon_location_y = (int)(Math.Cos(2d * Math.PI * (AWeap.Loc + direction) / 360d) * (stats.width / 2d) + location.Y);
						// target in range
						if (target is object && behavior != BehaviorMode.Mine)
							if (!team.IsFriendWith(target.team)) {
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

								if (!allow_mining && (OVessel.stats.default_weapons.Count == 0 || AWeap.stats.power == 0))
									continue;

								if (Helpers.Distance(ref location, ref OVessel.location) < AWeap.stats.range)
									if (!team.IsFriendWith(OVessel.team)) {
										// Dim dist As Integer = Helpers.Distance(ToX, ToY, OVessel.location.X, OVessel.location.Y) - OVessel.stats.width / 2
										var argp21 = AWeap.ForseeShootingLocation(OVessel);
										double score = Helpers.Distance(ref location, ref argp21) - OVessel.stats.width / 2d;
										if (score < AWeap.stats.range * 0.9d) {
											if (score < AWeap.stats.range) {
												if (!ReferenceEquals(OVessel.team, world.wilderness_team) && !team.IsFriendWith(OVessel.team))
													score /= 8;
												if (ReferenceEquals(target, OVessel))
													score /= 4;
												if ((AWeap.stats.special & (int)Weapon.SpecialBits.EMP) != 0 && OVessel.emp_damage * 0.8 > OVessel.stats.width)
													score *= 3;
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
						} else if ((AWeap.stats.special & (int)Weapon.SpecialBits.KeepFire) != 0) 
							AWeap.Fire(world.gameplay_random.Next(0, 360), new Point(weapon_location_x, weapon_location_y), this);;
					}
		}

		// get updates to display
		public static List<Upgrade> ListedUpgrades(List<Ship> ships) {
			var met_upgrades = new List<Upgrade>();
			foreach (Upgrade upgrade in Upgrade.upgrades.Values)
				if (CountShipsListingUpgrade(ships, upgrade) == ships.Count)
					met_upgrades.Add(upgrade);
			return (met_upgrades);
		}

		public List<Upgrade> InstalledOrInstallUpgrades() {
			var met_upgrades = new List<Upgrade>();
			foreach (Upgrade upgrade in this.upgrades)
				met_upgrades.Add(upgrade);
			if (this.Upgrading != null)
				met_upgrades.Add(this.Upgrading);
			return (met_upgrades);
		}
		public bool InstalledOrInstallUpgrade(Upgrade upgrade) {
			if (this.upgrades.Contains(upgrade))
				return (true);
			if (ReferenceEquals(Upgrading, upgrade))
				return (true);
			return (false);
		}
		/**
		 * @brief List upgrades that this ship have installed or that are available to it.
		 */
		public List<Upgrade> AvailableOrInstalledUgrades() {
			var met_upgrades = new List<Upgrade>();
			foreach (Upgrade upgrade in Upgrade.upgrades.Values)
				if (upgrade.GetAvailability(this) == Upgrade.Availability.Available || this.InstalledOrInstallUpgrade(upgrade))
					met_upgrades.Add(upgrade);
			return (met_upgrades);
		}
		/**
		 * @brief List upgrades that the ship could potentially do.
		 */
		public List<Upgrade> AvailableNotInstalledUpgrades() {
			var possible_upgrades = new List<Upgrade>();
			foreach (Upgrade upgrade in Upgrade.upgrades.Values)
				if (upgrade.GetAvailability(this) == Upgrade.Availability.Available && !this.InstalledOrInstallUpgrade(upgrade))
					possible_upgrades.Add(upgrade);
			return (possible_upgrades);
		}
		/**
		 * @brief Get if an upgrade is doable right now (price isnt counted).
		 */
		public bool CanUpgradeFree(Upgrade upgrade) {
			if (team.cheats_enabled)
				return (true);
			if (upgrade.GetAvailability(this) != Upgrade.Availability.Available)
				return (false);
			if (InstalledOrInstallUpgrade(upgrade))
				return (false);
			return (true);
		}
		public bool CanUpgradeFree(string upgrade_name) {
			return CanUpgradeFree(Upgrade.upgrades[upgrade_name]);
		}

		public static int CountShipsListingUpgrade(List<Ship> ships, Upgrade upgrade) {
			int count = 0;
			foreach (Ship ship in ships)
				if (upgrade.GetAvailability(ship) == Upgrade.Availability.Available || ship.InstalledOrInstallUpgrade(upgrade))
					count += 1;
			return (count);
		}

		public static int CountShipsBuyableNowUpgrade(List<Ship> ships, Upgrade upgrade) {
			int count = 0;
			foreach (Ship ship in ships)
				if (ship.Upgrading is null)
					if (upgrade.GetAvailability(ship) == Upgrade.Availability.Available && !ship.InstalledOrInstallUpgrade(upgrade))
						count += 1;
			return (count);
		}

		public static int CountShipsHavingUpgrade(List<Ship> ships, Upgrade upgrade) {
			int count = 0;
			foreach (Ship ship in ships)
				if (ship.upgrades.Contains(upgrade))
					count += 1;
			return (count);
		}

		// get the minimum loading progress of an upgrade for a list of ship, or int.MaxValue if not being upgraded
		public static int MinUpgradeProgress(List<Ship> ships, Upgrade upgrade) {
			int min = int.MaxValue;
			foreach (Ship ship in ships)
				if (ReferenceEquals(ship.Upgrading, upgrade))
					min = Math.Min(min, ship.upgrade_progress);
			return (min);
		}
		// apply all upgrades effects this ship have
		public void ApplyUpgrades() {
			foreach (Upgrade upgrade in upgrades) {
				upgrade.ApplyEffects(this);
			}
		}

		// Get if a ship have a specified upgrade
		public bool HaveUp(ref Upgrade upgrade) {
			return upgrades.Contains(upgrade);
		}

		/**
		 * @brief Starts upgrading for free.
		 */
		public void UpgradeForFree(Upgrade upgrade) {
			this.Upgrading = upgrade;
			this.upgrade_progress = 0;
		}
		public void UpgradeForFree(string upgrade_name) {
			UpgradeForFree(Upgrade.upgrades[upgrade_name]);
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
			foreach (Ship other_ship in world.ships)
				if (!ReferenceEquals(this, other_ship)) {
					double distance_sq;
					// relationship priority
					if (other_ship.team.affinity == AffinityEnum.Wilderness || team.affinity == AffinityEnum.Wilderness)
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
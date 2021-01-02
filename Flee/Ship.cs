using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class Ship : WorldEntity {
		public float speed = 0.0f;

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
		public bool bot_ship = true;		// if automated (it's a bot, usualy in a bot team)
		public bool auto = false;			// for in-team auto objects (cf missiles)


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
			ai_target_point = new PointF(location.X, location.Y);
			world.UpdateSector(this);
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
					ai_order = (int)AIOrder.Drift;
					ai_target = null;
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

		/* Gameplay */
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
			} else if (location.X < -512 || location.Y < -512 || location.X > world.ArenaSize.Width + 512 || location.Y > world.ArenaSize.Height + 512)
				if (world.ticks % 64 == 0)
					this.integrity -= 1 + this.stats.integrity / 8;
			// acceleration
			if (stats.turn == 0.0f && stats.speed == 0.0f)
				direction = (float)(direction + 25.0f / stats.width);
			//if (stats.turn != 0.0f)
			//	speed_vec = new PointF((float)(speed_vec.X * 0.925f), (float)(speed_vec.Y * 0.925f));
			if (stats.turn == 0.0f && stats.speed != 0.0) {
				if (speed_vec.X == 0.0f && speed_vec.Y == 0.0f)
					speed_vec = Helpers.GetNewPoint(new PointF(0.0f, 0.0f), direction, stats.speed);
			} 
			if ((stats.turn != 0.0f && stats.speed != 0.0f) || (stats.turn == 0.0f && stats.speed == 0.0f)) {
				var new_speed = Helpers.GetNewPoint(new Point(0, 0), direction, speed);
				speed_vec = new PointF((float)(speed_vec.X * 0.925f + new_speed.X * 0.075f), (float)(speed_vec.Y * 0.925f + new_speed.Y * 0.075f));
			}
			// movement
			location.X = location.X + speed_vec.X;
			location.Y = location.Y + speed_vec.Y;
			// reloadings dependent on emp_damage
			if (!this.IsDisabled()) {
				// integrity repairs
				if (world.ticks % 32 == 0 && this.deflectors >= 0) {
					if (team.affinity != AffinityEnum.Wilderness) {
						integrity += stats.repair;
					}
				}
				// shields
				if (stats.shield > 0 && this.deflectors >= 0) {
					shield += (float)(stats.shield * stats.shield_regeneration * this.EnabledEfficiency());
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
				this.emp_damage *= 0.992f;
				if (this.emp_damage < 0)
					this.emp_damage = 0;
			}
			if (this.IsDisabled())
				world.effects.Add(new Effect(-1, "EFF_emped", new PointF(this.location.X + world.gameplay_random.Next(-this.stats.width, this.stats.width) / 2, this.location.Y + world.gameplay_random.Next(-this.stats.width, this.stats.width) / 2), world.gameplay_random.Next(0, 360), this.stats.width / 16.0f));
			// stats bounds
			if (integrity > stats.integrity)
				integrity = stats.integrity;
			if (shield > stats.shield)
				shield = stats.shield;
			if (deflectors > stats.TotalDeflectorsMax())
				deflectors = stats.TotalDeflectorsMax();
			// limited lifespan objects
			this.TickLifespan();
		}
		public void TakeDamages(float Amount, Shoot From = null) {
			// celerity transpher
			if (From != null && this.stats.speed != 0.0f && this.stats.turn != 0.0f) {
				this.speed_vec.X += From.speed_vec.X * From.power / 8.0f / (float)this.stats.width;
				this.speed_vec.Y += From.speed_vec.Y * From.power / 8.0f / (float)this.stats.width;
			}
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
				From.Team.resources.Metal += Math.Max(1L, (long)(this.stats.cost.Metal * Amount / 1024.0));
				if (this.stats.cost.Crystal > 0 && world.gameplay_random.Next(0, 10000) < this.stats.cost.Crystal + Amount)
					From.Team.resources.Crystal += 1;
				From.Team.resources.Starfuel += (long)(this.stats.cost.Starfuel * Amount / 1024.0);
				if (this.stats.cost.Fissile > 0 && world.gameplay_random.Next(0, 10000) < this.stats.cost.Fissile + Amount)
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
		public bool IsDestroyed() {
			return (integrity <= 0 || lifespan <= 0);
		}
		public bool IsDisabled() {
			return (emp_damage > stats.integrity / 4);
		}
		public float EnabledEfficiency() {
			return (Math.Min(1.0f, Math.Max(0.0f, 1.0f - emp_damage / stats.integrity / 4.0f)));
		}
		public bool IsAutomata() {
			return (bot_ship || auto);
		}

		/* AI */
		public float agressivity = 1.0f;
		public enum AIOrder : int {
			None = 0,
			Drift = 0x1,		// Drift forward, dont stop
			Goto = 0x2,			// Go to target_point
			Mine = 0x4,			// Mine arround target_point
			Escort = 0x10,		// Folow formation_leader
			Retreat = 0x20,		// Escape combat
			Attack = 0x100,		// Attack the target (prioritary)
		}
		public int ai_order = (int)AIOrder.None;				// Flags to set the AI's behavior
		public PointF ai_target_point = new PointF(0f, 0f);		// point targeted by the order
		public Ship ai_formation_leader = null;					// ship targeted by the order
		public Ship ai_target = null;							// ship targeted by the order
		public bool allow_mining = true;                        // May shoot at asteroids
		public bool AIHasOrder(int order) {
			return ((this.ai_order & order) != 0);
		}
		public void AISetOrder(int order, bool value = true) {
			if (value)
				ai_order |= order;
			else
				ai_order = ai_order & ~order;
		}

		/* AI */
		public void AISanitize() {
			if (ai_formation_leader != null && ai_formation_leader.IsDestroyed())
				ai_formation_leader = null;
			if (ai_target != null && ai_target.IsDestroyed())
				ai_target = null;
			if (ReferenceEquals(team, world.wilderness_team))
				ai_order = (int)AIOrder.Drift;
		}
		public void AI() {
			float		required_direction = 0.0f;
			bool		require_speed = false;

			if (this.color == Color.Red && !ReferenceEquals(this.team, world.boss_team)) {
				Console.WriteLine();
			}
			AISanitize();
			// bots agressivity and target switching
			if (bot_ship) {
				// agressivity
				if (world.gameplay_random.Next(0, 1000) < 1)
					agressivity += 0.05f;
				// orders reset
				bool reset_orders = false;
				if (world.gameplay_random.Next(0, 4000) < 2)
					reset_orders = true;
				if (reset_orders) {
					this.ai_formation_leader = null;
					this.ai_target = null;
					this.ai_target_point = this.location;
					this.ai_order = 0;
				}
				// new order
				if (this.ai_order == 0) {
					if (world.gameplay_random.Next(0, 2) == 0)
						this.ai_formation_leader = GetClosestEscortableShip(0.0, 0.0, 1.0);
					this.ai_target = GetClosestShip(agressivity, 1.0, 0.0);
				}
				if (!ReferenceEquals(this.team, world.boss_team) && this.ai_formation_leader != null && this.stats.speed >= this.ai_formation_leader.speed)
					this.AISetOrder((int)AIOrder.Escort);
				else if (this.ai_target != null && !AIHasOrder((int)AIOrder.Escort))
					this.AISetOrder((int)AIOrder.Attack);
				if (this.integrity < this.stats.integrity * 0.6f) {
					this.AISetOrder((int)AIOrder.Retreat, true);
					this.AISetOrder((int)AIOrder.Attack, false);
				} else if (this.integrity > this.stats.integrity * 0.8f) {
					this.AISetOrder((int)AIOrder.Retreat, false);
				}
			}
			// Wilderness
			if (this.team.affinity == AffinityEnum.Wilderness) {
				required_direction = direction;
				require_speed = AIHasOrder((int)AIOrder.Drift);
				AIApplyNow(required_direction, require_speed);
				AIFire();
				return;
			}
			// AI cant operate when EMP disabled
			if (this.IsDisabled()) {
				if (!ReferenceEquals(team, world.wilderness_team))
					this.speed = 0;
				return;
			}
			// renew formation leader
			if (this.ai_formation_leader == null && AIHasOrder((int)AIOrder.Escort)) {
				this.ai_formation_leader = GetClosestEscortableShip(0.0, 0.0d, 1.0d);
			}
			if (this.ai_formation_leader == null && AIHasOrder((int)AIOrder.Retreat)) {
				this.ai_formation_leader = GetClosestEscortableShip(0.0, 0.0d, 1.0d);
			}
			// define attack range
			float attack_range = 300;
			if (this.ai_formation_leader != null && this.ai_formation_leader.weapons.Count > 0) 
				attack_range = this.ai_formation_leader.weapons[0].stats.range * 1.8f;
			// get a target
			if (AIHasOrder((int)AIOrder.Escort) && this.ai_formation_leader != null)
				this.ai_target = this.ai_formation_leader.ai_target;
			if (this.ai_target != null && this.ai_target.IsDestroyed()) 
				this.ai_target = null;
			if (this.ai_target == null) {
				if (AIHasOrder((int)AIOrder.Attack)) {
					this.ai_target = GetClosestShip(1.0, 0.0d, 0.0d);
					if (this.ai_target == null || Helpers.Distance(ref this.location, ref this.ai_target.location) > attack_range)
						this.AISetOrder((int)AIOrder.Attack, false);
				}
				else if (AIHasOrder((int)AIOrder.Mine))
					this.ai_target = GetClosestShip(0.0, 1.0d, 0.0d);
				else {
					this.ai_target = GetClosestShip(agressivity, 1.0d, 0.1d);
					if (!team.IsFriendWith(this.ai_target.team)) {

					}
				}
			}
			// effectively attack or folow
			if (this.ai_target != null && AIHasOrder((int)AIOrder.Attack)) {
				AITowardTargetAgressive(ref required_direction, ref require_speed, this.ai_target);
			} else if (this.ai_target != null && AIHasOrder((int)AIOrder.Mine)) {
				AITowardTargetMining(ref required_direction, ref require_speed, this.ai_target);
			} else if (this.ai_formation_leader != null && AIHasOrder((int)AIOrder.Escort)) {
				if (this.ai_formation_leader.ai_target != null && !team.IsFriendWith(ai_formation_leader.ai_target.team) && ai_formation_leader.ai_target.team.affinity != AffinityEnum.Wilderness && Helpers.Distance(ref this.ai_formation_leader.location, ref this.ai_formation_leader.ai_target.location) < attack_range)
					AITowardTargetAgressive(ref required_direction, ref require_speed, this.ai_formation_leader.ai_target);
				else
					AITowardTargetEscort(ref required_direction, ref require_speed, this.ai_formation_leader);
			} else if (AIHasOrder((int)AIOrder.Retreat)) { // TODO:
				if (this.ai_target != null) {
					AITowardTargetAgressive(ref required_direction, ref require_speed, this.ai_target);
					required_direction += 180;
					require_speed = true;
				}
				//AITowardTargetEscort(ref required_direction, ref require_speed, this.ai_formation_leader);
			} else if (AIHasOrder((int)AIOrder.Goto)) {
				if (Helpers.Distance(ref this.location, ref this.ai_target_point) < attack_range / 4) {
					this.AISetOrder((int)AIOrder.Goto, false);
					this.ai_target_point = new PointF(this.location.X + this.speed_vec.X * this.stats.speed, this.location.Y + this.speed_vec.Y * this.stats.speed);
				}
				AITowardTargetPoint(ref required_direction, ref require_speed, this.ai_target_point);
			} else if (AIHasOrder((int)AIOrder.Drift)) {
				required_direction = this.direction;
				require_speed = true;
			} else {
				required_direction = this.direction;
				//if (this.ai_target != null)
				//	required_direction = Helpers.GetAngle(this.location.X, this.location.Y, ai_target.location.X, ai_target.location.Y);
				require_speed = false;
			}
			// apply choices
			AIApplyNow(required_direction, require_speed);
			// weapons
			AIFire();
		}
		void AITowardTargetPoint(ref float required_direction, ref bool require_speed, PointF ptn) {
				required_direction = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)ptn.X, (int)ptn.Y);
				if (Helpers.Distance(location.X, location.Y, ptn.X, ptn.Y) <= 50d)
					require_speed = false;
				else
					require_speed = (Helpers.GetAngleDiff(this.direction, required_direction) < 90);
		}
		void AITowardTargetAgressive(ref float required_direction, ref bool require_speed, Ship target) {
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
		void AITowardTargetEscort(ref float required_direction, ref bool require_speed, Ship target) {
			var forseen_location = ForseeLocation(target);
			// world.Effects.Add(New Effect With {.Type = "EFF_OrderTarget", .Coo = forseen_location, .Direction = 45, .speed = 0})
			required_direction = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)forseen_location.X, (int)forseen_location.Y);
			double rel_forseen_dist = Helpers.Distance(ref location, ref forseen_location) - target.stats.width / 2d;
			double optimal_range = target.stats.width * 4.0f + 10.0f;
			if (target.weapons.Count > 0)
				optimal_range = target.weapons[0].stats.range * target.weapons[0].stats.range / rel_forseen_dist * 0.75f;
			// decrease optimal range for ships with high range
			if (weapons.Count > 0)
				optimal_range *= Math.Min(1.0f, Math.Max(0.2f, 150.0f / weapons[0].stats.range));
			double dist = Helpers.Distance(ref location, ref target.location);
			if (dist <= optimal_range * 0.5) {
				if (Helpers.GetAngleDiff(direction, required_direction) < 112d) {
					required_direction = required_direction + 180f;
					require_speed = Helpers.GetAngleDiff(direction, required_direction - 180f) > 45d;
				} else
					require_speed = Helpers.GetAngleDiff(direction, required_direction) > 90d;
			} else if (dist <= optimal_range * 1.0) {
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
				require_speed = Helpers.GetAngleDiff(direction, required_direction) < 45d;
			}
		}
		void AITowardTargetMining(ref float required_direction, ref bool require_speed, Ship target) {
			var forseen_location = ForseeLocation(target);
			// world.Effects.Add(New Effect With {.Type = "EFF_OrderTarget", .Coo = forseen_location, .Direction = 45, .speed = 0})
			required_direction = (float)Helpers.GetQA((int)location.X, (int)location.Y, (int)forseen_location.X, (int)forseen_location.Y);
			double rel_forseen_dist = Helpers.Distance(ref location, ref forseen_location) - ai_target.stats.width / 2d;
			double optimal_range = 180.0f;
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
				require_speed = (this.speed < target.speed);
			} else {
				if (target.stats.speed * 1.1d > stats.speed) {
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
			if (weapons.Count > 0 && fram % 2 == 0)
				foreach (Weapon AWeap in weapons)
					if (AWeap.Bar > 0) {
						double closest_score = double.MaxValue;
						Ship weapon_targeted_ship = null; // Pas de cible
						int weapon_location_x = (int)(Math.Sin(2d * Math.PI * (AWeap.Loc + direction) / 360d) * (stats.width / 2d) + location.X);
						int weapon_location_y = (int)(Math.Cos(2d * Math.PI * (AWeap.Loc + direction) / 360d) * (stats.width / 2d) + location.Y);
						// target in range
						if (ai_target is object && !AIHasOrder((int)AIOrder.Mine))
							if (!team.IsFriendWith(ai_target.team)) {
								var argp2 = AWeap.ForseeShootingLocation(ai_target);
								double dist = Helpers.Distance(ref location, ref argp2) - ai_target.stats.width / 2d;
								if (dist < AWeap.stats.range * 0.9d)
									weapon_targeted_ship = ai_target;
							}
						// target not in range, find another
						if (weapon_targeted_ship is null)
							foreach (Ship OVessel in this.sector.EnumerateNearbyShips()) {
								if (ReferenceEquals(OVessel, this))
									continue;
								if (!allow_mining && (OVessel.stats.default_weapons.Count == 0 || OVessel.team.affinity == AffinityEnum.Wilderness))
									continue;
								if (Helpers.Distance(ref location, ref OVessel.location) < AWeap.stats.range)
									if (!team.IsFriendWith(OVessel.team)) {
										// Dim dist As Integer = Helpers.Distance(ToX, ToY, OVessel.location.X, OVessel.location.Y) - OVessel.stats.width / 2
										var argp21 = AWeap.ForseeShootingLocation(OVessel);
										double score = Helpers.Distance(ref location, ref argp21) - OVessel.stats.width / 2;
										if (score < AWeap.stats.range * 0.9d) {
											if (score < AWeap.stats.range) {
												if (!ReferenceEquals(OVessel.team, world.wilderness_team) && !team.IsFriendWith(OVessel.team))
													score /= 8;
												if (ReferenceEquals(ai_target, OVessel))
													score /= 4;
												if ((AWeap.stats.special & (int)Weapon.SpecialBits.EMP) != 0 && OVessel.IsDisabled())
													score *= 6;
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

		public Ship GetClosestEscortableShip(PointF ptn, double enemies = 2.0d, double neutrals = 1.0d, double allies = 0.0d) {
			const double max_distance = double.PositiveInfinity;
			// maximum square distance
			double closest_distance_sq = double.PositiveInfinity;
			if (max_distance < Math.Sqrt(double.MaxValue))
				closest_distance_sq = max_distance * max_distance;
			// mods are applied to square distance so they should be sqare too
			enemies *= Math.Abs(enemies);
			neutrals *= Math.Abs(neutrals);
			allies *= Math.Abs(allies);
			// find the ship
			Ship closest_ship = null;
			foreach (Ship other_ship in world.ships)
				if (!ReferenceEquals(this, other_ship)) {
					double distance_sq;
					// dont look for faster ships
					if (this.stats.speed < other_ship.stats.speed)
						continue;
					// relationship priority
					if (other_ship.team.affinity == AffinityEnum.Wilderness || team.affinity == AffinityEnum.Wilderness)
						distance_sq = Helpers.DistanceSQ(ref ptn, ref other_ship.location) / neutrals;
					else if (team.IsFriendWith(other_ship.team))
						distance_sq = Helpers.DistanceSQ(ref ptn, ref other_ship.location) / allies;
					else
						distance_sq = Helpers.DistanceSQ(ref ptn, ref other_ship.location) / enemies;
					// test if closer
					if (distance_sq < closest_distance_sq) {
						closest_ship = other_ship;
						closest_distance_sq = distance_sq;
					}
				}
			return closest_ship;
		}
		public Ship GetClosestEscortableShip(double enemies = 2.0d, double neutrals = 1.0d, double allies = 0.0d) {
			return (GetClosestEscortableShip(this.location, enemies, neutrals, allies));
		}
		public Ship GetClosestShip(PointF ptn, double enemies = 2.0d, double neutrals = 1.0d, double allies = 0.0d) {
			const double max_distance = double.PositiveInfinity;
			// maximum square distance
			double closest_distance_sq = double.PositiveInfinity;
			if (max_distance < Math.Sqrt(double.MaxValue))
				closest_distance_sq = max_distance * max_distance;
			// mods are applied to square distance so they should be sqare too
			enemies *= Math.Abs(enemies);
			neutrals *= Math.Abs(neutrals);
			allies *= Math.Abs(allies);
			// find the ship
			Ship closest_ship = null;
			foreach (Ship other_ship in world.ships)
				if (!ReferenceEquals(this, other_ship)) {
					double distance_sq;
					// relationship priority
					if (other_ship.team.affinity == AffinityEnum.Wilderness || team.affinity == AffinityEnum.Wilderness)
						distance_sq = Helpers.DistanceSQ(ref ptn, ref other_ship.location) / neutrals;
					else if (team.IsFriendWith(other_ship.team))
						distance_sq = Helpers.DistanceSQ(ref ptn, ref other_ship.location) / allies;
					else
						distance_sq = Helpers.DistanceSQ(ref ptn, ref other_ship.location) / enemies;
					// test if closer
					if (distance_sq < closest_distance_sq) {
						closest_ship = other_ship;
						closest_distance_sq = distance_sq;
					}
				}
			return closest_ship;
		}
		public Ship GetClosestShip(double enemies = 2.0d, double neutrals = 1.0d, double allies = 0.0d) {
			return (GetClosestShip(this.location, enemies, neutrals, allies));
		}

		/**
		 * @brief Compute the point to aim to intercept another ship.
		 */
		public PointF ForseeLocation(Ship target_ship) {
			double dist = Helpers.Distance(ref location, ref target_ship.location);
			double time = dist / Math.Max(1.0d, stats.speed);
			return new PointF((float)(target_ship.location.X + target_ship.speed_vec.X * time), (float)(target_ship.location.Y + target_ship.speed_vec.Y * time));
		}
		public PointF ForseeLocation(Ship target_ship, float factor) {
			double dist = Helpers.Distance(ref location, ref target_ship.location);
			double time = dist / Math.Max(1.0d, stats.speed) * factor;
			return new PointF((float)(target_ship.location.X + target_ship.speed_vec.X * time), (float)(target_ship.location.Y + target_ship.speed_vec.Y * time));
		}

		/**
		 * @brief Cause the ship to turn toward a direction by at most its turn speed.
		 */
		public void TurnTowardDirection(float dir) {
			while (dir > 360f)
				dir -= 360f;
			while (dir < 0f)
				dir += 360f;
			while (direction > 360f)
				direction = direction - 360f;
			while (direction < 0f)
				direction = direction + 360f;
			if (Helpers.GetAngleDiff(direction, dir) < stats.turn) {
				direction = dir;
				return;
			} else if (dir > 180f)
				if (direction > dir - 180f && direction < dir)
					direction = (float)(direction + stats.turn);
				else
					direction = (float)(direction - stats.turn);
			else if (direction < dir + 180f && direction > dir)
				direction = (float)(direction - stats.turn);
			else
				direction = (float)(direction + stats.turn);
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
			if (Upgrading != null)
				return (false);
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
			upgrade.ApplyPurchaseEffects(this);
			this.Upgrading = upgrade;
			this.upgrade_progress = 0;
		}
		public void UpgradeForFree(string upgrade_name) {
			UpgradeForFree(Upgrade.upgrades[upgrade_name]);
		}



	}
}
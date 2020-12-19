using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Flee {
	
	/**
	 * @brief Represent an effect of upgrade.
	 */
	public class UpgradeEffect {

		/* members */
		public bool advanced = false;
		public string op = null;
		public string left = null;
		public string right = null;

		/* Construction */
		public UpgradeEffect(string str) {
			string[] strs = str.Split(' ');
			left = strs[0];
			if (strs.Length > 3 || strs.Length == 0)
				throw new Exception("bad effect or condition");
			if (strs.Length == 1) {
				advanced = true;
				op = strs[0];
			}
			if (strs.Length == 2) {
				advanced = true;
				op = strs[0];
				right = strs[1];
			}
			if (strs.Length == 3) {
				left = strs[0];
				op = strs[1];
				right = strs[2];
			}
		}

		/* Export */
		public override string ToString() {
			if (left != null && right != null)
				return (left + " " + op + " " + right);
			else if (right != null)
				return (op + " " + right);
			else
				return (op);
		}

		/* Apply this effect to a ship */
		public void Apply(Ship ship) {
			if (this.advanced) {
				switch (this.op) {
				// politic
				case "sos()": return;
				case "suicide()": ship.integrity = Int32.MinValue / 2; return;
				case "free()": ship.SetTeam(ship.world.wilderness_team); return;
				case "give()": return;
				case "surrender()": return;
				case "affinity()": ship.team.affinity = (AffinityEnum)Enum.Parse(typeof(AffinityEnum), this.right); return;
				//
				case "effect()":
					ship.world.effects.Add(new Effect(-1, this.right, ship.location, ship.direction, ship.speed_vec));
					return;
				case "max_ships()": ship.team.ship_count_limit += Convert.ToInt32(this.right); return;
				case "max_upgrades()": ship.team.upgrade_slots_bonus += Convert.ToInt32(this.right); return;
				case "brightshield()": ship.BrightShield(); return;
				case "ask_surrender()": return;
				case "team_surrender()": return;
				case "warp()": 
					/* // TODO: */ 
					return;
				case "abandon()": ship.team = ship.world.wilderness_team; return;
				case "ascend()": ship.team.has_ascended = true; return;
				case "toggle_cheats()": ship.team.cheats_enabled = !ship.team.cheats_enabled; return;
				case "set_bot()": ship.bot_ship = (Convert.ToInt32(this.right) != 0); return;
				case "team_set_bot()": ship.bot_ship = (Convert.ToInt32(this.right) != 0); return;
				case "color()": 
					ship.color = Helpers.ToColor(right);
					ship.UpdateSprite();
					return;
				case "clear_upgrades()":
					ship.Upgrading = null;
					ship.upgrade_progress = 0;
					ship.upgrades.Clear();
					ship.ResetStats();
					return;
				case "summon()":
					ship.world.ships.Add(new Ship(ship.world, ship.team, right) { location = new Point((int)(ship.location.X + ship.world.gameplay_random.Next(-10, 11)), (int)(ship.location.Y + ship.world.gameplay_random.Next(-10, 11))) });
					ship.world.ships[ship.world.ships.Count - 1].direction = ship.direction;
					if (ship.world.ships[ship.world.ships.Count - 1].weapons.Count > 0 && (ship.world.ships[ship.world.ships.Count - 1].weapons[0].stats.special & (int)Weapon.SpecialBits.SelfExplode) != 0) {
						if (ship.target != null && !ship.team.IsFriendWith(ship.target.team))
							ship.world.ships[ship.world.ships.Count - 1].target = ship.target;
						else
							ship.world.ships[ship.world.ships.Count - 1].target = null;
						ship.world.ships[ship.world.ships.Count - 1].behavior = Ship.BehaviorMode.Folow;
						ship.world.ships[ship.world.ships.Count - 1].agressivity = ship.agressivity * 100.0f;
						ship.world.ships[ship.world.ships.Count - 1].bot_ship = true;
					} else {
						ship.world.ships[ship.world.ships.Count - 1].behavior = Ship.BehaviorMode.Folow;
						ship.world.ships[ship.world.ships.Count - 1].target = ship;
					}
					return;
				default: throw new Exception("invalid op \'" + this.op + "\'");
				}
			} else {
				switch (this.left) {
				case "integrity": SetOp(this.op, ref ship.stats.integrity, this.right); return;
				case "ship.integrity": SetOp(this.op, ref ship.integrity, this.right); return;
				case "repair": SetOp(this.op, ref ship.stats.repair, this.right); return;
				case "shield": SetOp(this.op, ref ship.stats.shield, this.right); return;
				case "ship.shield": SetOp(this.op, ref ship.shield, this.right); return;
				case "shield_opacity":
					SetOp(this.op, ref ship.stats.shield_opacity, this.right);
					return;
				case "shield_regeneration":
					SetOp(this.op, ref ship.stats.shield_regeneration, this.right);
					return;
				case "deflectors": SetOp(this.op, ref ship.stats.deflectors, this.right); return;
				case "hot_deflectors": SetOp(this.op, ref ship.stats.hot_deflectors, this.right); return;
				case "cold_deflectors": SetOp(this.op, ref ship.stats.cold_deflectors, this.right); return;
				case "ship.deflectors": SetOp(this.op, ref ship.deflectors, this.right); return;
				case "deflectors_cooldown": SetOp(this.op, ref ship.stats.deflectors_cooldown, this.right); return;
				case "speed": SetOp(this.op, ref ship.stats.speed, this.right); return;
				case "ship.speed": SetOp(this.op, ref ship.speed, this.right); return;
				case "turn": SetOp(this.op, ref ship.stats.turn, this.right); return;
				case "weapon_salvo": SetOp(this.op, ref ship.weapons[0].stats.salvo, this.right); return;
				case "weapon_celerity": SetOp(this.op, ref ship.weapons[0].stats.celerity, this.right); return;
				case "weapon_loadtime": SetOp(this.op, ref ship.weapons[0].stats.loadtime, this.right); return;
				case "weapon_power": SetOp(this.op, ref ship.weapons[0].stats.power, this.right); return;
				case "weapon_range": SetOp(this.op, ref ship.weapons[0].stats.range, this.right); return;
				case "type":
					SetOp(this.op, ref ship.stats.name, this.right);
					ship.SetStats(ship.stats.name);
					return;
				case "width": SetOp(this.op, ref ship.stats.width, this.right); return;
				default: throw new Exception("invalid left operand \'" + this.left + "\'");
				}
			}
		}

		/* Value Change Operations */
		public static void SetOp(string op, ref double arg1, double arg2) {
			switch (op) {
			case "=": arg1 = arg2; break;
			case "+=": arg1 += arg2; break;
			case "-=": arg1 -= arg2; break;
			case "*=": arg1 *= arg2; break;
			case "/=": arg1 /= arg2; break;
			case "min()=": arg1 = Math.Min(arg1, arg2); break;
			case "max()=": arg1 = Math.Max(arg1, arg2); break;
			default: throw new Exception("invalid op " + op);
			}
		}
		public static void SetOp(string op, ref float arg1, float arg2) {
			double arg1bis = (double)arg1;
			SetOp(op, ref arg1bis, (double)arg2);
			arg1 = (float)arg1bis;
		}
		public static void SetOp(string op, ref int arg1, int arg2) {
			double arg1bis = (double)arg1;
			SetOp(op, ref arg1bis, (double)arg2);
			arg1 = (int)arg1bis;
		}
		public static void SetOp(string op, ref string arg1, string arg2) {
			switch (op) {
			case "=": arg1 = arg2; break;
			case "+=": arg1 += arg2; break;
			case "=+": arg1 = arg2 + arg1; break;
			default: throw new Exception("invalid op " + op);
			}
		}
		public static void SetOp(string op, ref double arg1, string arg2) {
			SetOp(op, ref arg1, Helpers.ToDouble(arg2));
		}
		public static void SetOp(string op, ref float arg1, string arg2) {
			SetOp(op, ref arg1, (float)Helpers.ToDouble(arg2));
		}
		public static void SetOp(string op, ref int arg1, string arg2) {
			double prev = arg1;
			SetOp(op, ref prev, Helpers.ToDouble(arg2));
			arg1 = (int)prev;
		}

		/* Value Return Operations */
		public static double ReturnOp(string op, ref double arg1, double arg2) {
			switch (op) {
			case "==": return ((arg1 == arg2) ? 1.0 : 0.0);
			case ">": return ((arg1 > arg2) ? 1.0 : 0.0);
			case ">=": return ((arg1 >= arg2) ? 1.0 : 0.0);
			case "<": return ((arg1 < arg2) ? 1.0 : 0.0);
			case "<=": return ((arg1 <= arg2) ? 1.0 : 0.0);
			case "=": return (arg1 = arg2);
			case "+": return (arg1 + arg2);
			case "-": return (arg1 - arg2);
			case "*": return (arg1 * arg2);
			case "/": return (arg1 / arg2);
			case "min()": return (Math.Min(arg1, arg2));
			case "max()": return (Math.Max(arg1, arg2));
			case "+=": return (arg1 += arg2);
			case "-=": return (arg1 -= arg2);
			case "*=": return (arg1 *= arg2);
			case "/=": return (arg1 /= arg2);
			case "min()=": return (arg1 = Math.Min(arg1, arg2));
			case "max()=": return (arg1 = Math.Max(arg1, arg2));
			default: throw new Exception("invalid op");
			}
		}
	}
}

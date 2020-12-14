using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Flee {
	
	/**
	 * @brief Represent an effect or condition of Upgrades.
	 */
	public class UpgradeEffect {

		/* members */
		public bool advanced_effect = false;
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
				advanced_effect = true;
				op = strs[0];
			}
			if (strs.Length == 2) {
				advanced_effect = true;
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
		public bool HasRequirements(Ship ship) {
			if (this.advanced_effect) {
				switch (this.op) {
				case "ship_slot()": return (ship.world.CountTeamShips(ship.team) < ship.team.ship_count_limit);
				case "ship_slots()": return (ship.team.ship_count_limit - ship.world.CountTeamShips(ship.team) >= Convert.ToInt32(this.right));
				}
			}
			switch (this.left) {
			case "integrity": return (ConditionOp(this.op, ship.stats.integrity, this.right));
			case "repair": return (ConditionOp(this.op, ship.stats.repair, this.right));
			case "shield": return (ConditionOp(this.op, ship.stats.shield, this.right));
			case "shield_opacity": return (ConditionOp(this.op, ship.stats.shield_opacity, this.right));
			case "shield_regeneration": return (ConditionOp(this.op, ship.stats.shield_regeneration, this.right));
			case "deflectors": return (ConditionOp(this.op, ship.stats.deflectors, this.right));
			case "deflectors_cooldown":  return (ConditionOp(this.op, ship.stats.deflectors_cooldown, this.right));
			case "speed": return (ConditionOp(this.op, ship.stats.speed, this.right));
			case "turn": return (ConditionOp(this.op, ship.stats.turn, this.right));
			case "weapon_count": return (ConditionOp(this.op, ship.weapons.Count, this.right));
			default: throw new Exception("invalid left operand \'" + this.left + "\'");
			}
		}
		public void Apply(Ship ship, bool first_time) {
			if (this.advanced_effect) {
				switch (this.op) {
				case "sos()": return;
				case "ask_surrender()": return;
				case "team_surrender()": return;
				case "jump()": ship.speed = Convert.ToInt32(this.right); return;
				case "warp()": ship.integrity = Int32.MinValue / 2; return;
				case "max_ships()":
					if (!first_time)
						return;
					ship.team.ship_count_limit += Convert.ToInt32(this.right); 
					return;
				case "bonus_slots()":
					if (!first_time)
						return;
					ship.team.upgrade_slots_bonus += Convert.ToInt32(this.right);
					return;
				case "affinity()":
					if (!first_time)
						return;
					ship.team.affinity = (AffinityEnum)Enum.Parse(typeof(AffinityEnum), this.right);
					return;
				case "multiply_integrity()": ship.integrity += ship.stats.integrity * (float)Helpers.ToDouble(this.right); return;
				case "multiply_shield()":
					ship.shield += ship.stats.shield * (float)Helpers.ToDouble(this.right);
					ship.ResetShieldPoint();
					return;
				case "abandon()": ship.team = ship.world.wilderness_team; return;
				case "give()": return;
				case "suicide()": ship.integrity = Int32.MinValue / 2; return;
				case "ascend()": ship.team.has_ascended = true; return;
				case "enable_cheats()": ship.team.cheats_enabled = true; return;
				case "disable_cheats()": ship.team.cheats_enabled = false; return;
				case "color()": ship.color = Helpers.ToColor(right); return;
				case "summon()":
					if (!first_time)
						return;
					ship.world.ships.Add(new Ship(ship.world, ship.team, right) { location = new Point((int)(ship.location.X + ship.world.gameplay_random.Next(-10, 11)), (int)(ship.location.Y + ship.world.gameplay_random.Next(-10, 11))) });
					ship.world.ships[ship.world.ships.Count - 1].direction = ship.direction;
					if (ship.world.ships[ship.world.ships.Count - 1].weapons.Count > 0 && (ship.world.ships[ship.world.ships.Count - 1].weapons[0].stats.special & (int)Weapon.SpecialBits.SelfExplode) != 0) {
						if (!ship.team.IsFriendWith(ship.target.team))
							ship.world.ships[ship.world.ships.Count - 1].target = ship.target;
						else
							ship.world.ships[ship.world.ships.Count - 1].target = null;
						ship.world.ships[ship.world.ships.Count - 1].behavior = Ship.BehaviorMode.Folow;
						ship.world.ships[ship.world.ships.Count - 1].agressivity = ship.agressivity * 100d;
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
				case "integrity": 
					SetOp(this.op, ref ship.stats.integrity, this.right);
					if (first_time)
						SetOp(this.op, ref ship.integrity, this.right);
					return;
				case "repair": SetOp(this.op, ref ship.stats.repair, this.right); return;
				case "shield": 
					SetOp(this.op, ref ship.stats.shield, this.right);
					ship.ResetShieldPoint();
					return;
				case "shield_opacity":
					SetOp(this.op, ref ship.stats.shield_opacity, this.right);
					ship.ResetShieldPoint();
					return;
				case "shield_regeneration":
					SetOp(this.op, ref ship.stats.shield_regeneration, this.right);
					ship.ResetShieldPoint();
					return;
				case "deflectors": SetOp(this.op, ref ship.stats.deflectors, this.right); return;
				case "deflectors_cooldown": SetOp(this.op, ref ship.stats.deflectors_cooldown, this.right); return;
				case "speed": SetOp(this.op, ref ship.stats.speed, this.right); return;
				case "turn": SetOp(this.op, ref ship.stats.turn, this.right); return;
				case "weapon_salvo": SetOp(this.op, ref ship.weapons[0].stats.salvo, this.right); return;
				case "weapon_celerity": SetOp(this.op, ref ship.weapons[0].stats.celerity, this.right); return;
				case "weapon_loadtime": SetOp(this.op, ref ship.weapons[0].stats.loadtime, this.right); return;
				case "weapon_power": SetOp(this.op, ref ship.weapons[0].stats.power, this.right); return;
				case "weapon_range": SetOp(this.op, ref ship.weapons[0].stats.range, this.right); return;
				case "type":
					if (!first_time)
						return;
					SetOp(this.op, ref ship.stats.name, this.right);
					ship.SetStats(ship.stats.name);
					return;
				default: throw new Exception("invalid left operand \'" + this.left + "\'");
				}
			}
		}

		/* Conditional Operations */
		public static bool ConditionOp(string op, double arg1, double arg2) {
			switch (op) {
			case "==": return (arg1 == arg2);
			case ">": return (arg1 > arg2);
			case ">=": return (arg1 >= arg2);
			case "<": return (arg1 < arg2);
			case "<=": return (arg1 <= arg2);
			default: throw new Exception("invalid op " + op);
			}
		}
		public static bool ConditionOp(string op, float arg1, float arg2) {
			return (ConditionOp(op, (double)arg1, (double)arg2));
		}
		public static bool ConditionOp(string op, int arg1, int arg2) {
			return (ConditionOp(op, (double)arg1, (double)arg2));
		}
		public static bool ConditionOp(string op, string arg1, string arg2) {
			switch (op) {
			case "==": return (arg1 == arg2);
			case ">": return (arg1.Length > arg2.Length);
			case ">=": return (arg1.Length >= arg2.Length);
			case "<": return (arg1.Length < arg2.Length);
			case "<=": return (arg1.Length <= arg2.Length);
			case "contains()": return (arg1.Contains(arg2));
			default: throw new Exception("invalid op " + op);
			}
		}
		public static bool ConditionOp(string op, double arg1, string arg2) {
			return (ConditionOp(op, (double)arg1, Helpers.ToDouble(arg2)));
		}
		public static bool ConditionOp(string op, float arg1, string arg2) {
			return (ConditionOp(op, arg1, (float)Helpers.ToDouble(arg2)));
		}
		public static bool ConditionOp(string op, int arg1, string arg2) {
			return (ConditionOp(op, arg1, Convert.ToInt32(arg2)));
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
			SetOp(op, ref arg1, Convert.ToInt32(arg2));
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

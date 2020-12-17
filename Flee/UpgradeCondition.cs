using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flee {

	/**
	 * @brief Represent condition of upgrade.
	 */
	public class UpgradeCondition {

		/* members */
		public bool advanced = false;
		public string op = null;
		public string left = null;
		public string right = null;

		/* Construction */
		public UpgradeCondition(string str) {
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

		/* Test if this contition is met */
		public bool Test(Ship ship) {
			if (this.advanced) {
				switch (this.op) {
				case "ship_slot()":
					return (ship.world.CountTeamShips(ship.team) < ship.team.ship_count_limit);
				case "ship_slots()":
					return (ship.team.ship_count_limit - ship.world.CountTeamShips(ship.team) >= Convert.ToInt32(this.right));
				}
			}
			switch (this.left) {
			case "integrity":
				return (ConditionOp(this.op, ship.stats.integrity, this.right));
			case "repair":
				return (ConditionOp(this.op, ship.stats.repair, this.right));
			case "shield":
				return (ConditionOp(this.op, ship.stats.shield, this.right));
			case "shield_opacity":
				return (ConditionOp(this.op, ship.stats.shield_opacity, this.right));
			case "shield_regeneration":
				return (ConditionOp(this.op, ship.stats.shield_regeneration, this.right));
			case "deflectors":
				return (ConditionOp(this.op, ship.stats.deflectors, this.right));
			case "deflectors_cooldown":
				return (ConditionOp(this.op, ship.stats.deflectors_cooldown, this.right));
			case "speed":
				return (ConditionOp(this.op, ship.stats.speed, this.right));
			case "turn":
				return (ConditionOp(this.op, ship.stats.turn, this.right));
			case "weapon_count":
				return (ConditionOp(this.op, ship.weapons.Count, this.right));
			default:
				throw new Exception("invalid left operand \'" + this.left + "\'");
			}
		}

		/* Conditional Operations */
		public static bool ConditionOp(string op, double arg1, double arg2) {
			switch (op) {
			case "==":
				return (arg1 == arg2);
			case ">":
				return (arg1 > arg2);
			case ">=":
				return (arg1 >= arg2);
			case "<":
				return (arg1 < arg2);
			case "<=":
				return (arg1 <= arg2);
			default:
				throw new Exception("invalid op " + op);
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
			case "==":
				return (arg1 == arg2);
			case ">":
				return (arg1.Length > arg2.Length);
			case ">=":
				return (arg1.Length >= arg2.Length);
			case "<":
				return (arg1.Length < arg2.Length);
			case "<=":
				return (arg1.Length <= arg2.Length);
			case "contains()":
				return (arg1.Contains(arg2));
			case "iscontainedin()":
				return (arg1.Contains(arg2));
			default:
				throw new Exception("invalid op " + op);
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













	}
}

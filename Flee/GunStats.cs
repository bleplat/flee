using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Flee {

	public class GunStats {

		// shared
		public static Dictionary<string, GunStats> classes = new Dictionary<string, GunStats>();

		// identity And appearence
		public string name = "Default-Gun";
		public string sprite = "MSL";
		public int effect = 0;
		public string desc = "";
		public int special = 0;
		public int emissive_mode = 0;
		public string emissive_sprite = null;

		// stats
		public float range = 180;
		public float celerity = 16;
		public float power = 0;
		public float emp_power = 0;
		public int loadtime = 15;
		public int salvo = 1;
		public int sub_ammos = 1;

		// constructor
		public GunStats(string name) {
			this.name = name;
		}

		// Import/Export
		public override string ToString() {
			string total = "gun " + name + Constants.vbLf;
			if (!string.IsNullOrEmpty(desc)) total += Constants.vbTab + "desc=" + desc + Constants.vbLf;

			total += Constants.vbTab + "gun " + name + Constants.vbLf;
			total += Constants.vbTab + "sprite=" + sprite + Constants.vbLf;
			if (effect != 0) total += Constants.vbTab + "effect=" + effect.ToString() + Constants.vbLf;

			total += Constants.vbTab + "range=" + range.ToString() + Constants.vbLf;
			total += Constants.vbTab + "celerity=" + celerity.ToString() + Constants.vbLf;
			total += Constants.vbTab + "power=" + power.ToString() + Constants.vbLf;
			total += Constants.vbTab + "emp_power=" + emp_power.ToString() + Constants.vbLf;
			total += Constants.vbTab + "loadtime=" + loadtime.ToString() + Constants.vbLf;
			if (salvo != 1) total += Constants.vbTab + "salvo=" + salvo.ToString() + Constants.vbLf;
			if (sub_ammos != 1) total += Constants.vbTab + "sub_ammos=" + salvo.ToString() + Constants.vbLf;

			if (special != 0) total += Constants.vbTab + "special=" + Weapon.SpecialToString()[special] + Constants.vbLf;

			return total;
		}
		public void SetProperty(string name, string value) {
			switch (name ?? "") {
				case "desc": desc = value; break;
				case "sprite": sprite = value; break;
				case "effect": effect = Convert.ToInt32(value); break;
				case "range": range = Convert.ToInt32(value); break;
				case "celerity": celerity = Convert.ToInt32(value); break;
				case "power": power = Convert.ToInt32(value); break;
				case "emp_power": emp_power = Convert.ToInt32(value); break;
				case "loadtime": loadtime = Convert.ToInt32(value); break;
				case "salvo": salvo = Convert.ToInt32(value); break;
				case "sub_ammos": sub_ammos = Convert.ToInt32(value); break;
				case "special": special = Weapon.SpecialFromString(value);
					if (this.sub_ammos == 1) {
						if ((special & (int)Weapon.SpecialBits.Flak) != 0)
							sub_ammos = 8;
						if ((special & (int)Weapon.SpecialBits.Rail) != 0)
							sub_ammos = 8;
						if ((special & (int)Weapon.SpecialBits.SelfExplode) != 0)
							sub_ammos = 16;
						if ((special & (int)Weapon.SpecialBits.SelfNuke) != 0)
							sub_ammos = 16;
					}
					break;
				case "emissive_mode": 
					emissive_mode = Shoot.ShootEmissiveMode(value);
					if (emissive_sprite == null) 
						emissive_sprite = "Plasma";
					break;
				case "emissive_sprite": emissive_sprite = value; break;
				default:throw new Exception("Property " + name + " is not part of gun class!");
			}
		}

		public GunStats Clone() {
			return (GunStats)MemberwiseClone();
		}
	}
}
using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class ShipStats {

		// shared
		public static Dictionary<string, ShipStats> classes = new Dictionary<string, ShipStats>();

		public static string DumpClasses() {
			string total = "";
			foreach (ShipStats stats in classes.Values) {
				total += stats.ToString();
				total += Constants.vbLf;
			}

			return total;
		}

		// identity
		public string name = "Default";
		public string desc = null;

		// visual
		public string sprite = "Plasma";
		public int level = 0;
		public int width = -1;

		// stats (base)
		public int integrity = 0;
		public int repair = 0;
		public double speed = 0.0d;
		public double turn = 0.0d;
		public List<string> default_weapons = new List<string>();

		// stats (shields)
		public int shield = 0;
		public int shield_regeneration = 10;
		public double shield_opacity = 0d;

		// stats (deflectors)
		public int deflectors = 0;
		public int deflectors_cooldown = 64;
		public double hot_deflector = 0d;
		public bool cold_deflector = false;

		// crafting
		public List<string> crafts = new List<string>();
		public MaterialSet cost = new MaterialSet(0L, 0L, 0L, 0L);
		public int complexity = 0;

		// upgrades
		public List<string> native_upgrades = new List<string>();

		// constructor
		public ShipStats(string name) {
			this.name = name;
			SetSprite(name);
		}

		public void SetSprite(string sprite) {
			this.sprite = sprite;
			var try_bmp = Helpers.GetSprite(this.sprite, -1, -1, default);
			if (try_bmp is object) {
				width = (int)(try_bmp.Width / 8d - 2d);
				level = (int)Math.Sqrt(width);
				integrity = (int)(width * width / 12d);
				if (width >= 25)
					repair = 1;
				cost.Metal = width * 2 + level * 10;
				if (width >= 20)
					cost.Crystal = 1L;
			}
		}

		// Import/Export
		public void SetProperty(string name, string value) {
			switch (name ?? "") {
				case "desc": {
						desc = value;
						break;
					}

				case "sprite": {
						SetSprite(value);
						break;
					}

				case "level": {
						level = Convert.ToInt32(value);
						break;
					}

				case "width": {
						width = Convert.ToInt32(value);
						break;
					}

				case "integrity": {
						integrity = Convert.ToInt32(value);
						break;
					}

				case "repair": {
						repair = Convert.ToInt32(value);
						break;
					}

				case "speed": {
						speed = Helpers.ToDouble(value);
						if (turn == 0d)
							turn = speed;
						break;
					}

				case "turn": {
						turn = Helpers.ToDouble(value);
						break;
					}

				case "weapon": {
						default_weapons.Add(value);
						break;
					}

				case "shield": {
						shield = Convert.ToInt32(value);
						if (shield_opacity == 0d)
							shield_opacity = 25d;
						break;
					}

				case "shield_regeneration": {
						shield_regeneration = (int)Helpers.ToDouble(value);
						break;
					}

				case "shield_opacity": {
						shield_opacity = Helpers.ToDouble(value);
						break;
					}

				case "deflectors": {
						deflectors = Convert.ToInt32(value);
						break;
					}

				case "deflectors_cooldown": {
						deflectors_cooldown = Convert.ToInt32(value);
						break;
					}

				case "hot_deflector": {
						hot_deflector = Helpers.ToDouble(value);
						break;
					}

				case "cold_deflector": {
						cold_deflector = (Convert.ToInt32(value)) != 0;
						break;
					}

				case "craft": {
						crafts.Add(value);
						break;
					}

				case "native_upgrade": {
						native_upgrades.Add(value);
						break;
					}

				case "cost": {
						cost = new MaterialSet(value);
						if (complexity == 0)
							complexity = (int)((width * 5) + (cost.Metal / 8) + (cost.Crystal * 15L) + (cost.Antimatter / 4d) + (cost.Fissile * 100L));
						break;
					}

				case "complexity": {
						complexity = Convert.ToInt32(value);
						break;
					}

				default: {
						throw new Exception("'" + name + "' is not a valid ship property");
						break;
					}
			}
		}

		public override string ToString() {
			string total = "ship " + name + Constants.vbLf;
			if ((sprite ?? "") != (name ?? "")) total += Constants.vbTab + "sprite=" + sprite + Constants.vbLf;

			total += Constants.vbTab + "level=" + level.ToString() + Constants.vbLf;
			if (width != Helpers.GetSprite(sprite, -1, -1, default).Width / 8d - 2d) total += Constants.vbTab + "width=" + width.ToString() + Constants.vbLf;

			total += Constants.vbTab + "integrity=" + integrity.ToString() + Constants.vbLf;
			if (speed != 0.0d) total += Constants.vbTab + "speed=" + Helpers.ToString(speed) + Constants.vbLf;

			if (turn != speed) total += Constants.vbTab + "turn=" + Helpers.ToString(turn) + Constants.vbLf;

			foreach (string item in default_weapons)
				total += Constants.vbTab + "weapon=" + item + Constants.vbLf;
			// weapons
			if (shield > 0) {
				total += Constants.vbTab + "shield=" + shield.ToString() + Constants.vbLf;
				if (shield_regeneration != 10) total += Constants.vbTab + "shield_regeneration=" + Helpers.ToString(shield_regeneration) + Constants.vbLf;

				if (shield_opacity != 25d) total += Constants.vbTab + "shield_opacity=" + Helpers.ToString(shield_opacity) + Constants.vbLf;
			}

			if (deflectors > 0) {
				total += Constants.vbTab + "deflectors=" + deflectors.ToString() + Constants.vbLf;
				if (deflectors_cooldown != 64) total += Constants.vbTab + "deflectors_cooldown=" + deflectors_cooldown.ToString() + Constants.vbLf;
			}

			if (hot_deflector != 0)
				total += Constants.vbTab + "hot_deflector=" + Helpers.ToString(hot_deflector) + Constants.vbLf;

			if (cold_deflector) total += Constants.vbTab + "cold_deflector=" + Convert.ToInt32(cold_deflector).ToString() + Constants.vbLf;

			foreach (string item in crafts)
				total += Constants.vbTab + "craft=" + item + Constants.vbLf;
			foreach (string item in native_upgrades)
				total += Constants.vbTab + "native_upgrade=" + item + Constants.vbLf;
			total += Constants.vbTab + "cost=" + cost.ToString() + Constants.vbLf;
			total += Constants.vbTab + "complexity=" + complexity.ToString() + Constants.vbLf;
			return total;
		}

		public ShipStats Clone() {
			return (ShipStats)MemberwiseClone();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Flee {

	/**
	 * @brief Represent a Purchasable item, either an upgrade or a 
	 */
	public class Upgrade2 {

		/**
		 * @brief Represent the availability of an upgrade for a ship.
		 */
		public enum Availability : int {
			Available = 0,		// Can be installed now.
			NotEnoughSpace = 1,	// Missing space on the ship.
			NotUnlocked = 2,	// Require another upgrade.
			NotCompatible = 4,	// Doesnt meet requirements.
			NotVisible = 8,		// Other reasons the upgrade will never be allowed for a ship.
		}

		/* static list */
		public static Dictionary<string, Upgrade2> upgrades = new Dictionary<string, Upgrade2>();
		public void LoadBuildUpgrades() {
			foreach (ShipStats ship_class in ShipStats.classes.Values) {
				string build_ship_upgrade_name = "Build_" + ship_class.name;
				string launch_ship_upgrade_name = "Launch_" + ship_class.name;
				if (upgrades[build_ship_upgrade_name] is null && upgrades[launch_ship_upgrade_name] is null) {
					upgrades[build_ship_upgrade_name] = (new Upgrade2(build_ship_upgrade_name));
					upgrades[build_ship_upgrade_name].required_conditions.Add(new UpgradeEffect("ship_slot()"));
					upgrades[build_ship_upgrade_name].cost = ship_class.cost;
					upgrades[build_ship_upgrade_name].install = false;
					upgrades[build_ship_upgrade_name].required_slots = 0;
					upgrades[build_ship_upgrade_name].effects.Add(new UpgradeEffect("summon() " + ship_class.name));
					upgrades[build_ship_upgrade_name].delay = ship_class.complexity;
					upgrades[build_ship_upgrade_name].desc = "Build a " + ship_class.name + ".";
					if (ship_class.desc is object)
						upgrades[build_ship_upgrade_name].desc = ship_class.desc;
				}
			}
		}

		/* Identity */
		public string name = "Default";
		public string desc = "";
		public string sprite_name = "Upgrades";
		public SpriteArray sprite_array = null;
		public Point sprite_coords = new Point(0, 0);
		public Color sprite_color = default;
		public void UpdateSpriteArray() {
			if (sprite_name != null)
			sprite_array = SpriteArray.GetSpriteArray(sprite_name, sprite_color);
		}

		/* Constructor */
		public Upgrade2(string name) {
			this.name = name;
		}

		/* requirements */
		public bool install = true;
		public bool bots_allowed = true;
		public int required_slots = 1;
		public bool teamwide = false;
		public List<Upgrade2> required_upgrades = new List<Upgrade2>();
		public List<UpgradeEffect> required_conditions = new List<UpgradeEffect>();

		/* cost */
		public MaterialSet cost = new MaterialSet(0L, 0L, 0L, 0L);

		/* effect */
		public int delay = 0;
		public List<UpgradeEffect> effects = new List<UpgradeEffect>();

		/* Import & Export */
		public override string ToString() {
			string total = "";
			total += "upgrade " + name + "\n";
			total += "\tsprite=" + sprite_name + "\n";
			total += "\tdesc=" + desc + "\n";
			total += "\tsprite_coords=" + sprite_coords.X.ToString() + ";" + sprite_coords.Y.ToString() + "\n";
			total += "\tsprite_color=" + sprite_color.ToString() + "\n";
			total += "\tinstall=" + (install ? "1" : "0") + "\n";
			total += "\tbots_allowed=" + (bots_allowed ? "1" : "0") + "\n";
			foreach (Upgrade2 upgrade in required_upgrades)
				total += "\trequired_upgrade=" + upgrade.name + "\n";
			foreach (UpgradeEffect condition in required_conditions)
				total += "\trequirement=" + condition.ToString() + "\n";
			total += "\tcost=" + cost.ToString() + "\n";
			total += "\trequired_slots=" + required_slots.ToString() + "\n";
			foreach (UpgradeEffect effect in effects)
				total += "\teffect=" + effect.ToString() + "\n";
			return total;
		}
		public void SetProperty(string name, string value) {
			switch (name) {
			case "desc": desc = value; break;
			case "sprite": 
				sprite_name = value;
				UpdateSpriteArray();
				break;
			case "sprite_coords": sprite_coords = Helpers.PointFromString(value); break;
			case "sprite_color": 
				sprite_color = Helpers.ToColor(value);
				UpdateSpriteArray();
				break;
			case "install": install = (Convert.ToInt32(value) != 0); break;
			case "bots_allowed": bots_allowed = (Convert.ToInt32(value) != 0); break;
			case "required_upgrade": required_upgrades.Add(upgrades[value]); break;
			case "requirement": required_conditions.Add(new UpgradeEffect(value)); break;
			case "cost": cost.LoadFromString(value); break;
			case "required_slots": required_slots = Convert.ToInt32(value); break;
			case "effect": effects.Add(new UpgradeEffect(value)); break;
			default: throw new Exception("invalid upgrade property");
			}
		}

		/* Test */
		public bool IsUnlockedBy(Ship ship) {
			foreach (Upgrade2 upgrade in ship.upgrades) {
				if (!ship.upgrades.Contains(upgrade)) 
					return (false);
			}
			return (true);
		}
		public bool AreConditionsMetBy(Ship ship) {
			foreach (UpgradeEffect condition in required_conditions) {
				if (!condition.HasRequirements(ship)) 
					return (false);
			}
			return (true);
		}
		public bool IsVisibleBy(Ship ship) {
			if (ship.bot_ship && !bots_allowed)
				return (false);
			return (IsUnlockedBy(ship) && AreConditionsMetBy(ship));
		}
		public bool IsInstallableBy(Ship ship) {
			if (ship.bot_ship && !bots_allowed)
				return (false);
			if (install && (ship.upgrade_slots - ship.upgrades.Count) > required_slots) 
				return (false);
			if (!IsVisibleBy(ship)) 
				return (false);
			return (true);
		}
		public Availability GetAvailability(Ship ship) {
			if (ship.bot_ship && !bots_allowed)
				return (Availability.NotVisible);
			if (install && (ship.upgrade_slots - ship.upgrades.Count) > required_slots) 
				return (Availability.NotEnoughSpace);
			if (!IsUnlockedBy(ship))
				return (Availability.NotUnlocked);
			if (!AreConditionsMetBy(ship))
				return (Availability.NotCompatible);
			return (Availability.Available);
		}

		/* Application */
		public void ApplyEffects(Ship ship) {
			foreach (UpgradeEffect effect in effects) {
				effect.Apply(ship, false);
			}
		}
	}
}
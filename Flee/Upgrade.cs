using System;
using System.Collections.Generic;
using System.Drawing;

namespace Flee {

	/**
	 * @brief Represent a Purchasable item, either an upgrade or a 
	 */
	public class Upgrade {

		/**
		 * @brief Represent the availability of an upgrade for a ship.
		 */
		public enum Availability : int {
			Available = 0,		// Can be installed now.
			TeamFull = 1,		// Missing space on the ship.
			NotEnoughSpace = 2,	// Missing space on the ship.
			NotUnlocked = 4,	// Require another upgrade.
			NotCompatible = 8,	// Doesnt meet requirements.
			CannotBuild = 16,	// The purchasable is a ship and cannot be build by the purchaser.
			NotVisible = 64,	// Other reasons the upgrade will never be allowed for a ship.
		}

		/* static list */
		public static Dictionary<string, Upgrade> upgrades = new Dictionary<string, Upgrade>();
		public static void LoadBuildUpgrades() {
			foreach (ShipStats ship_class in ShipStats.classes.Values) {
				string build_ship_upgrade_name = "Build_" + ship_class.name;
				string launch_ship_upgrade_name = "Launch_" + ship_class.name;
				if (!upgrades.ContainsKey(build_ship_upgrade_name) && !upgrades.ContainsKey(launch_ship_upgrade_name)) {
					upgrades[build_ship_upgrade_name] = (new Upgrade(build_ship_upgrade_name));
					upgrades[build_ship_upgrade_name].cost = ship_class.cost;
					upgrades[build_ship_upgrade_name].install = false;
					upgrades[build_ship_upgrade_name].required_upgrade_slots = 0;
					upgrades[build_ship_upgrade_name].once_effects.Add(new UpgradeEffect("summon() " + ship_class.name));
					upgrades[build_ship_upgrade_name].time = ship_class.complexity;
					upgrades[build_ship_upgrade_name].desc = "Build a " + ship_class.name + ".";
					if (ship_class.desc is object)
						upgrades[build_ship_upgrade_name].desc = ship_class.desc;
					upgrades[build_ship_upgrade_name].sprite_name = ship_class.sprite;
					upgrades[build_ship_upgrade_name].UpdateSpriteArray();
					upgrades[build_ship_upgrade_name].required_team_slots = 1;
					upgrades[build_ship_upgrade_name].require_craft = true;
					if (ship_class.name.StartsWith("Legendary_")) {
						upgrades[build_ship_upgrade_name].install = true;
						upgrades[build_ship_upgrade_name].required_team_slots = 0;
						upgrades[build_ship_upgrade_name].desc += "/!\\ Can only be built once.";
					}
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
		public Upgrade(string name) {
			this.name = name;
		}
		public void SetAsOutfit() {
			this.install = true;
			this.required_upgrade_slots = 1;
			this.required_team_slots = 0;
		}
		public void SetAsBuild() {
			this.install = false;
			this.required_upgrade_slots = 0;
			this.required_team_slots = 1;
		}
		public void SetAsAbility() {
			this.install = false;
			this.required_upgrade_slots = 0;
			this.required_team_slots = 0;
		}

		/* requirements */
		public bool install = true;
		public bool require_nonbot = false;
		public bool require_craft = false;
		public int required_upgrade_slots = 1;
		public bool teamwide = false;
		public List<Upgrade> required_upgrades = new List<Upgrade>();
		public List<UpgradeCondition> required_stats = new List<UpgradeCondition>();
		public int required_team_slots = 0;

		/* cost */
		public MaterialSet cost = new MaterialSet(0L, 0L, 0L, 0L);

		/* effect */
		public int time = 0;
		public List<UpgradeEffect> once_effects = new List<UpgradeEffect>();
		public List<UpgradeEffect> effects = new List<UpgradeEffect>();

		/* Import & Export */
		public override string ToString() {
			string total = "";
			total += "upgrade " + name + "\n";
			total += "\tdesc=" + desc + "\n";
			total += "\tsprite=" + sprite_name + "\n";
			total += "\tsprite_coords=" + sprite_coords.X.ToString() + ";" + sprite_coords.Y.ToString() + "\n";
			total += "\tsprite_color=" + sprite_color.ToString() + "\n";
			total += "\tinstall=" + (install ? "1" : "0") + "\n";
			total += "\tteamwide=" + (teamwide ? "1" : "0") + "\n";
			total += "\trequire_nonbot=" + (require_nonbot ? "1" : "0") + "\n";
			total += "\trequire_craft=" + (require_nonbot ? "1" : "0") + "\n";
			foreach (Upgrade upgrade in required_upgrades)
				total += "\trequired_upgrade=" + upgrade.name + "\n";
			foreach (UpgradeCondition condition in required_stats)
				total += "\rrequired_stat=" + condition.ToString() + "\n";
			total += "\tcost=" + cost.ToString() + "\n";
			total += "\trequired_slots=" + required_upgrade_slots.ToString() + "\n";
			total += "\trequired_team_slots=" + required_team_slots.ToString() + "\n";
			total += "\ttime=" + time + "\n";
			foreach (UpgradeEffect effect in effects)
				total += "\teffect=" + effect.ToString() + "\n";
			foreach (UpgradeEffect once_effect in once_effects)
				total += "\tonce_effect=" + once_effect.ToString() + "\n";
			return total;
		}
		public void SetProperty(string name, string value) {
			switch (name) {
			case "desc": desc += value + "\r\n"; break;
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
			case "teamwide": teamwide = (Convert.ToInt32(value) != 0); break;
			case "require_nonbot": require_nonbot = (Convert.ToInt32(value) != 0); break;
			case "require_craft": require_craft = (Convert.ToInt32(value) != 0); break;
			case "required_upgrade": required_upgrades.Add(upgrades[value]); break;
			case "required_stat": required_stats.Add(new UpgradeCondition(value)); break;
			case "cost": cost.LoadFromString(value); break;
			case "required_slots": required_upgrade_slots = Convert.ToInt32(value); break;
			case "required_team_slots": required_upgrade_slots = Convert.ToInt32(value); break;
			case "effect": effects.Add(new UpgradeEffect(value)); break;
			case "once_effect": once_effects.Add(new UpgradeEffect(value)); break;
			case "time": time = Convert.ToInt32(value); break;
			default: throw new Exception("invalid upgrade property");
			}
		}

		/* Test */
		public bool AreStatsRequirementsMetBy(Ship ship) {
			foreach (UpgradeCondition condition in required_stats) {
				if (!condition.Test(ship)) 
					return (false);
			}
			return (true);
		}
		public bool ArePriorUpgradeRequirementsMetBy(Ship ship) {
			foreach (Upgrade upgrade in this.required_upgrades) {
				if (!ship.upgrades.Contains(upgrade))
					return (false);
			}
			return (true);
		}
		public Availability GetAvailability(Ship ship) {
			if (ship.team.cheats_enabled)
				return (Availability.Available);
			if (ship.bot_ship && require_nonbot)
				return (Availability.NotVisible);
			if (this.teamwide && (ship.stats.role & (int)ShipRole.Shipyard) == 0)
				return (Availability.NotVisible);
			if (this.required_team_slots > ship.team.ship_count_limit - ship.team.ship_count_approximation) 
				return (Availability.NotVisible);
			if (require_craft && !ship.stats.crafts.Contains(this.name))
				return (Availability.NotVisible);
			if (install && (ship.upgrade_slots - ship.upgrades.Count) < required_upgrade_slots) 
				return (Availability.NotEnoughSpace);
			if (!AreStatsRequirementsMetBy(ship))
				return (Availability.NotCompatible);
			if (!ArePriorUpgradeRequirementsMetBy(ship))
				return (Availability.NotUnlocked);
			return (Availability.Available);
		}

		/* Application */
		public void ApplyOnceEffects(Ship ship) {
			foreach (UpgradeEffect once_effect in once_effects) {
				once_effect.Apply(ship);
			}
		}
		public void ApplyEffects(Ship ship) {
			foreach (UpgradeEffect effect in effects) {
				effect.Apply(ship);
			}
		}
	}
}
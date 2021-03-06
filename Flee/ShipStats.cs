﻿using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {

	/**
	 * @brief Special flags representing ships gameplay role.
	 */
	public enum ShipRole {
		Shipyard = 0x1, // Capable of summoning others. It keeps the team status as alive.
		Defense = 0x2, // Summoned with Shipyards.
		Starter = 0x4, // May be summoned with a Shipyard in a player team.
		Playable = 0x10, // May be summoned in a player team.
		NPC = 0x20, // May be summoned in an NPC team.
		Boss = 0x40, // May be summoned as a Boss.
		Derelict = 0x80 ,// May be summoned as a derelict object.
		Static = 0x100, // May be summoned as a static object such as a star.
		Mine = 0x200, // Is mined instead of looted.
	}

	/**
	 * @brief Represent a Ship base stats.
	 */
	public class ShipStats {

		/* role */
		static int ShipRoles(string roles_str) {
			int total = 0;
			foreach (string role_str in roles_str.Split('|')) {
				total |= (int)Enum.Parse(typeof(ShipRole), role_str, true);
			}
			return (total);
		}
		static string ShipRoles(int roles) {
			string total = "";
			foreach (ShipRole role in Enum.GetValues(typeof(ShipStats))) {
				if ((roles & (int)role) != 0) {
					if (total.Length > 0)
						total += '|';
					total += role.ToString();
				}
			}
			return (total);
		}

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

		// spawning rules
		public double spawning_frequency = 1.0;
		public int spawning_amount_min = 1;
		public int spawning_amount_max = 2;

		// identity
		public string name = "Default";
		public string desc = null;
		public int role = 0; // ShipRole flags

		// visual
		public string sprite = "Plasma";
		public int level = 0;
		public int width = -1;

		// stats (base)
		public float integrity = 0;
		public float repair = 0;
		public float speed = 0.0f;
		public float turn = 0.0f;
		public List<string> default_weapons = new List<string>();

		// stats (shields)
		public float shield = 0;
		public float shield_regeneration = 0.0005f;
		public float shield_opacity = 0.0f;

		// stats (deflectors)
		public int deflectors = 0;
		public int deflectors_cooldown = 128;
		public int hot_deflectors = 0;
		public int cold_deflectors = 0;
		public int TotalDeflectorsMax() { return (deflectors + hot_deflectors); }

		// crafting
		public List<string> crafts = new List<string>();
		public MaterialSet cost = new MaterialSet(0L, 0L, 0L, 0L);
		public int complexity = 0;

		// upgrades
		public List<string> native_upgrades = new List<string>();

		// constructor
		public ShipStats(string name) {
			this.name = name;
			try {
				SetSprite(name);
			} catch (Exception) {
			}
		}

		public void SetSprite(string sprite) {
			this.sprite = sprite;
			var try_bmp =  SpriteArray.GetSpriteArray(this.sprite).GetSprite(0, 0);
			if (try_bmp is object) {
				width = try_bmp.Width;
				level = (int)Math.Sqrt(width) / 2;
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
				speed = Helpers.ToFloat(value);
				if (turn == 0d)
					turn = speed;
				break;
			}
			case "turn": {
				turn = Helpers.ToFloat(value);
				break;
			}
			case "weapon": {
				default_weapons.Add(value);
				break;
			}
			case "shield": {
				shield = Convert.ToInt32(value);
				if (shield_opacity == 0.0)
					shield_opacity = 0.25f;
				break;
			}
			case "shield_regeneration": {
				shield_regeneration = Helpers.ToFloat(value);
				break;
			}
			case "shield_opacity": {
				shield_opacity = Helpers.ToFloat(value);
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
			case "hot_deflectors": {
				hot_deflectors = Convert.ToInt32(value);
				break;
			}
			case "cold_deflectors": {
				cold_deflectors = Convert.ToInt32(value);
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
					complexity = (int)((width * 5) + (cost.Metal / 8) + (cost.Crystal * 15L) + (cost.Starfuel / 4d) + (cost.Fissile * 100L));
				break;
			}
			case "complexity": {
				complexity = Convert.ToInt32(value);
				break;
			}
			// spawning
			case "role": role = ShipRoles(value); break;
			case "spawning_frequency": spawning_frequency = Helpers.ToDouble(value); break;
			case "spawning_amount_min": spawning_amount_min = Convert.ToInt32(value); break;
			case "spawning_amount_max": spawning_amount_max = Convert.ToInt32(value); break;
			case "spawning_amount": {
				spawning_amount_min = Convert.ToInt32(value);
				spawning_amount_max = spawning_amount_min + 1;
				break;
			}
			default: throw new Exception("'" + name + "' is not a valid ship property");
			}
		}

		public override string ToString() {
			string total = "ship " + name + Constants.vbLf;
			if ((sprite ?? "") != (name ?? ""))
				total += Constants.vbTab + "sprite=" + sprite + Constants.vbLf;

			total += Constants.vbTab + "level=" + level.ToString() + Constants.vbLf;
			if (width != Helpers.GetSprite(sprite, -1, -1, default).Width / 8d - 2d)
				total += Constants.vbTab + "width=" + width.ToString() + Constants.vbLf;

			total += Constants.vbTab + "integrity=" + integrity.ToString() + Constants.vbLf;
			if (speed != 0.0d)
				total += Constants.vbTab + "speed=" + Helpers.ToString(speed) + Constants.vbLf;

			if (turn != speed)
				total += Constants.vbTab + "turn=" + Helpers.ToString(turn) + Constants.vbLf;

			foreach (string item in default_weapons)
				total += Constants.vbTab + "weapon=" + item + Constants.vbLf;
			// weapons
			if (shield > 0) {
				total += Constants.vbTab + "shield=" + shield.ToString() + Constants.vbLf;
				if (shield_regeneration != 10)
					total += Constants.vbTab + "shield_regeneration=" + Helpers.ToString(shield_regeneration) + Constants.vbLf;

				if (shield_opacity != 25d)
					total += Constants.vbTab + "shield_opacity=" + Helpers.ToString(shield_opacity) + Constants.vbLf;
			}

			if (deflectors > 0) {
				total += Constants.vbTab + "deflectors=" + deflectors.ToString() + Constants.vbLf;
				if (deflectors_cooldown != 64)
					total += Constants.vbTab + "deflectors_cooldown=" + deflectors_cooldown.ToString() + Constants.vbLf;
			}

			if (hot_deflectors != 0)
				total += "\thot_deflectors=" + hot_deflectors.ToString() + "\n";

			if (cold_deflectors != 0)
				total += "\tcold_deflectors=" + cold_deflectors.ToString() + "\n";

			foreach (string item in crafts)
				total += Constants.vbTab + "craft=" + item + Constants.vbLf;
			foreach (string item in native_upgrades)
				total += Constants.vbTab + "native_upgrade=" + item + Constants.vbLf;
			total += Constants.vbTab + "cost=" + cost.ToString() + Constants.vbLf;
			total += Constants.vbTab + "complexity=" + complexity.ToString() + Constants.vbLf;
			// spawning
			if (role != 0)
				total += "role=" + ShipRoles(role) + "\n";
			if (spawning_frequency != 1.0f)
				total += "\tspawning_frequency=" + Helpers.ToString(spawning_frequency) + "\n";
			if (spawning_amount_min != 1)
				total += "\tspawning_amount_min=" + spawning_amount_min.ToString() + "\n";
			if (spawning_amount_max != 2)
				total += "\tspawning_amount_max=" + spawning_amount_max.ToString() + "\n";
			return total;
		}

		public ShipStats Clone() {
			return (ShipStats)MemberwiseClone();
		}
	}
}
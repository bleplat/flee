using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Flee {

	/**
	 * @brief Helpers to load resources.
	 */
	public static class Loader {

		/* Settings */
		public static string data_folder = "./data";
		public static string mods_folder = "./mods";
		public static bool enable_mods_folder = false;

		/* Load Specific */
		public static Bitmap LoadProjectSprite(string sprite_name) {
			Bitmap bmp;
			try {
				bmp = new Bitmap(data_folder + "/sprites/" + sprite_name + ".png");
			} catch {
				bmp = new Bitmap(data_folder + "/sprites/" + sprite_name + ".bmp");
			}
			if (bmp.PixelFormat != Helpers.GetScreenPixelFormat()) {
				bmp = new Bitmap(bmp).Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), Helpers.GetScreenPixelFormat());
			}
			return (bmp);
		}

		/* Get Helpers */
		public static ShipStats RandomShipFromRole(Random rand, int required_roles) {
			// TODO: dictionary of ints matcing matches lists
			double total_frequency = 0.0;
			List<ShipStats> matches = new List<ShipStats>();
			foreach (ShipStats ship in ShipStats.classes.Values) {
				if ((ship.role & required_roles) == required_roles) {
					matches.Add(ship);
					total_frequency += ship.spawning_frequency;
				}
			}
			if (matches.Count > 0) {
				double chosen = rand.Next(0, (int)(total_frequency * 10000)) / 10000.0;
				foreach (ShipStats match in matches) {
					chosen -= match.spawning_frequency;
					if (chosen <= 0.0)
						return (match);
				}
				return (matches[rand.Next(0, matches.Count)]);
			}
			throw new Exception("no ship found with required roles");
		}
		public static List<string> GetSpawnUpgrades(Ship ship) {
			var upgrades = new List<string>();
			foreach (string craft in ship.stats.crafts)
				foreach (Upgrade a_up in Upgrade.upgrades)
					if ((a_up.name ?? "") == ("Build_" + craft ?? "") || (a_up.name ?? "") == ("Launch_" + craft ?? ""))
						upgrades.Add(a_up.name);
			return upgrades;
		}
		public static string GetRandomSpawnUpgrade(Random rand, Ship ship) {
			var upgrades = GetSpawnUpgrades(ship);
			if (upgrades.Count == 0)
				return null;
			return upgrades[rand.Next(0, upgrades.Count)];
		}

		/* Load Helpers */
		public static void LoadLists() {
			var list_classes = new List<ListClass>();
			foreach (FileInfo file in new DirectoryInfo(data_folder + "/lists/upgrades/").EnumerateFiles()) {
				list_classes.AddRange(GetListsFromFile(file.FullName));
			}
			foreach (FileInfo file in new DirectoryInfo(data_folder + "/lists/weapons/").EnumerateFiles()) {
				list_classes.AddRange(GetListsFromFile(file.FullName));
			}
			foreach (FileInfo file in new DirectoryInfo(data_folder + "/lists/ships/").EnumerateFiles()) {
				list_classes.AddRange(GetListsFromFile(file.FullName));
			}
			LoadLists(list_classes);
		}
		public static int GetIndentation(string line) {
			int count = 0;
			foreach (char c in line)
				if (c == '\t' || c == ' ')
					count += 1;
				else
					break;

			return count;
		}
		public static void LoadLists(List<ListClass> list_classes) {
			foreach (ListClass a_class in list_classes)
				switch (a_class.type) {
				case "gun": {
					if (!GunStats.classes.ContainsKey(a_class.name))
						GunStats.classes[a_class.name] = new GunStats(a_class.name);
					foreach (ListProperty prop in a_class.properties)
						GunStats.classes[a_class.name].SetProperty(prop.name, prop.value);
					break;
				}
				case "ship": {
					if (!ShipStats.classes.ContainsKey(a_class.name))
						ShipStats.classes[a_class.name] = new ShipStats(a_class.name);
					foreach (ListProperty prop in a_class.properties)
						ShipStats.classes[a_class.name].SetProperty(prop.name, prop.value);
					break;
				}
				case "upgrade": {
					if (!Upgrade2.upgrades.ContainsKey(a_class.name))
						Upgrade2.upgrades[a_class.name] = new Upgrade2(a_class.name);
					foreach (ListProperty prop in a_class.properties)
						Upgrade2.upgrades[a_class.name].SetProperty(prop.name, prop.value);
					break;
				}
				default: {
					throw new Exception("Unknown class type: " + a_class.type);
					break;
				}
				}
		}
		public static List<ListClass> GetListsFromFile(string filename) {
			return GetListsFromRaw(File.ReadAllText(filename).Replace("\r\n", "\n"));
		}
		public static List<ListClass> GetListsFromRaw(string data) {
			// use as LoadList(File.ReadAllText("./lists/weapons.txt").Replace(vbCr & vbLf, vbLf))
			data += "\n";
			var list_classes = new List<ListClass>();
			var lines = data.Split('\n');
			string header = "";
			var paragraph = new List<string>();
			foreach (string line in lines) { // reading lines one by one
				if (header.Length > 0)
					if (line.Length > 0 && (line[0] == '\t' || line[0] == ' '))
						paragraph.Add(line.Substring(1, line.Length - 1));
					else {
						list_classes.Add(new ListClass(header, paragraph));
						header = "";
						paragraph.Clear();
					}

				if (header.Length == 0)
					if (line.Length == 0 || line[0] == '#')
						continue;
					else if (line[0] == ' ' || line[0] == '\t')
						throw new Exception("Malformed line: " + line);
					else
						header = line;
			}
			return list_classes;
		}

		/* Load & Unload */
		public static void Load() {
			Upgrade.LoadRegUpgrades();
			LoadLists();
			Upgrade.LoadBuildUpgrades();
		}
		public static void UnLoad() {
			ShipStats.classes.Clear();
			GunStats.classes.Clear();
			SpriteArray.ClearSpriteArrays();
		}
	}
}

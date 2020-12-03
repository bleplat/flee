using System;
using System.Collections.Generic;
using System.Drawing;

namespace Flee {
	public class Upgrade2 {
		public Upgrade2(string name) {
			this.name = name;
		}

		// Upgrade Name
		public string name = "Default";
		// Upgrade Sprite icon
		public string sprite = "Ups";
		public Point coords = new Point(0, 0);
		// Dependencies
		public MaterialSet cost = new MaterialSet(0L, 0L, 0L, 0L);
		public int ships = 0; // > 0 if this summon ships, checked against max ships
		public List<string> requieres = new List<string>();

		// Import/Export
		public override string ToString() {
			string total = "";
			total += "UPGRADE " + name + @"\n";
			total += "sprite=" + sprite + @"\n";
			total += "coords=" + coords.X.ToString() + ";" + coords.Y.ToString() + @"\n";
			total += "cost=" + cost.ToString() + @"\n";
			total += "ships=" + ships.ToString() + @"\n";
			return total;
		}

		public void SetProperty(string name, string value) {
			switch (name ?? "") {
				case "sprite": {
						sprite = value;
						break;
					}

				case "coords": {
						coords = Helpers.PointFromString(value);
						break;
					}

				case "cost": {
						cost.LoadFromString(value);
						break;
					}

				case "ships": {
						ships = Convert.ToInt32(value);
						break;
					}

				case "requiere": {
						ships = Convert.ToInt32(value);
						break;
					}
			}
		}
	}
}
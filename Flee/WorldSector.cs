using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Flee {
	public class WorldSector {

		/* Location */
		public World world;
		public Point coords = new Point(0, 0);
		public List<WorldSector> foreign_sectors = new List<WorldSector>();

		/* Content */
		public List<Ship> ships = new List<Ship>();
		public List<Shoot> shoots = new List<Shoot>();
		
		/* Build */
		public WorldSector(in World world, int x, int y) {
			this.world = world;
			coords.X = x;
			coords.Y = y;
		}
		public void InitForeignSectors() {
			foreign_sectors.Clear();
			foreign_sectors.Add(world.sectors[coords.X, coords.Y]);
			int x_max = Math.Min(World.sectors_count_x - 1, coords.X + 1);
			int y_max = Math.Min(World.sectors_count_y - 1, coords.Y + 1);
			for (int x = Math.Max(0, coords.X - 1); x <= x_max; x++) {
				for (int y = Math.Max(0, coords.Y - 1); y <= y_max; y++) {
					if (x != coords.X || y != coords.Y) {
						foreign_sectors.Add(world.sectors[x, y]);
					}
				}
			}
		}

		/* Enumerators */
		public IEnumerable<WorldSector> EnumerateNearbySectors() {
			yield return world.sectors[coords.X, coords.Y];
			int x_max = Math.Min(World.sectors_count_x - 1, coords.X + 1);
			int y_max = Math.Min(World.sectors_count_y - 1, coords.Y + 1);
			for (int x = Math.Max(0, coords.X - 1); x <= x_max; x++) {
				for (int y = Math.Max(0, coords.Y - 1); y <= y_max; y++) {
					if (x != coords.X || y != coords.Y) {
						yield return world.sectors[x, y];
					}
				}
			}
			yield break;
		}
		public IEnumerable<Ship> EnumerateNearbyShips() {
			foreach (WorldSector sector in foreign_sectors) {
				foreach (Ship ship in sector.ships) {
					yield return ship;
				}
			}
			yield break;
		}
		public IEnumerable<Shoot> EnumerateNearbyShoots() {
			foreach (WorldSector sector in foreign_sectors) {
				foreach (Shoot shoot in sector.shoots) {
					yield return shoot;
				}
			}
			yield break;
		}

	}
}
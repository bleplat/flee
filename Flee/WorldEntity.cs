using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Flee {
	public abstract class WorldEntity {
		// world
		public World world = null;
		// location & kinetics
		public PointF location = new PointF(5000f, 5000f);
		public PointF speed_vec = new PointF();
		public float direction = 0f;
		public float speed = 0f;
		// advanced location
		public Point sector_coords = new Point(-1, -1);
		public WorldSector sector = null;
		public Point ComputeCurrentSectorCoords() {
			Point current_sector = new Point((int)location.X * World.sectors_count_x / world.ArenaSize.Width, (int)location.Y * World.sectors_count_y / world.ArenaSize.Height);
			current_sector.X = Math.Max(0, Math.Min(World.sectors_count_x - 1, current_sector.X));
			current_sector.Y = Math.Max(0, Math.Min(World.sectors_count_y - 1, current_sector.Y));
			return (current_sector);
		}
		private void ExampleUpdateSector() {
			Point new_sector_coords = ComputeCurrentSectorCoords();
			if (new_sector_coords == sector_coords)
				return;
			//sector.ships.Remove(this); // < Remove ship/shoot from ships/shoots here >
			sector_coords = new_sector_coords;
			sector = world.sectors[sector_coords.X, sector_coords.Y];
			//sector.ships.Add(this); // < Add ship/shoot to ships/shoots here >
		}
		public abstract void UpdateSector();
		// ctor
		public WorldEntity(ref World world) {
			this.world = world;
		}
	}
}

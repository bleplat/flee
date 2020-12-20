using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Flee {
	public abstract class WorldEntity {

		/* Container & coords */
		public World world = null;
		public PointF location = new PointF(5000f, 5000f);
		public float direction = 0f;

		/* Sectors */
		public WorldSector sector = null;
		public Point ComputeCurrentSectorCoords() {
			Point current_sector = new Point((int)location.X * World.sectors_count_x / world.ArenaSize.Width, (int)location.Y * World.sectors_count_y / world.ArenaSize.Height);
			current_sector.X = Math.Max(0, Math.Min(World.sectors_count_x - 1, current_sector.X));
			current_sector.Y = Math.Max(0, Math.Min(World.sectors_count_y - 1, current_sector.Y));
			return (current_sector);
		}
		// world should implement an UpdateSector(WorldEntity entity) function.

		/* Movement */
		public PointF speed_vec = new PointF();
		public Point sector_coords = new Point(-1, -1);
		public void TickMove() {
			location.X += speed_vec.X;
			location.Y += speed_vec.Y;
		}

		/* Constructor */
		public WorldEntity(ref World world) {
			this.world = world;
		}

	}
}

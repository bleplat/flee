using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Flee {
	public class WorldEntity {
		// world
		public World world = null;
		// location & kinetics
		public PointF location = new PointF(5000f, 5000f);
		public PointF speed_vec = new PointF();
		public float direction = 0f;
		public float speed = 0f;
		// advanced location
		public Point sector_coords = new Point(-1, -1);
		public void UpdateSector() {
			// TODO: NOW:		
		}
		// ctor
		public WorldEntity(ref World world) {
			this.world = world;
		}
	}
}

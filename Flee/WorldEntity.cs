using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Flee {
	public abstract class WorldEntity {

		public enum Flags : int {
			NoColisions,	// the object wont colide with anything
			Derelict,		// Object's speed isnt affected by arbitrary void friction
		}
		public static int StringToFlags(string str_flags) {
			int flags = 0;
			foreach (Flags flag in Enum.GetValues(typeof(Flags))) {
				if (((int)flag) != 0) 
					; // TODO:
			}
			return (flags);
		}
		public static string FlagsToString(int flags) {
			string str_flags = "";
			foreach (Flags flag in Enum.GetValues(typeof(Flags))) {
				if ((flags & (int)flag) != 0) {
					if (str_flags != "")
						str_flags += "|";
					str_flags += flag.ToString();
				}
			}
			return (str_flags);
		}

		/* Container & coords */
		public World world = null;
		public PointF location = new PointF(5000f, 5000f);
		public float direction = 0f;

		/* Sectors */
		public WorldSector sector = null;
		public Point sector_coords = new Point(-1, -1);
		public Point ComputeCurrentSectorCoords() {
			Point current_sector = new Point((int)location.X * World.sectors_count_x / world.ArenaSize.Width, (int)location.Y * World.sectors_count_y / world.ArenaSize.Height);
			current_sector.X = Math.Max(0, Math.Min(World.sectors_count_x - 1, current_sector.X));
			current_sector.Y = Math.Max(0, Math.Min(World.sectors_count_y - 1, current_sector.Y));
			return (current_sector);
		}
		// world should implement an UpdateSector(WorldEntity entity) function.

		/* Movement */
		public PointF speed_vec = new PointF();
		public void TickMove() {
			location.X += speed_vec.X;
			location.Y += speed_vec.Y;
		}

		/* Misc */
		public int lifespan = Int32.MaxValue;
		public float width = 0;
		public float mass = 0;
		public int flags = 0;
		public void TickLifespan() {
			if (this.lifespan < Int32.MaxValue)
				this.lifespan -= 1;
		}

		/* Constructor */
		public WorldEntity(ref World world) {
			this.world = world;
		}
		public void LoadProperties(Dictionary<string, string> properties) {
			if (properties.ContainsKey("size")) {
				width = Helpers.ToFloat(properties["size"]);
				mass = Helpers.ToFloat(properties["size"]);
			}
			if (properties.ContainsKey("mass")) {
				mass = Helpers.ToFloat(properties["mass"]);
			}
			if (properties.ContainsKey("lifespan")) {
				lifespan = Convert.ToInt32(properties["lifespan"]);
			}
			if (properties.ContainsKey("entity_flags")) {
				flags = StringToFlags(properties["entity_flags"]);
			}
		}

	}
}

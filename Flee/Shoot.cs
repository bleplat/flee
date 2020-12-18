using System;
using System.Drawing;

namespace Flee {
	public class Shoot : WorldEntity {

		/**
		 * @brief Special effect for a projectile.
		 */
		public enum EmissiveMode {
			Trace = 0x1, // effect appears where the projectile was
			Fast = 0x2, // faster emission
			Back = 0x10, // effect is shot backward
			Front = 0x20, // effect is shot forward
			Side = 0x40, // effect is shot sideward
			Propeled = 0x100, // effect is spreaded backward
			Emissive = 0x200, // effect is spreaded arround
		}
		public static int ShootEmissiveMode(string roles_str) {
			int total = 0;
			foreach (string role_str in roles_str.Split('|')) {
				total |= (int)Enum.Parse(typeof(EmissiveMode), role_str, true);
			}
			return (total);
		}
		public static string ShootEmissiveMode(int roles) {
			string total = "";
			foreach (EmissiveMode role in Enum.GetValues(typeof(EmissiveMode))) {
				if ((roles & (int)role) != 0) {
					if (total.Length > 0)
						total += '|';
					total += role.ToString();
				}
			}
			return (total);
		}

		// Primaire
		public string type = "Default";
		public int time_to_live = 8;
		public int fram = 0;
		public Team Team = null;
		public SpriteArray sprites = null;
		public int sprite_y = 0;
		public int emissive_mode = 0;
		public string emissive_sprite = null;

		// Secondaire
		public double power = 10;
		public int special = 0;

		public Shoot(ref World world, ref Team team, int time_to_live, double power, int special, string type, PointF location, float direction, double speed, int sprite_y = 0, int emissive_mode = 0, string emissive_sprite = null) : base(ref world) {
			this.time_to_live = time_to_live;
			this.Team = team;
			this.power = power;
			this.special = special;
			this.type = type;
			this.location = location;
			this.direction = direction;
			this.speed_vec = Helpers.GetNewPoint(new Point(0, 0), direction, (float)speed);
			this.sprite_y = sprite_y;
			this.emissive_mode = emissive_mode;
			this.emissive_sprite = emissive_sprite;
			// UpdateSector(); // useless in practice
			this.sprites = SpriteArray.GetSpriteArray(this.type, this.Team.color);
			if (sprite_y == -1)
				this.sprite_y = Helpers.rand.Next(0, sprites.count_y);
		}
		public Shoot(ref World world, ref Team team, int time_to_live, double power, int special, string type, PointF location, float direction, PointF speed_vec, int sprite_y = 0, int emissive_mode = 0, string emissive_sprite = null) : base(ref world) {
			this.time_to_live = time_to_live;
			this.Team = team;
			this.power = power;
			this.special = special;
			this.type = type;
			this.location = location;
			this.direction = direction;
			this.speed_vec = speed_vec;
			this.sprite_y = sprite_y;
			this.emissive_mode = emissive_mode;
			this.emissive_sprite = emissive_sprite;
			// UpdateSector(); // useless in practice
			this.sprites = SpriteArray.GetSpriteArray(this.type, this.Team.color);
			if (sprite_y == -1)
				this.sprite_y = Helpers.rand.Next(0, sprites.count_y);
		}

		public void Check() {
			// ===' Fram '==='
			fram = (fram + 1) % sprites.count_x;
			location.X = location.X + speed_vec.X;
			location.Y = location.Y + speed_vec.Y;
			time_to_live = time_to_live - 1;
			float speed = (float)Math.Sqrt(this.speed_vec.X * this.speed_vec.X + this.speed_vec.Y * this.speed_vec.Y);
			if ((emissive_mode & (int)EmissiveMode.Fast) != 0) 
				speed *= 1.75f;
			if ((emissive_mode & (int)EmissiveMode.Trace) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, world.gameplay_random.Next(0, 360), 0));
			}
			if ((emissive_mode & (int)EmissiveMode.Front) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, direction, speed * 1.25f));
			}
			if ((emissive_mode & (int)EmissiveMode.Back) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, direction, speed * 0.75f));
			}
			if ((emissive_mode & (int)EmissiveMode.Side) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, direction + 90, speed * 0.25f));
				world.effects.Add(new Effect(-1, emissive_sprite, location, direction - 90, speed * 0.25f));
			}
			if ((emissive_mode & (int)EmissiveMode.Propeled) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, direction + 180 + world.gameplay_random.Next(-35, 36), speed * 0.5f));
			}
			if ((emissive_mode & (int)EmissiveMode.Emissive) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, world.gameplay_random.Next(0, 360), speed * 0.25f));
			}
		}

		public override void UpdateSector() {
			Point new_sector_coords = ComputeCurrentSectorCoords();
			if (new_sector_coords == sector_coords)
				return;
			if (sector is object)
				sector.shoots.Remove(this); // < Remove ship/shoot from ships/shoots here >
			sector_coords = new_sector_coords;
			sector = world.sectors[sector_coords.X, sector_coords.Y];
			sector.shoots.Add(this); // < Add ship/shoot to ships/shoots here >
		}

	}
}
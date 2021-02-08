using System;
using System.Drawing;

namespace Flee {
	public class Shoot : WorldEntity {
		public float speed = 0f;
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
		public int sprite_y = -1;
		public int emissive_mode = 0;
		public string emissive_sprite = null;

		// Secondaire
		public float power = 10;
		public float emp_power = 0;
		public int special = 0;

		public Shoot(ref World world, Weapon weapon, PointF location, float direction, float speed = -1) : base(ref world) {
			this.time_to_live = (int)(weapon.stats.range / weapon.stats.celerity);
			this.Team = weapon.ship.team;
			this.power = weapon.stats.power / weapon.stats.sub_ammos * weapon.ship.team.damage_multiplicator;
			this.emp_power = weapon.stats.emp_power / weapon.stats.sub_ammos * weapon.ship.team.damage_multiplicator;
			this.special = weapon.stats.special;
			this.type = weapon.stats.sprite;
			this.location = location;
			this.direction = direction;
			if (speed == -1)
				this.speed_vec = Helpers.GetNewPoint(new Point(0, 0), direction, weapon.stats.celerity);
			else
				this.speed_vec = Helpers.GetNewPoint(new Point(0, 0), direction, speed);
			this.emissive_mode = weapon.stats.emissive_mode;
			this.emissive_sprite = weapon.stats.emissive_sprite;
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
				world.effects.Add(new Effect(-1, emissive_sprite, location, (float)direction, speed * 1.25f));
			}
			if ((emissive_mode & (int)EmissiveMode.Back) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, (float)direction, speed * 0.75f));
			}
			if ((emissive_mode & (int)EmissiveMode.Side) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, (float)direction + 90, speed * 0.25f));
				world.effects.Add(new Effect(-1, emissive_sprite, location, (float)direction - 90, speed * 0.25f));
			}
			if ((emissive_mode & (int)EmissiveMode.Propeled) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, (float)direction + 180.0f + world.gameplay_random.Next(-35, 36), speed * 0.5f));
			}
			if ((emissive_mode & (int)EmissiveMode.Emissive) != 0) {
				world.effects.Add(new Effect(-1, emissive_sprite, location, world.gameplay_random.Next(0, 360), speed * 0.25f));
			}
		}



	}
}
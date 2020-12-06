using System.Drawing;

namespace Flee {
	public class Shoot : WorldEntity {

		// Primaire
		public string type = "Default";
		public int time_to_live = 8;
		public int fram = 0;
		public Team Team = null;
		public SpriteArray sprites = null;
		public int sprite_y = 0;

		// Secondaire
		public float Power = 10;
		public int special = 0;

		public Shoot(ref World world, ref Team team, int time_to_live, float power, int special, string type, PointF location, float direction, float speed, int sprite_y = 0) : base(ref world) {
			this.time_to_live = time_to_live;
			this.Team = team;
			this.Power = power;
			this.special = special;
			this.type = type;
			this.location = location;
			this.direction = direction;
			this.speed_vec = Helpers.GetNewPoint(new Point(0, 0), direction, speed);
			this.sprite_y = sprite_y;
			// UpdateSector(); // useless in practice
			this.sprites = SpriteArray.GetSpriteArray(this.type, (this.Team is object) ? this.Team.color : default);
		}
		public Shoot(ref World world, ref Team team, int time_to_live, float power, int special, string type, PointF location, float direction, PointF speed_vec, int sprite_y = 0) : base(ref world) {
			this.time_to_live = time_to_live;
			this.Team = team;
			this.Power = power;
			this.special = special;
			this.type = type;
			this.location = location;
			this.direction = direction;
			this.speed_vec = speed_vec;
			this.sprite_y = sprite_y;
			// UpdateSector(); // useless in practice
			this.sprites = SpriteArray.GetSpriteArray(this.type, (this.Team is object) ? this.Team.color : default);
		}

		public void Check() {
			// ===' Fram '==='
			fram = (fram + 1) % sprites.count_x;
			location.X = location.X + speed_vec.X;
			location.Y = location.Y + speed_vec.Y;
			time_to_live = time_to_live - 1;
			if ((special & (int)Weapon.SpecialBits.Plasma) != 0)
				world.Effects.Add(new Effect(-1, "Plasma", location, world.gameplay_random.Next(0, 360), 1));
			if ((special & (int)Weapon.SpecialBits.Propeled) != 0)
				world.Shoots.Add(new Shoot(ref world, ref Team, 5, 1, 0, "PRJ_A", location, world.gameplay_random.Next(0, 360), 2)); // TODO: make effect
			if ((special & (int)Weapon.SpecialBits.BioDrops) != 0) {
				world.Shoots.Add(new Shoot(ref world, ref Team, 8, 2, 0, "PRJ_B", location, direction + world.gameplay_random.Next(-90, 90), world.gameplay_random.Next(4, 8))); // TODO: make effect
				world.Shoots.Add(new Shoot(ref world, ref Team, 8, Power / 2, 0, "PRJ_B", location, world.gameplay_random.Next(0, 360), world.gameplay_random.Next(6, 10))); // TODO: make effect
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
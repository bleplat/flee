using System.Drawing;

namespace Flee {
	public class Shoot : WorldEntity {
		public Shoot(ref World world) : base(ref world) {
			// UpdateSector(); // useless in practice
		}

		public void Check() {
			// ===' Fram '==='
			fram = (ushort)(fram + 1);
			if (fram > 7)
				fram = 0;
			// ===' Déplacement '==='
			speed_vec = Helpers.GetNewPoint(new Point(0, 0), direction, speed);
			location.X = location.X + speed_vec.X;
			location.Y = location.Y + speed_vec.Y;
			// ===' Life '==='
			Life = Life - 1;
			// ===' Spécial '==='
			if ((special & (int)Weapon.SpecialBits.Plasma) != 0)
				world.Effects.Add(new Effect() { Type = "Plasma", Coo = location, Direction = world.gameplay_random.Next(0, 360), Life = 6, speed = 1f });

			if ((special & (int)Weapon.SpecialBits.Propeled) != 0)
				world.Shoots.Add(new Shoot(ref world) { Type = "PRJ_A", location = location, direction = world.gameplay_random.Next(0, 360), Life = 5, speed = 2f, Power = 1, Team = Team });

			if ((special & (int)Weapon.SpecialBits.BioDrops) != 0) {
				world.Shoots.Add(new Shoot(ref world) { Type = "PRJ_B", location = location, direction = direction + world.gameplay_random.Next(-90, 90), Life = 8, speed = world.gameplay_random.Next(4, 8), Power = 2, Team = Team });
				world.Shoots.Add(new Shoot(ref world) { Type = "PRJ_B", location = location, direction = world.gameplay_random.Next(0, 360), Life = 8, speed = world.gameplay_random.Next(6, 10), Power = (int)(Power / 2d), Team = Team });
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

		// Primaire
		public ushort fram = 0;
		public string Type = "Default";
		public Team Team = null;
		public int Life = 8;
		public int special = 0;
		// Secondaire
		public int Power = 10;
	}
}
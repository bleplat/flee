using System.Drawing;

namespace Flee {
	public class Shoot {
		public Shoot(ref World world) {
			this.world = world;
		}

		public void Check() {
			// ===' Fram '==='
			fram = (ushort)(fram + 1);
			if (fram > 7)
				fram = 0;
			// ===' Déplacement '==='
			Vec = Helpers.GetNewPoint(new Point(0, 0), Direction, speed);
			Coo.X = Coo.X + Vec.X;
			Coo.Y = Coo.Y + Vec.Y;
			// ===' Life '==='
			Life = Life - 1;
			// ===' Spécial '==='
			if ((special & (int)Weapon.SpecialBits.Plasma) != 0) world.Effects.Add(new Effect() { Type = "Plasma", Coo = Coo, Direction = world.gameplay_random.Next(0, 360), Life = 6, speed = 1f });

			if ((special & (int)Weapon.SpecialBits.Propeled) != 0) world.Shoots.Add(new Shoot(ref world) { Type = "PRJ_A", Coo = Coo, Direction = world.gameplay_random.Next(0, 360), Life = 5, speed = 2f, Power = 1, Team = Team });

			if ((special & (int)Weapon.SpecialBits.BioDrops) != 0) {
				world.Shoots.Add(new Shoot(ref world) { Type = "PRJ_B", Coo = Coo, Direction = Direction + world.gameplay_random.Next(-90, 90), Life = 8, speed = world.gameplay_random.Next(4, 8), Power = 2, Team = Team });
				world.Shoots.Add(new Shoot(ref world) { Type = "PRJ_B", Coo = Coo, Direction = world.gameplay_random.Next(0, 360), Life = 8, speed = world.gameplay_random.Next(6, 10), Power = (int)(Power / 2d), Team = Team });
			}
		}

		public World world = null;
		// Primaire
		public ushort fram = 0;
		public string Type = "Default";
		public Team Team = null;
		public PointF Coo = new PointF();
		public PointF Vec = new PointF();
		public float Direction = 0f;
		public float speed = 0f;
		public int Life = 8;
		public int special = 0;
		// Secondaire
		public int Power = 10;
	}
}
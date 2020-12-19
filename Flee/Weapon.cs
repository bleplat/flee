using System;
using System.Drawing;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class Weapon {
		public Ship ship = null;

		// Special effect flag
		public enum SpecialBits {
			ReloadRNG = 1,		// Reload time become non-constant
			NoAim = 2,			// Shoots doesnt aim
			Explode = 4,
			KeepFire = 8,		// Never stops firing
			SelfExplode = 16,
			SelfNuke = 32,
			SpreadOrigin = 64,
			Rail = 128,
			Flak = 256,
			Launch = 512,
			EMP = 1024,
			Straight = 2048,
		}

		public static int SpecialFromString(string input) {
			int total = 0;
			foreach (string item in input.Split('|')) { // TODO: NOW: replace with '|'
				if (item != "")
					total += (int)(SpecialBits)Enum.Parse(typeof(SpecialBits), item);
			}
			return total;
		}
		public static string SpecialToString() {
			string total = "";
			foreach (SpecialBits special in Enum.GetValues(typeof(SpecialBits))) {
				total += special.ToString() + "|";
			}
			return total;
		}

		// stats
		private GunStats base_stats = null;
		public GunStats stats = null;
		public int Loc = 0;

		// state
		public int Load = 0;
		public int Bar = 0;

		// creation
		public Weapon(Ship ship) {
			this.ship = ship;
		}

		public Weapon(Ship ship, int angle, GunStats gun_class) {
			this.ship = ship;
			Loc = angle;
			base_stats = gun_class;
			ResetStats();
		}

		public Weapon(Ship ship, string input) {
			this.ship = ship;
			FromString(input);
			ResetStats();
		}

		public void ResetStats() {
			stats = base_stats.Clone();
		}

		public void Fire(float QA, Point PTN, Ship Launcher) {
			if (Bar > 0) {
				Bar = Bar - 1;
				PointF spawn_point = PTN; // TODO: simplify
				int time_to_live = (int)(stats.range / (double)stats.celerity);
				double power = this.stats.power;
				power *= this.ship.team.damage_multiplicator;
				int dispersion = stats.sub_ammos;
				if ((base_stats.special & (int)SpecialBits.Straight) != 0)
					QA = ship.direction;
				if ((base_stats.special & (int)SpecialBits.NoAim) != 0)
					QA = ship.world.gameplay_random.Next(0, 360);
				if ((base_stats.special & (int)SpecialBits.SpreadOrigin) != 0)
					spawn_point = new PointF(PTN.X + ship.world.gameplay_random.Next(-7, 8), PTN.Y + ship.world.gameplay_random.Next(-7, 8));
				if ((base_stats.special & (int)SpecialBits.Launch) != 0) {
					ship.world.ships.Add(new Ship(ship.world, ship.team, this.stats.sprite));
					ship.world.ships[ship.world.ships.Count - 1].location = Launcher.location;
					ship.world.ships[ship.world.ships.Count - 1].direction = Launcher.direction;
					ship.world.ships[ship.world.ships.Count - 1].speed = this.stats.celerity;
					ship.world.ships[ship.world.ships.Count - 1].auto = true;
					ship.world.ships[ship.world.ships.Count - 1].target = ship.target;
					ship.world.ships[ship.world.ships.Count - 1].agressivity = 10000.0f;
					ship.world.ships[ship.world.ships.Count - 1].behavior = Ship.BehaviorMode.Folow;
					return;
				}
				if ((base_stats.special & (int)SpecialBits.Rail) != 0) {
					for (int i = 0, loopTo = dispersion; i <= loopTo; i++)
						ship.world.shoots.Add(new Shoot(ref ship.world, this, spawn_point, QA, stats.celerity + i / 1.5f));
				} else if ((base_stats.special & (int)SpecialBits.Flak) != 0) {
					for (double i = -(dispersion / 2d), loopTo1 = dispersion / 2d; i <= loopTo1; i++)
						ship.world.shoots.Add(new Shoot(ref ship.world, this, spawn_point, (float)(QA + i * (360d / dispersion / 16d)), (float)(stats.celerity + (i + dispersion) % 4d / 2.0d)));
				} else if ((base_stats.special & (int)SpecialBits.SelfExplode) != 0 || (base_stats.special & (int)SpecialBits.SelfNuke) != 0) {
					for (double i = -(dispersion / 2d), loopTo2 = dispersion / 2d; i <= loopTo2; i++)
						ship.world.shoots.Add(new Shoot(ref ship.world, this, spawn_point, (float)(QA + i * (360.0f / dispersion))));
					ship.world.shoots.Add(new Shoot(ref ship.world, this, spawn_point, QA, stats.celerity));
				} else
					ship.world.shoots.Add(new Shoot(ref ship.world, this, spawn_point, QA));
			}
		}

		// calculat point to aim to reach a moving target
		public PointF ForseeShootingLocation(Ship target_ship) {
			// TODO: Improve by taking exact weapon location into account
			double dist_1 = Helpers.Distance(ref ship.location, ref target_ship.location);
			double time_1 = dist_1 / stats.celerity * 0.9d;
			var target_ptn_1 = new PointF((float)(target_ship.location.X + target_ship.speed_vec.X * time_1), (float)(target_ship.location.Y + target_ship.speed_vec.Y * time_1));
			double dist_2 = Helpers.Distance(ref ship.location, ref target_ptn_1);
			double time_2 = dist_2 / stats.celerity * 0.9d;
			var target_ptn_2 = new PointF((float)(target_ship.location.X + target_ship.speed_vec.X * time_2), (float)(target_ship.location.Y + target_ship.speed_vec.Y * time_2));
			return new PointF((target_ptn_1.X + target_ptn_2.X) / 2f, (target_ptn_1.Y + target_ptn_2.Y) / 2f);
		}

		// Import/Export
		public override string ToString() {
			return Loc.ToString() + ";" + stats.name;
		}

		public void FromString(string input) {
			var parts = input.Split(';');
			Loc = Convert.ToInt32(parts[0]);
			base_stats = GunStats.classes[parts[1]];
			ResetStats();
		}
	}
}
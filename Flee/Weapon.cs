using System;
using System.Drawing;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class Weapon {
		public Ship ship = null;

		// Special effect flag
		public enum SpecialBits {
			Plasma = 1,
			Propeled = 2,
			Explode = 4,
			BioDrops = 8,
			SelfExplode = 16,
			SelfNuke = 32,
			SpreadOrigin = 64,
			Rail = 128,
			Flak = 256
		}

		public static int SpecialFromString(string input) {
			int total = 0;
			var elements = input.Split(';');
			foreach (string element in elements) {
				if (element.Length == 0)
					continue;
				switch (element ?? "") {
					case "Plasma": {
							total = total | (int)SpecialBits.Plasma;
							break;
						}

					case "Propeled": {
							total = total | (int)SpecialBits.Propeled;
							break;
						}

					case "Explode": {
							total = total | (int)SpecialBits.Explode;
							break;
						}

					case "BioDrops": {
							total = total | (int)SpecialBits.BioDrops;
							break;
						}

					case "SelfExplode": {
							total = total | (int)SpecialBits.SelfExplode;
							break;
						}

					case "SelfNuke": {
							total = total | (int)SpecialBits.SelfNuke;
							break;
						}

					case "SpreadOrigin": {
							total = total | (int)SpecialBits.SpreadOrigin;
							break;
						}

					case "Rail": {
							total = total | (int)SpecialBits.Rail;
							break;
						}

					case "Flak": {
							total = total | (int)SpecialBits.Flak;
							break;
						}

					default: {
							throw new Exception("Special doesnt exists: " + element);
							break;
						}
				}
			}

			return total;
		}

		public static string SpecialToString() {
			string total = "";
			if ((Conversions.ToLong(total) & (long)SpecialBits.Plasma) != 0L)
				total += "Plasma;";
			if ((Conversions.ToLong(total) & (long)SpecialBits.Propeled) != 0L)
				total += "Propeled;";
			if ((Conversions.ToLong(total) & (long)SpecialBits.Explode) != 0L)
				total += "Explode;";
			if ((Conversions.ToLong(total) & (long)SpecialBits.BioDrops) != 0L)
				total += "BioDrops;";
			if ((Conversions.ToLong(total) & (long)SpecialBits.SelfExplode) != 0L)
				total += "SelfExplode;";
			if ((Conversions.ToLong(total) & (long)SpecialBits.SelfNuke) != 0L)
				total += "SelfNuke;";
			if ((Conversions.ToLong(total) & (long)SpecialBits.SpreadOrigin) != 0L)
				total += "SpreadOrigin;";
			if ((Conversions.ToLong(total) & (long)SpecialBits.Rail) != 0L)
				total += "Rail;";
			if ((Conversions.ToLong(total) & (long)SpecialBits.Flak) != 0L)
				total += "Flak;";
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
				PointF spawn_point = PTN;
				if ((base_stats.special & (int)SpecialBits.SpreadOrigin) != 0) spawn_point = new PointF(PTN.X + ship.world.gameplay_random.Next(-7, 8), PTN.Y + ship.world.gameplay_random.Next(-7, 8));

				if ((base_stats.special & (int)SpecialBits.Rail) != 0) {
					int dispersion = 16;
					for (int i = 0, loopTo = dispersion; i <= loopTo; i++)
						ship.world.Shoots.Add(new Shoot(ref ship.world) { Coo = spawn_point, Type = base_stats.sprite, Direction = QA, speed = (float)(stats.celerity + i / 2d), Life = (int)(stats.range / (double)stats.celerity), Power = (int)(stats.power / (double)dispersion), Team = Launcher.team, special = base_stats.special });
				} else if ((base_stats.special & (int)SpecialBits.Flak) != 0) {
					int dispersion = 16;
					for (double i = -(dispersion / 2d), loopTo1 = dispersion / 2d; i <= loopTo1; i++)
						ship.world.Shoots.Add(new Shoot(ref ship.world) { Coo = spawn_point, Type = base_stats.sprite, Direction = (float)(QA + i * (360d / dispersion / 16d)), speed = (float)(stats.celerity + (i + dispersion) % 4d / 2.0d), Life = (int)(stats.range / (double)stats.celerity + (i + dispersion) % 3d), Power = (int)(stats.power / (double)dispersion), Team = Launcher.team, special = base_stats.special });
				} else if ((base_stats.special & (int)SpecialBits.SelfExplode) != 0 || (base_stats.special & (int)SpecialBits.SelfNuke) != 0) {
					int dispersion = 16;
					for (double i = -(dispersion / 2d), loopTo2 = dispersion / 2d; i <= loopTo2; i++)
						ship.world.Shoots.Add(new Shoot(ref ship.world) { Coo = spawn_point, Type = base_stats.sprite, Direction = (float)(QA + i * (360d / dispersion)), speed = stats.celerity, Life = (int)(stats.range / (double)stats.celerity), Power = (int)(stats.power / 4d / dispersion), Team = Launcher.team, special = base_stats.special });
					ship.world.Shoots.Add(new Shoot(ref ship.world) { Coo = spawn_point, Type = base_stats.sprite, Direction = QA, speed = stats.celerity, Life = (int)(stats.range / (double)stats.celerity), Power = stats.power, Team = Launcher.team, special = base_stats.special });
				} else ship.world.Shoots.Add(new Shoot(ref ship.world) { Coo = spawn_point, Type = base_stats.sprite, Direction = QA, speed = stats.celerity, Life = (int)(stats.range / (double)stats.celerity), Power = stats.power, Team = Launcher.team, special = base_stats.special });
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
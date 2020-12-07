using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public sealed class Helpers {
		public static readonly ulong INVALID_UID = (ulong)(ulong.MaxValue - 1m);
		private static ulong last_uid = 0UL;

		public static ulong GetNextUniqueID() {
			last_uid = (ulong)(last_uid + 1m);
			return last_uid;
		}

		public static double GetQA(int x1, int y1, int x2, int y2) {
			// Dim calc As Double = ((y2 - y1) / (x2 - x1))
			// Dim QA As Double = Math.Atan(calc) * 360
			double QA = Math.Atan2(x1 - x2, y1 - y2);
			QA = QA * (180d / Math.PI);
			QA = QA - 180d;
			if (QA < 0d)
				QA = QA + 360d;
			if (QA >= 360d)
				QA = QA - 360d;
			return QA;
		}

		public static double Distance(ref PointF p1, ref PointF p2) {
			return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}

		public static double Distance(float x1, float y1, float x2, float y2) {
			return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
		}

		public static double DistanceSQ(ref PointF p1, ref PointF p2) {
			return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
		}

		public static double DistanceSQ(float x1, float y1, float x2, float y2) {
			return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
		}

		public static PointF GetNewPoint(PointF AncPoint, float Dir, float speed) {
			float Tox = (float)(Math.Sin(2d * Math.PI * Dir / 360d) * speed + AncPoint.X);
			float Toy = (float)(Math.Cos(2d * Math.PI * Dir / 360d) * speed + AncPoint.Y);
			AncPoint.X = Tox;
			AncPoint.Y = Toy;
			return AncPoint;
		}

		public static double Modulo(double a, double n) {
			return (a % n + n) % n;
		}

		public static double GetAngleDiff(double a1, double a2) {
			return Math.Abs(Modulo(a2 - a1 + 180d, 360d) - 180d);
		}

		private static Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();

		public static Bitmap GetSprite(string img_name, int x, int y, Color Scolor = default) {
			Bitmap bmp = null;
			string full_img_name;
			if (Scolor == default)
				full_img_name = img_name + x + y;
			else
				full_img_name = img_name + x + y + Scolor.ToString();
			// Find already loaded image
			if (bitmaps.TryGetValue(full_img_name, out bmp))
				return bmp;
			// Image is not in cache, get from file
			// Non sprited image
			if (x == -1 && y == -1)             // Base image requested
				try {
					string file_name = "./sprites/" + img_name + ".bmp";
					bmp = new Bitmap(Image.FromFile(file_name)); // My.Resources.ResourceManager.GetObject(img_name, My.Resources.Culture)
				} catch (Exception ex1) {
					try {
						string file_name = "./sprites/" + img_name + ".png";
						bmp = new Bitmap(Image.FromFile(file_name)); // My.Resources.ResourceManager.GetObject(img_name, My.Resources.Culture)
					} catch (Exception ex2) {
						return null;
					}
				}
			else {
				// Get base image
				bmp = GetSprite(img_name, -1, -1, default);
				// New colored image from non-colored image
				int ItW = (int)(bmp.Width / 8d);
				bmp = bmp.Clone(new Rectangle(new Point(x * ItW + 1, y * ItW + 1), new Size(ItW - 2, ItW - 2)), System.Drawing.Imaging.PixelFormat.DontCare);
				// Coloring
				if (Scolor != default) 
					Recolor(bmp, Scolor);
			}
			if (bmp is null)
				return null;
			bmp.MakeTransparent(Color.Black);
			bitmaps.Add(full_img_name, bmp);
			return bmp;
		}
		public static void Recolor(Bitmap bmp, Color color) {
			for (int i = 0, loopTo = bmp.Width - 1; i <= loopTo; i++)
				for (int j = 0, loopTo1 = bmp.Height - 1; j <= loopTo1; j++) {
					Color pixel = bmp.GetPixel(i, j);
					if (pixel.R == pixel.G && pixel.G == pixel.B && pixel.R != 0)
						bmp.SetPixel(i, j, Color.FromArgb(pixel.R * color.R / 256, pixel.G * color.G / 256, pixel.B * color.B / 256));
				}
			bmp.MakeTransparent(Color.Black);
		}
		public static void SwapcolorOldConcept(Bitmap bmp, Color color) {
			Color c_0 = (color.R >= color.G && color.R >= color.B) ? Color.FromArgb(color.R, 0, 0) : ((color.G >= color.B) ? Color.FromArgb(0, color.G, 0) : Color.FromArgb(0, 0, color.B));
			Color c_2 = (color.B <= color.R && color.B <= color.G) ? Color.FromArgb(0, 0, color.B) : ((color.G <= color.R) ? Color.FromArgb(0, color.G, 0) : Color.FromArgb(color.R, 0, 0));
			Color c_1 = (c_0.R == 0 && c_2.R == 0) ? Color.FromArgb(color.R, 0, 0) : ((c_0.G == 0 && c_2.G == 0) ? Color.FromArgb(0, color.G, 0) : Color.FromArgb(0, 0, color.B));
			for (int i = 0, loopTo = bmp.Width - 1; i <= loopTo; i++)
				for (int j = 0, loopTo1 = bmp.Height - 1; j <= loopTo1; j++) {
					Color pixel = bmp.GetPixel(i, j);
					bmp.SetPixel(i, j, Color.FromArgb(
						(pixel.R * c_0.R + pixel.G * c_1.R + pixel.B * c_2.R) / 256,
						(pixel.R * c_0.G + pixel.G * c_1.G + pixel.B * c_2.G) / 256,
						(pixel.R * c_0.B + pixel.G * c_1.B + pixel.B * c_2.B) / 256
					));
				}
			bmp.MakeTransparent(Color.Black);
		}
		public static void Swapcolor(Bitmap bmp, Color color) {
			Color color2 = Color.White;
			for (int i = 0, loopTo = bmp.Width - 1; i <= loopTo; i++)
				for (int j = 0, loopTo1 = bmp.Height - 1; j <= loopTo1; j++) {
					Color pixel = bmp.GetPixel(i, j);
					bmp.SetPixel(i, j, Color.FromArgb(
						(pixel.R * color.R + pixel.G * 0) / 256,
						(pixel.R * color.G + pixel.G * 0) / 256,
						(pixel.R * color.B + pixel.G * 0) / 256
					));
				}
			bmp.MakeTransparent(Color.Black);
		}

		public static Rectangle GetRect(ref Point PT1, ref Point PT2) {
			var NR = new Rectangle();
			if (PT1.X < PT2.X) {
				NR.X = PT1.X;
				NR.Width = PT2.X - PT1.X;
			} else {
				NR.X = PT2.X;
				NR.Width = PT1.X - PT2.X;
			}

			if (PT1.Y < PT2.Y) {
				NR.Y = PT1.Y;
				NR.Height = PT2.Y - PT1.Y;
			} else {
				NR.Y = PT2.Y;
				NR.Height = PT1.Y - PT2.Y;
			}

			return NR;
		}

		public static Color ImproveColor(Color color) {
			int R = color.R;
			int G = color.G;
			int B = color.B;
			if (R <= G && R <= B) {
				R -= 48;
				G += 48;
				B += 48;
			}

			if (G <= R && G <= B) {
				R += 48;
				G -= 48;
				B += 48;
			}

			if (B <= R && B <= G) {
				R += 48;
				G += 48;
				B -= 48;
			}

			R = Math.Min(255, Math.Max(0, R));
			G = Math.Min(255, Math.Max(0, G));
			B = Math.Min(255, Math.Max(0, B));
			return Color.FromArgb(R, G, B);
		}

		public static double NormalizeAngleUnsigned(double angle) {
			while (angle < 0d)
				angle += 360d;
			while (angle >= 360d)
				angle -= 360d;
			return angle;
		}

		public static int GetAngle(double x1, double y1, double x2, double y2) {
			if (x1 == x2 && y1 == y2)
				return -1;
			double QA = Math.Atan2(x2 - x1, y2 - y1) * 180.0d / Math.PI;
			QA = NormalizeAngleUnsigned(QA);
			return (int)QA;
		}

		public static Point PointFromString(string input) {
			var components = new string[3];
			components = input.Split(';');
			return new Point(Convert.ToInt32(components[0]), Convert.ToInt32(components[1]));
		}

		public static int GetIndentation(string line) {
			int count = 0;
			foreach (char c in line)
				if (c == '\t' || c == ' ')
					count += 1;
				else
					break;

			return count;
		}

		public static void LoadLists() {
			var list_classes = new List<ListClass>();
			list_classes.AddRange(GetListsFromFile("./lists/weapons.txt"));
			list_classes.AddRange(GetListsFromFile("./lists/derelict_ships.txt"));
			list_classes.AddRange(GetListsFromFile("./lists/static_ships.txt"));
			list_classes.AddRange(GetListsFromFile("./lists/human_ships.txt"));
			list_classes.AddRange(GetListsFromFile("./lists/alien_ships.txt"));
			list_classes.AddRange(GetListsFromFile("./lists/boss_ships.txt"));
			LoadLists(list_classes);
		}

		public static void LoadLists(List<ListClass> list_classes) {
			foreach (ListClass a_class in list_classes)
				switch (a_class.type ?? "") {
				case "gun": {
					if (!GunStats.classes.ContainsKey(a_class.name))
						GunStats.classes[a_class.name] = new GunStats(a_class.name);

					foreach (ListProperty prop in a_class.properties)
						GunStats.classes[a_class.name].SetProperty(prop.name, prop.value);
					break;
				}

				case "ship": {
					if (!ShipStats.classes.ContainsKey(a_class.name))
						ShipStats.classes[a_class.name] = new ShipStats(a_class.name);

					foreach (ListProperty prop in a_class.properties)
						ShipStats.classes[a_class.name].SetProperty(prop.name, prop.value);
					break;
				}

				default: {
					throw new Exception("Unknown class type: " + a_class.type);
					break;
				}
				}
		}

		public static List<ListClass> GetListsFromFile(string filename) {
			return GetListsFromRaw(File.ReadAllText(filename).Replace(Constants.vbCr + Constants.vbLf, Constants.vbLf));
		}

		public static List<ListClass> GetListsFromRaw(string data) {
			// use as LoadList(File.ReadAllText("./lists/weapons.txt").Replace(vbCr & vbLf, vbLf))
			data += Constants.vbLf;
			var list_classes = new List<ListClass>();
			var lines = data.Split('\n');
			string header = "";
			var paragraph = new List<string>();
			foreach (string line in lines) { // reading lines one by one
				if (header.Length > 0)
					if (line.Length > 0 && (line[0] == '\t' || line[0] == ' '))
						paragraph.Add(line.Substring(1, line.Length - 1));
					else {
						list_classes.Add(new ListClass(header, paragraph));
						header = "";
						paragraph.Clear();
					}

				if (header.Length == 0)
					if (line.Length == 0 || line[0] == '#')
						continue;
					else if (line[0] == ' ' || line[0] == '\t')
						throw new Exception("Malformed line: " + line);
					else
						header = line;
			}

			return list_classes;
		}

		// locale indpendent double conversions
		static readonly NumberFormatInfo to_double_format = new NumberFormatInfo() { NegativeSign = "-", NumberDecimalSeparator = "." };
		public static double ToDouble(string s) {
			return double.Parse(s.Replace(",", "."), to_double_format);
		}

		static readonly CultureInfo to_double_culture = CultureInfo.CreateSpecificCulture("en-US");
		public static string ToString(double d) {
			return d.ToString("0.00", to_double_culture);
		}

		public static string RandomStationName(Random rand) {
			var station_names = new List<string>();
			foreach (string ship_class_name in ShipStats.classes.Keys)
				if (ship_class_name.Contains("Station") && !ship_class_name.Contains("Player") && !(ship_class_name == "Station"))
					station_names.Add(ship_class_name);

			return station_names[rand.Next(0, station_names.Count)];
		}

		public static string RandomTurretName(Random rand) {
			switch (rand.Next(0, 3)) {
			case 0: {
				return "Anti-Light_Turret";
			}

			case 1: {
				return "Anti-Heavy_Turret";
			}

			case 2: {
				return "Pointvortex_Turret";
			}

			default: {
				return "";
			}
			}
		}

		public static List<string> GetSpawnUpgrades(Ship ship) {
			var upgrades = new List<string>();
			foreach (string craft in ship.stats.crafts)
				foreach (Upgrade a_up in Upgrade.upgrades)
					if ((a_up.name ?? "") == ("Build_" + craft ?? "") || (a_up.name ?? "") == ("Launch_" + craft ?? ""))
						upgrades.Add(a_up.name);

			return upgrades;
		}

		public static string GetRandomSpawnUpgrade(Random rand, Ship ship) {
			var upgrades = GetSpawnUpgrades(ship);
			if (upgrades.Count == 0)
				return null;

			return upgrades[rand.Next(0, upgrades.Count)];
		}
	}
}
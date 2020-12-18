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
		public static Random rand = new Random();

		/* Math */
		public static double GetQA(int x1, int y1, int x2, int y2) {
			double QA = Math.Atan2(x1 - x2, y1 - y2);
			QA = QA * (180d / Math.PI);
			QA = QA - 180d;
			if (QA < 0d)
				QA = QA + 360d;
			if (QA >= 360d)
				QA = QA - 360d;
			return QA;
		}
		public static double Distance(ref PointF p1) {
			return Math.Sqrt((p1.X - 0) * (p1.X - 0) + (p1.Y - 0) * (p1.Y - 0));
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
		public static Rectangle MakeRectangle(ref Point PT1, ref Point PT2) {
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

		/* Graphics */
		public static System.Drawing.Imaging.PixelFormat _screen_pixel_format = System.Drawing.Imaging.PixelFormat.Undefined;
		public static System.Drawing.Imaging.PixelFormat GetScreenPixelFormat() {
			if (_screen_pixel_format == System.Drawing.Imaging.PixelFormat.Undefined) {
				Graphics g = Graphics.FromHwnd(IntPtr.Zero);
				Bitmap bitmap = new Bitmap(4, 4, g);
				_screen_pixel_format = bitmap.PixelFormat;
			}
			return (_screen_pixel_format);
		}
		private static Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
		public static Bitmap GetSprite(string img_name, int x, int y, Color Scolor = default) {
			SpriteArray sa = SpriteArray.GetSpriteArray(img_name, Scolor);
			return (sa.GetSprite(x, y));
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

		/* Conversions */
		static readonly NumberFormatInfo to_double_format = new NumberFormatInfo() { NegativeSign = "-", NumberDecimalSeparator = "." };
		public static double ToDouble(string s) {
			return double.Parse(s.Replace(",", "."), to_double_format);
		}
		static readonly CultureInfo to_double_culture = CultureInfo.CreateSpecificCulture("en-US");
		public static string ToString(double d) {
			return d.ToString("0.00", to_double_culture);
		}
		public static Color ToColor(string str) {
			switch (str) {
			case "UpgradeColor1": return (Color.Green);
			case "UpgradeColor2": return (Color.Yellow);
			case "UpgradeColor3": return (Color.Orange);
			case "UpgradeColor4": return (Color.Red);
			default: break;
			}
			try {
				return (Color.FromName(str));
			} catch {
				if (str[0] == '#')
					return (Color.FromArgb(int.Parse(str.Substring(1), System.Globalization.NumberStyles.HexNumber) | unchecked((int)0xFF000000)));
				else
					throw new Exception("invalid color format");
			}
		}

		/* Misc */
		public static ulong GetNextUniqueID() {
			last_uid = (ulong)(last_uid + 1m);
			return last_uid;
		}

		/* Flee Lists Loading */
		public static Point PointFromString(string input) {
			var components = new string[3];
			components = input.Split(';');
			return new Point(Convert.ToInt32(components[0]), Convert.ToInt32(components[1]));
		}

		/* Flee specific */
		public double RateShip(Ship ship) {
			double rst = 0.0;
			rst += ship.integrity / 100.0;
			rst += ship.shield / 80.0 * ship.stats.shield_opacity;
			rst += ship.stats.deflectors / 2.0;
			rst += ship.stats.speed / 4.0;
			rst += ship.stats.default_weapons.Count / 3.0;
			return (rst);
		}
	}
}
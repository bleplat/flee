using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Flee {
	public class SpriteArray {
		
		// Settings
		int count_x = 0;
		int count_y = 0;
		int count_z = 0;
		int width = 0;
		int height = 0;
		// Storage
		List<Bitmap> sprites = new List<Bitmap>();
		public Bitmap GetSprite(int x, int y = 0, int z = 0) {
			return (sprites[(z * count_x * count_y + y * count_x + x) % sprites.Count]);
		}
		//
		public SpriteArray(string file) {
			this.Load(file);
		}
		public void Load(string file) {
			Bitmap bmp = new Bitmap(file);
			this.Load(bmp);
		}
		public void Load(Bitmap bmp) {
			LoadDimentions(bmp);
			sprites.Clear();
			LoadSprites(bmp);
		}
		public void LoadDimentions(Bitmap bmp) {
			count_x = 0;
			count_y = 0;
			count_z = 0;
			width = 0;
			height = 0;
			if (bmp.Width < 3 || bmp.Height < 3)
				throw new Exception("Image is too small to be a valid sprite grid!");
			// check white corners
			if (bmp.GetPixel(0, 0) != Color.White || bmp.GetPixel(0, 1) != Color.White || bmp.GetPixel(1, 0) != Color.White)
				throw new Exception("Incorrect sprite grid format (top left corner)!");
			if (bmp.GetPixel(bmp.Width - 1, 0) != Color.White || bmp.GetPixel(bmp.Width - 1, 1) != Color.White || bmp.GetPixel(bmp.Width - 2, 0) != Color.White)
				throw new Exception("Incorrect sprite grid format (top right corner)!");
			if (bmp.GetPixel(bmp.Width - 1, bmp.Height - 1) != Color.White || bmp.GetPixel(bmp.Width - 1, bmp.Height - 2) != Color.White || bmp.GetPixel(bmp.Width - 2, bmp.Height - 1) != Color.White)
				throw new Exception("Incorrect sprite grid format (bottop right corner)!");
			if (bmp.GetPixel(0, bmp.Height - 1) != Color.White || bmp.GetPixel(0, bmp.Height - 2) != Color.White || bmp.GetPixel(1, bmp.Height - 1) != Color.White)
				throw new Exception("Incorrect sprite grid format (bottom left corner)!");
			// check corner interior
			if (bmp.GetPixel(1, 1) == Color.White || bmp.GetPixel(bmp.Width - 2, 1) == Color.White || bmp.GetPixel(bmp.Width - 2, bmp.Height - 2) == Color.White || bmp.GetPixel(1, bmp.Height - 2) == Color.White)
				throw new Exception("Sprite grid format used a non-superted in a corner of a frame located in a corner of the grid!");
			// check sprite size / count
			for (int i_x = 2; i_x < bmp.Width / 2; i_x++) {
				if (bmp.GetPixel(i_x, 1) == Color.White) {
					int out_width = i_x + 1;
					if ((bmp.Width % out_width) != 0)
						throw new Exception("Encountered white pixel on an horizontal side of a sprite!");
					count_x = bmp.Width * out_width;
					width = out_width - 2;
				}
			}
			for (int i_y = 2; i_y < bmp.Height / 2; i_y++) {
				if (bmp.GetPixel(i_y, 1) == Color.White) {
					int out_height = i_y + 1;
					if ((bmp.Height % out_height) != 0)
						throw new Exception("Encountered white pixel on a vertical side of a sprite!");
					count_y = bmp.Height * out_height;
					height = out_height - 2;
				}
			}
			count_z = 1;
		}
		private void LoadSprites(Bitmap big_bmp) {
			int out_width = width + 2;
			int out_height = height + 2;
			for (int y = 0; y < count_y; y++) {
				for (int x = 0; x < count_x; y++) {
					Bitmap bmp = big_bmp.Clone(new Rectangle(new Point(x * out_width + 1, y * out_height + 1), new Size(width, height)), System.Drawing.Imaging.PixelFormat.DontCare);
				}
			}
		}
	}
}

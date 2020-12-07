using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Flee {
	public class SpriteArray {
		
		// Array Properties
		public int count_x = 0;
		public int count_y = 0;
		public int count_z = 0;
		public int width = 0;
		public int height = 0;
		public bool color_swap_mode = false;

		// Sprites
		List<Bitmap> sprites = new List<Bitmap>();
		public Bitmap GetSprite(int x, int y, int z = 0) {
			return (sprites[(z * count_x * count_y + y * count_x + x) % sprites.Count]);
		}

		// Construction
		public SpriteArray() {
		}
		public SpriteArray(string file) {
			this.Load(file);
		}
		public SpriteArray(string file, Color color) {
			this.Load(file);
			this.ColorSprites(color);
		}
		public void Load(string file) {
			Bitmap bmp = FromSpritesFolder(file);
			this.Load(bmp);
		}
		public void Load(Bitmap bmp) {
			LoadDimentions(bmp);
			sprites.Clear();
			LoadSprites(bmp);
		}
		static Bitmap FromSpritesFolder(string file) {
			Bitmap bmp;
			try {
				bmp = new Bitmap("./sprites/" + file + ".png");
			} catch {
				bmp = new Bitmap("./sprites/" + file + ".bmp");
			}
			if (bmp.PixelFormat != Helpers.ScreenPixelFormat()) {
				bmp = new Bitmap(bmp).Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), Helpers.ScreenPixelFormat());
			}
			return (bmp);
		}

		// Edition
		public void LoadDimentions(Bitmap bmp) {
			// default values
			count_x = 0;
			count_y = 0;
			count_z = 0;
			width = 0;
			height = 0;
			if (bmp.Width < 3 || bmp.Height < 3)
				throw new Exception("Image is too small to be a valid sprite grid!");
			// grid color
			Color grid_color = bmp.GetPixel(0, 0);
			color_swap_mode = (grid_color.R == 255 && grid_color.G == 0 && grid_color.B == 0);
			// check white corners
			if (bmp.GetPixel(0, 0) != grid_color || bmp.GetPixel(0, 1) != grid_color || bmp.GetPixel(1, 0) != grid_color)
				throw new Exception("Incorrect sprite grid format (top left corner)!");
			if (bmp.GetPixel(bmp.Width - 1, 0) != grid_color || bmp.GetPixel(bmp.Width - 1, 1) != grid_color || bmp.GetPixel(bmp.Width - 2, 0) != grid_color)
				throw new Exception("Incorrect sprite grid format (top right corner)!");
			if (bmp.GetPixel(bmp.Width - 1, bmp.Height - 1) != grid_color || bmp.GetPixel(bmp.Width - 1, bmp.Height - 2) != grid_color || bmp.GetPixel(bmp.Width - 2, bmp.Height - 1) != grid_color)
				throw new Exception("Incorrect sprite grid format (bottop right corner)!");
			if (bmp.GetPixel(0, bmp.Height - 1) != grid_color || bmp.GetPixel(0, bmp.Height - 2) != grid_color || bmp.GetPixel(1, bmp.Height - 1) != grid_color)
				throw new Exception("Incorrect sprite grid format (bottom left corner)!");
			// check corner interior
			if (bmp.GetPixel(1, 1) == grid_color || bmp.GetPixel(bmp.Width - 2, 1) == grid_color || bmp.GetPixel(bmp.Width - 2, bmp.Height - 2) == grid_color || bmp.GetPixel(1, bmp.Height - 2) == grid_color)
				throw new Exception("Sprite grid format used a non-suported color in a corner of a frame located in a corner of the grid!");
			// check sprite size / count
			count_x = 1;
			width = bmp.Width - 2;
			for (int i_x = 2; i_x < bmp.Width / 2; i_x++) {
				if (bmp.GetPixel(i_x, 1) == grid_color) {
					int out_width = i_x + 1;
					if ((bmp.Width % out_width) != 0)
						throw new Exception("Encountered grid pixel on an horizontal side of a sprite!");
					count_x = bmp.Width / out_width;
					width = out_width - 2;
					break;
				}
			}
			count_y = 1;
			height = bmp.Height - 2;
			for (int i_y = 2; i_y < bmp.Height / 2; i_y++) {
				if (bmp.GetPixel(i_y, 1) == grid_color) {
					int out_height = i_y + 1;
					if ((bmp.Height % out_height) != 0)
						throw new Exception("Encountered white pixel on a vertical side of a sprite!");
					count_y = bmp.Height / out_height;
					height = out_height - 2;
					break;
				}
			}
			count_z = 1;
		}
		private void LoadSprites(Bitmap big_bmp) {
			int out_width = width + 2;
			int out_height = height + 2;
			for (int y = 0; y < count_y; y++) {
				for (int x = 0; x < count_x; x++) {
					Bitmap bmp = big_bmp.Clone(new Rectangle(new Point(x * out_width + 1, y * out_height + 1), new Size(width, height)), System.Drawing.Imaging.PixelFormat.DontCare);
					bmp.MakeTransparent(Color.Black);
					sprites.Add(bmp);
				}
			}
		}
		public void ColorSprites(Color color) {
			foreach (Bitmap bmp in sprites) {
				if (color_swap_mode)
					Helpers.Swapcolor(bmp, color);
				else
					Helpers.Recolor(bmp, color);
			}
		}

		// Common loading and cache
		private static Dictionary<string, SpriteArray> sprite_arrays = new Dictionary<string, SpriteArray>();
		private static string SpriteArrayName(string name, Color color = default) {
			return (name + ":" + color.ToString());
		}
		public static SpriteArray GetSpriteArray(string name) {
			SpriteArray sprite_array = null;
			if (sprite_arrays.TryGetValue(name, out sprite_array))
				return (sprite_array);
			sprite_array = new SpriteArray(name);
			sprite_arrays[name] = sprite_array;
			return (sprite_array);
		}
		public static SpriteArray GetSpriteArray(string name, Color color) {
			if (color == default)
				return (GetSpriteArray(name));
			SpriteArray sprite_array = null;
			string sprite_array_name = SpriteArrayName(name, color);
			if (sprite_arrays.TryGetValue(sprite_array_name, out sprite_array))
				return (sprite_array);
			sprite_array = GetSpriteArray(name).Clone();
			sprite_array.ColorSprites(color);
			sprite_arrays[sprite_array_name] = sprite_array;
			return (sprite_array);
		}

		// Cloning
		public SpriteArray Clone() {
			SpriteArray array = new SpriteArray();
			array.count_x = this.count_x;
			array.count_y = this.count_y;
			array.count_z = this.count_z;
			array.width = this.width;
			array.height = this.height;
			array.color_swap_mode = this.color_swap_mode;
			foreach (Bitmap sprite in sprites) {
				array.sprites.Add((Bitmap)sprite.Clone());
			}
			return (array);
		}
	}
}

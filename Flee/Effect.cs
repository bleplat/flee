using System;
using System.Configuration;
using System.Drawing;

namespace Flee {
	public class Effect {
		static Random rand = new Random();

		// Members
		public int time_to_live = 8;
		public string type = "Default";
		public PointF location = new PointF();
		public float direction = 0f;
		public PointF speed_vec = new PointF();
		public int sprite_y = 0;
		public float rotation_speed = 0;

		public int fram = 0;
		public SpriteArray sprites = null;

		// Constructor
		public Effect(int time_to_live, string type, PointF location, float direction = 0.0f, float speed = 0, int sprite_y = -1) {
			this.time_to_live = time_to_live;
			this.type = type;
			this.location = location;
			this.direction = direction;
			this.speed_vec = Helpers.GetNewPoint(new Point(0, 0), direction, speed);
			this.sprite_y = sprite_y;
			SetSpriteArray();
			if (sprite_y == -1)
				this.sprite_y = rand.Next(0, sprites.count_y);
		}
		public Effect(int time_to_live, string type, PointF location, float direction, PointF speed_vec, int sprite_y = -1) {
			this.time_to_live = time_to_live;
			this.type = type;
			this.location = location;
			this.direction = direction;
			this.speed_vec = speed_vec;
			this.sprite_y = sprite_y;
			SetSpriteArray();
			if (sprite_y == -1)
				this.sprite_y = rand.Next(0, sprites.count_y);
		}
		private void SetSpriteArray() {
			this.sprites = SpriteArray.GetSpriteArray(type);
			this.sprite_y %= sprites.count_y;
			if (time_to_live == -1)
				time_to_live = sprites.count_x;
		}

		// Per tick
		public void Check() {
			fram = (fram + 1) % sprites.count_x;
			location.X = location.X + speed_vec.X;
			location.Y = location.Y + speed_vec.Y;
			direction += rotation_speed;
			time_to_live = time_to_live - 1;
		}

	}
}
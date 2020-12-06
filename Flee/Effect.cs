using System.Drawing;

namespace Flee {
	public class Effect {

		// Members
		public int time_to_live = 8;
		public string type = "Default";
		public PointF location = new PointF();
		public float direction = 0f;
		public PointF speed_vec = new PointF();
		public int sprite_y = 0;

		public int fram = 0;
		public SpriteArray sprites = null;

		// Constructor
		public Effect(int time_to_live, string type, PointF location, float direction = 0.0f, float speed = 0, int sprite_y = 0) {
			this.time_to_live = time_to_live;
			this.type = type;
			this.location = location;
			this.direction = direction;
			this.speed_vec = Helpers.GetNewPoint(new Point(0, 0), direction, speed);
			this.sprite_y = sprite_y;
			SetSpriteArray();
		}
		public Effect(int time_to_live, string type, PointF location, float direction, PointF speed_vec, int sprite_y = 0) {
			this.time_to_live = time_to_live;
			this.type = type;
			this.location = location;
			this.direction = direction;
			this.speed_vec = speed_vec;
			this.sprite_y = sprite_y;
			SetSpriteArray();
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
			time_to_live = time_to_live - 1;
		}

	}
}
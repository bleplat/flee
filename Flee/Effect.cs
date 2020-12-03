using System.Drawing;

namespace Flee {
	public class Effect {
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
		}
		// Primaire
		public ushort fram = 0;
		public ushort sprite_y = 0;
		public string Type = "Default";
		public PointF Coo = new PointF();
		public PointF Vec = new PointF();
		public float Direction = 0f;
		public float speed = 0f;
		public int Life = 8;
	}
}
using System.Collections.Generic;
using System.Drawing;

namespace Flee {
	public enum AffinityEnum {
		KIND = 2, // Not hostile to other KIND teams
		MEAN = 4, // Hostile to KIND, but not always so other MEAN
		ALOOF = 8 // Alway hostile to other teams
	}

	public class Team {
		public World world;
		private static int last_id = -1;
		private static List<Color> available_colors = new List<Color>();

		// stats
		public int id;
		public int affinity;
		public Color color;

		// state
		public MaterialSet resources = new MaterialSet();
		public ushort ship_count_limit = 12;
		public ushort upgrade_slots_bonus = 0;
		public int ship_count_approximation = 0;
		public bool cheats_enabled = false;
		public bool has_ascended = false;

		// IA
		public bool bot_team = true;
		public int upgrade_limit = 32;

		public Team(World world, int affinity) {
			this.world = world;

			// id
			id = last_id + 1;
			last_id = id;

			// affinity
			if (affinity == 0)
				if (id == 0)
					this.affinity = (int)AffinityEnum.KIND; // player
				else if (id == 1)
					this.affinity = (int)AffinityEnum.MEAN; // derelict
				else if (id == 2)
					this.affinity = (int)AffinityEnum.ALOOF; // bosses/endgames
				else if (id % 7 == 0)
					this.affinity = (int)AffinityEnum.KIND;
				else
					this.affinity = (int)AffinityEnum.MEAN;
			else
				this.affinity = affinity;

			// max ships
			if (id != 0)
				if (this.affinity == (int)AffinityEnum.KIND)
					ship_count_limit = 24;
				else if (this.affinity == (int)AffinityEnum.MEAN)
					ship_count_limit = 32;
				else
					ship_count_limit = 40;

			// color
			if (available_colors.Count == 0) {
				// allies colors
				// available_colors.Add(Color.FromArgb(0, 160, 0)) ' dark green (confused with player)
				// available_colors.Add(Color.FromArgb(0, 192, 96)) ' blueish green (confused with player)
				available_colors.Add(Color.FromArgb(0, 80, 255)); // deep blue (perfect)
				available_colors.Add(Color.FromArgb(0, 128, 128)); // dark cyan (perfect)
				available_colors.Add(Color.FromArgb(128, 128, 255)); // pale blue (a bit light)
				available_colors.Add(Color.FromArgb(64, 128, 64)); // desaturated green (looks neutral)
				available_colors.Add(Color.FromArgb(128, 255, 128)); // pale green (a bit bright)
				available_colors.Add(Color.FromArgb(173, 136, 26)); // olive (looks yellow)
				available_colors.Add(Color.FromArgb(173, 76, 38)); // brown (too redish, looks hostile)
				available_colors.Add(Color.FromArgb(128, 0, 255)); // dark purple (too pinkish)
																   // available_colors.Add(Color.FromArgb(128, 128, 128)) ' (confused with neutrals)
																   // enemies colors
				available_colors.Add(Color.FromArgb(173, 34, 69)); // crismon (pinkish 5th)
				available_colors.Add(Color.FromArgb(255, 128, 255)); // pink (pinkish 4th)
				available_colors.Add(Color.FromArgb(255, 128, 0)); // orange (orangish 2nd)
																   // available_colors.Add(Color.FromArgb(255, 0, 192)) ' red purple (pinkish 3rd, confusing)
				available_colors.Add(Color.FromArgb(255, 0, 128)); // red pink (pinkish 2nd)
				available_colors.Add(Color.FromArgb(255, 64, 0)); // orange-red (orangish 1st)
				available_colors.Add(Color.FromArgb(255, 0, 255)); // primary magenta (pinkish 1st)
				available_colors.Add(Color.FromArgb(255, 255, 0)); // primary yellow
				available_colors.Add(Color.FromArgb(255, 48, 48)); // coral
			}

			if (this.affinity == (int)AffinityEnum.ALOOF)
				color = Color.FromArgb(255, 0, 0); // primary red
			else if (id == 0)
				color = Color.FromArgb(0, 255, 0); // primary green
			else {
				int i_color;
				if ((this.affinity & (int)AffinityEnum.KIND) != 0)
					i_color = world.gameplay_random.Next(0, (int)(available_colors.Count / 4d));
				else
					i_color = world.gameplay_random.Next((int)(available_colors.Count / 4d * 3d), available_colors.Count);

				color = available_colors[i_color];
				available_colors.RemoveAt(i_color);
			}
		}

		public bool IsFriendWith(Team other) {
			if (ReferenceEquals(this, other))
				return true;

			if (other is null)
				return false;

			if ((affinity & (int)AffinityEnum.KIND) != 0 && (other.affinity & (int)AffinityEnum.KIND) != 0)
				return true;

			if ((affinity & (int)AffinityEnum.MEAN) != 0 && (other.affinity & (int)AffinityEnum.MEAN) != 0)
				if (id % 6 == other.id % 6)
					return true;

			return false;
		}
	}
}
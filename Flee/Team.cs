using System;
using System.Collections.Generic;
using System.Drawing;

namespace Flee {

	/**
	 * @brief Represent the default behavior of a team.
	 */
	public enum AffinityEnum {
		Wilderness = 1, // Relationships are not relevant
		Neutral = 2, // Friendly to Frienly and Dissident teams, but not Hostile
		Friendly = 4, // Friendly to other Friendly teams
		Dissident = 8, // Hostile to other teams
		Hostile = 16 // Hostile to other teams
	}

	public class Engagement {
		public Point location;
		public int timeout;
	}

	public class Team {
		public World world;

		/* Identity */
		public AffinityEnum affinity;
		public bool bot_team = true;

		/* Generation */
		public Color color = default;
		public int station_type_index = 0;
		public int vocabulary_type_index = 0;

		/* State */
		public MaterialSet resources = new MaterialSet();
		public int ship_count_limit = 12;
		public int ship_count_approximation = 0;
		public int upgrade_slots_bonus = 0;
		public int upgrade_limit = 32;
		public bool cheats_enabled = false;
		public bool has_ascended = false;
		public float damage_multiplicator = 1.0f;

		/* Engagements */
		public List<Engagement> engagements = new List<Engagement>();
		public void NotifyEngagement(PointF coords) {
			Point rounded_coords = new Point((int)coords.X & unchecked((int)0xFFFFFE00), (int)coords.Y & unchecked((int)0xFFFFFE00));
			foreach (Engagement engagement in engagements) {
				if (engagement.location == rounded_coords) {
					return;
				}
			}
			engagements.Add(new Engagement() {location = rounded_coords, timeout = 254});
		}

		/* Per Tick */
		public void Tick() {
			for (int i_engagement = engagements.Count - 1; i_engagement >= 0; i_engagement--) {
				engagements[i_engagement].timeout -= 2;
				if (engagements[i_engagement].timeout < 0)
					engagements.RemoveAt(i_engagement);
			}
		}

		/* Construction */
		public Team(World world) {
			this.world = world;
		}
		public Team(World world, AffinityEnum affinity, Random rand) {
			this.world = world;
			SetAffinityAndShipLimit(affinity);
			InitTeam(rand);
		}
		public void SetAffinityAndShipLimit(AffinityEnum affinity) {
			this.affinity = affinity;
			if (affinity == 0)
				throw new Exception("0 affinity not permited");
			if (affinity == AffinityEnum.Friendly)
				this.ship_count_limit = 24;
			else if (affinity == AffinityEnum.Dissident)
				this.ship_count_limit = 32;
			else if (affinity == AffinityEnum.Neutral)
				this.ship_count_limit = 32;
			else if (affinity == AffinityEnum.Hostile)
				this.ship_count_limit = 40;
			else if (affinity == AffinityEnum.Wilderness)
				this.ship_count_limit = Int32.MaxValue;
		}
		public void InitWildernessTeam() {
			SetAffinityAndShipLimit(AffinityEnum.Wilderness);
			this.color = Color.DimGray;
		}
		public void InitBossTeam() {
			SetAffinityAndShipLimit(AffinityEnum.Hostile);
			this.color = Color.Red;
		}
		public void InitPlayerTeam(AffinityEnum affinity = AffinityEnum.Friendly) {
			SetAffinityAndShipLimit(affinity);
			this.ship_count_limit = 12;
			this.color = Color.Lime;
			this.bot_team = false;
		}
		public void InitTeam(Random rand) {
			InitTeamColor(new Random(rand.Next()));
			InitTeamGenerationIndices(new Random(rand.Next()));
		}
		private static List<Color> friendly_colors = new List<Color>();
		private static List<Color> hostile_colors = new List<Color>();
		public void InitTeamColor(Random rand) {
			// re-fill available colors
			if (friendly_colors.Count == 0 || hostile_colors.Count == 0) {
				// friendlies colors
				friendly_colors.Add(Color.FromArgb(0, 160, 0)); // dark green (confused with player)
				friendly_colors.Add(Color.FromArgb(0, 192, 96)); // blueish green (confused with player)
				friendly_colors.Add(Color.FromArgb(0, 80, 255)); // deep blue (perfect)
				friendly_colors.Add(Color.FromArgb(0, 128, 128)); // dark cyan (perfect)
				friendly_colors.Add(Color.FromArgb(128, 128, 255)); // pale blue (a bit light)
				friendly_colors.Add(Color.FromArgb(64, 128, 64)); // desaturated green (looks neutral)
				friendly_colors.Add(Color.FromArgb(128, 255, 128)); // pale green (a bit bright)
				friendly_colors.Add(Color.FromArgb(173, 136, 26)); // olive (looks yellow)
				friendly_colors.Add(Color.FromArgb(64, 64, 255)); // a bit light blue
				// hostiles colors
				friendly_colors.Add(Color.FromArgb(173, 76, 38)); // brown (too redish, looks hostile)
				friendly_colors.Add(Color.FromArgb(128, 0, 255)); // dark purple (too pinkish)
				hostile_colors.Add(Color.FromArgb(192, 0, 0)); // dark red
				hostile_colors.Add(Color.FromArgb(173, 34, 69)); // crismon (pinkish 5th)
				hostile_colors.Add(Color.FromArgb(255, 128, 255)); // pink (pinkish 4th)
				hostile_colors.Add(Color.FromArgb(255, 128, 0)); // orange (orangish 2nd)
				hostile_colors.Add(Color.FromArgb(255, 0, 192)); // red purple (pinkish 3rd, confusing)
				hostile_colors.Add(Color.FromArgb(255, 0, 128)); // red pink (pinkish 2nd)
				hostile_colors.Add(Color.FromArgb(255, 64, 0)); // orange-red (orangish 1st)
				hostile_colors.Add(Color.FromArgb(255, 0, 255)); // primary magenta (pinkish 1st)
				hostile_colors.Add(Color.FromArgb(255, 255, 0)); // primary yellow
				hostile_colors.Add(Color.FromArgb(255, 48, 48)); // coral
			}
			// choose a color
			if (this.affinity == AffinityEnum.Friendly)
				color = friendly_colors[rand.Next(0, friendly_colors.Count)];
			else
				color = hostile_colors[rand.Next(0, hostile_colors.Count)];
		}
		public void InitTeamGenerationIndices(Random rand) {
			this.station_type_index = rand.Next(0, Int32.MaxValue);
			this.vocabulary_type_index = rand.Next(0, Int32.MaxValue);
		}

			/**
			 *  Get the relationship with another team. 
			 */
			public bool IsFriendWith(Team other) {
			if (ReferenceEquals(this, other))
				return (true);
			if (other is null)
				return (false);
			if (this.affinity == AffinityEnum.Hostile || other.affinity == AffinityEnum.Hostile)
				return (false);
			if (this.affinity == AffinityEnum.Neutral || other.affinity == AffinityEnum.Neutral)
				return (true);
			if (this.affinity == AffinityEnum.Friendly && other.affinity == AffinityEnum.Friendly) 
				return (true);
			if (this.affinity == AffinityEnum.Dissident && other.affinity == AffinityEnum.Dissident)
				return (true);
			return (false);
		}
	}
}
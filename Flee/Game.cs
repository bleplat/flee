using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flee {

	public enum PlayState {
		Paused,
		Playing,
		Timelapse
	}

	/**
	 * @brief The current game instance, without counting the interface.
	 */
	public class Game {

		/* Settings */
		public int seed = 0;
		public int tick_duration_ms = 33;
		public bool is_multiplayer = false;
		public bool is_host = true;

		/* Members */
		GameForm parent_form = null;
		public World world = null;
		public Team player_team = null;
		public const ulong MAIN_BASE = ulong.MaxValue;
		public PlayState play_state = PlayState.Paused;

		/* Constructor */
		public Game(GameForm parent_form) {
			this.parent_form = parent_form;
		}
		public void StartSingleplayer() {
			this.is_multiplayer = false;
			this.is_host = true;
			// create the world
			world = new World(seed);
			player_team = world.CreateAndSpawnPlayer(AffinityEnum.Friendly);
			// set playing
			play_state = PlayState.Playing;
		}
		public void StartMultiplayer() {
			StartSingleplayer();
			this.is_multiplayer = true;
			this.is_host = true;
			throw new Exception("not implementd");
		}
		public void StartMultiplayer(string join_ip) {
			throw new Exception("not implementd");
		}

		/* Funtions */
		public void Tick() {
			// Tick world
			if (play_state == PlayState.Playing) {
				world.Tick();
			}
			if (play_state == PlayState.Timelapse) {
				for (int i = 1; i <= 10; i++)
					world.Tick();
			}
		}
	}

}

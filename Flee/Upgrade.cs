using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class Upgrade {

		// global
		public static List<Upgrade> upgrades = new List<Upgrade>();

		// identity
		public string name = "DefaultUpgradeName";
		public string desc = "DefaultUpgradeDesc";
		public string file = "Ups";
		public Point frame_coords = new Point(0, 0);

		// condition
		public bool not_for_bots = false;
		public string need = "";
		public MaterialSet cost = new MaterialSet();

		// effect
		public ulong delay = 0UL;
		public string effect = "";
		public int upgrade_slots_requiered = 1;
		public string spawned_ship = null;

		// constructor
		public Upgrade() {
		}

		public Upgrade(string name) {
			char[] undercase_separators = { '_' };

			this.name = name;
			if (this.name.StartsWith("Build_") || this.name.StartsWith("Launch_") || this.name.StartsWith("Summon_") || this.name.StartsWith("Spawn_")) {
				spawned_ship = name.Split(undercase_separators, 2)[1];
				file = ShipStats.classes[spawned_ship].sprite;
				cost = ShipStats.classes[spawned_ship].cost;
				if (ShipStats.classes[spawned_ship].cost is object)
					desc = ShipStats.classes[spawned_ship].desc;

				upgrade_slots_requiered = 0;
			}
		}

		public static void LoadRegUpgrades() {

			// dev mode
			upgrades.Add(new Upgrade() { name = "Developper_Mode", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 10UL, need = "?Up:Ascend", effect = "!Cheats", upgrade_slots_requiered = 0, not_for_bots = true, desc = "You deserved it." });
			upgrades.Add(new Upgrade() { name = "Developper_Mode", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 10UL, need = "?Up:Ascend", effect = "!Cheats", upgrade_slots_requiered = 0, not_for_bots = true, desc = "You deserved it." });
			upgrades.Add(new Upgrade() { name = "Developper_Mode", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 10UL, need = "?Up:Ascend", effect = "!Cheats", upgrade_slots_requiered = 0, not_for_bots = true, desc = "You deserved it." });
			upgrades.Add(new Upgrade() { name = "Developper_Mode", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 10UL, need = "?Up:Ascend", effect = "!Cheats", upgrade_slots_requiered = 0, not_for_bots = true, desc = "You deserved it." });
			upgrades.Add(new Upgrade() { name = "Developper_Mode", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 10UL, need = "?Up:Ascend", effect = "!Cheats", upgrade_slots_requiered = 0, not_for_bots = true, desc = "You deserved it." });
			upgrades.Add(new Upgrade() { name = "Developper_Mode", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 10UL, need = "?Up:Ascend", effect = "!Cheats", upgrade_slots_requiered = 0, not_for_bots = true, desc = "You deserved it." });
			upgrades.Add(new Upgrade() { name = "Developper_Mode", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 10UL, need = "?Up:Ascend", effect = "!Cheats", upgrade_slots_requiered = 0, not_for_bots = true, desc = "You deserved it." });
			upgrades.Add(new Upgrade() { name = "Developper_Mode", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 10UL, need = "?Up:Ascend", effect = "!Cheats", upgrade_slots_requiered = 0, not_for_bots = true, desc = "You deserved it." });
			upgrades.Add(new Upgrade() { name = "Free", file = "Ups", frame_coords = new Point(4, 7), cost = new MaterialSet(), delay = 10UL, need = "", effect = "!Free", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Release this ship's crew." });
			upgrades.Add(new Upgrade() { name = "Suicide", file = "Ups", frame_coords = new Point(4, 7), cost = new MaterialSet(), delay = 10UL, need = "?Type:Nothing", effect = "!Suicide", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Release this ship's crew in space." });

			// paints
			upgrades.Add(new Upgrade() { name = "Paint_1", file = "Ups", frame_coords = new Point(5, 1), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 25UL, need = "?Type:Nothing", effect = "!C:Lime", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Apply paint." });
			upgrades.Add(new Upgrade() { name = "Paint_2", file = "Ups", frame_coords = new Point(5, 2), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 25UL, need = "?Type:Nothing", effect = "!C:Cyan", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Apply paint." });
			upgrades.Add(new Upgrade() { name = "Paint_3", file = "Ups", frame_coords = new Point(5, 3), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 25UL, need = "?Type:Nothing", effect = "!C:Yellow", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Apply paint." });
			upgrades.Add(new Upgrade() { name = "Paint_4", file = "Ups", frame_coords = new Point(5, 4), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 25UL, need = "?Type:Nothing", effect = "!C:Orange", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Apply paint." });
			upgrades.Add(new Upgrade() { name = "Paint_5", file = "Ups", frame_coords = new Point(5, 5), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 25UL, need = "?Type:Nothing", effect = "!C:Blue", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Apply paint." });
			upgrades.Add(new Upgrade() { name = "Paint_6", file = "Ups", frame_coords = new Point(5, 6), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 25UL, need = "?Type:Nothing", effect = "!C:White", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Apply paint." });
			upgrades.Add(new Upgrade() { name = "Paint_7", file = "Ups", frame_coords = new Point(5, 7), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 25UL, need = "?Type:Nothing", effect = "!C:DimGray", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Apply paint." });

			// jumps
			upgrades.Add(new Upgrade() { name = "Jump", file = "Ups2", frame_coords = new Point(0, 1), cost = new MaterialSet(0L, 1L, 0L, 0L), delay = 150UL, need = "?Up:Jump_Engine", effect = "!Jump:58", upgrade_slots_requiered = 0, desc = "Jump." });
			upgrades.Add(new Upgrade() { name = "Jump_II", file = "Ups2", frame_coords = new Point(0, 3), cost = new MaterialSet(0L, 1L, 0L, 25L), delay = 200UL, need = "?Up:Jump_Engine_II", effect = "!Jump:82", upgrade_slots_requiered = 0, desc = "Higher Jump, but takes longer to load." });
			upgrades.Add(new Upgrade() { name = "Warp", file = "Ups2", frame_coords = new Point(0, 5), cost = new MaterialSet(0L, 1L, 0L, 50L), delay = 250UL, need = "?Up:Warp_Drive", effect = "!Teleport:1", upgrade_slots_requiered = 0, desc = "Warp anywhere. " + Constants.vbNewLine + "This jump takes a while to load." });

			// fixes
			upgrades.Add(new Upgrade() { name = "Tardisication", file = "Ups", frame_coords = new Point(4, 0), cost = new MaterialSet(1500L, 16L, 1L, 750L), delay = 50UL, need = "", effect = "!UpsMax:1", upgrade_slots_requiered = 0, not_for_bots = true, desc = "Break physics laws and increase the space inside the ship, allowing more upgrades." });
			upgrades.Add(new Upgrade() { name = "Repair_Armor", file = "Ups", frame_coords = new Point(5, 0), cost = new MaterialSet(200L, 0L, 0L, 0L), delay = 150UL, need = "", effect = "!Fix:10", upgrade_slots_requiered = 0, desc = "Help fixing the hull." });
			upgrades.Add(new Upgrade() { name = "Reload_Shield", file = "Ups", frame_coords = new Point(5, 0), cost = new MaterialSet(0L, 0L, 1L, 0L), delay = 50UL, need = "?Up:Auto_Nanobots", effect = "!FixSFull", upgrade_slots_requiered = 0, desc = "Help fixing the shield" });
			upgrades.Add(new Upgrade() { name = "Break_Uranium", file = "Ups2", frame_coords = new Point(7, 7), cost = new MaterialSet(-1200, -8, 1L, 0L), delay = 1UL, need = "?Type:Station", effect = "!Fix:1", upgrade_slots_requiered = 0, desc = "Sacrifice some Uranium for crystal AndAlso metal." });
			upgrades.Add(new Upgrade() { name = "Ascend", file = "Ups", frame_coords = new Point(7, 7), cost = new MaterialSet(4096L, 32L, 16L, 1024L), delay = 600UL, need = "?Up:Warp_Drive", effect = "!Ascend %Speed:300 %Life:100 !Regen:16 %Wloadmax:-100 !Shield:600 !Shieldop:100 !C:White %Shieldreg:2500", upgrade_slots_requiered = 1, not_for_bots = true, desc = "Warp your mind to an alternative universe and reach the next level of existence." });
			upgrades.Add(new Upgrade() { name = "Auto_Nanobots", file = "Ups", frame_coords = new Point(5, 0), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 50UL, need = "?Type:Nothing", effect = "%Life:1000 !Regen:1000", upgrade_slots_requiered = 1, desc = "Gives lots of hull" });

			// levelup 1->2
			upgrades.Add(new Upgrade() { name = "Improve_To_Scout", file = "Scout", frame_coords = new Point(0, 0), cost = new MaterialSet(1300L, 3L, 0L, 0L), delay = 300UL, need = "?Type:Simpleship", effect = "!Type:Scout", upgrade_slots_requiered = 1, desc = "Change into scout ship." });
			upgrades.Add(new Upgrade() { name = "Improve_To_Bomber", file = "Bomber", frame_coords = new Point(0, 0), cost = new MaterialSet(1600L, 3L, 0L, 0L), delay = 300UL, need = "?Type:Simpleship", effect = "!Type:Bomber", upgrade_slots_requiered = 1, desc = "Change into attack ship." });
			upgrades.Add(new Upgrade() { name = "Improve_To_Artillery", file = "Artillery", frame_coords = new Point(0, 0), cost = new MaterialSet(1600L, 3L, 0L, 0L), delay = 300UL, need = "?Type:Simpleship", effect = "!Type:Artillery", upgrade_slots_requiered = 1, desc = "Change into artillery." });
			upgrades.Add(new Upgrade() { name = "Improve_To_Dronner", file = "Dronner", frame_coords = new Point(0, 0), cost = new MaterialSet(2000L, 5L, 0L, 0L), delay = 300UL, need = "?Type:Simpleship", effect = "!Type:Dronner", upgrade_slots_requiered = 1, desc = "Change into ship builder." });

			// deflectors
			upgrades.Add(new Upgrade() { name = "Deflector", file = "EFF_Deflected", frame_coords = new Point(0, 0), cost = new MaterialSet(400L, 1L, 0L, 25L), delay = 200UL, need = "", effect = "!Deflector:1", upgrade_slots_requiered = 1, desc = "As an alternative to shields, this device block any projectile, but is very slow to reload." + Constants.vbNewLine + "Due to the way it works, It is not efficient against fast reloading weapons." });
			upgrades.Add(new Upgrade() { name = "Deflector_II", file = "EFF_Deflected", frame_coords = new Point(1, 0), cost = new MaterialSet(500L, 2L, 0L, 25L), delay = 200UL, need = "+Lvl:2" + NPU(), effect = "!Deflector:1", upgrade_slots_requiered = 1, desc = "Block another projectile." });
			upgrades.Add(new Upgrade() { name = "Deflector_III", file = "EFF_Deflected", frame_coords = new Point(2, 0), cost = new MaterialSet(600L, 4L, 0L, 25L), delay = 200UL, need = "" + NPU(), effect = "!Deflector:1", upgrade_slots_requiered = 1, desc = "Block another projectile." });
			upgrades.Add(new Upgrade() { name = "Deflector_IV", file = "EFF_Deflected", frame_coords = new Point(3, 0), cost = new MaterialSet(7000L, 8L, 0L, 25L), delay = 200UL, need = "" + NPU(), effect = "!Deflector:1", upgrade_slots_requiered = 1, desc = "Block another projectile." });
			upgrades.Add(new Upgrade() { name = "Hot_Deflector", file = "EFF_Deflected2", frame_coords = new Point(4, 0), cost = new MaterialSet(900L, 3L, 0L, 100L), delay = 200UL, need = "?Up:Deflector", effect = "!HotDeflector:8", upgrade_slots_requiered = 1, desc = "This unreliable device was create in an attempt to fix the Deflector vulnerabilities." });
			upgrades.Add(new Upgrade() { name = "Hot_Deflector_II", file = "EFF_Deflected2", frame_coords = new Point(5, 0), cost = new MaterialSet(1100L, 3L, 0L, 100L), delay = 200UL, need = "+Lvl:2" + NPU(), effect = "!HotDeflector:8", upgrade_slots_requiered = 1, desc = "The Hot Deflector is twice as reliable." });
			upgrades.Add(new Upgrade() { name = "Cold_Deflector", file = "EFF_Deflected3", frame_coords = new Point(7, 0), cost = new MaterialSet(2300L, 6L, 0L, 150L), delay = 200UL, need = "+Lvl:2 ?Up:Deflector", effect = "!ColdDeflector:1", upgrade_slots_requiered = 1, desc = "This deflector is able to catch an incredible amount of fire, but prevent your weapons from reloading while it is recovering, AndAlso cause damage to your hull from the inside." });

			// shields
			upgrades.Add(new Upgrade() { name = "Shield", file = "Ups", frame_coords = new Point(3, 0), cost = new MaterialSet(200L, 1L, 0L, 0L), delay = 100UL, need = "", effect = "!Shield:40 !Shieldop:25", upgrade_slots_requiered = 1, desc = "Protection shield blocking 25% of incomming damages." });
			upgrades.Add(new Upgrade() { name = "Shield_II", file = "Ups", frame_coords = new Point(3, 1), cost = new MaterialSet(400L, 1L, 0L, 0L), delay = 100UL, need = "" + NPU(), effect = "%Shield:10 !Shieldop:25", upgrade_slots_requiered = 1, desc = "Shield stop 25% more damages." });
			upgrades.Add(new Upgrade() { name = "Shield_III", file = "Ups", frame_coords = new Point(3, 2), cost = new MaterialSet(600L, 1L, 0L, 0L), delay = 100UL, need = "" + NPU(), effect = "%Shield:10 !Shieldop:25", upgrade_slots_requiered = 1, desc = "Shield stop 25% more damages." });
			upgrades.Add(new Upgrade() { name = "Shield_IV", file = "Ups", frame_coords = new Point(3, 3), cost = new MaterialSet(800L, 2L, 0L, 50L), delay = 100UL, need = "" + NPU(), effect = "%Shield:10 !Shieldop:25", upgrade_slots_requiered = 1, desc = "Shield stop 25% more damages." });
			upgrades.Add(new Upgrade() { name = "Shield_Capacitor", file = "Ups", frame_coords = new Point(3, 4), cost = new MaterialSet(600L, 1L, 0L, 0L), delay = 100UL, need = "?Up:Shield", effect = "%Shield:50", upgrade_slots_requiered = 1, desc = "Shield last 50% longer." });
			upgrades.Add(new Upgrade() { name = "Shield_Capacitor_II", file = "Ups", frame_coords = new Point(3, 5), cost = new MaterialSet(700L, 2L, 0L, 0L), delay = 100UL, need = "" + NPU(), effect = "%Shield:50", upgrade_slots_requiered = 1, desc = "Shield last 50% longer." });
			upgrades.Add(new Upgrade() { name = "Shield_Loader", file = "Ups", frame_coords = new Point(3, 7), cost = new MaterialSet(800L, 1L, 0L, 100L), delay = 100UL, need = "?Up:Shield", effect = "%Shieldreg:50", upgrade_slots_requiered = 1, desc = "Shield recover 50% faster." });

			// shields (advanced)
			upgrades.Add(new Upgrade() { name = "Dynamic_Shield", file = "Ups2", frame_coords = new Point(1, 0), cost = new MaterialSet(700L, 2L, 0L, 0L), delay = 100UL, need = "+Lvl:2 ?Up:Shield", effect = "%Shieldreg:100 %Shield:-50", upgrade_slots_requiered = 1, desc = "Shield recover twice as fast, but last half as long." });
			upgrades.Add(new Upgrade() { name = "Partial_Shield", file = "Ups2", frame_coords = new Point(1, 1), cost = new MaterialSet(700L, 2L, 0L, 0L), delay = 100UL, need = "+Lvl:2 ?Up:Shield_II ?Up:Shield_Loader", effect = "%Shieldreg:75 !Shieldop:-20", upgrade_slots_requiered = 1, desc = "Shield recover 75% faster, but let 20% of shoot damages pass." });
			upgrades.Add(new Upgrade() { name = "Surface_Shield", file = "Ups2", frame_coords = new Point(1, 2), cost = new MaterialSet(700L, 2L, 0L, 0L), delay = 100UL, need = "+Lvl:2 ?Up:Shield_II", effect = "%Shield:75 !Shieldop:-20", upgrade_slots_requiered = 1, desc = "Shield last 75% longer, but let 20% of shoot damages pass." });
			upgrades.Add(new Upgrade() { name = "Alternative_Shield", file = "Ups2", frame_coords = new Point(1, 3), cost = new MaterialSet(700L, 3L, 0L, 100L), delay = 100UL, need = "+Lvl:2 ?Up:Shield_Loader", effect = "%Shieldreg:25 %Shield:25 !Shieldop:-20", upgrade_slots_requiered = 1, desc = "Shield recover 25% faster AndAlso last 25% longer, but let 20% of shoot damages pass." });
			upgrades.Add(new Upgrade() { name = "Bidirectional_Shield", file = "Ups2", frame_coords = new Point(1, 4), cost = new MaterialSet(700L, 2L, 0L, 100L), delay = 100UL, need = "+Lvl:1 ?Up:Shield", effect = "%Shield:25 %Wloadmax:15", upgrade_slots_requiered = 1, desc = "Shield last 25% longer, but weapons shoot slower." });
			upgrades.Add(new Upgrade() { name = "Shield_Capacitor_III", file = "Ups2", frame_coords = new Point(3, 2), cost = new MaterialSet(1200L, 3L, 1L, 0L), delay = 200UL, need = "?Up:Shield_Capacitor_II", effect = "%Shield:75", upgrade_slots_requiered = 1, desc = "Shield last 50% longer." });
			upgrades.Add(new Upgrade() { name = "Shield_Loader_II", file = "Ups2", frame_coords = new Point(3, 4), cost = new MaterialSet(1200L, 3L, 0L, 200L), delay = 200UL, need = "?Up:Shield_Loader", effect = "%Shieldreg:35", upgrade_slots_requiered = 1, desc = "Shield recover 35% faster." });
			upgrades.Add(new Upgrade() { name = "Shield_Patch", file = "Ups2", frame_coords = new Point(3, 0), cost = new MaterialSet(600L, 1L, 1L, 200L), delay = 200UL, need = "?Up:Shield_IV", effect = "!Shieldop:25", upgrade_slots_requiered = 1, desc = "Shield stop +25% of the damages (this can fix some of the other upgrades effects)" });

			// reloading
			upgrades.Add(new Upgrade() { name = "Reloader", file = "Ups", frame_coords = new Point(2, 0), cost = new MaterialSet(100L, 1L, 0L, 0L), delay = 100UL, need = "+Lvl:1 ?W", effect = "%Wloadmax:-15", upgrade_slots_requiered = 1, desc = "Weapons reload 15% faster." });
			upgrades.Add(new Upgrade() { name = "Reloader_II", file = "Ups", frame_coords = new Point(2, 1), cost = new MaterialSet(200L, 1L, 0L, 0L), delay = 100UL, need = "?W" + NPU(), effect = "%Wloadmax:-15", upgrade_slots_requiered = 1, desc = "Weapons reload 15% faster." });
			upgrades.Add(new Upgrade() { name = "Reloader_III", file = "Ups", frame_coords = new Point(2, 2), cost = new MaterialSet(300L, 1L, 0L, 0L), delay = 100UL, need = "?W" + NPU(), effect = "%Wloadmax:-15", upgrade_slots_requiered = 1, desc = "Weapons reload 15% faster." });
			upgrades.Add(new Upgrade() { name = "Reloader_IV", file = "Ups", frame_coords = new Point(2, 3), cost = new MaterialSet(400L, 1L, 0L, 0L), delay = 100UL, need = "?W" + NPU(), effect = "%Wloadmax:-15", upgrade_slots_requiered = 1, desc = "Weapons reload 15% faster." });
			upgrades.Add(new Upgrade() { name = "Reloader_V", file = "Ups", frame_coords = new Point(2, 4), cost = new MaterialSet(500L, 1L, 0L, 0L), delay = 100UL, need = "?W" + NPU(), effect = "%Wloadmax:-15", upgrade_slots_requiered = 1, desc = "Weapons reload 15% faster." });
			upgrades.Add(new Upgrade() { name = "Reloader_VI", file = "Ups", frame_coords = new Point(2, 5), cost = new MaterialSet(600L, 1L, 0L, 0L), delay = 100UL, need = "?W" + NPU(), effect = "%Wloadmax:-15", upgrade_slots_requiered = 1, desc = "Weapons reload 15% faster." });
			upgrades.Add(new Upgrade() { name = "Reloader_VII", file = "Ups", frame_coords = new Point(2, 6), cost = new MaterialSet(700L, 1L, 0L, 0L), delay = 100UL, need = "?W" + NPU(), effect = "%Wloadmax:-15", upgrade_slots_requiered = 1, desc = "Weapons reload 15% faster." });
			upgrades.Add(new Upgrade() { name = "Reloader_VIII", file = "Ups", frame_coords = new Point(2, 7), cost = new MaterialSet(800L, 1L, 0L, 0L), delay = 100UL, need = "?W" + NPU(), effect = "%Wloadmax:-15", upgrade_slots_requiered = 1, desc = "Weapons reload 15% faster." });

			// multishot
			upgrades.Add(new Upgrade() { name = "Double_Barrel", file = "Ups", frame_coords = new Point(7, 0), cost = new MaterialSet(900L, 1L, 0L, 0L), delay = 100UL, need = "?W", effect = "%Wbar:100 %Wloadmax:80", upgrade_slots_requiered = 1, desc = "Loading is 80% longer, but weapons get double shoot." + Constants.vbNewLine + "'Two barrilets arround an axis.'" });
			upgrades.Add(new Upgrade() { name = "Double_Double_Barrel", file = "Ups", frame_coords = new Point(7, 5), cost = new MaterialSet(1200L, 1L, 0L, 0L), delay = 120UL, need = "+Lvl:2 ?W" + NPU(), effect = "%Wbar:100 %Wloadmax:90", upgrade_slots_requiered = 1, desc = "Loading is 90% longer, but weapons get double shoot again." + Constants.vbNewLine + "'Two groups, of two barrilets arround an axis, arround an axis.'" });
			upgrades.Add(new Upgrade() { name = "Double_Double_Barrel_Doubled", file = "Ups", frame_coords = new Point(7, 6), cost = new MaterialSet(1400L, 1L, 0L, 0L), delay = 140UL, need = "+Lvl:2 ?W" + NPU(), effect = "%Wbar:100 %Wloadmax:100", upgrade_slots_requiered = 1, desc = "Loading is twice as much expensive in time, but they get an awesome improvement." + Constants.vbNewLine + "'Two groups, of two barrilets arround an axis, arround an axis. AndAlso itself doubled arround another axis. Trust the engineers.'" });

			// speed and turn
			upgrades.Add(new Upgrade() { name = "Steering", file = "Ups2", frame_coords = new Point(2, 0), cost = new MaterialSet(200L, 0L, 0L, 0L), delay = 100UL, need = "?S", effect = "!Agility:1", upgrade_slots_requiered = 1, desc = "Ship turn faster." });
			upgrades.Add(new Upgrade() { name = "Steering_II", file = "Ups2", frame_coords = new Point(2, 1), cost = new MaterialSet(400L, 0L, 0L, 25L), delay = 150UL, need = "?S" + NPU(), effect = "!Agility:1", upgrade_slots_requiered = 1, desc = "Ship turn faster." });
			upgrades.Add(new Upgrade() { name = "Steering_III", file = "Ups2", frame_coords = new Point(2, 2), cost = new MaterialSet(1200L, 2L, 0L, 50L), delay = 200UL, need = "?S" + NPU(), effect = "!Agility:1", upgrade_slots_requiered = 1, desc = "Ship turn faster." });
			upgrades.Add(new Upgrade() { name = "Engines", file = "Ups", frame_coords = new Point(1, 0), cost = new MaterialSet(200L, 1L, 0L, 0L), delay = 100UL, need = "?S", effect = "%Speed:5", upgrade_slots_requiered = 1, desc = "Ship is 5% faster." });
			upgrades.Add(new Upgrade() { name = "Engines_II", file = "Ups", frame_coords = new Point(1, 1), cost = new MaterialSet(200L, 1L, 0L, 0L), delay = 105UL, need = "?S" + NPU(), effect = "%Speed:10", upgrade_slots_requiered = 1, desc = "Ship is 10% faster." });
			upgrades.Add(new Upgrade() { name = "Engines_III", file = "Ups", frame_coords = new Point(1, 2), cost = new MaterialSet(200L, 1L, 0L, 0L), delay = 110UL, need = "?S" + NPU(), effect = "%Speed:10", upgrade_slots_requiered = 1, desc = "Ship is 10% faster." });
			upgrades.Add(new Upgrade() { name = "Engines_IV", file = "Ups", frame_coords = new Point(1, 3), cost = new MaterialSet(200L, 1L, 0L, 0L), delay = 115UL, need = "?S" + NPU(), effect = "%Speed:5", upgrade_slots_requiered = 1, desc = "Ship is 5% faster." });
			upgrades.Add(new Upgrade() { name = "Engines_V", file = "Ups", frame_coords = new Point(1, 4), cost = new MaterialSet(200L, 1L, 0L, 25L), delay = 120UL, need = "?S" + NPU(), effect = "%Speed:5", upgrade_slots_requiered = 1, desc = "Ship is 5% faster." });
			upgrades.Add(new Upgrade() { name = "Engines_VI", file = "Ups", frame_coords = new Point(1, 5), cost = new MaterialSet(200L, 1L, 0L, 50L), delay = 125UL, need = "?S" + NPU(), effect = "%Speed:10", upgrade_slots_requiered = 1, desc = "Ship is 10% faster." });
			upgrades.Add(new Upgrade() { name = "Engines_VII", file = "Ups", frame_coords = new Point(1, 6), cost = new MaterialSet(200L, 1L, 0L, 75L), delay = 130UL, need = "?S" + NPU(), effect = "%Speed:10", upgrade_slots_requiered = 1, desc = "Ship is 10% faster." });
			upgrades.Add(new Upgrade() { name = "Engines_VIII", file = "Ups", frame_coords = new Point(1, 7), cost = new MaterialSet(200L, 1L, 0L, 100L), delay = 140UL, need = "?S" + NPU(), effect = "%Speed:10", upgrade_slots_requiered = 1, desc = "Ship is 10% faster." });

			// jump engines
			upgrades.Add(new Upgrade() { name = "Jump_Engine", file = "Ups2", frame_coords = new Point(0, 0), cost = new MaterialSet(800L, 2L, 0L, 0L), delay = 200UL, need = "?S", effect = "", upgrade_slots_requiered = 1, desc = "Allow the ship to make jumps, at the price of crystals." });
			upgrades.Add(new Upgrade() { name = "Jump_Engine_II", file = "Ups2", frame_coords = new Point(0, 2), cost = new MaterialSet(1000L, 2L, 0L, 25L), delay = 250UL, need = "?S ?Up:Jump_Engine", effect = "", upgrade_slots_requiered = 1, desc = "Allow higher jumps, but is more expensive in crystals." });
			upgrades.Add(new Upgrade() { name = "Warp_Drive", file = "Ups2", frame_coords = new Point(0, 4), cost = new MaterialSet(1200L, 3L, 0L, 600L), delay = 300UL, need = "?NotStation", effect = "", upgrade_slots_requiered = 1, desc = "Allow you to warp anywhere." });

			// hull
			upgrades.Add(new Upgrade() { name = "Hull", file = "Ups", frame_coords = new Point(0, 0), cost = new MaterialSet(200L, 0L, 0L, 0L), delay = 80UL, need = "", effect = "%Life:5", upgrade_slots_requiered = 1, desc = "Increase the hull." });
			upgrades.Add(new Upgrade() { name = "Hull_II", file = "Ups", frame_coords = new Point(0, 1), cost = new MaterialSet(300L, 0L, 0L, 0L), delay = 90UL, need = "" + NPU(), effect = "%Life:10", upgrade_slots_requiered = 1, desc = "Increase the hull." });
			upgrades.Add(new Upgrade() { name = "Hull_III", file = "Ups", frame_coords = new Point(0, 2), cost = new MaterialSet(600L, 0L, 0L, 0L), delay = 100UL, need = "" + NPU(), effect = "%Life:10", upgrade_slots_requiered = 1, desc = "Increase the hull." });
			upgrades.Add(new Upgrade() { name = "Hull_IV", file = "Ups", frame_coords = new Point(0, 3), cost = new MaterialSet(1300L, 0L, 0L, 0L), delay = 110UL, need = "" + NPU(), effect = "%Life:10", upgrade_slots_requiered = 1, desc = "Increase the hull." });
			upgrades.Add(new Upgrade() { name = "Hull_V", file = "Ups", frame_coords = new Point(0, 4), cost = new MaterialSet(1900L, 0L, 0L, 0L), delay = 120UL, need = "" + NPU(), effect = "%Life:10", upgrade_slots_requiered = 1, desc = "Increase the hull." });
			upgrades.Add(new Upgrade() { name = "Hull_VI", file = "Ups", frame_coords = new Point(0, 5), cost = new MaterialSet(2500L, 0L, 0L, 0L), delay = 130UL, need = "" + NPU(), effect = "%Life:10", upgrade_slots_requiered = 1, desc = "Increase the hull." });
			upgrades.Add(new Upgrade() { name = "Heavy_Hull", file = "Ups", frame_coords = new Point(0, 6), cost = new MaterialSet(3000L, 0L, 0L, 200L), delay = 140UL, need = "" + NPU(), effect = "%Life:15", upgrade_slots_requiered = 1, desc = "Increase the hull." });
			upgrades.Add(new Upgrade() { name = "Heavy_Hull_II", file = "Ups", frame_coords = new Point(0, 7), cost = new MaterialSet(4000L, 0L, 0L, 400L), delay = 150UL, need = "" + NPU(), effect = "%Life:20", upgrade_slots_requiered = 1, desc = "Increase the hull, but half the ship speed." });

			// repairs
			upgrades.Add(new Upgrade() { name = "Repair_Bots", file = "Ups", frame_coords = new Point(7, 1), cost = new MaterialSet(400L, 1L, 0L, 0L), delay = 100UL, need = "+Life:100", effect = "!Regen:1", upgrade_slots_requiered = 1, desc = "Repair the ship faster." });
			upgrades.Add(new Upgrade() { name = "Repair_Bots_II", file = "Ups", frame_coords = new Point(7, 2), cost = new MaterialSet(700L, 1L, 0L, 0L), delay = 120UL, need = "+Life:200" + NPU(), effect = "!Regen:1", upgrade_slots_requiered = 1, desc = "Repair the ship faster." });
			upgrades.Add(new Upgrade() { name = "Repair_Bots_III", file = "Ups", frame_coords = new Point(7, 3), cost = new MaterialSet(1400L, 1L, 0L, 0L), delay = 130UL, need = "+Life:300" + NPU(), effect = "!Regen:1", upgrade_slots_requiered = 1, desc = "Repair the ship faster." });
			upgrades.Add(new Upgrade() { name = "Repair_Bots_IV", file = "Ups", frame_coords = new Point(7, 4), cost = new MaterialSet(2000L, 2L, 0L, 0L), delay = 150UL, need = "+Life:400" + NPU(), effect = "!Regen:1", upgrade_slots_requiered = 1, desc = "Repair the ship faster." });

			// max units
			upgrades.Add(new Upgrade() { name = "Multiunit_Control", file = "Ups", frame_coords = new Point(6, 0), cost = new MaterialSet(1000L, 1L, 0L, 0L), delay = 100UL, need = "?Base", effect = "!Maxships:2", upgrade_slots_requiered = 1, desc = "Control 2 more ships" });
			upgrades.Add(new Upgrade() { name = "Multiunit_Control_II", file = "Ups", frame_coords = new Point(6, 1), cost = new MaterialSet(2000L, 2L, 0L, 0L), delay = 150UL, need = "?Base" + NPU(), effect = "!Maxships:3", upgrade_slots_requiered = 1, desc = "Control 3 more ships" });
			upgrades.Add(new Upgrade() { name = "Multiunit_Control_III", file = "Ups", frame_coords = new Point(6, 2), cost = new MaterialSet(3000L, 3L, 0L, 0L), delay = 200UL, need = "?Base" + NPU(), effect = "!Maxships:2", upgrade_slots_requiered = 1, desc = "Control 2 more ships" });
			upgrades.Add(new Upgrade() { name = "Multiunit_Control_IV", file = "Ups", frame_coords = new Point(6, 3), cost = new MaterialSet(4000L, 4L, 0L, 0L), delay = 200UL, need = "?Base" + NPU(), effect = "!Maxships:3", upgrade_slots_requiered = 1, desc = "Control 3 more ships" });
			upgrades.Add(new Upgrade() { name = "Multiunit_Control_V", file = "Ups", frame_coords = new Point(6, 4), cost = new MaterialSet(5000L, 5L, 0L, 0L), delay = 250UL, need = "?Base" + NPU(), effect = "!Maxships:2", upgrade_slots_requiered = 1, desc = "Control 2 more ships" });
			upgrades.Add(new Upgrade() { name = "Multiunit_Control_VI", file = "Ups", frame_coords = new Point(6, 5), cost = new MaterialSet(6000L, 6L, 0L, 0L), delay = 250UL, need = "?Base" + NPU(), effect = "!Maxships:3", upgrade_slots_requiered = 1, desc = "Control 3 more ships" });
			upgrades.Add(new Upgrade() { name = "Multiunit_Control_VII", file = "Ups", frame_coords = new Point(6, 6), cost = new MaterialSet(7000L, 7L, 0L, 100L), delay = 250UL, need = "?Base" + NPU(), effect = "!Maxships:2", upgrade_slots_requiered = 1, desc = "Control 2 more ships" });
			upgrades.Add(new Upgrade() { name = "Multiunit_Control_VIII", file = "Ups", frame_coords = new Point(6, 7), cost = new MaterialSet(8000L, 8L, 0L, 200L), delay = 300UL, need = "?Base" + NPU(), effect = "!Maxships:2", upgrade_slots_requiered = 1, desc = "Control 2 more ships" });

			// max upgrades (level)
			upgrades.Add(new Upgrade() { name = "Space_Shemes", file = "Ups", frame_coords = new Point(4, 0), cost = new MaterialSet(1000L, 1L, 0L, 0L), delay = 100UL, need = "?Base", effect = "!Upsbonus:2", upgrade_slots_requiered = 1, desc = "New ships can install 2 additional upgrades." });
			upgrades.Add(new Upgrade() { name = "Space_Shemes_II", file = "Ups", frame_coords = new Point(4, 1), cost = new MaterialSet(2000L, 2L, 0L, 0L), delay = 100UL, need = "?Base" + NPU(), effect = "!Upsbonus:2", upgrade_slots_requiered = 1, desc = "New ships can install 2 additional upgrades." });
			upgrades.Add(new Upgrade() { name = "Space_Shemes_III", file = "Ups", frame_coords = new Point(4, 2), cost = new MaterialSet(3000L, 4L, 0L, 0L), delay = 100UL, need = "?Base" + NPU(), effect = "!Upsbonus:2", upgrade_slots_requiered = 1, desc = "New ships can install 2 additional upgrades." });
			upgrades.Add(new Upgrade() { name = "Space_Shemes_IV", file = "Ups", frame_coords = new Point(4, 4), cost = new MaterialSet(4000L, 8L, 0L, 0L), delay = 100UL, need = "?Base" + NPU(), effect = "!Upsbonus:2", upgrade_slots_requiered = 1, desc = "New ships can install 2 additional upgrades." });
			upgrades.Add(new Upgrade() { name = "Space_Shemes_V", file = "Ups", frame_coords = new Point(4, 3), cost = new MaterialSet(5000L, 16L, 0L, 0L), delay = 100UL, need = "?Base" + NPU(), effect = "!Upsbonus:2", upgrade_slots_requiered = 1, desc = "New ships can install 2 additional upgrades." });

			// special single-time ships
			upgrades.Add(new Upgrade() { name = "Conceive_Legend_I", file = "Legend_I", frame_coords = new Point(0, 0), cost = new MaterialSet(1400L, 16L, 0L, 600L), delay = 650UL, need = "?MS ?Type:Station", effect = "!Sum:Legend_I", upgrade_slots_requiered = 1, desc = "Build a legendary ship." + Constants.vbNewLine + @"/!\ You can only build this ship once!" });
			upgrades.Add(new Upgrade() { name = "Conceive_Legend_K", file = "Legend_K", frame_coords = new Point(0, 0), cost = new MaterialSet(1200L, 14L, 0L, 400L), delay = 650UL, need = "?MS ?Type:Station", effect = "!Sum:Legend_K", upgrade_slots_requiered = 1, desc = "Build a legendary ship." + Constants.vbNewLine + @"/!\ You can only build this ship once!" });
			upgrades.Add(new Upgrade() { name = "Conceive_Legend_L", file = "Legend_L", frame_coords = new Point(0, 0), cost = new MaterialSet(1300L, 15L, 0L, 400L), delay = 650UL, need = "?MS ?Type:Station", effect = "!Sum:Legend_L", upgrade_slots_requiered = 1, desc = "Build a legendary ship." + Constants.vbNewLine + @"/!\ You can only build this ship once!" });
			upgrades.Add(new Upgrade() { name = "Conceive_Legend_U", file = "Legend_U", frame_coords = new Point(0, 0), cost = new MaterialSet(1200L, 16L, 0L, 200L), delay = 650UL, need = "?MS ?Type:Station", effect = "!Sum:Legend_U", upgrade_slots_requiered = 1, desc = "Build a legendary ship." + Constants.vbNewLine + @"/!\ You can only build this ship once!" });
			upgrades.Add(new Upgrade() { name = "Conceive_Legend_Y", file = "Legend_Y", frame_coords = new Point(0, 0), cost = new MaterialSet(1400L, 14L, 0L, 300L), delay = 650UL, need = "?MS ?Type:Station", effect = "!Sum:Legend_Y", upgrade_slots_requiered = 1, desc = "Build a legendary ship." + Constants.vbNewLine + @"/!\ You can only build this ship once!" });

			// other spawns
			upgrades.Add(new Upgrade() { name = "Launch_MSL", file = "MSL", frame_coords = new Point(0, 0), cost = new MaterialSet(400L, 0L, 0L, 50L), delay = 100UL, need = "?Type:Scout ?MS", effect = "!Sum:MSL", upgrade_slots_requiered = 0, desc = "Launch a missile." + Constants.vbNewLine + @"/!\ Requires a free ship slot!" });
			upgrades.Add(new Upgrade() { name = "Build_Nuke", file = "Nuke", frame_coords = new Point(0, 0), cost = new MaterialSet(8000L, 32L, 8L, 2000L), delay = 250UL, need = "?Type:Ambassador", effect = "!Sum:Nuke", upgrade_slots_requiered = 0, desc = "When something go wrong, it still can go worse." + Constants.vbNewLine + @"/!\ Playing with the atom may lead to unexpected results." });
			upgrades.Add(new Upgrade() { name = "Build_Finalizer", file = "Finalizer", frame_coords = new Point(0, 0), cost = new MaterialSet(0L, 0L, 0L, 0L), delay = 600UL, need = "?Up:Ascend", effect = "!Sum:Finalizer", upgrade_slots_requiered = 0, desc = "Yet, not everybody made it through." + Constants.vbNewLine + "We will return for them." });
		}

		public static void LoadBuildUpgrades() {
			foreach (ShipStats ship_class in ShipStats.classes.Values) {
				string build_ship_upgrade_name = "Build_" + ship_class.name;
				string launch_ship_upgrade_name = "Launch_" + ship_class.name;
				if (UpgradeFromName(build_ship_upgrade_name) is null && UpgradeFromName(launch_ship_upgrade_name) is null) {
					upgrades.Add(new Upgrade(build_ship_upgrade_name) { cost = ship_class.cost, delay = (ulong)(ship_class.complexity), need = "?MS", effect = "!Sum:" + ship_class.name, upgrade_slots_requiered = 0, desc = "Build a " + ship_class.name + "." });
					if (ship_class.desc is null)
						upgrades[upgrades.Count - 1].desc = ship_class.desc;
				}
			}
		}

		public static string NPU() {
			return " ?Up:" + upgrades[upgrades.Count - 1].name;
		}

		public static Upgrade UpgradeFromName(string up_name) {
			foreach (var aUp in upgrades)
				if ((aUp.name ?? "") == (up_name ?? ""))
					return aUp;

			return null;
		}

		public static void ForceUpgradeToShip(Ship a_ship, string up_name) {
			foreach (var aUp in upgrades)
				if ((aUp.name ?? "") == (up_name ?? ""))
					a_ship.Upgrading = aUp;
		}
	}
}
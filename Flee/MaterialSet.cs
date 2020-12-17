using System;

namespace Flee {
	public class MaterialSet {

		// Materials
		public long Metal = 0L; // TODO: Resname "metals"
		public long Crystal = 0L; // TODO: Resname "crystals"
		public long Fissile = 0L; // TODO: Resname "fissile"
		public long Starfuel = 0L; // TODO: Resname "antimatter"

		// Constructor
		public MaterialSet() {
			// Default
		}

		public MaterialSet(long ResM, long ResC, long ResU, long ResA) {
			Metal = ResM;
			Crystal = ResC;
			Fissile = ResU;
			Starfuel = ResA;
		}

		public MaterialSet(string input) {
			LoadFromString(input);
		}

		// Test if this material set contains at least some requierements
		public bool HasEnough(ref MaterialSet requierement) {
			return Metal >= requierement.Metal && Crystal >= requierement.Crystal && Fissile >= requierement.Fissile && Starfuel >= requierement.Starfuel;
		}

		// Remove materials from this set
		public void Deplete(ref MaterialSet depletion) {
			Metal -= depletion.Metal;
			Crystal -= depletion.Crystal;
			Fissile -= depletion.Fissile;
			Starfuel -= depletion.Starfuel;
		}

		// Add materials to this set
		public void Add(ref MaterialSet addition) {
			Metal += addition.Metal;
			Crystal += addition.Crystal;
			Fissile += addition.Fissile;
			Starfuel += addition.Starfuel;
		}

		public void Add(long ResM, long ResC, long ResU, long ResA) {
			Metal += ResM;
			Crystal += ResC;
			Fissile += ResU;
			Starfuel += ResA;
		}

		public void AddLoot(ref MaterialSet looted_addition) {
			Metal = (long)(Metal + looted_addition.Metal / 8d);
			Crystal = (long)(Crystal + looted_addition.Crystal / 2d);
			Fissile = (long)(Fissile + looted_addition.Fissile / 2d);
			Starfuel = (long)(Starfuel + looted_addition.Starfuel / 4d);
		}

		public MaterialSet MultipliedBy(double amount) {
			return new MaterialSet((long)(Metal * amount), (long)(Crystal * amount), (long)(Fissile * amount), (long)(Starfuel * amount));
		}

		// Import / Export
		public override string ToString() {
			return Metal + ";" + Crystal + ";" + Fissile + ";" + Starfuel;
		}

		public void LoadFromString(string input) {
			var inputs = new string[6];
			inputs = input.Split(';');
			Metal = Convert.ToInt64(inputs[0]);
			Crystal = Convert.ToInt64(inputs[1]);
			Fissile = Convert.ToInt64(inputs[2]);
			Starfuel = Convert.ToInt64(inputs[3]);
		}
	}
}
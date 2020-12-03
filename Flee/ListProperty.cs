using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class ListProperty {
		public string name;
		public string value;

		public ListProperty(string line) {
			if (Conversions.ToString(line[0]) == @"\t" || Conversions.ToString(line[0]) == " ") throw new Exception("Malformed property: " + line);

			var tuple = line.Split('=');
			name = tuple[0];
			value = tuple[1];
		}
	}
}
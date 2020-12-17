using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class ListProperty {
		public string name;
		public string value;

		public ListProperty(string line) {
			if (line[0] == '\t' || line[0] == ' ')
				throw new Exception("Malformed property: " + line);

			var tuple = line.Split(new char[] { '=' }, 2);
			name = tuple[0];
			value = tuple[1];
		}
	}
}
using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	public class ListClass {
		public string type;
		public string name;
		public List<ListProperty> properties = new List<ListProperty>();

		public ListClass(string header, List<string> lines) {
			var header_parts = header.Split(' ');
			type = header_parts[0];
			name = header_parts[1];
			foreach (string line in lines) {
				if (line.Length == 0 || line[0] == '#')
					continue;

				if (!line.Contains("="))
					throw new Exception("Invalid property: " + line);

				properties.Add(new ListProperty(line));
			}
		}
	}
}
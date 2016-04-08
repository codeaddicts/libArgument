using System;

namespace Codeaddicts.libArgument {
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = false)]
	public class Docs : Attribute {
		public readonly string description;

		public Docs (string description) {
			this.description = description;
		}
	}
}


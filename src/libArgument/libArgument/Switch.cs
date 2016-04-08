using System;

namespace Codeaddicts.libArgument {
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
	public class Switch : Argument {
		public Switch () { }
		public Switch (params string[] names) : base (names) { }
	}
}


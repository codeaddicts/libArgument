using System;

namespace Codeaddicts.libArgument.Attributes {

	[Obsolete ("Please use Codeaddicts.libArgument.Argument", true)]
	public class Argument {
		public Argument () { }
		public Argument (params string[] names) { }
	}

	[Obsolete ("Please use Codeaddicts.libArgument.Switch", true)]
	public class Switch {
		public Switch () { }
		public Switch (params string[] names) { }
	}
}


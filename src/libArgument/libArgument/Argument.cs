using System;
using System.Linq;

namespace Codeaddicts.libArgument {
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
	public class Argument : Attribute {
		public string[] names;

		public Argument () { }

		public Argument (params string[] names) {
			this.names = names;
		}

		public void AutoInfer (string fieldname) {
			if (names == null || !names.Any ())
				names = new [] { string.Format ("--{0}", fieldname) };
		}
	}
}


using System;

namespace Codeaddicts.libArgument.Attributes
{
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
	public class Switch : Argument
	{
		public Switch () : base () {
		}

		public Switch (string fullname) : base (fullname) {
		}

		public Switch (string shortname, string fullname) : base (shortname, fullname) {
		}

		public new Switch InferName (string name) {
			return new Switch (name);
		}
	}
}


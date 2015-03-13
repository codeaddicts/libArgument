using System;

namespace libArgument
{
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
	public class Switch : Argument
	{
		public Switch (string shortname, string fullname) : base (shortname, fullname) {
		}
	}
}


using System;

namespace Codeaddicts.libArgument.Attributes
{
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
	public class Switch : Argument
	{
		public Switch (string shortname, string fullname) : base (shortname, fullname) {
		}
	}
}


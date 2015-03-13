using System;

namespace libArgument
{
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = false)]
	public class CastAs : Attribute
	{
		public CastingType Type;

		public CastAs (CastingType type)
		{
			this.Type = type;
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Codeaddicts.libArgument.Attributes;

namespace Codeaddicts.libArgument
{
	public static class ArgumentParser
	{
		public static T Parse<T> (string[] args) where T : class, new()
		{
			T options = new T ();
			var list = new List<string> (args);
			var fields = typeof (T).GetFields (BindingFlags.Instance | BindingFlags.Public);
			foreach (var item in fields)
				ParseField<T> (options, list, item.Name);
			return options;
		}

		static void ParseField<T> (T options, List<string> args, string name) where T : class, new()
		{
			var field = typeof(T).GetField (name);
			var attributes = field.GetCustomAttributes (false);
			var enumerable = attributes as object[] ?? attributes.ToArray ();
			var cast = enumerable.First (attrib => attrib as CastAs != null) as CastAs ?? new CastAs (CastingType.String);
			foreach (var attrib in enumerable) {
				if (attrib is Switch) {
					field.SetValue (options, true);
					return;
				}
				if (!(attrib is Argument && attrib as Argument != null))
					continue;
				var attribute = attrib as Argument;
				if (!args.Contains (attribute.FriendlyShort) || args.Contains (attribute.FriendlyFull))
					continue;
				var str = args.Contains (attribute.FriendlyShort) ? attribute.FriendlyShort : attribute.FriendlyFull;
				var index = 1 + args.IndexOf (str);
				var indexInRange = index <= args.Count - 1;
				if (cast.Type != CastingType.Boolean && !indexInRange)
					throw new ArgumentOutOfRangeException (string.Format ("Parameter of argument {0} out of range.", str));
				switch (cast.Type) {
				case CastingType.Boolean:
					var bool_result = false;
					if (!Boolean.TryParse (args [index], out bool_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to Boolean.", str));
					field.SetValue (options, bool_result);
					return;
				case CastingType.Object:
					field.SetValue (options, args [index]);
					return;
				case CastingType.Int32:
					var int32_result = 0;
					if (!Int32.TryParse (args [index], out int32_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to Int32.", str));
					field.SetValue (options, int32_result);
					return;
				case CastingType.Int64:
					var int64_result = 0L;
					if (!Int64.TryParse (args [index], out int64_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to Int64.", str));
					field.SetValue (options, int64_result);
					return;
				case CastingType.UInt32:
					var uint32_result = 0u;
					if (!UInt32.TryParse (args [index], out uint32_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to UInt32.", str));
					field.SetValue (options, uint32_result);
					return;
				case CastingType.UInt64:
					var uint64_result = 0uL;
					if (!UInt64.TryParse (args [index], out uint64_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to UInt64.", str));
					field.SetValue (options, uint64_result);
					return;
				case CastingType.Long:
					var long_result = 0L;
					if (!long.TryParse (args [index], out long_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to long.", str));
					field.SetValue (options, long_result);
					return;
				case CastingType.ULong:
					var ulong_result = 0uL;
					if (!ulong.TryParse (args [index], out ulong_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to ulong.", str));
					field.SetValue (options, ulong_result);
					return;
				}
			}
		}
	}
}


using System;
using System.Collections.Generic;
using System.Globalization;
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
			var attributes = field.GetCustomAttributes (true);
			var cast = attributes.FirstOrDefault (attrib => attrib as CastAs != null) as CastAs ?? new CastAs (CastingType.String);
			foreach (var attrib in attributes) {
				if (attrib as Switch != null && (args.Contains ((attrib as Switch).FriendlyShort) || args.Contains ((attrib as Switch).FriendlyFull))) {
					field.SetValue (options, true);
					return;
				}
				if (attrib as Argument == null)
					continue;
				var attribute = attrib as Argument;
				if (!(args.Contains (attribute.FriendlyShort) || args.Contains (attribute.FriendlyFull)))
					continue;
				var str = args.Contains (attribute.FriendlyShort) ? attribute.FriendlyShort : attribute.FriendlyFull;
				var index = args.IndexOf (str) + 1;
				var indexInRange = index <= args.Count - 1;
				if (cast.Type != CastingType.Boolean && !indexInRange)
					throw new ArgumentOutOfRangeException (string.Format ("Parameter of argument {0} out of range.", str));
				switch (cast.Type) {
				case CastingType.String:
					field.SetValue (options, args [index]);
					return;
				case CastingType.Boolean:
					bool bool_result;
					if (!Boolean.TryParse (args [index], out bool_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to Boolean.", str));
					field.SetValue (options, bool_result);
					return;
				case CastingType.Object:
					field.SetValue (options, args [index]);
					return;
				case CastingType.Int32:
					int int32_result;
					if (!Int32.TryParse (args [index], out int32_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to Int32.", str));
					field.SetValue (options, int32_result);
					return;
				case CastingType.Int64:
					long int64_result;
					if (!Int64.TryParse (args [index], out int64_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to Int64.", str));
					field.SetValue (options, int64_result);
					return;
				case CastingType.UInt32:
					uint uint32_result;
					if (!UInt32.TryParse (args [index], out uint32_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to UInt32.", str));
					field.SetValue (options, uint32_result);
					return;
				case CastingType.UInt64:
					ulong uint64_result;
					if (!UInt64.TryParse (args [index], out uint64_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to UInt64.", str));
					field.SetValue (options, uint64_result);
					return;
				case CastingType.Long:
					long long_result;
					if (!long.TryParse (args [index], out long_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to long.", str));
					field.SetValue (options, long_result);
					return;
				case CastingType.ULong:
					ulong ulong_result;
					if (!ulong.TryParse (args [index], out ulong_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to ulong.", str));
					field.SetValue (options, ulong_result);
					return;
				case CastingType.Float:
					float float_result;
					if (!float.TryParse (args [index], NumberStyles.Float, CultureInfo.InvariantCulture, out float_result))
						throw new InvalidCastException (string.Format ("Can't cast parameter of argument {0} to float.", str));
					field.SetValue (options, float_result);
					return;
				}
			}
		}
	}
}


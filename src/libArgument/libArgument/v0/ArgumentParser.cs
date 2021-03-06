﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Codeaddicts.libArgument.v0 {
	
	/// <summary>
	/// Codeaddicts ArgumentParser
	/// </summary>
	internal static class ArgumentParser {
		static List<DocNode> documentation;

		/// <summary>
		/// Parses the specified arguments.
		/// </summary>
		/// <param name="args">Arguments.</param>
		/// <typeparam name="T">The class that contains the options.</typeparam>
		public static T Parse<T> (string[] args) where T : class, new()
		{
			// Create the documentation cache for this class
			if (documentation == null)
				documentation = new List<DocNode> ();
			else
				documentation.Clear ();

			// Instantiate the class that contains the options
			var options = new T ();

			// Create a new List<string> from the supplied arguments
			var list = new List<string> (args);

			// Get all public fields from the class instance
			var fields = options.GetType ().GetFields (BindingFlags.Instance | BindingFlags.Public);

			// Iterate over all fields
			foreach (var item in fields) {

				// Parse the current field
				ParseDoc (options, item.Name);
				ParseField (options, list, item.Name);

			}

			// Return the generated class instance
			return options;
		}

		/// <summary>
		/// Generates and prints the argument doc.
		/// </summary>
		/// <typeparam name="T">The class that contains the options.</typeparam>
		public static void Help () {
            /*
			var accum = new StringBuilder ();
			foreach (var node in documentation) {
				if (node.Names.Any ()) {
					foreach (var name in node.Names) {
						accum.AppendFormat ("{0,-20} | ", name);
						if (!string.IsNullOrEmpty (node.Description))
							accum.AppendLine (node.Description);
						else
							accum.AppendLine ("<No Description>");
					}
				}
			}
			Console.WriteLine (accum);
			*/
		}

		/// <summary>
		/// Parses the field.
		/// </summary>
		/// <param name="options">Options.</param>
		/// <param name="args">Arguments.</param>
		/// <param name="fieldname">Name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		static void ParseField<T> (T options, List<string> args, string fieldname) where T : class, new()
		{
			var switches = args
				.Where (arg => arg.Length > 0 && !arg.StartsWith ("--") && arg [0] == '-')
				.SelectMany (arg => arg.Substring (1)).ToList ();
			
			foreach (var @switch in switches)
				args.Add (string.Format ("-{0}", @switch));

			// Get field
			var field = options.GetType ().GetField (fieldname);

			// Get attributes
			var attributes = field.GetCustomAttributes (true);

			// Iterate over the attributes
			foreach (var attrib in attributes) {

				// Check if the current attribute is of type Switch
				var @switch = attrib as Switch;
				if (@switch != null) {
					@switch.AutoInfer (field.Name);

					var @continue = false;
					foreach (var arg in args) {
						if (@switch.Names.Any (str => arg == str)) {
							field.SetValue (options, true);
							@continue = true;
							break;
						}
					}

					if (@continue)
						continue;
				}

				// Get the current attribute
				var argument = attrib as ArgumentBase;

				// Check if the current attribute is an Argument attribute
				if (argument != null) {
					argument.AutoInfer (field.Name);

					for (var i = 0; i < args.Count; i++) {
						var arg = args [i];
						
						if (argument.Names.Any (str => arg.Contains ("=")
							? (
								true
								&& arg.StartsWith (str)
								&& arg.Length > str.Length
								&& arg[str.Length] == '='
							) : arg == str)) {
							
							var next = arg.Contains ("=")
								? arg.Substring (arg.IndexOf ("=") + 1)
								: args.SkipWhile (str => arg != str).Skip (1).FirstOrDefault ();

							// Verify that next is not null
							if (next == null)
								throw new ArgumentOutOfRangeException ();

							next = next.Trim (' ', '\t', '"');

							// Cast the string to the type of the field
							object value;
							if (!TryGetGenericWithArray<T> (options, field, next, out value))
								throw new ArgumentException (string.Format ("Cannot convert '{0}' to <{1}>.", next, field.FieldType));

							// Set the value of the field
							field.SetValue (options, value);

							// Correct args
							if (field.FieldType.IsArray) {
								args.RemoveRange (i, 2);
								i--;
							}

							continue;
						}
					}
				}
			}
		}

		static bool TryGetGeneric (Type type, string str, out object value) {
			if (TryGetPrimitive (type, str, out value))
				return true;
			if (TryGetEnum (type, str, out value))
				return true;
			return false;
		}

		static bool TryGetGenericWithArray<T> (T options, FieldInfo field, string str, out object value) {
			if (TryGetGeneric (field.FieldType, str, out value))
				return true;
			if (TryGetArray (options, field, str, out value))
				return true;
			return false;
		}

		static bool TryGetPrimitive (Type type, string str, out object value) {
			value = null;
			if (!type.IsPrimitive && !type.IsEquivalentTo (typeof (string)))
				return false;
			try {
				value = TypeDescriptor.GetConverter (type).ConvertFromInvariantString (str);
			} catch {
				return false;
			}
			return true;
		}

		static bool TryGetEnum (Type type, string str, out object value) {
			value = null;
			if (!type.IsEnum)
				return false;
			try {
				value = Enum.Parse (type, str, true);
			} catch {
				return false;
			}
			return true;
		}

		static bool TryGetArray<T> (T options, FieldInfo field, string str, out object value) {
			value = null;
			if (!field.FieldType.IsArray)
				return false;
			try {
				var elementtype = field.FieldType.GetElementType ();
				object tmpval;
				if (!TryGetGeneric (elementtype, str, out tmpval))
					return false;
				Console.WriteLine (tmpval);
				var arr = field.GetValue (options) as Array;
				Array newarr;
				if (arr == null) {
					newarr = Array.CreateInstance (elementtype, 1);
					newarr.SetValue (tmpval, 0);
				} else {
					newarr = Array.CreateInstance (elementtype, arr.Length + 1);
					Array.Copy (arr, 0, newarr, 0, arr.Length);
					newarr.SetValue (tmpval, arr.Length);
				}
				value = newarr;
			} catch {
				return false;
			}
			return true;
		}

		static void ParseDoc<T> (T options, string fieldname) where T : class, new()
		{
			// Get field
			var field = options.GetType ().GetField (fieldname);

			// Get or create a documentation node for this field
			var node = documentation.Exists (_node => _node.FieldName == fieldname)
				? documentation.First (_node => _node.FieldName == fieldname)
				: new DocNode (fieldname);

			// Get attributes
			var attributes = field.GetCustomAttributes (true);

			// Iterate over all attributes
			foreach (var attrib in attributes) {

				// Check if the current attribute is of type Argument or Switch
				var xarg = attrib as ArgumentBase;
				if (xarg != null) {
					xarg.AutoInfer (field.Name);
					//node.Names = xarg.Names;
				}

				// Check if the current attribute is of type Docs
				//var doc = attrib as Docs;

				// If not, skip this attribute
				//if (doc == null)
				//	continue;

				// Return the documentation for this field
				//node.Description = doc.Description;
			}

			if (documentation.Exists (_node => _node.FieldName == fieldname))
				documentation.Remove (documentation.First (_node => _node.FieldName == fieldname));
			
			documentation.Add (node);
		}
	}
}

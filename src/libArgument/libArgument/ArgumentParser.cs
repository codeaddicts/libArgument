using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Codeaddicts.libArgument.Attributes;
using Codeaddicts.libArgument.Documentation;

namespace Codeaddicts.libArgument
{
	/// <summary>
	/// Codeaddicts ArgumentParser
	/// </summary>
	public static class ArgumentParser
	{
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
				ParseDoc<T> (options, item.Name);
				ParseField<T> (options, list, item.Name);

			}

			// Return the generated class instance
			return options;
		}

		/// <summary>
		/// Generates and prints the argument doc.
		/// </summary>
		/// <typeparam name="T">The class that contains the options.</typeparam>
		public static void Help ()
		{
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
						if (@switch.names.Any (str => arg == str)) {
							field.SetValue (options, true);
							@continue = true;
							break;
						}
					}

					if (@continue)
						continue;
				}

				// Get the current attribute
				var argument = attrib as Argument;

				// Check if the current attribute is an Argument attribute
				if (argument != null) {
					argument.AutoInfer (field.Name);

					foreach (var arg in args) {
						
						if (argument.names.Any (str => arg.Contains ("=") ? arg.StartsWith (str) && arg.Length > str.Length && arg[str.Length] == '=' : arg == str)) {
							
							var next = arg.Contains ("=")
								? arg.Substring (arg.IndexOf ("=") + 1)
								: args.SkipWhile (str => arg != str).Skip (1).FirstOrDefault ();
							
							if (next == null)
								throw new ArgumentOutOfRangeException ();

							// Cast the string to the type of the field
							object value;
							try {
								value = TypeDescriptor.GetConverter (field.FieldType).ConvertFromInvariantString (next.Trim ('"'));
							} catch {
								throw new Exception (string.Format ("Cannot convert <string> to <{0}>.", field.FieldType));
							}

							// Set the value of the field and return
							field.SetValue (options, value);
							continue;
						}
					}
				}
			}
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
				var xarg = attrib as Argument;
				if (xarg != null) {
					xarg.AutoInfer (field.Name);
					node.Names = xarg.names;
				}

				// Check if the current attribute is of type Docs
				var doc = attrib as Docs;

				// If not, skip this attribute
				if (doc == null)
					continue;

				// Return the documentation for this field
				node.Description = doc.description;
			}

			if (documentation.Exists (_node => _node.FieldName == fieldname))
				documentation.Remove (documentation.First (_node => _node.FieldName == fieldname));
			
			documentation.Add (node);
		}
	}
}

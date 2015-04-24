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
			T options = new T ();

			// Create a new List<string> from the supplied arguments
			var list = new List<string> (args);

			// Get all public fields from the class instance
			var fields = typeof (T).GetFields (BindingFlags.Instance | BindingFlags.Public);

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
			documentation.Sort ((n1, n2) => {
				if (!string.IsNullOrEmpty (n1.ShortName) && !string.IsNullOrEmpty (n2.ShortName))
					return n1.ShortName.CompareTo (n2.ShortName);
				if (string.IsNullOrEmpty (n1.FullName) && string.IsNullOrEmpty (n2.ShortName))
					return n1.ShortName.CompareTo (n2.FullName);
				if (!string.IsNullOrEmpty (n1.FullName) && !string.IsNullOrEmpty (n2.FullName))
					return n1.FullName.CompareTo (n2.FullName);
				if (string.IsNullOrEmpty (n1.ShortName) && string.IsNullOrEmpty (n2.FullName))
					return n1.FullName.CompareTo (n2.ShortName);
				return n1.FieldName.CompareTo (n2.FieldName);
			});
			var accum = new StringBuilder ();
			foreach (var node in documentation) {
				if (!string.IsNullOrEmpty (node.ShortName))
					accum.AppendFormat ("{0}", node.ShortName.PadRight (Console.WindowWidth / 10) + "| ");
				if (!string.IsNullOrEmpty (node.ShortName) && !string.IsNullOrEmpty (node.FullName))
					accum.AppendFormat ("\n{0}", node.FullName.PadRight (Console.WindowWidth / 10) + "| ");
				else if (string.IsNullOrEmpty (node.ShortName) && !string.IsNullOrEmpty (node.FullName))
					accum.Append (node.FullName.PadRight (Console.WindowWidth / 10) + "| ");
				else if (string.IsNullOrEmpty (node.ShortName) && string.IsNullOrEmpty (node.FullName))
					accum.Append (node.FieldName.PadRight (Console.WindowWidth / 10) + "| ");
				if (!string.IsNullOrEmpty (node.Description))
					accum.AppendLine (node.Description);
				else
					accum.AppendLine ("<No Description>");
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
			// Get field
			var field = typeof(T).GetField (fieldname);

			// Get attributes
			var attributes = field.GetCustomAttributes (true);

			// Iterate over the attributes
			foreach (var attrib in attributes) {

				// Check if the current attribute is of type Switch
				var @switch = attrib as Switch;
				if (@switch != null) {

					// Check if infername is set to true
					if (@switch.infername)

						// Instantiate a new Switch with the name of the field
						@switch = @switch.InferName (field.Name);

					// Check if the current attribute contains a valid parameter name
					if (args.Contains (@switch.FriendlyShort) || args.Contains (@switch.FriendlyFull)) {

						// Set value and return
						field.SetValue (options, true);
						return;
					}
				}

				// Get the current attribute
				var attribute = attrib as Argument;

				// Check if the current attribute is an Argument attribute
				if (attribute == null)

					// Skip this iteration
					continue;

				// Check if infername is set to true
				if (attribute.infername)

					// Instantiate a new Attribute with the name of the field
					attribute = attribute.InferName (field.Name);

				// Check if the arguments contain any of the valid parameter names
				if (!(args.Contains (attribute.FriendlyShort) || args.Contains (attribute.FriendlyFull)))

					// Skip this iteration
					continue;

				// Get the parameter name
				var paramname = args.Contains (attribute.FriendlyShort) ? attribute.FriendlyShort : attribute.FriendlyFull;

				// Get the type of the field
				var type = field.FieldType;

				// Get the index of the current value
				var index = args.IndexOf (paramname) + 1;

				// Check if the index is in the range of our parameter count
				var indexInRange = index < args.Count;

				// Check if the index is in range of our parameter count
				if (!indexInRange)
					throw new ArgumentOutOfRangeException (string.Format ("Parameter of argument {0} is out of range.", paramname));

				// Cast the string to the type of the field
				object value;
				try {
					value = TypeDescriptor.GetConverter (type).ConvertFromInvariantString (args [index]);
				} catch {
					throw new Exception (string.Format ("Cannot convert <string> to <{0}>.", type));
				}

				// Set the value of the field and return
				field.SetValue (options, value);
				return;
			}
		}

		static void ParseDoc<T> (T options, string fieldname) where T : class, new()
		{
			// Get field
			var field = typeof(T).GetField (fieldname);

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
				var xswitch = attrib as Switch;
				if (xswitch != null) {
					if (xswitch.infername)
						xswitch = xswitch.InferName (field.Name);
					node.ShortName = xswitch.FriendlyShort;
					node.FullName = xswitch.FriendlyFull;
				} else if (xarg != null) {
					if (xarg.infername)
						xarg = xarg.InferName (field.Name);
					node.ShortName = xarg.FriendlyShort;
					node.FullName = xarg.FriendlyFull;
				}

				// Check if the current attribute is of type Docs
				var doc = attrib as Docs;

				// If not, skip this attribute
				if (doc == null)
					continue;

				// Return the documentation for this field
				node.Description = doc.description;
			}

			if (documentation.Exists (_node => _node.FieldName == fieldname)) {
				documentation.Remove (documentation.First (_node => _node.FieldName == fieldname));
			}
			documentation.Add (node);
		}
	}
}

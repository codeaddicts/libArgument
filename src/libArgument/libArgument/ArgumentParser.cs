using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Codeaddicts.libArgument.Attributes;
using System.ComponentModel;

namespace Codeaddicts.libArgument
{
	/// <summary>
	/// Codeaddicts ArgumentParser
	/// </summary>
	public static class ArgumentParser
	{
		/// <summary>
		/// Parses the specified arguments.
		/// </summary>
		/// <param name="args">Arguments.</param>
		/// <typeparam name="T">The class that contains the options.</typeparam>
		public static T Parse<T> (string[] args) where T : class, new()
		{
			// Instantiate the class that contains the options
			T options = new T ();

			// Create a new List<string> and fill it with the arguments
			var list = new List<string> (args);

			// Get all public fields from the class instance
			var fields = typeof (T).GetFields (BindingFlags.Instance | BindingFlags.Public);

			// Iterate over all fields
			foreach (var item in fields)

				// Parse the current field
				ParseField<T> (options, list, item.Name);

			// Return the generated class instance
			return options;
		}

		/// <summary>
		/// Generates and prints the argument doc.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		static void PrintHelp<T> () where T : class, new()
		{
			// Not ready yet.
		}

		/// <summary>
		/// Parses the field.
		/// </summary>
		/// <param name="options">Options.</param>
		/// <param name="args">Arguments.</param>
		/// <param name="name">Name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		static void ParseField<T> (T options, List<string> args, string name) where T : class, new()
		{
			// Get field
			var field = typeof(T).GetField (name);

			// Get attributes
			var attributes = field.GetCustomAttributes (true);

			// Iterate over the attributes
			foreach (var attrib in attributes) {

				// Check if the current attribute is a Switch attribute
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
	}
}


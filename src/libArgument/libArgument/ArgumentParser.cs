using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Argument parser.
    /// </summary>
    /// <typeparam name="T">The target class.</typeparam>
    public static class ArgumentParser<T> where T: class, new() {

        /// <summary>
        /// The cache.
        /// </summary>
        static APCache<T> cache;

        /// <summary>
        /// Parses the specified arguments.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static T Parse (string [] args) {

            // Create the cache
            cache = new APCache<T> (args);

            // Iterate over the fields
            var i = 0;
            while (cache.See ()) {
                var current = cache.Fields [i];
                ParseField (i++, current);
            }

            // Return the modified instance
            return cache.Target;
        }

        /// <summary>
        /// Parses a field.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="field">Field.</param>
        static void ParseField (int position, FieldInfo field) {

            // Get the attributes of the field
            var attrs = field.GetCustomAttributes (true);

            // Iterate over the attributes
            foreach (var attr in attrs) {
                var arg = attr as ArgumentBase;

                // Skip the attribute if it does not
                // inherit the argument base class
                if (arg == null) {
                    continue;
                }

                // Parse the field
                arg.AutoInfer (field.Name);
                while (cache.See ()) {
                    ParseField (position, field, arg);
                }
                cache.Flush ();
            }
        }

        /// <summary>
        /// Skips an attribute and throws an exception
        /// if the attribute was required.
        /// </summary>
        /// <param name="attr">Attr.</param>
        static void Skip (ArgumentBase attr) {
            if (attr.IsRequired) {
                var args = string.Join ("|", attr.Names);
                throw new ArgumentException ($"Required argument '{args}' not set.");;
            }
            cache.SkipArgument ();
        }

        /// <summary>
        /// Parses a field.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="position">Position.</param>
        /// <param name="field">Field.</param>
        /// <param name="attr">Attr.</param>
        static void ParseField (int position, FieldInfo field, ArgumentBase attr) {
            string current = cache.PeekArgument ();
            string next = current;

            // Check if the argument is a switch
            if (attr is Switch) {

                // Skip the field if the name of the argument
                // is not to be found in the name list of the attribute
                if (!attr.Names.Contains (current)) {
                    cache.SkipArgument ();
                    return;
                }

                // Set the field value to true
                try {
                    field.SetValue (cache.Target, true);
                } catch { }
                cache.RemoveArgument ();
                cache.Finish ();
                return;
            }

            // Check if the argument is positional
            if (attr.IsPositional) {

                // Check if the last position is requested
                if (attr.Position == -1) {
                    if (cache.See (1)) {
                        Skip (attr);
                        return;
                    }
                } else if (attr.Position != cache.ActualPosition) {
                    Skip (attr);
                    return;
                }
            }

            // Console.WriteLine (current);

            // Check if the argument is in the following form:
            // [argument]=[value]
            var GnuStyle = current.Contains ("=");

            // Check if the attribute is positional
            if (!attr.IsPositional) {

                // Check if the argument is a GNU-style argument
                if (GnuStyle) {
                    var parts = current.Split (new char [] { '=' }, 2);
                    current = parts [0];
                    next = parts [1];
                }

                // Skip the field if the name of the argument
                // is not to be found in the name list of the attribute
                if (!attr.Names.Contains (current)) {
                    Skip (attr);
                    return;
                }

                // Get the next argument
                if (!GnuStyle) {
                    next = cache.PeekArgument (1);
                }
            }

            object result;
            var i = 0;
            do {

                // Try getting the value
                if (!TryGetGeneric (field.FieldType, field, next, out result)) {
                    throw new ArgumentException (
                        $"Cannot convert '{next}' to <{field.FieldType}>."
                    );
                }

                // Set the value of the field
                try {
                    field.SetValue (cache.Target, result);
                } catch { }

                // Skip the remaining arguments
                try {
                    if ((i == 0 || !attr.IsVariadic) && !attr.IsPositional && !GnuStyle) {
                        cache.RemoveArgument ();
                    }
                    cache.RemoveArgument ();
                    if (attr.IsPositional)
                        break;
                } catch { }

                // Check if the argument is variadic
                if (attr.IsVariadic) {

                    // Skip if there are no more elements
                    if (!cache.See ()) {
                        break;
                    }
                    try {

                        // Get the next argument
                        next = cache.PeekArgument ();

                        // Check if the next argument is an actual argument
                        if (next.StartsWith ("-", StringComparison.Ordinal)) {
                            break;
                        }
                    } catch {
                        break;
                    }
                }

                i++;
            } while (attr.IsVariadic);

            // Increment the actual position by one
            cache.Increment ();
        }

        static bool TryGetGeneric (Type type, FieldInfo field, string arg, out object result) {
            result = null;

            // Check if the field is a string
            if (type.IsEquivalentTo (typeof (string))) {
                result = arg;
                return true;
            }

            // Check if the field is a primitive
            if (type.IsPrimitive) {

                // Check if the field is a string
                if (type.IsEquivalentTo (typeof (string))) {
                    result = arg;
                    return true;
                }

                try {
                    var converter = TypeDescriptor.GetConverter (type);
                    result = converter.ConvertFromInvariantString (arg);
                    return true;
                } catch { }
            }

            // Check if the field is an enum
            if (type.IsEnum) {
                try {
                    result = Enum.Parse (type, arg, true);
                    return true;
                } catch { }
            }

            // Check if the field is an array
            if (type.IsArray) {
                try {
                    var elementtype = type.GetElementType ();
                    object tmpval;
                    if (!TryGetGeneric (elementtype, field, arg, out tmpval))
                        return false;
                    var arr = field.GetValue (cache.Target) as Array;
                    Array newarr;
                    if (arr == null) {
                        newarr = Array.CreateInstance (elementtype, 1);
                        newarr.SetValue (tmpval, 0);
                    } else {
                        newarr = Array.CreateInstance (elementtype, arr.Length + 1);
                        Array.Copy (arr, 0, newarr, 0, arr.Length);
                        newarr.SetValue (tmpval, arr.Length);
                    }
                    result = newarr;
                    return true;
                } catch { }
            }

            return false;
        }
    }
}


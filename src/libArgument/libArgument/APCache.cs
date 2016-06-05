using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Argument Parser Cache.
    /// </summary>
    class APCache<TClass> where TClass: class, new() {

        /// <summary>
        /// The target class.
        /// </summary>
        public readonly TClass Target;

        /// <summary>
        /// The fields.
        /// </summary>
        public readonly FieldInfo [] Fields;

        /// <summary>
        /// The arguments.
        /// </summary>
        public readonly List<string> Arguments;

        /// <summary>
        /// The argument position.
        /// </summary>
        int argumentPosition;

        /// <summary>
        /// The actual position.
        /// </summary>
        int actualPosition;

        /// <summary>
        /// Gets the actual position.
        /// </summary>
        /// <value>The actual position.</value>
        public int ActualPosition => actualPosition;

        public APCache (IEnumerable<string> args) {
            Target = new TClass ();
            Fields = Target.GetType ().GetFields (
                BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.FlattenHierarchy
            );
            Arguments = args.ToList ();
        }

        public bool See (int n = 0) {
            return Arguments.Count > argumentPosition + n;
        }

        public string PeekArgument (int n = 0) {
            return Arguments [argumentPosition + n];
        }

        public void RemoveArgument () {
            Arguments.RemoveAt (argumentPosition);
        }

        public void SkipArgument () {
            ++argumentPosition;
        }

        public void Flush () {
            argumentPosition = 0;
        }

        public void Finish () {
            argumentPosition = Arguments.Count;
        }

        public void Increment () {
            ++actualPosition;
        }
    }
}


using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Variadic argument.
    /// </summary>
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
    public class ArgumentList : ArgumentBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentList"/> class.
        /// </summary>
        public ArgumentList () {
            isVariadic = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentList"/> class.
        /// </summary>
        /// <param name="names">Names.</param>
        public ArgumentList (params string [] names)
            : base (names) {
            isVariadic = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentList"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="names">Names.</param>
        public ArgumentList (ArgumentPosition position, params string [] names)
            : base (position, names) {
            isVariadic = true;
        }
    }
}


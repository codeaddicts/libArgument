using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Argument.
    /// </summary>
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
    public class Argument : ArgumentBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class.
        /// </summary>
        public Argument () { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="names">Names.</param>
        public Argument (params string [] names)
            : base (names) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="names">Names.</param>
        public Argument (ArgumentPosition position, params string [] names)
            : base (position, names) { }
    }
}


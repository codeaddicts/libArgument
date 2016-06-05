using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Switch.
    /// </summary>
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
	public class Switch : ArgumentBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// </summary>
		public Switch () { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// </summary>
        /// <param name="names">Names.</param>
		public Switch (params string[] names)
            : base (names) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="names">Names.</param>
        public Switch (ArgumentPosition position, params string [] names)
            : base (position, names) { }
	}
}


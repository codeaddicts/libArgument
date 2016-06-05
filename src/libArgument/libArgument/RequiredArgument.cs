using System;
namespace Codeaddicts.libArgument {

    /// <summary>
    /// Required argument.
    /// </summary>
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
    public class RequiredArgument : ArgumentBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredArgument"/> class.
        /// </summary>
        public RequiredArgument () {
            isRequired = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredArgument"/> class.
        /// </summary>
        /// <param name="names">Names.</param>
        public RequiredArgument (params string [] names)
            : base (names) {
            isRequired = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredArgument"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="names">Names.</param>
        public RequiredArgument (ArgumentPosition position, params string [] names)
            : base (position, names) {
            isRequired = true;
        }
    }
}


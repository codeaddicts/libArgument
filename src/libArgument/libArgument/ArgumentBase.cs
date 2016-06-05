using System;
using System.Linq;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Argument.
    /// </summary>
	[AttributeUsage (AttributeTargets.Field, AllowMultiple = true)]
	public abstract class ArgumentBase : Attribute {

        /// <summary>
        /// The position.
        /// </summary>
        int position;

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public int Position => position;

        /// <summary>
        /// The names.
        /// </summary>
		string [] names;

        /// <summary>
        /// Gets the names.
        /// </summary>
        /// <value>The names.</value>
        public string [] Names => names;

        /// <summary>
        /// A value indicating whether the argument
        /// is positional or not.
        /// </summary>
        readonly bool isPositional;

        /// <summary>
        /// Gets a value indicating whether the argument
        /// is positional or not.
        /// </summary>
        /// <value>The value.</value>
        public bool IsPositional => isPositional;

        /// <summary>
        /// A value indicating whether the argument
        /// is required or not.
        /// </summary>
        protected bool isRequired;

        /// <summary>
        /// Gets a value indicating whether the argument
        /// is required or not.
        /// </summary>
        /// <value>The value.</value>
        public bool IsRequired => isRequired;

        /// <summary>
        /// A value indicating whether the argument
        /// is variadic or not.
        /// </summary>
        protected bool isVariadic;

        /// <summary>
        /// Gets a value indicating whether the argument
        /// is variadic or not.
        /// </summary>
        /// <value>The value.</value>
        public bool IsVariadic => isVariadic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentBase"/> class.
        /// </summary>
		protected ArgumentBase () { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentBase"/> class.
        /// </summary>
        /// <param name="names">Names.</param>
		protected ArgumentBase (params string [] names) {
			this.names = names;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentBase"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="names">Names.</param>
        protected ArgumentBase (ArgumentPosition position, params string [] names) {
            this.names = names;
            switch (position) {
            case ArgumentPosition.Last:
                this.position = -1;
                break;
            }
            isPositional = true;
        }

        /// <summary>
        /// Infers the argument name from the name of the field
        /// if no argument names are supplied.
        /// </summary>
        /// <param name="fieldname">The name of the field.</param>
		public void AutoInfer (string fieldname) {
            if (isPositional)
                return;
			if (names == null || !names.Any ())
				names = new [] { string.Format ("--{0}", fieldname) };
		}
	}
}


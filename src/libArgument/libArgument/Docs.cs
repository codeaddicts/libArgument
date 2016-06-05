using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Documentation.
    /// </summary>
	[AttributeUsage (AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false)]
	public class Docs : Attribute {

        /// <summary>
        /// The description.
        /// </summary>
		public readonly string Description;

		public Docs (string description) {
			Description = description;
		}
	}
}


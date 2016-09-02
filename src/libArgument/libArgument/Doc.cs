using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Documentation.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class Doc : DocBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="Doc"/> class.
        /// </summary>
        /// <param name="description">Description.</param>
        public Doc (string description)
            : base (description) { }
    }
}


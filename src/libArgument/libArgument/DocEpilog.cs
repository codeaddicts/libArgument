using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Documentation: Epilog.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class DocEpilog : DocBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="DocEpilog"/> class.
        /// </summary>
        /// <param name="epilog">Epilog.</param>
        public DocEpilog (string epilog)
            : base (epilog) { }
    }
}


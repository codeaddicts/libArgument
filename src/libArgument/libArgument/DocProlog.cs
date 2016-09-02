using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Documentation: Prolog.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class DocProlog : DocBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="DocProlog"/> class.
        /// </summary>
        /// <param name="prolog">Prolog.</param>
        public DocProlog (string prolog)
            : base (prolog) { }
    }
}


using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Documentation: Version.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class DocVersion : DocBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="DocVersion"/> class.
        /// </summary>
        /// <param name="version">Version.</param>
        public DocVersion (string version)
            : base (version) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocVersion"/> class.
        /// </summary>
        /// <param name="version">Version.</param>
        public DocVersion (Version version)
            : base (version.ToString ()) { }
    }
}


using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Documentation base.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class DocBase : Argument {

        /// <summary>
        /// The content.
        /// </summary>
        public readonly string Content;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocBase"/> class.
        /// </summary>
        /// <param name="content">Content.</param>
        public DocBase (string content) {
            Content = content;
        }
    }
}


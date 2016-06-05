using System;

namespace Codeaddicts.libArgument {

    /// <summary>
    /// Documentation node.
    /// </summary>
	class DocNode {

        /// <summary>
        /// The name of the field.
        /// </summary>
		public readonly string FieldName;

        /// <summary>
        /// The names.
        /// </summary>
		public string[] Names;

        /// <summary>
        /// The description.
        /// </summary>
		public string Description;

		public DocNode (string fieldName, string[] names = null, string description = "") {
			FieldName = fieldName;
			Description = description;
			Names = names;
		}
	}
}


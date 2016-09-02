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
        /// The description.
        /// </summary>
		public string Description;

		public DocNode (string fieldName, string description = "") {
			FieldName = fieldName;
			Description = description;
		}
	}
}


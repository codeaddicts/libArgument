using System;

namespace Codeaddicts.libArgument {

	class DocNode {
		public readonly string FieldName;
		public string[] Names;
		public string Description;

		public DocNode (string fieldName, string[] names = null, string description = "") {
			FieldName = fieldName;
			Description = description;
			Names = names;
		}
	}
}


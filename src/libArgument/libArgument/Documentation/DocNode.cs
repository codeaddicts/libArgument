using System;

namespace Codeaddicts.libArgument.Documentation
{

	public class DocNode
	{
		public readonly string FieldName;
		public string FullName;
		public string ShortName;
		public string Description;

		public DocNode (
			string fieldName,
			string fullName = "",
			string shortName = "",
			string description = "") {
			FieldName = fieldName;
			FullName = fullName;
			ShortName = shortName;
			Description = description;
		}
	}
}


using System;
using NUnit.Framework;

namespace Codeaddicts.libArgument.Tests {
	[TestFixture]
	public class EnumTests {

		public enum TestEnum {
			none,
			aa,
			bb,
			cc
		}

		public class Options {

			[Argument ("--enum")]
			public TestEnum Enumerable;
		}

		[Test]
		public void TestEnumConversion () {
			var args = new [] { "--enum", "bb" };
			Assert.AreEqual (TestEnum.bb, ArgumentParser<Options>.Parse (args).Enumerable);
		}
	}
}


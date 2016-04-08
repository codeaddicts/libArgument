using System;
using NUnit.Framework;

namespace Codeaddicts.libArgument.Tests {

	[TestFixture]
	public class ArrayTests {
		
		public class Options {

			[Argument ("--libs")]
			public string[] libs;
		}

		[Test]
		public void TestEnumConversion () {
			var args = new [] { "--libs", "1.so", "--libs", "2.so", "--libs", "3.so" };
			Assert.That (ArgumentParser.Parse<Options> (args).libs, Is.EquivalentTo (new [] { "1.so", "2.so", "3.so" }));
		}
	}
}


using System;
using NUnit.Framework;

namespace Codeaddicts.libArgument.Tests {

	[TestFixture]
	public class ArrayTests {
		
		public class Options {

			[ArgumentList ("--libs")]
			public string[] libs;
		}

		[Test]
        public void TestArgumentList () {
			var args = new [] { "--libs", "1.so", "2.so", "3.so" };
			Assert.That (ArgumentParser<Options>.Parse (args).libs, Is.EquivalentTo (new [] { "1.so", "2.so", "3.so" }));
		}
	}
}


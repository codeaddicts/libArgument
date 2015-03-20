using System;
using Codeaddicts.libArgument;
using Codeaddicts.libArgument.Attributes;
using NUnit.Framework;

namespace Codeaddicts.libArgument.Tests
{
	[TestFixture]
	public class NewFeatureTests
	{
		public class Options {
			[Argument]
			public string msg;

			[Argument]
			public int num;
		}

		[Test]
		public void TestInferName () {
			var args = new string[] { "--msg", "Test", "--num", "1234" };
			var options = ArgumentParser.Parse<Options> (args);
			StringAssert.AreEqualIgnoringCase ("Test", options.msg);
			Assert.AreEqual (1234, options.num);
		}
	}
}


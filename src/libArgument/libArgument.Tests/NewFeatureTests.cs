using System;
using Codeaddicts.libArgument;
using Codeaddicts.libArgument.Attributes;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Codeaddicts.libArgument.Tests
{
	[TestFixture]
	public class NewFeatureTests
	{
		public class Options {
			[Argument]
			[Docs ("A message")]
			public string msg;

			[Argument]
			[Docs ("A number")]
			public int num;

			[Switch]
			[Docs ("A something")]
			public bool something;
		}

		[Test]
		public void TestInferName () {
			var args = new [] { "--msg", "Test", "--num", "1234", "--something" };
			var options = ArgumentParser.Parse<Options> (args);
			StringAssert.AreEqualIgnoringCase ("Test", options.msg);
			Assert.AreEqual (1234, options.num);
			Assert.That (options.something);
		}

		[Test]
		public void TestDocumentation () {
			var options = ArgumentParser.Parse<Options> (new string[] { });
			ArgumentParser.Help ();
			Assert.Pass ();
		}
	}
}


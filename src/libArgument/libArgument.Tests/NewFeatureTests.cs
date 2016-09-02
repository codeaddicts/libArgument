using System;
using Codeaddicts.libArgument;
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

			[Switch]
			public bool something;

			[Switch ("-a")] public bool a;
			[Switch ("-b")] public bool b;
			[Switch ("-c")] public bool c;

            [Argument (ArgumentPosition.Last)]
            public string Filename;
		}

		[Test]
		public void TestInferName () {
			var args = new [] { "--msg", "test", "--num", "1234", "--something" };
			var options = ArgumentParser<Options>.Parse (args);
			StringAssert.AreEqualIgnoringCase ("test", options.msg);
			Assert.AreEqual (1234, options.num);
			Assert.That (options.something);
		}

		[Test]
		public void TestGNUStyleArguments () {
			var options = ArgumentParser<Options>.Parse (new [] { "--msg=test", "--num=1234", "--something" });
			StringAssert.AreEqualIgnoringCase ("test", options.msg);
			Assert.AreEqual (1234, options.num);
			Assert.That (options.something);
		}

		[Test]
		public void TestPOSIXStyleSwitches () {
            Assert.Ignore ("Not supported as of v1.0");
			var options = ArgumentParser<Options>.Parse (new [] { "-abc" });
			Assert.That (options.a);
			Assert.That (options.b);
			Assert.That (options.c);
		}

        [Test]
        public void TestPositionalArgumentsLast () {
            var options = ArgumentParser<Options>.Parse (new [] { "--msg=test", "hello" });
            StringAssert.AreEqualIgnoringCase ("hello", options.Filename);
            StringAssert.AreEqualIgnoringCase ("test", options.msg);
        }
	}
}


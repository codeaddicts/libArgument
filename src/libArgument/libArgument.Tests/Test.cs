using System;
using Codeaddicts.libArgument;
using Codeaddicts.libArgument.Attributes;
using NUnit.Framework;

namespace Codeaddicts.libArgument.Tests
{
	public static class Tests
	{
		[TestFixture]
		public class GeneralTests
		{
			public class Options {
				[Argument ("", "msg")]
				public string Msg;

				[Switch ("", "enable-log")]
				public bool Log;

				[Argument ("n", "num")]
				[CastAs (CastingType.Int32)]
				public int ANumber;

				[Argument ("f", "float")]
				[CastAs (CastingType.Float)]
				public float AFloat;
			}

			[Test]
			public void TestArgument () {
				var args = new [] { "--msg", "Hello, World!" };
				StringAssert.AreEqualIgnoringCase ("Hello, World!", ArgumentParser.Parse<Options> (args).Msg);
			}

			[Test]
			public void TestEmptyArgument () {
				var args = new [] { "--msg" };
				Assert.Throws (typeof(ArgumentOutOfRangeException), () => ArgumentParser.Parse<Options> (args));
			}

			[Test]
			public void TestSwitch () {
				var args = new [] { "--enable-log" };
				Assert.AreEqual (true, ArgumentParser.Parse<Options> (args).Log);
			}

			[Test]
			public void TestConversion () {
				var args = new [] { "-n", "123", "-f", "3.1416" };
				var options = ArgumentParser.Parse<Options> (args);
				Assert.AreEqual (123, options.ANumber);
				Assert.AreEqual (3.1416f, options.AFloat);
			}
		}
	}
}


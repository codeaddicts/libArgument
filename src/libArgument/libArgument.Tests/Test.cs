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
				public string Msg = "Test";

				[Switch ("", "enable-log")]
				public bool Log;

				[Argument ("n", "num")]
				[CastAs (CastingType.Int32)]
				public int ANumber;

				[Argument ("f", "float")]
				[CastAs (CastingType.Float)]
				public float AFloat;

				[Argument ("b", "bool")]
				[CastAs (CastingType.Boolean)]
				public bool ABool;
			}

			[Test]
			public void TestArgument () {
				var args = new [] { "--msg", "Hello, World!" };
				StringAssert.AreEqualIgnoringCase ("Hello, World!", ArgumentParser.Parse<Options> (args).Msg);
			}

			[Test]
			public void TestDefaultArgument () {
				StringAssert.AreEqualIgnoringCase ("Test", ArgumentParser.Parse<Options> (new string[] {}).Msg);
			}

			[Test]
			public void TestDefaultBoolArgument () {
				Assert.IsFalse (ArgumentParser.Parse<Options> (new string[] {}).ABool);
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
				var args = new [] { "-n", "123", "-f", "3.1416", "--bool", "true" };
				var options = ArgumentParser.Parse<Options> (args);
				Assert.AreEqual (123, options.ANumber);
				Assert.AreEqual (3.1416f, options.AFloat);
				Assert.That (options.ABool);
			}
		}
	}
}


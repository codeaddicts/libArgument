using System;
using Codeaddicts.libArgument;
using NUnit.Framework;

namespace Codeaddicts.libArgument.Tests
{
	[TestFixture]
	public class GeneralTests
	{
		public class Options {
			[Argument ("--msg")]
			public string Msg = "Test";

			[Switch ("--enable-log")]
			public bool Log;

			[Argument ("-n", "--num")]
			public int ANumber;

			[Argument ("-f", "--float")]
			public float AFloat;

			[Argument ("-b", "--bool")]
			public bool ABool;

			[ArgumentList ("--collect")]
			public int[] ACollection;
		}

		[Test]
		public void TestArgument () {
			var args = new [] { "--msg", "Hello, World!" };
			StringAssert.AreEqualIgnoringCase ("Hello, World!", ArgumentParser<Options>.Parse (args).Msg);
		}

		[Test]
		public void TestDefaultArgument () {
			StringAssert.AreEqualIgnoringCase ("Test", ArgumentParser<Options>.Parse (new string[] {}).Msg);
		}

		[Test]
		public void TestDefaultBoolArgument () {
			Assert.IsFalse (ArgumentParser<Options>.Parse (new string[] {}).ABool);
		}

		[Test]
		public void TestEmptyArgument () {
			var args = new [] { "--msg" };
			Assert.Throws (typeof(ArgumentOutOfRangeException), () => ArgumentParser<Options>.Parse (args));
		}

		[Test]
		public void TestSwitch () {
			var args = new [] { "--enable-log" };
			Assert.That (ArgumentParser<Options>.Parse (args).Log);
		}

		[Test]
		public void TestConversion () {
			var args = new [] { "-n", "123", "-f", "3.1416", "--bool", "true" };
			var options = ArgumentParser<Options>.Parse (args);
			Assert.AreEqual (123, options.ANumber);
			Assert.AreEqual (3.1416f, options.AFloat);
			Assert.That (options.ABool);
		}

        [Test]
        public void TestConversionOrder () {
            var args = new [] { "--bool", "true", "-f", "3.1416", "-n", "123" };
            var options = ArgumentParser<Options>.Parse (args);
            Assert.AreEqual (123, options.ANumber);
            Assert.AreEqual (3.1416f, options.AFloat);
            Assert.That (options.ABool);
        }

		[Test]
		public void TestArrayIntegration () {
			var args = new [] { "-n", "321", "--collect", "1", "9", "--bool", "true" };
			var options = ArgumentParser<Options>.Parse (args);
			Assert.AreEqual (options.ANumber, 321);
			Assert.IsTrue (options.ABool);
			Assert.That (options.ACollection, Is.EquivalentTo (new [] { 1, 9 }));
		}
	}
}


﻿using System;
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
			var args = new [] { "--msg", "test", "--num", "1234", "--something" };
			var options = ArgumentParser.Parse<Options> (args);
			StringAssert.AreEqualIgnoringCase ("test", options.msg);
			Assert.AreEqual (1234, options.num);
			Assert.That (options.something);
		}

		[Test]
		public void TestDocumentation () {
			var options = ArgumentParser.Parse<Options> (new string [] { });
			Assert.DoesNotThrow (ArgumentParser.Help);
		}

		[Test]
		public void TestGNUStyleArguments () {
			var options = ArgumentParser.Parse<Options> (new [] { "--msg=test", "--num=1234", "--something" });
			StringAssert.AreEqualIgnoringCase ("test", options.msg);
			Assert.AreEqual (1234, options.num);
			Assert.That (options.something);
		}
	}
}


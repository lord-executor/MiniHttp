using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MiniHttp
{
	[TestFixture]
	public class ArgumentsFixture
	{
		[Test]
		public void TestDefaultValues()
		{
			var args = Arguments.Parse(new string[0]);

			Assert.AreEqual(Environment.CurrentDirectory, args.WebRoot);
			Assert.AreEqual(8888, args.Port);
		}

		[Test]
		public void TestPortConfiguration()
		{
			var args = Arguments.Parse(new[] { "--port=1234" });

			Assert.AreEqual(1234, args.Port);
		}

		[Test]
		public void TestWebRootConfiguration()
		{
			var args = Arguments.Parse(new[] { @"--root=C:\some\place" });

			Assert.AreEqual(@"C:\some\place", args.WebRoot);
		}

		[Test]
		public void TestImplicitWebRootConfiguration()
		{
			var args = Arguments.Parse(new[] { @"C:\some\place" });

			Assert.AreEqual(@"C:\some\place", args.WebRoot);
		}

		[Test]
		public void TestImplicitWebRootSupersedence()
		{
			var args = Arguments.Parse(new[] { @"C:\some\place", @"--root=C:\explicit\place" });

			Assert.AreEqual(@"C:\some\place", args.WebRoot);
		}

		[Test]
		public void TestUnmatchedArgument()
		{
			var args = Arguments.Parse(new[] { "--port=1234", "--test=42", @"C:\some\place", "something" });

			Assert.AreEqual(1234, args.Port);
			Assert.AreEqual(@"--test=42", args.WebRoot);
		}
	}
}

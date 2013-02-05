using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace MiniHttp.RequestHandlers.Processing
{
	[TestFixture]
	public class FileSourceResolverFixture
	{
		private string[] lines = new[] { "line one", "line to" };

		[TestFixtureSetUp]
		public void SetUp()
		{
			File.WriteAllLines("root.txt", new string[0], Encoding.UTF8);
			Directory.CreateDirectory("subdir");
			File.WriteAllLines("subdir/resolved.txt", lines, Encoding.UTF8);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			File.Delete("root.txt");
			Directory.Delete("subdir", true);
		}

		public void TestResolution()
		{
			var root = new FileInfo("root.txt");
			var resolver = new FileSourceResolver(root);

			var source = resolver.CreateSource("subdir/resolved.txt");
			Assert.IsInstanceOf<StreamLineSource>(source);
			Assert.IsNotNull(source.Resolver);
			Assert.AreNotSame(resolver, source.Resolver);
			CollectionAssert.AreEqual(lines, source.ToList());
		}
	}
}

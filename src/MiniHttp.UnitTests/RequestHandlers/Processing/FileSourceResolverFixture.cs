using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using MiniHttp.Utilities;

namespace MiniHttp.RequestHandlers.Processing
{
	[TestFixture]
	public class FileSourceResolverFixture
	{
		private string[] lines = new[] { "line one", "line to" };
        private IUrlMapper _urlMapper;

		[TestFixtureSetUp]
		public void SetUp()
		{
			File.WriteAllLines("root.txt", new string[0], Encoding.UTF8);
			Directory.CreateDirectory("subdir");
			File.WriteAllLines("subdir/resolved.txt", lines, Encoding.UTF8);
            _urlMapper = new ServerUrlMapper(new DirectoryInfo(Directory.GetCurrentDirectory()));
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			File.Delete("root.txt");
			Directory.Delete("subdir", true);
		}

        [Test]
		public void TestResolution()
		{
			var root = new FileInfo("root.txt");
            var resolver = new FileSourceResolver(_urlMapper, root);

            using (var source = resolver.CreateSource("subdir/resolved.txt"))
            {
                Assert.IsInstanceOf<StreamLineSource>(source);
                Assert.IsNotNull(source.Resolver);
                Assert.AreNotSame(resolver, source.Resolver);
                CollectionAssert.AreEquivalent(lines, source.Select(line => line.Value).ToList());
            }
		}

        [Test]
        public void TestAbsoluteResolution()
        {
            var subdir = new FileInfo("subdir/resolved.txt");
            var resolver = new FileSourceResolver(_urlMapper, subdir);

            using (var source = resolver.CreateSource("/root.txt"))
            {
                Assert.IsInstanceOf<StreamLineSource>(source);
                Assert.IsNotNull(source.Resolver);
                Assert.AreNotSame(resolver, source.Resolver);
                Assert.AreEqual(0, source.ToList().Count);
            }
        }
	}
}

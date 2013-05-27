using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Text.RegularExpressions;

namespace MiniHttp.Utilities
{
	[TestFixture]
	public class ServerUrlMapperFixture
	{
        private static readonly DirectoryInfo WebRoot = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "testroot"));
		private static readonly IEnumerable<string> Paths = new[] { "/", "/test", "/test/file.txt" };

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            WebRoot.Create();

            var fileExp = new Regex(@"\.\w+$");
            foreach (var path in Paths)
            {
                if (path == "/")
                    continue;

                if (fileExp.IsMatch(path))
                    File.Create(Path.Combine(WebRoot.FullName, path.Substring(1))).Close();
                else
                    Directory.CreateDirectory(Path.Combine(WebRoot.FullName, path.Substring(1)));
            }
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            WebRoot.Delete(true);
        }

		[Test]
		public void TestMapUrlToFileAndDirectory()
		{
			TestMapUrlToFileAndDirectoryWithBaseBuilder(new UriBuilder("http", "localhost"));
		}

		[Test]
		public void TestMapUrlWithQueryToFileAndDirectory()
		{
			var builder = new UriBuilder("http", "localhost");
			builder.Query = "?test=value";
			TestMapUrlToFileAndDirectoryWithBaseBuilder(builder);
		}

		[Test]
		public void TestMapUrlWithFragmentToFileAndDirectory()
		{
			var builder = new UriBuilder("http", "localhost");
			builder.Fragment = "testfragment";
			TestMapUrlToFileAndDirectoryWithBaseBuilder(builder);
		}

		private void TestMapUrlToFileAndDirectoryWithBaseBuilder(UriBuilder builder)
		{
			var mapper = new ServerUrlMapper(WebRoot);

			foreach (var path in Paths)
			{
				builder.Path = path;
				var file = mapper.MapUrlToFile(builder.Uri.GetPath());
				Assert.AreEqual(Path.GetFullPath(Path.Combine(WebRoot.FullName, path.Substring(1))), file.FullName);

                var dir = mapper.MapUrlToDirectory(builder.Uri.GetPath());
				Assert.AreEqual(Path.GetFullPath(Path.Combine(WebRoot.FullName, path.Substring(1))), dir.FullName);
			}
		}

		[Test]
		public void TestMapFileToUrl()
		{
			var mapper = new ServerUrlMapper(WebRoot);

			foreach (var path in Paths)
			{
				var file = new FileInfo(Path.Combine(WebRoot.FullName, path.Substring(1)));
				var dir = new DirectoryInfo(Path.Combine(WebRoot.FullName, path.Substring(1)));

                var url = mapper.MapFileToUrl(file);
				Assert.AreEqual(path, url.Path);
                url = mapper.MapFileToUrl(dir);
				Assert.AreEqual(path, url.Path);
			}
		}
	}
}

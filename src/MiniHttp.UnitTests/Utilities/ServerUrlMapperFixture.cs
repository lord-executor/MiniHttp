using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace MiniHttp.Utilities
{
	[TestFixture]
	public class ServerUrlMapperFixture
	{
		private static readonly DirectoryInfo WebRoot = new DirectoryInfo(@"C:\some\web\root");
		private static readonly IEnumerable<string> Paths = new[] { "/", "/test", "/test/file.txt" };

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
				var file = mapper.MapUrlToFile(builder.Uri);
				Assert.AreEqual(Path.GetFullPath(Path.Combine(WebRoot.FullName, path.Substring(1))), file.FullName);

				var dir = mapper.MapUrlToDirectory(builder.Uri);
				Assert.AreEqual(Path.GetFullPath(Path.Combine(WebRoot.FullName, path.Substring(1))), dir.FullName);
			}
		}

		[Test]
		public void TestMapFileToUrl()
		{
			var mapper = new ServerUrlMapper(WebRoot);

			foreach (var path in Paths)
			{
				var baseUrl = new UriBuilder("http", "localhost");
				var file = new FileInfo(Path.Combine(WebRoot.FullName, path.Substring(1)));
				var dir = new DirectoryInfo(Path.Combine(WebRoot.FullName, path.Substring(1)));

				var url = mapper.MapFileToUrl(file, baseUrl.Uri);
				Assert.AreEqual(path, url.AbsolutePath);
				url = mapper.MapFileToUrl(dir, baseUrl.Uri);
				Assert.AreEqual(path, url.AbsolutePath);
			}
		}
	}
}

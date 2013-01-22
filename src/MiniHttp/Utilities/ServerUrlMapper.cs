using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MiniHttp.Utilities
{
	public class ServerUrlMapper : IUrlMapper
	{
		private readonly DirectoryInfo _webRoot;

		public ServerUrlMapper(DirectoryInfo webRoot)
		{
			_webRoot = webRoot;
		}

		public FileInfo MapUrlToFile(Uri url)
		{
			return new FileInfo(Path.Combine(_webRoot.FullName, url.LocalPath.Substring(1)));
		}

		public DirectoryInfo MapUrlToDirectory(Uri url)
		{
			return new DirectoryInfo(Path.Combine(_webRoot.FullName, url.LocalPath.Substring(1)));
		}

		public Uri MapFileToUrl(FileSystemInfo file, Uri baseUri = null)
		{
			if (!file.FullName.StartsWith(_webRoot.FullName))
				throw new InvalidOperationException("File path must refer to a path in the webroot");

            var path = file.FullName.Substring(_webRoot.FullName.Length);
            if (file.Attributes.HasFlag(FileAttributes.Directory))
		        path = String.Format("{0}/", path);

			if (baseUri == null)
                return new Uri(path, UriKind.Relative);

            return new Uri(baseUri, path);
		}
	}
}

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

		public FileInfo MapUrlToFile(UrlPath path)
		{
            if (!path.IsAbsolute)
                throw new Exception(String.Format("can only map absolute paths and '{0}' is not absolute", path));

			return new FileInfo(Path.Combine(_webRoot.FullName, path.Path.Substring(1)));
		}

        public DirectoryInfo MapUrlToDirectory(UrlPath path)
		{
            if (!path.IsAbsolute)
                throw new Exception(String.Format("can only map absolute paths and '{0}' is not absolute", path));

			return new DirectoryInfo(Path.Combine(_webRoot.FullName, path.Path.Substring(1)));
		}

		public UrlPath MapFileToUrl(FileSystemInfo file)
		{
			if (!file.FullName.StartsWith(_webRoot.FullName))
				throw new InvalidOperationException("File path must refer to a path in the webroot");

            var path = file.FullName.Substring(_webRoot.FullName.Length);
            if (path == String.Empty)
                path = "/";
            
            return new UrlPath(path.Replace(Path.DirectorySeparatorChar, '/'));
		}
	}
}

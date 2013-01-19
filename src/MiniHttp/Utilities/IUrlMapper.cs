using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MiniHttp.Utilities
{
	public interface IUrlMapper
	{
		FileInfo MapUrlToFile(Uri url);
		DirectoryInfo MapUrlToDirectory(Uri url);
		Uri MapFileToUrl(FileSystemInfo fileOrDir, Uri baseUri = null);
	}
}

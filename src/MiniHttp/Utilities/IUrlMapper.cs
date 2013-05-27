using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MiniHttp.Utilities
{
	public interface IUrlMapper
	{
		FileInfo MapUrlToFile(UrlPath path);
		DirectoryInfo MapUrlToDirectory(UrlPath path);
		UrlPath MapFileToUrl(FileSystemInfo fileOrDir);
	}
}

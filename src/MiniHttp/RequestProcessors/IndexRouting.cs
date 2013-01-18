using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MiniHttp.Server;

namespace MiniHttp.RequestProcessors
{
    public class IndexRouting : IRequestProcessor
    {
        private readonly DirectoryInfo _rootDir;

        public IndexRouting(DirectoryInfo rootDir)
        {
            _rootDir = rootDir;
        }

        #region IRequestProcessor Members

        public void ProcessRequest(RequestContext context)
        {
            var dir = new DirectoryInfo(Path.Combine(_rootDir.FullName, context.Url.AbsolutePath.Substring(1)));
            if (!dir.Exists)
                return;

            var file = dir.GetFiles("index.*").FirstOrDefault();
            if (file != null)
            {
                var newUri = new UriBuilder(context.Url);
                newUri.Path = Path.Combine(newUri.Path, file.Name);
                context.Url = newUri.Uri;
            }
        }

        #endregion
    }
}

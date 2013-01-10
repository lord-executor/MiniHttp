using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MiniHttp.Server;

namespace MiniHttp.RequestHandlers
{
    public class ProcessingFileHandler : IRequestHandler
    {
        private readonly DirectoryInfo _rootDir;

        public ProcessingFileHandler(DirectoryInfo rootDir)
        {
            _rootDir = rootDir;
        }

        protected virtual FileInfo MapPath(string relativePath)
        {
            return new FileInfo(Path.Combine(_rootDir.FullName, relativePath));
        }

        protected virtual Stream Process(FileInfo input)
        {
            // this will of course change
            return input.OpenRead();
        }

        #region IRequestHandler Members

        public bool HandleRequest(RequestContext context)
        {
            var file = MapPath(context.Url.AbsolutePath.Substring(1));
            if (!file.Exists)
                return false;
            
            context.Response.ContentType = MimeTypes.GetMimeType(file.Extension);
            Process(file).CopyTo(context.Response.OutputStream);
            
            return true;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MiniHttp.RequestHandlers.Processing
{
    public class FileSourceResolver : ISourceResolver
    {
        private readonly string _basePath;

        public FileSourceResolver(FileInfo file)
        {
            _basePath = file.DirectoryName;
        }

        #region ISourceResolver Members

        public LineSource CreateSource(string path)
        {
            var file = new FileInfo(Path.Combine(_basePath, path));
            return new StreamLineSource(file.OpenRead(), new FileSourceResolver(file));
        }

        #endregion
    }
}

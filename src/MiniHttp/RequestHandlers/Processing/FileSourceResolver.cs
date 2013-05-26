using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MiniHttp.Utilities;

namespace MiniHttp.RequestHandlers.Processing
{
    public class FileSourceResolver : ISourceResolver
    {
        private readonly IUrlMapper _mapper;
        private readonly string _basePath;

        public FileSourceResolver(IUrlMapper mapper, FileInfo file)
        {
            _mapper = mapper;
            _basePath = file.DirectoryName;
        }

        #region ISourceResolver Members

		public ILineSource CreateSource(string path)
        {
            FileInfo file;

            if (path.StartsWith("/"))
                file = _mapper.MapUrlToFile(new UrlPath(path));
            else
                file = new FileInfo(Path.Combine(_basePath, path));
            return new StreamLineSource(file.OpenRead(), new FileSourceResolver(_mapper, file));
        }

        #endregion
    }
}

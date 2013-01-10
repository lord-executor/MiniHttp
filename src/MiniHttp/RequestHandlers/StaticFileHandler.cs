using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MiniHttp.Server;
using System.IO;

namespace MiniHttp.RequestHandlers
{
    public class StaticFileHandler : ProcessingFileHandler
    {
        public StaticFileHandler(DirectoryInfo rootDir)
            : base(rootDir)
        {
        }

        protected override Stream Process(FileInfo input)
        {
            return input.OpenRead();
        }
    }
}

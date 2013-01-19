using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MiniHttp.Server;
using System.IO;
using MiniHttp.Utilities;

namespace MiniHttp.RequestHandlers
{
    public class StaticFileHandler : ProcessingFileHandler
    {
        public StaticFileHandler(IUrlMapper urlMapper)
			: base(urlMapper)
        {
        }

        protected override Stream Process(FileInfo input)
        {
            return input.OpenRead();
        }
    }
}

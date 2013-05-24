using MiniHttp.RequestHandlers;
using MiniHttp.Server;
using MiniHttp.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniHttp.Plugins.RequestHandlers
{
    public class MarkdownHandler : ProcessingFileHandler
    {
        public MarkdownHandler(IUrlMapper urlMapper)
            : base(urlMapper)
        {
        }

        protected override Stream Process(FileInfo input)
        {
            return input.OpenRead();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing
{
    public interface ISourceResolver
    {
        LineSource CreateSource(string path);
    }
}

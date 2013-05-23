using MiniHttp.RequestHandlers.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Server
{
    public interface IProcessingHandler : IRequestHandler
    {
        IProcessingHandler AddProcessor(Func<IProcessor> processor);
    }
}

using MiniHttp.RequestHandlers.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Configuration
{
    public class DummyProcessor : IProcessor
    {
        public IProcessingResult Process(Line line)
        {
            return null;
        }
    }
}

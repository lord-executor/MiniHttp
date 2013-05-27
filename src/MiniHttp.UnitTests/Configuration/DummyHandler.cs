using MiniHttp.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Configuration
{
    public class DummyHandler : IRequestHandler
    {
        public DummyHandler()
        {
        }

        public bool HandleRequest(RequestContext context)
        {
            return false;
        }
    }
}

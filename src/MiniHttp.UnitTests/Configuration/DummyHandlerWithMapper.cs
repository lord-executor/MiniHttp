using MiniHttp.Server;
using MiniHttp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Configuration
{
    public class DummyHandlerWithMapper : IRequestHandler
    {
        public IUrlMapper Mapper { get; private set; }

        public DummyHandlerWithMapper(IUrlMapper mapper)
        {
            Mapper = mapper;
        }

        public bool HandleRequest(RequestContext context)
        {
            return false;
        }
    }
}

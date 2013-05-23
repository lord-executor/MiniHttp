using MiniHttp.RequestHandlers.Processing;
using MiniHttp.Server;
using MiniHttp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Configuration
{
    public class DummyHandlerWithProcessors : IProcessingHandler
    {
        public IUrlMapper Mapper { get; private set; }
        public IEnumerable<IProcessor> Processors { get { return _processors; } }

        private readonly IList<IProcessor> _processors;

        public DummyHandlerWithProcessors(IUrlMapper mapper)
        {
            Mapper = mapper;
            _processors = new List<IProcessor>();
        }

        public bool HandleRequest(RequestContext context)
        {
            return false;
        }

        public IProcessingHandler AddProcessor(Func<IProcessor> processor)
        {
            _processors.Add(processor());
            return this;
        }
    }
}

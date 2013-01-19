﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MiniHttp.Server;
using MiniHttp.RequestHandlers.Processing;
using MiniHttp.Utilities;

namespace MiniHttp.RequestHandlers
{
    public class ProcessingFileHandler : IRequestHandler
    {
        private readonly IUrlMapper _urlMapper;
        private readonly List<Func<IProcessor>> _processorFactories;

		public ProcessingFileHandler(IUrlMapper urlMapper)
        {
			_urlMapper = urlMapper;
			_processorFactories = new List<Func<IProcessor>>();
        }

		public ProcessingFileHandler AddProcessor(Func<IProcessor> processor)
        {
            _processorFactories.Add(processor);
            return this;
        }

        protected virtual Stream Process(FileInfo input)
        {
            var resolver = new FileSourceResolver(input);
            var output = new MemoryStream();
            var writer = new StreamWriter(output);
			var processors = _processorFactories.Select(f => f()).ToList();

            using (var source = new StreamLineSource(input.OpenRead(), resolver))
            {
                var iterator = source.GetLineIterator();
                while (iterator.MoveNext())
                {
					var result = processors.Aggregate(iterator.Current, (line, processor) => line == null ? null : processor.Process(line).Apply(iterator));
                    if (result != null)
                        writer.WriteLine(result);
                }
            }

            writer.Flush();
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        #region IRequestHandler Members

        public bool HandleRequest(RequestContext context)
        {
            var file = _urlMapper.MapUrlToFile(context.Url);
            if (!file.Exists)
                return false;
            
            context.Response.ContentType = MimeTypes.GetMimeType(file.Extension);
            using (var content = Process(file))
            {
                content.CopyTo(context.Response.OutputStream);
            }
            
            return true;
        }

        #endregion
    }
}

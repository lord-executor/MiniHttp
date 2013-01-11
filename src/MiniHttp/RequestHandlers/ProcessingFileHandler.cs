using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MiniHttp.Server;
using MiniHttp.RequestHandlers.Processing;

namespace MiniHttp.RequestHandlers
{
    public class ProcessingFileHandler : IRequestHandler
    {
        private readonly DirectoryInfo _rootDir;
        private readonly List<Func<IProcessor>> _processorFactories;

        public ProcessingFileHandler(DirectoryInfo rootDir)
        {
            _rootDir = rootDir;
			_processorFactories = new List<Func<IProcessor>>();
        }

		public ProcessingFileHandler AddProcessor(Func<IProcessor> processor)
        {
            _processorFactories.Add(processor);
            return this;
        }

        protected virtual FileInfo MapPath(string relativePath)
        {
            return new FileInfo(Path.Combine(_rootDir.FullName, relativePath));
        }

        protected virtual Stream Process(FileInfo input)
        {
            var resolver = new FileSourceResolver(input);
            var output = new MemoryStream();
            var writer = new StreamWriter(output);
			var processors = _processorFactories.Select(f => f()).ToList();

            using (var source = new StreamLineSource(input.OpenRead(), resolver))
            {
                var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                {
					var result = processors.Aggregate(enumerator.Current, (line, processor) => line == null ? null : processor.Process(line).Apply(enumerator));
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
            var file = MapPath(context.Url.AbsolutePath.Substring(1));
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

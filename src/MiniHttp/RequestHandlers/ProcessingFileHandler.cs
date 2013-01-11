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
        private readonly List<IProcessor> _processors;

        public ProcessingFileHandler(DirectoryInfo rootDir)
        {
            _rootDir = rootDir;
            _processors = new List<IProcessor>();
        }

        public ProcessingFileHandler AddProcessor(IProcessor processor)
        {
            _processors.Add(processor);
            return this;
        }

        protected virtual FileInfo MapPath(string relativePath)
        {
            return new FileInfo(Path.Combine(_rootDir.FullName, relativePath));
        }

        protected virtual Stream Process(FileInfo input)
        {
            var resolver = new FileSourceResolver(input);
            var source = new StreamLineSource(input.OpenRead(), resolver);
            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var result = _processors.Aggregate(enumerator.Current, (line, processor) => line == null ? null : processor.Process(line).Apply(enumerator));
                if (result != null)
                    writer.WriteLine(result);
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
            Process(file).CopyTo(context.Response.OutputStream);
            
            return true;
        }

        #endregion
    }
}

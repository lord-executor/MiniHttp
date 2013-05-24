using MarkdownSharp;
using MiniHttp.RequestHandlers;
using MiniHttp.RequestHandlers.Processing;
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

        protected override Stream Process(ISourceResolver resolver, Stream inputStream)
        {
            var options = new MarkdownOptions();
            options.EmptyElementSuffix = " />";
            var markdown = new Markdown(options);
            string transformed;

            using (var reader = new StreamReader(inputStream))
            {
                transformed = markdown.Transform(reader.ReadToEnd());
            }

            var transformedStream = new MemoryStream(transformed.Length);
            var writer = new StreamWriter(transformedStream);
            writer.Write(transformed);
            writer.Flush();

            transformedStream.Seek(0, SeekOrigin.Begin);

            return transformedStream; //base.Process(resolver, transformedStream);
        }
    }
}

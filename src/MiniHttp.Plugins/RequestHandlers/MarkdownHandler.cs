using MarkdownSharp;
using MiniHttp.Processors;
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
            this.AddProcessor(() => new TemplateProcessor());
        }

        public override bool HandleRequest(RequestContext context)
        {
            var file = _urlMapper.MapUrlToFile(context.Url.GetPath());
            if (file.Exists && file.Extension != ".md")
                return false;
            
            if (File.Exists(String.Format("{0}.md", file.FullName)))
            {
                var builder = new UriBuilder(context.Url);
                builder.Path += ".md";
                context.Url = builder.Uri;
            }

            var handled = base.HandleRequest(context);
            if (handled)
                context.Response.ContentType = MimeTypes.GetMimeType(".html");

            return handled;
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
            writer.WriteLine("@template(\"/_template.html\")");
            writer.Write(transformed);
            writer.Flush();

            transformedStream.Seek(0, SeekOrigin.Begin);

            return base.Process(resolver, transformedStream);
        }
    }
}

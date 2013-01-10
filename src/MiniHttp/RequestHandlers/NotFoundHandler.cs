using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.Server;
using System.Reflection;
using System.IO;

namespace MiniHttp.RequestHandlers
{
    public class NotFoundHandler : IRequestHandler
    {
        private static MemoryStream _404PageStream;

        private static void WriteToStream(Stream stream)
        {
            if (_404PageStream == null)
            {
                _404PageStream = new MemoryStream();
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(typeof(NotFoundHandler), "Resources.404.html")
                    .CopyTo(_404PageStream);
                _404PageStream.Seek(0, SeekOrigin.Begin);
            }

            _404PageStream.CopyTo(stream);
            _404PageStream.Seek(0, SeekOrigin.Begin);
        }

        #region IRequestHandler Members

        public bool HandleRequest(RequestContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/html";
            WriteToStream(context.Response.OutputStream);
            return true;
        }

        #endregion
    }
}

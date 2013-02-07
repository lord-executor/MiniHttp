using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MiniHttp.Server
{
    public class RequestContext
    {
        public Uri OriginalUri { get; private set; }
        public Uri Url { get; set; }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }
        public IList<Exception> Errors { get; private set; } 

        public RequestContext(HttpListenerContext context)
        {
			OriginalUri = Url = context.Request.Url;
            Request = new Request(context.Request);
            Response = new Response(context.Response);
            Errors = new List<Exception>();
        }

        public RequestContext(Uri url, IRequest request, IResponse response)
        {
            OriginalUri = Url = url;
            Request = request;
            Response = response;
            Errors = new List<Exception>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MiniHttp.Server
{
    public class RequestContext
    {
        public Uri Url { get; private set; }
        public HttpListenerRequest Request { get; private set; }
        public HttpListenerResponse Response { get; private set; }

        public RequestContext(HttpListenerContext context)
        {
            Request = context.Request;
            Response = context.Response;
            Url = Request.Url;
        }
    }
}

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
        public HttpListenerRequest Request { get; private set; }
        public HttpListenerResponse Response { get; private set; }
        public IList<Exception> Errors { get; private set; } 

        public RequestContext(HttpListenerContext context)
        {
            Request = context.Request;
            Response = context.Response;
            OriginalUri = Url = Request.Url;
            Errors = new List<Exception>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MiniHttp.Server
{
    [Serializable]
    public class HttpServerException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public HttpServerException()
        {
        }

        public HttpServerException(string message) : base(message)
        {
        }

        public HttpServerException(string message, Exception inner) : base(message, inner)
        {
        }

        protected HttpServerException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MiniHttp.Server
{
    public interface IRequest
    {
		IPEndPoint RemoteEndPoint { get; }
		string HttpMethod { get; }
		string UserAgent { get; }
		Uri UrlReferrer { get; }
		string ContentType { get; }
		Stream InputStream { get; }
    }
}

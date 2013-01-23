using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;

namespace MiniHttp.Server
{
    public interface IResponse
    {
		int StatusCode { get; set; }
		string ContentType { get; set; }
		string RedirectLocation { get; set; }
        Stream OutputStream { get; }
        TextWriter Output { get; }
        long ContentLength { get; }
        IDictionary<string, string> Headers { get; }
        CookieCollection Cookies { get; }

		void Send();
    }
}

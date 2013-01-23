using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MiniHttp.Server
{
	public class Request : IRequest
	{
		private readonly HttpListenerRequest _request;

		public IPEndPoint RemoteEndPoint
		{
			get { return _request.RemoteEndPoint; }
		}

		public string HttpMethod
		{
			get { return _request.HttpMethod; }
		}

		public string UserAgent
		{
			get { return _request.UserAgent; }
		}

		public Uri UrlReferrer
		{
			get { return _request.UrlReferrer; }
		}

		public string ContentType
		{
			get { return _request.ContentType; }
		}

		public Stream InputStream
		{
			get { return _request.InputStream; }
		}

        public IDictionary<string, string> Headers { get; private set; }

	    public CookieCollection Cookies
	    {
            get { return _request.Cookies; }
	    }

	    public Request(HttpListenerRequest request)
		{
			_request = request;
            Headers = request.Headers.Cast<string>().ToDictionary(key => key, key => request.Headers[key]);
		}
	}
}

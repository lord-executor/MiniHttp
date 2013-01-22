using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MiniHttp.Server
{
	public class Response : IResponse
	{
		private readonly HttpListenerResponse _response;

		public int StatusCode {
			get { return _response.StatusCode; }
			set { _response.StatusCode = value; }
		}

		public string ContentType
		{
			get { return _response.ContentType; }
			set { _response.ContentType = value; }
		}

		public string RedirectLocation
		{
			get { return _response.RedirectLocation; }
			set { _response.RedirectLocation = value; }
		}

		public Stream OutputStream { get; private set; }

		public TextWriter Output { get; private set; }

		public Response(HttpListenerResponse response)
		{
			_response = response;
			OutputStream = new MemoryStream();
			Output = new StreamWriter(OutputStream);

			StatusCode = 200;
		}

		public void AddHeader(string name, string value)
		{
			_response.AddHeader(name, value);
		}

		public void Send()
		{
			Output.Flush();

			_response.ContentLength64 = OutputStream.Length;
			_response.SendChunked = false;

			OutputStream.Seek(0, SeekOrigin.Begin);
			OutputStream.CopyTo(_response.OutputStream);

			_response.Close();
			Output = null;
			OutputStream = null;
		}
	}
}

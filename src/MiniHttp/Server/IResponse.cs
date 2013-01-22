using System;
using System.Collections.Generic;
using System.Linq;
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

		void AddHeader(string name, string value);

		void Send();
    }
}

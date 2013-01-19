using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MiniHttp.Server;
using System.IO;

namespace MiniHttp.RequestHooks
{
    public class ServerError : IRequestHook
	{
		#region IRequestHook Members

		public void ProcessRequest(RequestContext context)
        {
            if (context.Errors.Count > 0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = 500;

                var writer = new StreamWriter(context.Response.OutputStream);
                foreach (var e in context.Errors)
                {
                    writer.WriteLine("Unhandled Exception:");
                    writer.WriteLine(e);
                }
            }
        }

        #endregion
    }
}

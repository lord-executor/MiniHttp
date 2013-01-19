using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MiniHttp.Server
{
	public delegate void RequestPreHook(RequestContext context);

    public delegate bool RequestHandler(RequestContext context);

    public delegate void RequestPostHook(RequestContext context);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MiniHttp.Server
{
    public delegate bool RequestHandler(RequestContext context);
}

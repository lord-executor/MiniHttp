﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Server
{
    public interface IRequestHandler
    {
        bool HandleRequest(RequestContext context);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Server
{
    public interface IRequestHook
    {
        void ProcessRequest(RequestContext context);
    }
}

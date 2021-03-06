﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace MiniHttp.Server
{
    public class RouteDefinition
    {
        public Regex MatchExpression { get; private set; }
        public RequestHandler Handler { get; private set; }

        public RouteDefinition(Regex matchExpression, RequestHandler handler)
        {
            MatchExpression = matchExpression;
            Handler = handler;
        }

        public bool TryRoute(RequestContext context)
        {
            var match = MatchExpression.Match(context.Url.PathAndQuery);
            if (match.Success)
            {
                if (Handler(context))
                    return true;
            }
            return false;
        }
    }
}

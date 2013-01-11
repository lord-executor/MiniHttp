using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    public class Identity : Transform
    {
        public Identity(Line line)
            : base(line)
        {
        }
    }
}

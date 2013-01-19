using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing
{
    public class Line
    {
		public ILineSource Source { get; private set; }
        public string Value { get; private set; }

		public Line(ILineSource source, string value)
        {
            Source = source;
            Value = value;
        }

		public ILineSource CreateSource(string path)
        {
            return Source.Resolver.CreateSource(path);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

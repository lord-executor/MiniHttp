using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing
{
    public class DummyLineSource : LineSource
    {
        private readonly List<Line> _lines;

        public DummyLineSource(ISourceResolver resolver)
            : base(resolver)
        {
            _lines = new List<Line>();
        }

        public DummyLineSource(ISourceResolver resolver, IEnumerable<string> lines)
            : this(resolver)
        {
            _lines.AddRange(lines.Select(l => new Line(this, l)));
        }

        public override IEnumerator<Line> GetEnumerator()
        {
            return _lines.GetEnumerator();
        }

        public void Add(string line)
        {
            _lines.Add(new Line(this, line));
        }
    }
}

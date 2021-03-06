﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing
{
    public class DummyLineSource : LineSource
    {
        private readonly List<Line> _lines;
        private readonly Action<bool> _disposeCallback;

        public DummyLineSource(ISourceResolver resolver)
            : base(resolver)
        {
            _lines = new List<Line>();
        }

        public DummyLineSource(ISourceResolver resolver, Action<bool> disposeCallback)
            : this(resolver)
        {
            _disposeCallback = disposeCallback;
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

        protected override void Dispose(bool disposing)
        {
            if (_disposeCallback != null)
                _disposeCallback(disposing);
            base.Dispose(disposing);
        }
    }
}

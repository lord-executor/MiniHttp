using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MiniHttp.RequestHandlers.Processing
{
    public class StreamLineSource : LineSource
    {
        private readonly TextReader _reader;

        public StreamLineSource(Stream stream, ISourceResolver resolver)
            : base(resolver)
        {
            _reader = new StreamReader(stream);
        }

        protected override IEnumerator<Line> GetLineEnumerator()
        {
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                yield return new Line(this, line);
            }
        }
    }
}

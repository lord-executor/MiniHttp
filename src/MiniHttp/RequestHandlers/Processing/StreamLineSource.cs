using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MiniHttp.RequestHandlers.Processing
{
    public class StreamLineSource : LineSource
    {
        private readonly Stream _stream;

        public StreamLineSource(Stream stream, ISourceResolver resolver)
            : base(resolver)
        {
            _stream = stream;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _stream.Dispose();
        }

        /// <remarks>
        /// Obviously, getting an enumerator while there is still another enumerator of this instance
        /// in use, is not a good idea and would result in very 'interesting' behavior - since the
        /// underlying stream itself cannot be in two places at once.
        /// </remarks>
        protected override IEnumerator<Line> GetLineEnumerator()
        {
            var reader = new StreamReader(_stream);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return new Line(this, line);
            }

            if (_stream.CanSeek)
                _stream.Seek(0, SeekOrigin.Begin);
        }
    }
}

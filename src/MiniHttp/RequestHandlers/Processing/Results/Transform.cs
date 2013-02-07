using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    public class Transform : IProcessingResult
    {
        private readonly Line _line;

        protected Transform(Line line)
        {
            _line = line;
        }

        public Transform(Line line, string transformed)
        {
            _line = new Line(line.Source, transformed);
        }

        #region IProcessingResult Members

        public Line Apply(ILineIterator iterator)
        {
            return _line;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    public class Template : IProcessingResult
    {
        private readonly LineSource _source;

        public Template(LineSource source)
        {
            _source = source;
        }

        #region IProcessingResult Members

        public Line Apply(LineSource.LineSourceEnumerator enumerator)
        {
            enumerator.Template(_source);
            return null;
        }

        #endregion
    }
}

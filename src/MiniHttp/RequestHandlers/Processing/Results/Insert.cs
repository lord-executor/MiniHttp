using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    public class Insert : IProcessingResult
    {
        private readonly LineSource _source;

        public Insert(LineSource source)
        {
            _source = source;
        }

        #region IProcessingResult Members

        public Line Apply(LineSource.LineSourceEnumerator enumerator)
        {
            enumerator.Insert(_source);
            return null;
        }

        #endregion
    }
}

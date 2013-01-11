using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    public class Suppress : IProcessingResult
    {
        #region IProcessingResult Members

        public Line Apply(LineSource.LineSourceEnumerator enumerator)
        {
            return null;
        }

        #endregion
    }
}

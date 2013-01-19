using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    public class Resume : IProcessingResult
    {
        #region IProcessingResult Members

		public Line Apply(LineSourceExtensions.LineIterator iterator)
        {
			iterator.Resume();
            return null;
        }

        #endregion
    }
}

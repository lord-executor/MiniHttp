using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    public class Insert : IProcessingResult
    {
		private readonly ILineSource _source;

		public Insert(ILineSource source)
        {
            _source = source;
        }

        #region IProcessingResult Members

		public Line Apply(LineSourceExtensions.LineIterator iterator)
        {
			iterator.Insert(_source);
            return null;
        }

        #endregion
    }
}

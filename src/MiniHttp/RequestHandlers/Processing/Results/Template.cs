using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    public class Template : IProcessingResult
    {
		private readonly ILineSource _source;

		public Template(ILineSource source)
        {
            _source = source;
        }

        #region IProcessingResult Members

		public Line Apply(LineSourceExtensions.LineIterator iterator)
        {
			iterator.Template(_source);
            return null;
        }

        #endregion
    }
}

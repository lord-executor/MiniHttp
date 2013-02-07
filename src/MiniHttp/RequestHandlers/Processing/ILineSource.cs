using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing
{
	public interface ILineSource : IEnumerable<Line>, IDisposable
	{
		ISourceResolver Resolver { get; }
	}

	public abstract class LineSource : ILineSource
	{
		#region Constructors

		protected LineSource(ISourceResolver resolver)
		{
			Resolver = resolver;
		}

		#endregion


		#region ILineSource Members

		public ISourceResolver Resolver { get; private set; }

		#endregion


		#region IEnumerable<Line> Members

		public abstract IEnumerator<Line> GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion


		#region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {}

		~LineSource()
        {
            Dispose(false);
        }

        #endregion
	}
}

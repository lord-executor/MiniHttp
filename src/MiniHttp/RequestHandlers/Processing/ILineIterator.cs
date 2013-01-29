using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing
{
    public interface ILineIterator : IEnumerator<Line>
    {
        void Template(ILineSource source, bool disposeWhenDone = true);

        void Insert(ILineSource source, bool disposeWhenDone = true);

        void Resume();
    }
}

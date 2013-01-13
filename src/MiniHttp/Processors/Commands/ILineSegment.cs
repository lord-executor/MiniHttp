using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Processors.Commands
{
	public interface ILineSegment
	{
		string Accept(ILineSegmentHandler handler);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Processors.Commands
{
	public interface ILineSegmentHandler
	{
		string Handle(Command command);
		string Handle(Content content);
	}
}

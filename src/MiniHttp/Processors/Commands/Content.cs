using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Processors.Commands
{
	public class Content : ILineSegment
	{
		public string Value { get; private set; }

		public Content(string content)
		{
			Value = content;
		}

		public string Accept(ILineSegmentHandler handler)
		{
			return handler.Handle(this);
		}
	}
}

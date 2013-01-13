using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Processors.Commands
{
	public class Content
	{
		public string Value { get; private set; }

		public Content(string content)
		{
			Value = content;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Processors.Commands
{
	public class Command
	{
		public string Name { get; private set; }
		public IList<string> Arguments { get; private set; }

		public Command(string command, IList<string> arguments)
		{
			Name = command;
			Arguments = arguments;
		}
	}
}

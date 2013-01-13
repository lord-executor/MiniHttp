using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Processors.Commands
{
	public class Command
	{
		public string Name { get; private set; }
		public string Argument { get; private set; }

		public Command(string command, string argument)
		{
			Name = command;
			Argument = argument;
		}
	}
}

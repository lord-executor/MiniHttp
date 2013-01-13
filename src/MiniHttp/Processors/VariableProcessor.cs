using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing;
using MiniHttp.Processors.Commands;

namespace MiniHttp.Processors
{
	public class VariableProcessor : BasicCommandProcessor
	{
		private readonly Dictionary<string, string> _variables;

		public VariableProcessor()
		{
			_variables = new Dictionary<string, string>();
		}

		public override string Handle(Command command)
		{
			if (command.Name == "var")
			{
				var decl = command.Argument.Split(new[] { '=' }, 2);
				_variables[decl[0]] = decl[1];
				return String.Empty;
			}
			else if (command.Name == "val")
			{
				return _variables.ContainsKey(command.Argument) ? _variables[command.Argument] : "null";
			}

			return String.Format("@{0}({1})", command.Name, command.Argument);
		}
	}
}

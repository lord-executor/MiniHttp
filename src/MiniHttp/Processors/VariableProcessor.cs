using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing;
using MiniHttp.Processors.Commands;

namespace MiniHttp.Processors
{
	public class VariableProcessor : BasicCommandProcessor<string>
	{
		private readonly Dictionary<string, string> _variables;

		public VariableProcessor()
		{
			_variables = new Dictionary<string, string>();
			RegisterCommand("var", Var);
			RegisterCommand("val", Val);
		}

		private string Var(Command command)
		{
			var decl = command.Argument.Split(new[] { '=' }, 2);
			_variables[decl[0]] = decl[1];
			return String.Empty;
		}

		private string Val(Command command)
		{
			return _variables.ContainsKey(command.Argument) ? _variables[command.Argument] : "null";
		}

		public override string HandleContent(Content content)
		{
			return content.Value;
		}
	}
}

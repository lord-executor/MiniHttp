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
		private readonly Dictionary<string, Stack<string>> _variables;

		public VariableProcessor()
		{
			_variables = new Dictionary<string, Stack<string>>();
			RegisterCommand("varPush", Push);
			RegisterCommand("varPop", Pop);
			RegisterCommand("varPopQuiet", PopQuiet);
			RegisterCommand("varVal", Val);
		}

		private string Push(Command command)
		{
			if (command.Arguments.Count != 2)
				throw new ArgumentException(String.Format("Push requires two arguments, but got [{0}]", String.Join(",", command.Arguments)));
			
			if (!_variables.ContainsKey(command.Arguments[0]))
				_variables[command.Arguments[0]] = new Stack<string>();

			_variables[command.Arguments[0]].Push(command.Arguments[1]);
			return String.Empty;
		}

		private string Pop(Command command)
		{
			var varName = command.Arguments.Single();
			if (!_variables.ContainsKey(varName) || _variables[varName].Count == 0)
				throw new InvalidOperationException(String.Format("Cannot Pop an empty stack ({0})", varName));

			return _variables[varName].Pop();
		}

		private string PopQuiet(Command command)
		{
			Pop(command);
			return String.Empty;
		}

		private string Val(Command command)
		{
			var varName = command.Arguments.Single();
			return _variables.ContainsKey(varName) && _variables[varName].Count > 0 ? _variables[varName].Peek() : "null";
		}

		public override string HandleContent(Content content)
		{
			return content.Value;
		}
	}
}

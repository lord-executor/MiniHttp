using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing;
using System.Text.RegularExpressions;
using MiniHttp.Processors.Commands;

namespace MiniHttp.Processors
{
	public abstract class BasicCommandProcessor<TResult> : Processor, ICommandHandler<TResult>
	{
		private readonly Dictionary<string, Func<Command, TResult>> _commands;

		public bool SuppressEmptyLines { get; protected set; }

		protected BasicCommandProcessor()
		{
			_commands = new Dictionary<string, Func<Command, TResult>>();
			SuppressEmptyLines = true;
		}

		protected override IProcessingResult ProcessLine(Line line)
		{
			var commands = new CommandExtractor<TResult>(line, this);
			if (!commands.HasCommands)
				return Identity();

			return Aggregate(commands.ProcessSegments());
		}

		protected void RegisterCommand(string commandName, Func<Command, TResult> handler)
		{
			_commands.Add(commandName, handler);
		}

		public bool HasCommand(string commandName)
		{
			return _commands.ContainsKey(commandName);
		}

		public TResult Execute(Command command)
		{
			return _commands[command.Name](command);
		}

		public abstract TResult HandleContent(Content content);

		protected virtual IProcessingResult Aggregate(IEnumerable<TResult> results)
		{
			var transformedLine = String.Concat(results);
			if (SuppressEmptyLines && String.IsNullOrWhiteSpace(transformedLine))
				return Suppress();

			return Transform(transformedLine);
		}
	}
}

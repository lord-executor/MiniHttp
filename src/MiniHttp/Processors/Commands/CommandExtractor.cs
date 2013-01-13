using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing;
using System.Text.RegularExpressions;

namespace MiniHttp.Processors.Commands
{
	public class CommandExtractor<TResult>
	{
		private static readonly Regex CommandExp = new Regex(@"@(\w+)\(([^)]*)\)");

		private readonly string _lineContent;
		private readonly IList<Match> _matches;
		private readonly ICommandHandler<TResult> _handler;

		public bool HasCommands { get { return _matches.Count > 0; } }

		public CommandExtractor(Line line, ICommandHandler<TResult> handler)
		{
			_lineContent = line.Value;
			_handler = handler;
			_matches = CommandExp.Matches(_lineContent).Cast<Match>().Where(m => _handler.HasCommand(m.Groups[1].Value)).ToList();
		}

		public IEnumerable<TResult> ProcessSegments()
		{
			var lastCommand = 0;
			foreach (var match in _matches)
			{
				var command = new Command(match.Groups[1].Value, match.Groups[2].Value);

				yield return _handler.HandleContent(new Content(_lineContent.Substring(lastCommand, match.Index - lastCommand)));
				yield return _handler.Execute(command);
				lastCommand = match.Index + match.Length;
			}

			if (lastCommand < _lineContent.Length - 1)
				yield return _handler.HandleContent(new Content(_lineContent.Substring(lastCommand)));
		}
	}
}

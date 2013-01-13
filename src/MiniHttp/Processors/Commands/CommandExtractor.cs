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
		// @"""((?:[^""]|\\"")*?(?<!\\))""" -> single argument
		//   matches a single argument wrapped in double quotes ('"') - double quotes in an argument may be escaped with '\"'
		//   e.g.     "bla" or "saying \"bla\" is good"
		//   but not  "saying "bla" is good"
		private static readonly Regex ArgumentExp = new Regex(@"""((?:[^""]|\\"")*?(?<!\\))""", RegexOptions.Compiled);
		// @"\((?:\s*(x)(((?:\s*,\s*)x)*))?\s*\)" -> argument list
		//   matches comma separated lists of 'x's (where x can be any expression)
		//   e.g.     (x ,x, x) or ( x ) or ()
		//   but not  (x, ) or (x x) or (x, x,)
		private static readonly Regex CommandExp = new Regex(@"@(\w+)\((\s*(?:""(?:(?:[^""]|\\"")*?(?<!\\))"")(?:(?:(?:\s*,\s*)""(?:(?:[^""]|\\"")*?(?<!\\))"")*))?\s*\)", RegexOptions.Compiled);

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
				var arguments = ArgumentExp.Matches(match.Groups[2].Value).Cast<Match>().Select(m => Unescape(m.Groups[1].Value)).ToList();
				var command = new Command(match.Groups[1].Value, arguments);

				yield return _handler.HandleContent(new Content(_lineContent.Substring(lastCommand, match.Index - lastCommand)));
				yield return _handler.Execute(command);
				lastCommand = match.Index + match.Length;
			}

			if (lastCommand < _lineContent.Length - 1)
				yield return _handler.HandleContent(new Content(_lineContent.Substring(lastCommand)));
		}

		private string Unescape(string arg)
		{
			return arg.Replace("\\\"", "\"");
		}
	}
}

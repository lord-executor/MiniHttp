using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing;
using System.Text.RegularExpressions;

namespace MiniHttp.Processors.Commands
{
	public class CommandExtractor : IEnumerable<ILineSegment>
	{
		private static readonly Regex CommandExp = new Regex(@"^\s*@(\w+)\(([^)]*)\)\s*$");

		private readonly string _lineContent;
		private readonly MatchCollection _matches;

		public bool HasCommands { get { return _matches.Count > 0; } }

		public CommandExtractor(Line line)
		{
			_lineContent = line.Value;
			_matches = CommandExp.Matches(_lineContent);
		}

		public IEnumerator<ILineSegment> GetEnumerator()
		{
			var lastCommand = 0;
			foreach (var match in _matches.Cast<Match>())
			{
				yield return new Content(_lineContent.Substring(lastCommand, match.Index - lastCommand));
				yield return new Command(match.Groups[1].Value, match.Groups[2].Value);
				lastCommand = match.Index + match.Length;
			}

			if (lastCommand < _lineContent.Length - 1)
				yield return new Content(_lineContent.Substring(lastCommand));
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

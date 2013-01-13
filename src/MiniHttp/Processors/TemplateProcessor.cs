using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing;
using System.Text.RegularExpressions;
using MiniHttp.Processors.Commands;

namespace MiniHttp.Processors
{
    public class TemplateProcessor : BasicCommandProcessor<IProcessingResult>
    {
		private Line _currentLine;

		public TemplateProcessor()
		{
			RegisterCommand("template", c => InsertCommand(_currentLine, c));
			RegisterCommand("import", c => InsertCommand(_currentLine, c));
			RegisterCommand("content", ResumeCommand);
		}

        protected override IProcessingResult ProcessLine(Line line)
        {
			_currentLine = line;
			return base.ProcessLine(line);
        }

		private IProcessingResult InsertCommand(Line line, Command command)
		{
			return Insert(line.CreateSource(command.Argument));
		}

		private IProcessingResult ResumeCommand(Command command)
		{
			return Resume();
		}

		public override IProcessingResult HandleContent(Content content)
		{
			return null;
		}

		protected override IProcessingResult Aggregate(IEnumerable<IProcessingResult> results)
		{
			return results.Single(r => r != null);
		}
	}
}

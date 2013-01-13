using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing;
using System.Text.RegularExpressions;
using MiniHttp.Processors.Commands;

namespace MiniHttp.Processors
{
	public abstract class BasicCommandProcessor : Processor, ILineSegmentHandler
	{
		protected override IProcessingResult ProcessLine(Line line)
		{
			var commands = new CommandExtractor(line);
			if (!commands.HasCommands)
				return Identity();

			return Transform(String.Concat(commands.Select(segment => segment.Accept(this))));
		}

		public abstract string Handle(Command command);

		public virtual string Handle(Content content)
		{
			return content.Value;
		}
	}
}

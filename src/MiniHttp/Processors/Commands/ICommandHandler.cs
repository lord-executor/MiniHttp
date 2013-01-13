using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Processors.Commands
{
	public interface ICommandHandler<TResult>
	{
		bool HasCommand(string commandName);
		TResult Execute(Command command);
		TResult HandleContent(Content content);
	}
}

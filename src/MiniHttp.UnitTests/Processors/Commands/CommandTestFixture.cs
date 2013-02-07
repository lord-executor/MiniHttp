using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MiniHttp.Processors.Commands
{
	[TestFixture]
	public class CommandTestFixture
	{
		[Test]
		public void TestCommand()
		{
			var name = "command";
			var arguments = new [] { "arg one", "arg two" };
			var command = new Command(name, arguments);

			Assert.AreEqual(name, command.Name);
			CollectionAssert.AreEqual(arguments, command.Arguments);
		}
	}
}

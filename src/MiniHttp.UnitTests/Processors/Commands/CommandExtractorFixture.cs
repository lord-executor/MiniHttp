using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using MiniHttp.RequestHandlers.Processing;

namespace MiniHttp.Processors.Commands
{
	[TestFixture]
	public class CommandExtractorFixture
	{
		[Test]
		public void Test()
		{
			var line = new Line(null, "line");
			var extractor = new CommandExtractor<string>(line, null);

			Assert.IsFalse(extractor.HasCommands);
		}

		[Test]
		public void TestHasCommandWithHandlerCommand()
		{
			var handlerMock = new Mock<ICommandHandler<string>>(MockBehavior.Strict);
			handlerMock.Setup(h => h.HasCommand("command")).Returns(true);
			var line = new Line(null, "@command()");
			var extractor = new CommandExtractor<string>(line, handlerMock.Object);

			Assert.IsTrue(extractor.HasCommands);
			handlerMock.VerifyAll();
		}

		[Test]
		public void TestHasCommandWithoutHandlerCommand()
		{
			var handlerMock = new Mock<ICommandHandler<string>>(MockBehavior.Strict);
			handlerMock.Setup(h => h.HasCommand("command")).Returns(false);
			var line = new Line(null, "@command()");
			var extractor = new CommandExtractor<string>(line, handlerMock.Object);

			Assert.IsFalse(extractor.HasCommands);
			handlerMock.VerifyAll();
		}

		[Test]
		public void TestCommandAtLineStartEmptyContent()
		{
			var command = "@command()";
			var handlerMock = new Mock<ICommandHandler<string>>(MockBehavior.Strict);
			handlerMock.Setup(h => h.HasCommand("command")).Returns(true);
			handlerMock.Setup(h => h.HandleContent(It.Is<Content>(c => c.Value == String.Empty))).Returns("content");
			handlerMock.Setup(h => h.Execute(It.Is<Command>(c => c.Name == "command" && c.Arguments.Count == 0))).Returns("command");
			var line = new Line(null, command);
			var extractor = new CommandExtractor<string>(line, handlerMock.Object);

			Assert.IsTrue(extractor.HasCommands);
			Assert.AreEqual(new [] { "content", "command" }, extractor.ProcessSegments().ToList());
			handlerMock.VerifyAll();
		}

		//[Test]
		//public void TestSingleCommandOneArg()
		//{
		//}

		//[Test]
		//public void TestSingleCommandTwoArgs()
		//{
		//}

		//[Test]
		//public void TestMultipleCommands()
		//{
		//}
	}
}

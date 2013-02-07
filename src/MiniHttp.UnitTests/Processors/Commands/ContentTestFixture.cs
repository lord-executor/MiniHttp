using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MiniHttp.Processors.Commands
{
	[TestFixture]
	public class ContentTestFixture
	{
		[Test]
		public void TestContent()
		{
			var text = "content";
			var content = new Content(text);

			Assert.AreEqual(text, content.Value);
		}
	}
}

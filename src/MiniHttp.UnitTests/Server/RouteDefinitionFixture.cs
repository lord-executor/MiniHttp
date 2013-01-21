using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace MiniHttp.Server
{
	[TestFixture]
	public class RouteDefinitionFixture
	{
		private Regex _expression = new Regex(@"a+b+");
		private RequestHandler _anyHandler = (context => true);
		private RequestHandler _noneHandler = (context => false);

		[Test]
		public void TestConstructor()
		{
			var route = new RouteDefinition(_expression, _anyHandler);

			Assert.AreSame(_expression, route.MatchExpression);
			Assert.AreSame(_anyHandler, route.Handler);
		}

		[Test]
		public void TestTryRoute()
		{
			// TODO: refactor request context first
		}
	}
}

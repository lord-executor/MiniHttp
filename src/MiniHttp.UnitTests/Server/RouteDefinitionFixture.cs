using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace MiniHttp.Server
{
	[TestFixture]
	public class RouteDefinitionFixture
	{
		private readonly Regex _expression = new Regex(@"a+b+");

		[Test]
		public void TestConstructor()
		{
            RequestHandler anyHandler = (context => true);

            var route = new RouteDefinition(_expression, anyHandler);

			Assert.AreSame(_expression, route.MatchExpression);
            Assert.AreSame(anyHandler, route.Handler);
		}

		[Test]
		public void TestTryRouteExpressionMatchHandlerAccept()
		{
            var requestContext = new RequestContext(new Uri("http://localhost/aaabb"), null, null);
		    var handlerMock = new Mock<IRequestHandler>(MockBehavior.Strict);
            // handler accepts and handles the request
		    handlerMock.Setup(h => h.HandleRequest(requestContext)).Returns(true);
            var route = new RouteDefinition(_expression, handlerMock.Object.HandleRequest);

            var result = route.TryRoute(requestContext);

            Assert.IsTrue(result);
		    handlerMock.VerifyAll();
		}

        [Test]
        public void TestTryRouteExpressionMatchHandlerRefuse()
        {
            var requestContext = new RequestContext(new Uri("http://localhost/aaabb"), null, null);
            var handlerMock = new Mock<IRequestHandler>(MockBehavior.Strict);
            // handler refuses request
            handlerMock.Setup(h => h.HandleRequest(requestContext)).Returns(false);
            var route = new RouteDefinition(_expression, handlerMock.Object.HandleRequest);

            var result = route.TryRoute(requestContext);

            Assert.IsFalse(result);
            handlerMock.VerifyAll();
        }

        [Test]
        public void TestTryRouteExpressionFail()
        {
            // handler is not invoked if the route expression does not match the URL
            var requestContext = new RequestContext(new Uri("http://localhost/aaacccbb"), null, null);
            // explicitly use mock with NO setup since nothing should be called
            var handlerMock = new Mock<IRequestHandler>(MockBehavior.Strict);
            var route = new RouteDefinition(_expression, handlerMock.Object.HandleRequest);

            var result = route.TryRoute(requestContext);

            Assert.IsFalse(result);
            handlerMock.VerifyAll();
        }
	}
}

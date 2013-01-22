using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace MiniHttp.RequestHandlers.Processing
{
    [TestFixture]
    public class LineFixture
    {
        private const string LineValue = "LineValue";

        [Test]
        public void TestConstructor()
        {
            var sourceMock = new Mock<ILineSource>(MockBehavior.Strict);
            var line = new Line(sourceMock.Object, LineValue);

            Assert.AreEqual(LineValue, line.Value);
            Assert.AreSame(sourceMock.Object, line.Source);
            sourceMock.VerifyAll();
        }

        [Test]
        public void TestToString()
        {
            var sourceMock = new Mock<ILineSource>(MockBehavior.Strict);
            var line = new Line(sourceMock.Object, LineValue);

            Assert.AreEqual(LineValue, line.ToString());
            sourceMock.VerifyAll();
        }

        [Test]
        public void TestCreateSource()
        {
            var resultMock = new Mock<ILineSource>(MockBehavior.Strict);
            var resolverMock = new Mock<ISourceResolver>(MockBehavior.Strict);
            resolverMock.Setup(r => r.CreateSource("test")).Returns(resultMock.Object);
            var sourceMock = new Mock<ILineSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Resolver).Returns(resolverMock.Object);
            var line = new Line(sourceMock.Object, LineValue);

            var newSource = line.CreateSource("test");
            Assert.AreSame(resultMock.Object, newSource);

            sourceMock.VerifyAll();
            resolverMock.VerifyAll();
            resultMock.VerifyAll();
        }
    }
}

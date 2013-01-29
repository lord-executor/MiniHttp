using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;

namespace MiniHttp.RequestHandlers.Processing.Results
{
    [TestFixture]
    public class ResultFixture
    {
        [Test]
        public void TestIdentityBehavior()
        {
            var iteratorMock = new Mock<ILineIterator>(MockBehavior.Strict);
            var originalLine = new Line(null, "line");
            var identity = new Identity(originalLine);

            var line = identity.Apply(iteratorMock.Object);

            Assert.AreSame(originalLine, line);
            iteratorMock.VerifyAll();
        }

        [Test]
        public void TestInsertBehavior()
        {
            var sourceMock = new Mock<ILineSource>(MockBehavior.Strict);
            var insert = new Insert(sourceMock.Object);

            var iteratorMock = new Mock<ILineIterator>(MockBehavior.Strict);
            iteratorMock.Setup(iterator => iterator.Insert(sourceMock.Object, true));

            var line = insert.Apply(iteratorMock.Object);

            Assert.IsNull(line);
            sourceMock.VerifyAll();
            iteratorMock.VerifyAll();
        }

        [Test]
        public void TestResumeBehavior()
        {
            var resume = new Resume();
            var iteratorMock = new Mock<ILineIterator>(MockBehavior.Strict);
            iteratorMock.Setup(iterator => iterator.Resume());

            var line = resume.Apply(iteratorMock.Object);

            Assert.IsNull(line);
            iteratorMock.VerifyAll();
        }

        [Test]
        public void TestSuppressBehavior()
        {
            var suppress = new Suppress();
            var iteratorMock = new Mock<ILineIterator>(MockBehavior.Strict);

            var line = suppress.Apply(iteratorMock.Object);

            Assert.IsNull(line);
            iteratorMock.VerifyAll();
        }

        [Test]
        public void TestTemplateBehavior()
        {
            var sourceMock = new Mock<ILineSource>(MockBehavior.Strict);
            var template = new Template(sourceMock.Object);
            var iteratorMock = new Mock<ILineIterator>(MockBehavior.Strict);
            iteratorMock.Setup(iterator => iterator.Template(sourceMock.Object, true));

            var line = template.Apply(iteratorMock.Object);

            Assert.IsNull(line);
            iteratorMock.VerifyAll();
            sourceMock.VerifyAll();
        }

        [Test]
        public void TestTransformBehavior()
        {
            var originalSourceMock = new Mock<ILineSource>(MockBehavior.Strict);
            var originalLine = new Line(originalSourceMock.Object, "line");
            var transform = new Transform(originalLine, "transformed line");
            var iteratorMock = new Mock<ILineIterator>(MockBehavior.Strict);

            var line = transform.Apply(iteratorMock.Object);

            Assert.AreNotSame(originalLine, line);
            Assert.AreEqual("transformed line", line.Value);
            Assert.AreSame(originalLine.Source, line.Source);
            iteratorMock.VerifyAll();
            originalSourceMock.VerifyAll();
        }
    }
}

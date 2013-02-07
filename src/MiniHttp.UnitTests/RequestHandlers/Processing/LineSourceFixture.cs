using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace MiniHttp.RequestHandlers.Processing
{
    [TestFixture]
    public class LineSourceFixture
    {
        [Test]
        public void TestConstructor()
        {
            var resolverMock = new Mock<ISourceResolver>(MockBehavior.Strict);
            var source = new DummyLineSource(resolverMock.Object);

            Assert.AreSame(source.Resolver, resolverMock.Object);
            resolverMock.VerifyAll();
        }

        [Test]
        public void TestGetEnumerable()
        {
            var source = new DummyLineSource(null);

            var generic = source.GetEnumerator();
            var nonGeneric = (source as System.Collections.IEnumerable).GetEnumerator();

            Assert.NotNull(generic);
            Assert.NotNull(nonGeneric);
            Assert.IsTrue(AreEqual(generic, nonGeneric));
        }

        [Test]
        public void TestManualDisposal()
        {
            bool disposed = false;
            bool? disposing = null;
            Action<bool> callback = arg => { disposed = true; disposing = arg; };
            
            using (new DummyLineSource(null, callback))
            {}

            Assert.IsTrue(disposed);
            Assert.IsTrue(disposing.HasValue && disposing.Value);
        }

        [Test]
		[IgnoreAttribute]
        [Category("disposal")] // the WaitForPendingFinalizers call crashes the NUnit console runner
        public void TestFinalizerDisposal()
        {
            bool disposed = false;
            bool? disposing = null;
            Action<bool> callback = arg => { disposed = true; disposing = arg; };

            var source = new DummyLineSource(null, callback);
            source = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsTrue(disposed);
            Assert.IsTrue(disposing.HasValue && !disposing.Value);
        }

        private bool AreEqual(System.Collections.IEnumerator left, System.Collections.IEnumerator right)
        {
            while (left.MoveNext())
            {
                if (!right.MoveNext())
                    return false;

                if (!ReferenceEquals(left.Current, right.Current))
                    return false;
            }

            if (right.MoveNext())
                return false;

            return true;
        }
    }
}

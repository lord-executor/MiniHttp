using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MiniHttp.RequestHandlers.Processing
{
    [TestFixture]
    public class LineIteratorFixture
    {
        private readonly string[] _lines = new[] { "a", "b", "c", "d" };
        private readonly string[] _templ = new[] { "t", "u", "v", "w" };

        [Test]
        public void TestInitialState()
        {
            var source = new DummyLineSource(null, _lines);
            var iterator = source.GetLineIterator();
            Assert.IsNull(iterator.Current);
        }

        [Test]
        public void TestUnresettable()
        {
            var iterator = new DummyLineSource(null).GetLineIterator();

            Assert.Throws<NotImplementedException>(iterator.Reset);
        }

        [Test]
        public void TestSingleSourceIteration()
        {
            var helper = new IteratorHelper(new DummyLineSource(null, _lines));
            helper.Read();

            CollectionAssert.AreEqual(_lines, helper.RawLines);
        }

        [Test]
        public void TestSourceInsert()
        {
            var helper = new IteratorHelper(new DummyLineSource(null, _lines));

            helper.Iterator.Insert(new DummyLineSource(null, _lines));
            helper.Read();

            CollectionAssert.AreEqual(_lines.Concat(_lines), helper.RawLines);
        }

        [Test]
        public void TestSourceInsertDisposal()
        {
            var disposed = false;
            var helper = new IteratorHelper(new DummyLineSource(null, _lines));

            helper.Iterator.Insert(new DummyLineSource(null, arg => { disposed = true; }) { "one", "two" });
            helper.Read(1);
            Assert.IsFalse(disposed);
            helper.Read(2);
            Assert.IsTrue(disposed);
        }

        [Test]
        public void TestSourceInsertMidstream()
        {
            var helper = new IteratorHelper(new DummyLineSource(null, _lines));

            helper.Read(2);
            helper.Iterator.Insert(new DummyLineSource(null, _lines));
            helper.Read();

            CollectionAssert.AreEqual(_lines.Take(2).Concat(_lines).Concat(_lines.Skip(2)), helper.RawLines);
        }

        [Test]
        public void TestSourceTemplate()
        {
            var helper = new IteratorHelper(new DummyLineSource(null, _lines));
            helper.Read(2);
            helper.Iterator.Template(new DummyLineSource(null, _templ));
            helper.Read();

            CollectionAssert.AreEqual(
                _lines.Take(2)
                .Concat(_templ)
                .Concat(_lines.Skip(2)), helper.RawLines);
        }

        [Test]
        public void TestSourceTemplateWithResume()
        {
            var helper = new IteratorHelper(new DummyLineSource(null, _lines));
            helper.Read(2);
            helper.Iterator.Template(new DummyLineSource(null, _templ));
            helper.Read(3);
            helper.Iterator.Resume();
            helper.Read();

            CollectionAssert.AreEqual(
                _lines.Take(2)
                .Concat(_templ.Take(3))
                .Concat(_lines.Skip(2))
                .Concat(_templ.Skip(3)), helper.RawLines);
        }

        [Test]
        public void TestDoubleTemplateWithResume()
        {
            var helper = new IteratorHelper(new DummyLineSource(null, _lines));
            helper.Read(2);
            helper.Iterator.Template(new DummyLineSource(null, _templ));
            helper.Read(1);
            helper.Iterator.Template(new DummyLineSource(null, _templ));
            helper.Read(3);
            helper.Iterator.Resume();
            helper.Read(1);
            helper.Iterator.Resume();
            helper.Read();

            CollectionAssert.AreEqual(
                _lines.Take(2)
                .Concat(_templ.Take(1))
                .Concat(_templ.Take(3))
                .Concat(_templ.Skip(1).Take(1))
                .Concat(_lines.Skip(2))
                .Concat(_templ.Skip(2))
                .Concat(_templ.Skip(3))
                , helper.RawLines);
        }

        [Test]
        public void TestResumeWithInsufficientStack()
        {
            var helper = new IteratorHelper(new DummyLineSource(null, _lines));

            helper.Read(2);
            Assert.Throws<InvalidOperationException>(helper.Iterator.Resume);
        }

        private class IteratorHelper
        {
            private readonly IList<Line> _lines;
            public ILineIterator Iterator { get; private set; }
            public IEnumerable<string> RawLines
            {
                get { return _lines.Select(l => l.Value); }
            } 

            public IteratorHelper(ILineSource source)
            {
                Iterator = source.GetLineIterator();
                _lines = new List<Line>();
            }

            public void Read(int num = -1)
            {
                while (num-- != 0 && Iterator.MoveNext())
                {
                    _lines.Add(Iterator.Current);
                }

                if (num > 0)
                    throw new Exception(String.Format("Couldn't read enough lines, {0} missing", num));
            }
        }
    }
}

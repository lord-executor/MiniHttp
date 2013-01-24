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
            var source = new DummyLineSource(null, _lines);
            var linearLines = Linearize(source.GetLineIterator());

            Assert.AreEqual(_lines.Length, linearLines.Count);
            Assert.True(_lines.Zip(linearLines, (l, line) => ReferenceEquals(l, line.Value)).All(v => v));
        }

        private IList<Line> Linearize(IEnumerator<Line> enumerator)
        {
            var list = new List<Line>();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            return list;
        }
    }
}

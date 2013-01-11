using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing
{
    public abstract class LineSource : IDisposable
    {
        public ISourceResolver Resolver { get; private set; }

        protected LineSource(ISourceResolver resolver)
        {
            Resolver = resolver;
        }

        protected abstract IEnumerator<Line> GetLineEnumerator();

        #region IEnumerable<Line> Members

        public virtual LineSourceEnumerator GetEnumerator()
        {
            return new LineSourceEnumerator(this);
        }

        #endregion


        #region LineSourceEnumerator

        public class LineSourceEnumerator : IEnumerator<Line>
        {
            private readonly Stack<IEnumerator<Line>> _currentEnumerator;
            private Line _currentLine;

            public LineSourceEnumerator(LineSource source)
            {
                _currentEnumerator = new Stack<IEnumerator<Line>>();
                _currentEnumerator.Push(source.GetLineEnumerator());
            }

            public void Insert(LineSource source, bool disposeWhenDone = true)
            {
                var enumerator = source.GetLineEnumerator();
                if (disposeWhenDone)
                    enumerator = new DisposingEnumerator<Line>(enumerator, new [] { source });
                _currentEnumerator.Push(enumerator);
            }

            public void Flip()
            {
                if (_currentEnumerator.Count < 2)
                    throw new InvalidOperationException("Needs at least two sources on the stack");

                var first = _currentEnumerator.Pop();
                var second = _currentEnumerator.Pop();
                _currentEnumerator.Push(first);
                _currentEnumerator.Push(second);
            }

            #region IEnumerator<Line> Members

            public Line Current
            {
                get { return _currentLine; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                while (_currentEnumerator.Count > 0)
                {
                    _currentEnumerator.Pop().Dispose();
                }
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                while (!_currentEnumerator.Peek().MoveNext())
                {
                    _currentEnumerator.Pop().Dispose();
                    
                    if (_currentEnumerator.Count == 0)
                    {
                        return false;
                    }
                }

                _currentLine = _currentEnumerator.Peek().Current;
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException("You cannot do that");
            }

            #endregion

            private class DisposingEnumerator<T> : IEnumerator<T>
            {
                private readonly IEnumerator<T> _enumerator;
                private readonly IEnumerable<IDisposable> _disposables;

                public DisposingEnumerator(IEnumerator<T> enumerator, IEnumerable<IDisposable> disposables)
                {
                    _enumerator = enumerator;
                    _disposables = disposables;
                }

                #region IEnumerator<T> Members

                public T Current
                {
                    get { return _enumerator.Current; }
                }

                #endregion

                #region IDisposable Members

                public void Dispose()
                {
                    Dispose(true);
                }

                private void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        _enumerator.Dispose();
                        foreach (var disposable in _disposables)
                        {
                            disposable.Dispose();
                        }
                    }
                }

                ~DisposingEnumerator()
                {
                    Dispose(false);
                }

                #endregion

                #region IEnumerator Members

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    return _enumerator.MoveNext();
                }

                public void Reset()
                {
                    _enumerator.Reset();
                }

                #endregion
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {}

        ~LineSource()
        {
            Dispose(false);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.RequestHandlers.Processing
{
	public static class LineSourceExtensions
	{
		public static LineIterator GetLineIterator(this ILineSource source)
		{
			return new LineIterator(source);
		}

		#region LineSourceEnumerator

		public class LineIterator : IEnumerator<Line>
		{
			private readonly Stack<SourceWrapper> _currentEnumerator;
			private Line _currentLine;

			public LineIterator(ILineSource source)
			{
				_currentEnumerator = new Stack<SourceWrapper>();
				_currentEnumerator.Push(new SourceWrapper(source));
			}

			public void Template(ILineSource source, bool disposeWhenDone = true)
			{
				_currentEnumerator.Push(new SourceWrapper(source, disposeWhenDone, SourceType.Template));
			}

			public void Insert(ILineSource source, bool disposeWhenDone = true)
			{
				_currentEnumerator.Push(new SourceWrapper(source, disposeWhenDone));
			}

			public void Resume()
			{
				if (_currentEnumerator.Count < 2)
					throw new InvalidOperationException("Needs at least two sources on the stack");

				var lifted = new Stack<SourceWrapper>();
				var current = _currentEnumerator.Pop();

				do
				{
					lifted.Push(_currentEnumerator.Pop());
				} while (lifted.Peek().SourceType == SourceType.Template);

				_currentEnumerator.Push(current);

				while (lifted.Count > 0)
					_currentEnumerator.Push(lifted.Pop());
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
					_currentEnumerator.Pop().Enumerator.Dispose();
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
				while (!_currentEnumerator.Peek().Enumerator.MoveNext())
				{
					_currentEnumerator.Pop().Enumerator.Dispose();

					if (_currentEnumerator.Count == 0)
					{
						return false;
					}
				}

				_currentLine = _currentEnumerator.Peek().Enumerator.Current;
				return true;
			}

			public void Reset()
			{
				throw new NotImplementedException("You cannot do that");
			}

			#endregion

			private enum SourceType
			{
				Normal = 0,
				Template,
			}

			private class SourceWrapper
			{
				public IEnumerator<Line> Enumerator { get; private set; }
				public SourceType SourceType { get; private set; }

				public SourceWrapper(ILineSource source, bool disposeWhenDone = true, SourceType type = SourceType.Normal)
				{
					Enumerator = source.GetEnumerator();
					if (disposeWhenDone)
						Enumerator = new DisposingEnumerator<Line>(Enumerator, new[] { source });
					SourceType = type;
				}
			}

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
	}
}

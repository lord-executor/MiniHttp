using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniHttp.RequestHandlers.Processing.Results;

namespace MiniHttp.RequestHandlers.Processing
{
    public interface IProcessor
    {
        IProcessingResult Process(Line line);
    }

    public abstract class Processor : IProcessor
    {
        private Line _current;

        #region IProcessor Members

        public IProcessingResult Process(Line line)
        {
            _current = line;
            return ProcessLine(line);
        }

        #endregion

        protected abstract IProcessingResult ProcessLine(Line line);

        protected Identity Identity()
        {
            return new Identity(_current);
        }

        protected Transform Transform(string transformed)
        {
            return new Transform(_current, transformed);
        }

        protected Template Template(LineSource source)
        {
            return new Template(source);
        }

        protected Insert Insert(LineSource source)
        {
            return new Insert(source);
        }

        protected Resume Resume()
        {
            return new Resume();
        }

        protected Suppress Suppress()
        {
            return new Suppress();
        }
    }
}

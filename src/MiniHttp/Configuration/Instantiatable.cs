using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Configuration
{
    public abstract class Instantiatable<TInterface>
    {
        protected abstract string TypeName { get; set; }

        public virtual TInterface Instantiate(params object[] args)
        {
            var type = Type.GetType(TypeName);
            if (type == null)
                throw new Exception(String.Format("Unable to locate type {0}", TypeName));

            try
            {
                return (TInterface)Activator.CreateInstance(type, args);
            }
            catch (MissingMethodException)
            {
                return (TInterface)Activator.CreateInstance(type);
            }
        }
    }
}

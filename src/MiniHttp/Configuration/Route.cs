using MiniHttp.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MiniHttp.Configuration
{
    public class Route : Instantiatable<IRequestHandler>
    {
        [XmlAttribute("expression")]
        public string RouteExpression { get; set; }

        [XmlAttribute("type")]
        public string Type
        {
            get { return TypeName; }
            set { TypeName = value; }
        }

        [XmlElement("processor")]
        public Processor[] Processors { get; set; }

        protected override string TypeName { get; set; }

        public override IRequestHandler Instantiate(params object[] args)
        {
            var handler = base.Instantiate(args);

            if (Processors != null && Processors.Length > 0)
            {
                if (!(handler is IProcessingHandler))
                    throw new Exception(String.Format("{0} is not an IProcessingHandler, yet it is configured to use processors", TypeName));

                foreach (var processor in Processors)
                {
                    (handler as IProcessingHandler).AddProcessor(() => processor.Instantiate(args));
                }
            }

            return handler;
        }
    }
}

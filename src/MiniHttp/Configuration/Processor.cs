using MiniHttp.RequestHandlers.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MiniHttp.Configuration
{
    public class Processor : Instantiatable<IProcessor>
    {
        [XmlAttribute("type")]
        public string Type
        {
            get { return TypeName; }
            set { TypeName = value; }
        }

        protected override string TypeName { get; set; }
    }
}

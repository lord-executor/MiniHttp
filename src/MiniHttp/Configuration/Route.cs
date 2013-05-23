using MiniHttp.Server;
using System;
using System.Collections.Generic;
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

        protected override string TypeName { get; set; }
    }
}

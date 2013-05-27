using MiniHttp.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MiniHttp.Configuration
{
    public class Hook : Instantiatable<IRequestHook>
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

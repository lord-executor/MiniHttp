using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MiniHttp.Configuration
{
    [XmlRoot("server")]
    public class Server
    {
        [XmlAttribute("port")]
        public int Port { get; set; }

        [XmlElement("preHook")]
        public Hook[] PreHooks { get; set; }

        [XmlElement("postHook")]
        public Hook[] PostHooks { get; set; }

        public static Server LoadConfig(TextReader reader)
        {
            var serializer = new XmlSerializer(typeof(Server));
            return (Server)serializer.Deserialize(reader);
        }
    }
}

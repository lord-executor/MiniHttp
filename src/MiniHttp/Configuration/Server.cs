using MiniHttp.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MiniHttp.Utilities;

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

        [XmlElement("route")]
        public Route[] Routes { get; set; }

        public static Server LoadConfig(TextReader reader)
        {
            var serializer = new XmlSerializer(typeof(Server));
            return (Server)serializer.Deserialize(reader);
        }

        public void RegisterServerModules(HttpServer server, IUrlMapper mapper)
        {
            if (PreHooks != null)
                PreHooks.Select(hook => hook.Instantiate(mapper)).Each( server.RegisterPreHook);
            if (PostHooks != null)
                PostHooks.Select(hook => hook.Instantiate(mapper)).Each(server.RegisterPostHook);

            Routes.Each(route => server.RegisterRoute(route.RouteExpression, route.Instantiate(mapper)));
        }
    }
}

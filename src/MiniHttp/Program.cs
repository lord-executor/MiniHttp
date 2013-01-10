using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MiniHttp.RequestHandlers;
using MiniHttp.Server;

namespace MiniHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpServer();
            server.RegisterRoute(@".*", new StaticFileHandler(new DirectoryInfo(Environment.CurrentDirectory)));
            server.RegisterRoute(@".*", new NotFoundHandler());
            server.Start();

            Console.WriteLine("Server running");
            Console.WriteLine("Press RETURN to stop");
            Console.ReadLine();

            server.Stop();
        }

    }
}

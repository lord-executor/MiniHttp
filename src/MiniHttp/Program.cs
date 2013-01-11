using System;
using System.IO;
using MiniHttp.Processors;
using MiniHttp.RequestHandlers;
using MiniHttp.Server;

namespace MiniHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            var webroot = new DirectoryInfo(Environment.CurrentDirectory);
            var server = new HttpServer();

            server.RegisterRoute(@"\.html$", new ProcessingFileHandler(webroot).AddProcessor(() => new TemplateProcessor()));
            server.RegisterRoute(@".*", new StaticFileHandler(webroot));
            server.RegisterRoute(@".*", new NotFoundHandler());
            server.Start();

            Console.WriteLine("Server running");
            Console.WriteLine("Press RETURN to stop");
            Console.ReadLine();

            server.Stop();
        }
    }
}

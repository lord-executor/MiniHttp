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
			new Program(Arguments.Parse(args)).Run();
        }

		private readonly Arguments _arguments;
		private readonly HttpServer _server;

		private Program(Arguments arguments)
		{
			_arguments = arguments;
			Console.WriteLine(arguments);

			_server = new HttpServer(arguments.Port);
			RegisterRoutes();
		}

		private void RegisterRoutes()
		{
			var webroot = new DirectoryInfo(_arguments.WebRoot);
			if (!webroot.Exists)
				throw new DirectoryNotFoundException(String.Format(webroot.FullName));

			_server.RegisterRoute(@"\.html$", new ProcessingFileHandler(webroot).AddProcessor(() => new TemplateProcessor()));
			_server.RegisterRoute(@".*", new StaticFileHandler(webroot));
			_server.RegisterRoute(@".*", new NotFoundHandler());
		}

		public void Run()
		{
			_server.Start();

			Console.WriteLine("Server running");
			Console.WriteLine("Press RETURN to stop");
			Console.ReadLine();

			_server.Stop();
		}
    }
}

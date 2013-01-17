using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Reflection;
using System.Linq;
using MiniHttp.Server;
using MiniHttp.RequestProcessors;
using MiniHttp.RequestHandlers;

#if !MINIMAL
using MiniHttp.Processors;
#endif

namespace MiniHttp
{
    class Program
    {
        static void Main(string[] args)
        {
			new Program(Arguments.Parse(args)).Run();
        }

		private readonly Arguments _arguments;
		private HttpServer _server;

		private Program(Arguments arguments)
		{
			_arguments = arguments;
			Console.WriteLine(arguments);
		}

		private void StartServer()
		{
			_server = new HttpServer(_arguments.Port);
			RegisterModules();
			_server.Start();
		}

#if MINIMAL

		private void RegisterModules()
		{
			var webroot = new DirectoryInfo(_arguments.WebRoot);
			if (!webroot.Exists)
				throw new DirectoryNotFoundException(String.Format(webroot.FullName));

            _server.RegisterPostprocessor(new ServerErrorProcessor());
			_server.RegisterRoute(@".*", new StaticFileHandler(webroot));
			_server.RegisterRoute(@".*", new NotFoundHandler());
		}

#else

		private void RegisterModules()
		{
			var webroot = new DirectoryInfo(_arguments.WebRoot);
			if (!webroot.Exists)
				throw new DirectoryNotFoundException(String.Format(webroot.FullName));

			_server.RegisterPostprocessor(new ServerErrorProcessor());

			_server.RegisterRoute(@"\.html$", new ProcessingFileHandler(webroot).AddProcessor(() => new TemplateProcessor()).AddProcessor(() => new VariableProcessor()));
			_server.RegisterRoute(@".*", new StaticFileHandler(webroot));
			_server.RegisterRoute(@".*", new NotFoundHandler());
		}

#endif

		public void Run()
		{
			var retry = true;

			do
			{
				retry = false;
				try
				{
					StartServer();
				}
				catch (HttpListenerException e)
				{

					if (e.ErrorCode == 5) // Access denied (needs admin privileges)
					{
						switch (PermissionPrompt())
						{
							case 1:
								Console.WriteLine("quitting...");
								return;
							case 2:
								if (RegisterUrlAcl())
								{
									Console.WriteLine("URL prefix registered. You can reverse this process by executing the following command as an administrator:");
									Console.WriteLine("> netsh http delete urlacl url={0}", _server.ListenerPrefix);
									retry = true;
									continue;
								}
								else
								{
									Console.WriteLine("URL prefix registration failed... You'll have to figure out what went wrong on your own.");
									Console.WriteLine("quitting...");
									return;
								}
						}
					}
					else
						throw;
				}
			} while (retry);

			PrintServerInfo();
			Console.WriteLine("Server running...");
			Console.WriteLine("Press RETURN to stop");
			Console.ReadLine();

			_server.Stop();
		}

		private void PrintServerInfo()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var attributes = assembly.GetCustomAttributes(false);

			var title = attributes.OfType<AssemblyTitleAttribute>().FirstOrDefault();
			Console.Write(title == null ? "MiniHttp" : title.Title);

#if MINIMAL
			Console.Write(" (minimal)");
#endif

			var version = attributes.OfType<AssemblyVersionAttribute>().FirstOrDefault();
			Console.Write(" version {0}", assembly.GetName().Version);
			Console.WriteLine();

			var description = attributes.OfType<AssemblyDescriptionAttribute>().FirstOrDefault();
			if (description != null)
				Console.WriteLine(description.Description);
		}

		private int PermissionPrompt()
		{
			Console.WriteLine("MiniHttp does not have the necessary rights to run. Possible actions:");
			Console.WriteLine(" 1. Restart MiniHttp with elevated permissions (start as admin)");
			Console.WriteLine(" 2. Allow MiniHttp to configure the necessary rights using 'netsh' (permanent)");
			Console.Write("Choose: ");

			while (true)
			{
				var line = Console.ReadLine();
				int num;
				if (Int32.TryParse(line, out num))
				{
					if (num > 0 && num < 3)
						return num;
				}
				Console.WriteLine("Invalid selection '{0}'. Try again");
			}
		}

		private bool RegisterUrlAcl()
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "netsh.exe",
					Arguments = String.Format("http add urlacl url={0} user=Users", _server.ListenerPrefix),
					Verb = "runas",
					UseShellExecute = true,
				}
			};
			process.Start();
			process.WaitForExit();

			return process.ExitCode == 0;
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using System.IO;

namespace MiniHttp
{
	public class Arguments
	{
		private readonly OptionSet _options;

		public string WebRoot { get; private set; }
		public int Port { get; private set; }

		private Arguments()
		{
			SetDefaults();

			_options = new OptionSet()
			{
				{ "r|root=", "the web-root path (default: current directory)", v => WebRoot = v },
				{ "p|port=", "the port on which to run the server (default: 8888)", (int v) => Port = v },
			};
		}

		public override string ToString()
		{
			var writer = new StringWriter();

			writer.WriteLine("root: {0}", WebRoot);
			writer.WriteLine("port: {0}", Port);

			return writer.ToString();
		}

		private void SetDefaults()
		{
			WebRoot = Environment.CurrentDirectory;
			Port = 8888;
		}

		private void ParseArgs(string[] args)
		{
			var leftovers = _options.Parse(args);
			if (leftovers.Count > 0)
				WebRoot = leftovers.First();
		}

		public static Arguments Parse(string[] args)
		{
			var arguments = new Arguments();
			arguments.ParseArgs(args);
			return arguments;
		}
	}
}

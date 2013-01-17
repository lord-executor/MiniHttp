using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace MiniHttp.Server
{
    public class HttpServer
    {
        private readonly HttpListener _listener;
        private readonly Thread _dispatcherThread;
        private readonly List<RequestPreprocessor> _preporcessors;
        private readonly List<RequestPostprocessor> _postporcessors;
        private readonly List<RouteDefinition> _routes;

		public string ListenerPrefix { get; private set; }

        public HttpServer(int port)
        {
			ListenerPrefix = String.Format("http://+:{0}/", port);
            _listener = new HttpListener();
			_listener.Prefixes.Add(ListenerPrefix);

            _dispatcherThread = new Thread(Dispatch);
            _preporcessors = new List<RequestPreprocessor>();
            _postporcessors = new List<RequestPostprocessor>();
            _routes = new List<RouteDefinition>();
        }

        public void Start()
        {
            if (_listener.IsListening)
                throw new Exception("Already running");
            _listener.Start();
            _dispatcherThread.Start();
        }

        public void Stop()
        {
            _listener.Stop(); // stopping the listener causes an exception in the dispatcher
            _dispatcherThread.Join(5000);
        }

        public void RegisterPreprocessor(RequestPreprocessor preprocessor)
        {
            if (_listener.IsListening)
                throw new Exception("Already running");

            _preporcessors.Add(preprocessor);
        }

        public void RegisterPreprocessor(IRequestProcessor preprocessor)
        {
            RegisterPreprocessor(preprocessor.ProcessRequest);
        }

        public void RegisterPostprocessor(RequestPostprocessor postprocessor)
        {
            if (_listener.IsListening)
                throw new Exception("Already running");

            _postporcessors.Add(postprocessor);
        }

        public void RegisterPostprocessor(IRequestProcessor postprocessor)
        {
            RegisterPostprocessor(postprocessor.ProcessRequest);
        }

        public void RegisterRoute(string pathExpression, RequestHandler handler)
        {
			if (_listener.IsListening)
                throw new Exception("Already running");

            _routes.Add(new RouteDefinition(new Regex(pathExpression), handler));
        }

        public void RegisterRoute(string pathExpression, IRequestHandler handler)
        {
            RegisterRoute(pathExpression, handler.HandleRequest);
        }

        private void Dispatch()
        {
			while (_listener.IsListening)
            {
                try
                {
                    var context = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(threadContext => ProcessRequest(new RequestContext(context)));
                }
                catch (Exception e)
                {
					if (_listener.IsListening)
                        Console.WriteLine(e);
                }
            }
        }

        private void RunRoutes(RequestContext context)
        {
            if (!_routes.Any(route => route.TryRoute(context)))
            {
                context.Response.StatusCode = 500;
                Console.Error.WriteLine("    Unhandled request");
                throw new HttpServerException("Unhandled request");
            }
            else
            {
                Console.WriteLine("    HTTP {0} - {1}", context.Response.StatusCode, context.Response.ContentType);
            }
        }

        private void ProcessRequest(RequestContext context)
        {
            Console.WriteLine("{0} @ {1} {2}", context.Request.RemoteEndPoint.Address, context.Request.HttpMethod, context.Request.Url);

            context.Response.AddHeader("Cache-Control", "no-cache");

            try
            {
                _preporcessors.ForEach(p => p(context));

                try
                {
                    RunRoutes(context);
                }
                catch (Exception e)
                {
                    if (e is HttpListenerException)
                        throw;

                    context.Errors.Add(e);
                    LogException(context, e);
                }

                _postporcessors.ForEach(p => p(context));
            }
            catch (Exception e)
            {
                if (e is HttpListenerException && (e as HttpListenerException).ErrorCode == 64) // The specified network name is no longer available
                {
                    Console.Error.WriteLine("    ... request aborted.");
                    return;
                }
                LogException(context, e);
            }

            try
            {
                context.Response.Close();
            }
            catch (Exception e)
            {
                LogException(context, e);
            }
        }

        private void LogException(RequestContext context, Exception e)
        {
            Console.Error.WriteLine("    Unhandled exception");
            Console.Error.WriteLine(e);
            context.Response.StatusCode = 500;
        }
    }
}

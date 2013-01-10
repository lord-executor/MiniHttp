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
        private readonly List<RouteDefinition> _routes;
        private bool _running = false;

        public HttpServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://+:8888/");

            _dispatcherThread = new Thread(Dispatch);
            _routes = new List<RouteDefinition>();
        }

        public void Start()
        {
            if (_running)
                throw new Exception("Already running");
            _running = true;
            _listener.Start();
            _dispatcherThread.Start();
        }

        public void Stop()
        {
            _running = false;
            _listener.Stop(); // stopping the listener causes an exception in the dispatcher
            _dispatcherThread.Join(5000);
        }

        public void RegisterRoute(string pathExpression, RequestHandler handler)
        {
            if (_running)
                throw new Exception("Already running");

            _routes.Add(new RouteDefinition(new Regex(pathExpression), handler));
        }

        public void RegisterRoute(string pathExpression, IRequestHandler handler)
        {
            RegisterRoute(pathExpression, handler.HandleRequest);
        }

        private void Dispatch()
        {
            while (_running)
            {
                try
                {
                    var context = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(threadContext => ProcessRequest(context));
                }
                catch (Exception e)
                {
                    if (_running)
                        Console.WriteLine(e);
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine("{0} @ {1} {2}", context.Request.RemoteEndPoint.Address, context.Request.HttpMethod, context.Request.Url);

            try
            {
                if (!_routes.Any(route => route.TryRoute(context)))
                {
                    context.Response.StatusCode = 500;
                    Console.WriteLine("    Unhandled request");
                }
                else
                {
                    Console.WriteLine("    HTTP {0} - {1}", context.Response.StatusCode, context.Response.ContentType);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("    Unhandled exception");
                Console.WriteLine(e);
                context.Response.StatusCode = 500;
                context.Response.ContentLength64 = 0;
            }

            context.Response.Close();
        }
    }
}

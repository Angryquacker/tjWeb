using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TjWeb
{
    class RequestHandler
    {
        public HttpListener Listener;

        private List<HandlerFunction> GetFunctions = new List<HandlerFunction>();
        private List<HandlerFunction> PostFunctions = new List<HandlerFunction>();
        private List<HandlerFunction> PutFunctions = new List<HandlerFunction>();
        private List<HandlerFunction> DeleteFunctions = new List<HandlerFunction>();

        public RequestHandler(HttpListener Listener)
        {
            this.Listener = Listener;

            for (int i = 0;i < Server.Routes.Count;i++)
            {
                switch (Server.Routes[i].Type)
                {
                    case RouteType.GET:
                        GetFunctions.Add(Server.Routes[i]);
                        break;
                    case RouteType.POST:
                        PostFunctions.Add(Server.Routes[i]);
                        break;
                    case RouteType.PUT:
                        PutFunctions.Add(Server.Routes[i]);
                        break;
                    case RouteType.DELETE:
                        DeleteFunctions.Add(Server.Routes[i]);
                        break;
                }
            }
        }

        public void Handle()
        {
            HttpListenerContext context = Listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse HttpResponse = context.Response;
            ResponseObject response = new ResponseObject(HttpResponse);


            switch (request.HttpMethod)
            {
                case "GET":
                    for (int i = 0;i < GetFunctions.Count;i++)
                    {
                        if(request.RawUrl == GetFunctions[i].Route)
                        {
                            GetFunctions[i].Function(request, response);
                        }
                    }
                    break;
                case "POST":
                    for (int i = 0; i < PostFunctions.Count; i++)
                    {
                        if (request.RawUrl == PostFunctions[i].Route)
                        {
                            PostFunctions[i].Function(request, response);
                        }
                    }
                    break;
                case "PUT":
                    for (int i = 0; i < PutFunctions.Count; i++)
                    {
                        if (request.RawUrl == PutFunctions[i].Route)
                        {
                            PutFunctions[i].Function(request, response);
                        }
                    }
                    break;
                case "DELETE":
                    for (int i = 0; i < DeleteFunctions.Count; i++)
                    {
                        if (request.RawUrl == DeleteFunctions[i].Route)
                        {
                            DeleteFunctions[i].Function(request, response);
                        }
                    }
                    break;
            }

            Server.ActiveThreads--;
        }
    }
}
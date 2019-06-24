using System.Collections.Generic;
using System.Net;

namespace TjWeb
{
    class RequestHandler
    {
        //The HttpListener
        public HttpListener Listener;

        //The lists of each route type
        private List<HandlerFunction> GetFunctions = new List<HandlerFunction>();
        private List<HandlerFunction> PostFunctions = new List<HandlerFunction>();
        private List<HandlerFunction> PutFunctions = new List<HandlerFunction>();
        private List<HandlerFunction> DeleteFunctions = new List<HandlerFunction>();

        /*
         * Method -> Constructor [Sets each of the routes to the respected lists]
         * @Param (HttpListener) Listener -> The HttpListener to use
         * Returns - Void
         */ 
        public RequestHandler(HttpListener Listener)
        {
            //Set the listener
            this.Listener = Listener;

            //Go through each Route and add it to its respected lists of the correct type
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

        /*
         * Method -> Handle [Determines which function to run]
         * Returns -> Void
         */ 
        public void Handle()
        {
            //Get the Context, Request, and Response objects
            HttpListenerContext context = Listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse HttpResponse = context.Response;

            //Create a custom response object
            ResponseObject response = new ResponseObject(HttpResponse);


            //Determine which function to run, then run it
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

            //This class is now useless, so subtract one from the active thread count
            Server.ActiveThreads--;
        }
    }
}
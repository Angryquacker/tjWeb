using System.Collections.Generic;
using System.Net;

namespace TjWeb
{
    class RequestHandler
    {
        //The HttpListener
        public HttpListener Listener;

        //The lists of each route type
        private List<HandlerFunction> GetFunctions;
        private List<HandlerFunction> PostFunctions;
        private List<HandlerFunction> PutFunctions;
        private List<HandlerFunction> DeleteFunctions;

        /*
         * Method -> Constructor [Sets each of the routes to the respected lists]
         * @Param (HttpListener) Listener -> The HttpListener to use
         * Returns - Void
         */ 
        public RequestHandler(HttpListener Listener)
        {
            //Set the listener
            this.Listener = Listener;

            GetFunctions = Server.GetFunctions;
            PostFunctions = Server.PostFunctions;
            PutFunctions = Server.PutFunctions;
            DeleteFunctions = Server.DeleteFunctions;
        }

        /*
         * Method -> Handle [Determines which function to run]
         * Returns -> Void
         */ 
        public void Handle()
        {
            //This handler has been activate, and the class is now garbage, so subtract one from the active thread count
            Server.ActiveThreads--;

            //Get the Context, Request, and Response objects
            HttpListenerContext context = Listener.GetContext();
            HttpListenerRequest req = context.Request;
            HttpListenerResponse HttpResponse = context.Response;

            //Create a custom response and request object
            ResponseObject response = new ResponseObject(HttpResponse);
            RequestObject request = new RequestObject(req);

            //Get the true route string (allows for an addition of a query string)
            string temp = req.RawUrl.Split('?')[0].Replace("%20", "").TrimEnd();

            //Determine which function to run, then run it
            switch (req.HttpMethod)
            {
                case "GET":
                    for (int i = 0;i < GetFunctions.Count;i++)
                    {
                        if(temp == GetFunctions[i].Route)
                        {
                            GetFunctions[i].Function(request, response);
                        }
                    }
                    break;
                case "POST":
                    for (int i = 0; i < PostFunctions.Count; i++)
                    {
                        if (temp == PostFunctions[i].Route)
                        {
                            PostFunctions[i].Function(request, response);
                        }
                    }
                    break;
                case "PUT":
                    for (int i = 0; i < PutFunctions.Count; i++)
                    {
                        if (temp == PutFunctions[i].Route)
                        {
                            PutFunctions[i].Function(request, response);
                        }
                    }
                    break;
                case "DELETE":
                    for (int i = 0; i < DeleteFunctions.Count; i++)
                    {
                        if (temp == DeleteFunctions[i].Route)
                        {
                            DeleteFunctions[i].Function(request, response);
                        }
                    }
                    break;
            }            
        }
    }
}
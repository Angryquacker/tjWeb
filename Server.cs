using System;
using System.Net;
using System.Collections.Generic;
using System.Threading;

namespace TjWeb
{
    public class Server
    {
        //Main HttpListener to run the server
        private HttpListener Listener = new HttpListener();

        //A list of all the user-defined routes
        public static List<HandlerFunction> Routes = new List<HandlerFunction>();

        //The maximum threads allowed to run and the active threads
        private int MaxThreads = 1;
        public static int ActiveThreads = 0;

        //The amount of time to wait before checking the thread count
        public int LoopSpeed = 500;

        //The host address
        private String MainAddress;

        //The lists of each route type
        public static List<HandlerFunction> GetFunctions = new List<HandlerFunction>();
        public static List<HandlerFunction> PostFunctions = new List<HandlerFunction>();
        public static List<HandlerFunction> PutFunctions = new List<HandlerFunction>();
        public static List<HandlerFunction> DeleteFunctions = new List<HandlerFunction>();

        /*
         * Method -> Server Constructor
         * @Param (String) Address -> The host address of the server
         * @Param (int) Port -> The port to run the server on
         * Returns -> Void
         */
        public Server(String Address, int Port)
        {
            //If the last character of the address is a /, remove it (Prevents error for user-defined routes)
            if (Address[Address.Length - 1] == '/')
            {
                Address = Address.Substring(0, Address.Length - 1);
            }

            //Add the port to the host address
            MainAddress = Address += ":" + Port;
        }

        /*
         * Method -> AddRoute [Adds a user-defined route]
         * @Param (RouteType) Type -> The type of route to create [GET, POST, PUT, DELETE]
         * @Param (String) Route -> The route of the request
         * @Param (Action<HttpListenerRequest, ResponseObject>) Function -> The function to run when the route is found
         * Returns -> Void
         */
        public void AddRoute(RouteType Type, String Route, Action<RequestObject, ResponseObject> Function)
        {
            //Add the route
            Routes.Add(new HandlerFunction(Type, Function, Route));

            switch (Routes[Routes.Count - 1].Type)
            {
                case RouteType.GET:
                    GetFunctions.Add(Server.Routes[Routes.Count - 1]);
                    break;
                case RouteType.POST:
                    PostFunctions.Add(Server.Routes[Routes.Count - 1]);
                    break;
                case RouteType.PUT:
                    PutFunctions.Add(Server.Routes[Routes.Count - 1]);
                    break;
                case RouteType.DELETE:
                    DeleteFunctions.Add(Server.Routes[Routes.Count - 1]);
                    break;
            }
        }

        /*
         * Method -> SetMaxThreads [Sets the maximum threads allowed to run]
         * @Param (int) max -> The maximum number of threads allowed to run
         * Retunrns -> Void
         */ 
        public void SetMaxThreads(int max)
        {
            //Set the max thread count
            MaxThreads = max;
        }

        /*
         * Method -> SetThreadLoopTimeout [Sets the time to wait inbetween thread checks]
         * @Param (int) WaitTime -> The time to wait
         * Returns -> Void
         */ 
        public void SetThreadLoopTimeout(int WaitTime)
        {
            //Set the time to wait inbetween thread checks
            LoopSpeed = WaitTime;
        } 

        /*
         * Method -> HandlerLoop [Makes sure all the Threads are Active]
         * Returns -> Void
         */ 
        private void HandlerLoop()
        {
            //If not all of the threads are active...
            if(MaxThreads != ActiveThreads)
            {
                //Collect unused objects and...
                GC.Collect();

                for (int i = 0;i < MaxThreads - ActiveThreads;i++)
                {
                    //Start a new thread with a new handler
                    RequestHandler tempHandler = new RequestHandler(Listener);
                    Thread temp = new Thread(tempHandler.Handle);
                    temp.Start();

                    //Null-terminate the active handler to save space
                    tempHandler = null;

                    //Add one to the active threads
                    ActiveThreads++;
                }
            }

            //Wait the specified time, then restart the check
            Thread.Sleep(LoopSpeed);
            HandlerLoop();
        }

        /*
         * Method -> Start [Starts the server]
         * Returns -> Void
         */ 
        public void Start()
        {
            //For each of the routes...
            foreach (HandlerFunction Prefix in Routes)
            {
                //If the route does not end in a /, add one, then add the route to the HttpListener Prefixes
                if (Prefix.Route[Prefix.Route.Length -1] != '/')
                {
                    Listener.Prefixes.Add(MainAddress + Prefix.Route + "/");
                } 
                else
                {
                    Listener.Prefixes.Add(MainAddress + Prefix.Route);
                }
            }

            //Start the Listener
            Listener.Start();
            
            //Start the thread loop in a new thread
            Thread HandlerLoop = new Thread(this.HandlerLoop);
            HandlerLoop.Start();
        }

        /*
         * Method -> Server Destructor [Closes the listener]
         * Returns -> Void
         */ 
        ~Server()
        {
            //Stop the Listener
            Listener.Close();
        }
    }
}

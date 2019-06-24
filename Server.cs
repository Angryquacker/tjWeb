using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TjWeb
{
    public class Server
    {
        private HttpListener Listener = new HttpListener();

        public static List<HandlerFunction> Routes = new List<HandlerFunction>();
        private int MaxThreads = 1;
        public static int ActiveThreads = 0;

        private String MainAddress;

        public Server(String Address, int Port)
        {
            if (Address[Address.Length - 1] == '/')
            {
                Address = Address.Substring(0, Address.Length - 1);
            }

            MainAddress = Address += ":" + Port;
        }

        public void AddRoute(RouteType Type, String Route, Action<HttpListenerRequest, ResponseObject> Function)
        {
            Routes.Add(new HandlerFunction(Type, Function, Route));
        }

        public void SetMaxThreads(int max)
        {
            MaxThreads = max;
        }

        private void HandlerLoop()
        {
            if(MaxThreads != ActiveThreads)
            {
                GC.Collect();

                for (int i = 0;i < MaxThreads - ActiveThreads;i++)
                {
                    RequestHandler tempHandler = new RequestHandler(Listener);
                    Thread temp = new Thread(tempHandler.Handle);
                    temp.Start();
                    ActiveThreads++;
                }
            }

            Console.WriteLine("Active Threads: {0}", ActiveThreads);
            Thread.Sleep(500);
            HandlerLoop();
        }

        public void Start()
        {
            foreach (HandlerFunction Prefix in Routes)
            {

                if (Prefix.Route[Prefix.Route.Length -1] != '/')
                {
                    Listener.Prefixes.Add(MainAddress + Prefix.Route + "/");
                } 
                else
                {
                    Listener.Prefixes.Add(MainAddress + Prefix.Route);
                }
            }

            Listener.Start();
            Thread HandlerLoop = new Thread(this.HandlerLoop);
            HandlerLoop.Start();
        }

        ~Server()
        {
            Listener.Close();
        }
    }
}

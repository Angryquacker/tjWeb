using System;
using TjWeb;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TjWebTest
{
    class Test
    {
        static void Main(string[] args)
        {
            Server TestServer = new Server("http://localhost", 8080);

            TestServer.SetMaxThreads(100);

            TestServer.AddRoute(RouteType.GET, "/", (HttpListenerRequest req, ResponseObject res) =>
            {
                res.SetStatus(200, "OK");
                res.SetHeader("Content-Type", "text/html");

                res.AddText("<h1>Just a Test, Man</h1>");
                res.Send();
            });

            TestServer.Start();
        }
    }
}

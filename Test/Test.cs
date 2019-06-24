using TjWeb;
using System.Net;

namespace TjWebTest
{
    class Test
    {
        static void Main(string[] args)
        {
            //Create an example server running on the localhost on port 8080
            Server TestServer = new Server("http://localhost", 8080);

            //Set the maximum number of threads to 100
            TestServer.SetMaxThreads(100);

            //Set the time inbetween thread checks to 550 ms
            TestServer.SetThreadLoopTimeout(550);

            //Add a new route of type GET, on route "/"
            TestServer.AddRoute(RouteType.GET, "/", (HttpListenerRequest req, ResponseObject res) =>
            {
                //Set the status of the request
                res.SetStatus(200, "OK");

                //Set the content type to  text/html
                res.SetHeader("Content-Type", "text/html");

                //Add the simple html string then send it
                res.AddText("<h1>Just a Test, Man</h1>");
                res.Send();
            });

            //Start the server
            TestServer.Start();
        }
    }
}

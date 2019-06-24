# TjWeb - A Simple and LightWeight Web Framework

###Version 1.0.0 - First Release

##Creating The Server
- To create the server, create a new server object
	- `Server Test = new Server(String address, int port);` -> address [host address of the server], port [port to run the server on]
	- Ex. `Server Test = new Server("http://localhost", 8080);` (Will start a server on "http://localhost:8080")

- To set the maximum number of threads (This will determine the max number of concurrent requests), use the Set max thread method [Default 1]
	- `Test.SetMaxThreads(int max);` -> max [maximum number of threads allowed]
	- Ex. `Test.SetMaxThreads(50);`

- To set the time in between checking for the thread count, use the set interval method [Defaault 500ms]
	- `Test.SetThreadLoopTimeout(int WaitTime)` -> WaitTime [The time to wait inbetween thread checks (Time is in ms)]
	- Ex. `Test.SetThreadLoopTimeout(550);`

##Adding a Route
- To Add a route to the server, use the AddRoute method
	- `Test.AddRoute(RouteType Route, String Route, Action<HttpListenerRequest, ResponseObject> Function)` -> Type [The type of route to accpet (RouteType.GET, RouteType.POST, RouteType.PUT, RouteType.DELETE)], Route [The route to accept], Function [A method/lambda accepting two paramaters of type HttpListenerRequest, and ResponseObject]
	- Ex. `Test.AddRoute(RouteType.GET, "/", (HttpListenerRequest req, ResponseObject res) => { /*Do stuff here */ });`

##Sending a Response 
- To Send a response, first create the response, then send it

- To Set the status code and message of a response, use the SetStatus method
	- `res.SetStatus(int StatusCode, String Message);` -> StatusCode [The status code of the response], Message [The description of the status]
	- Ex. `res.SetStatus(200, "OK");`

- To Add a header, use the SetHeader method
	- `res.SetHeader(String header, String value);` -> header [The header to set], value [The value to set that header]
	- Ex. `res.SetHeader("Content-Type", "text/html");`

- To Add an HTML file to the response, use the AddFile method
	- `res.AddFile(String FilePath);` -> FilePath [The path of the file to send]
	- Ex. `res.AddFile("./test.html");`

- To Add text to the response, use the AddText method
	- `res.AddText(String Text);` -> Text [The Text to send as a response]
	- Ex. `res.AddText("<h1>Test Text</h1>");`

- Finally, to send the response, simply do this: `res.Send();`

##Starting the Server
- To Start the server, simply use the start method
	- `Test.Start();`

##Example Server
- An exmaple of a working web server as seen in the text file `./Test/Test.cs`

`
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
`
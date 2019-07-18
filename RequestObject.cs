using System;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TjWeb
{
    public class RequestObject
    {
        public readonly HttpListenerRequest Req;

        public JObject Body;
        public JObject Query;
        public CookieCollection Cookies;
        public String[] AcceptTypes;
        public Encoding ContentEncoding;
        public String ContentType;
        public int HashCode;
        public JObject Headers;
        public RouteType HttpMethod;
        public bool IsAuthenticated;
        public bool SslSecure;
        public bool IsWebSocket;
        public IPEndPoint LocalEndpoint;
        public Version HttpVersion;
        public IPEndPoint OriginEndpoint;
        public String SPN;
        public TransportContext TransContext;
        public Uri URI;
        public String UserAgent;
        public String[] UserLanguages;

        public RequestObject(HttpListenerRequest Req)
        {
            //Set the request object
            this.Req = Req;

            //Set the request body
            System.IO.Stream sin = Req.InputStream;
            System.Text.Encoding encoding = Req.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(sin, encoding);

            //Read the body data
            String ReqString = reader.ReadToEnd();

            //Make sure the body exists
            if(ReqString.Length > 0)
            {
                //Parse the body into JSON
                Body = JObject.Parse(ReqString);
            }

            //Set the query string
            Query = new JObject();
            for (int i = 0;i < Req.QueryString.Count;i++)
            {
                //Create a temp array to hold the values of that key
                JArray data = new JArray();

                //add each value to the temp array
                foreach (String value in Req.QueryString.GetValues(i))
                {
                    data.Add(value);
                }

                //add the key to the query object
                Query[Req.QueryString.GetKey(i)] = data;
            }

            //Set the cookies
            Cookies = Req.Cookies;

            //Set the accept types
            AcceptTypes = Req.AcceptTypes;

            //Set the content encoding
            ContentEncoding = Req.ContentEncoding;

            //Set the content type
            ContentType = Req.ContentType;

            //Set the hasd code
            HashCode = Req.GetHashCode();

            //Set the headers
            Headers = new JObject();
            for (int i = 0;i < Req.Headers.Count;i++)
            {
                //create a temp array to hold the values of that key
                JArray data = new JArray();

                //add each value to the temp array
                foreach (String value in Req.Headers.GetValues(i))
                {
                    data.Add(value);
                }

                //add the key to the headers object
                Headers[Req.Headers.GetKey(i)] = data;
            }

            //Set the HTTP Method
            switch(Req.HttpMethod.ToLower())
            {
                case "get":
                    HttpMethod = RouteType.GET;
                    break;
                case "post":
                    HttpMethod = RouteType.POST;
                    break;
                case "put":
                    HttpMethod = RouteType.PUT;
                    break;
                case "delete":
                    HttpMethod = RouteType.DELETE;
                    break;
            }

            //Set the authentication status
            IsAuthenticated = Req.IsAuthenticated;

            //Set the secure status
            SslSecure = Req.IsSecureConnection;

            //Set the websocket status
            IsWebSocket = Req.IsWebSocketRequest;

            //Set the local endpoint
            LocalEndpoint = Req.LocalEndPoint;

            //Set the http protocal version
            HttpVersion = Req.ProtocolVersion;

            //Set the request location
            OriginEndpoint = Req.RemoteEndPoint;

            //Set the SPN (Service Provider Name)
            SPN = Req.ServiceName;

            //Set the Transport Context
            TransContext = Req.TransportContext;

            //Set the URI (Unifrom Resource Identitfier) 
            URI = Req.UrlReferrer;

            //Set the user agent
            UserAgent = Req.UserAgent;

            //Set the perfered languages
            UserLanguages = Req.UserLanguages;
        }
    }
}

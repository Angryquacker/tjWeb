using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TjWeb
{
    public class HandlerFunction
    {
        public RouteType Type;
        public Action<HttpListenerRequest, ResponseObject> Function;
        public String Route;
        public List<String> WildCards = new List<String>();

        public HandlerFunction(RouteType Type, Action<HttpListenerRequest, ResponseObject> Function, String Route)
        {
            this.Type = Type;
            this.Function = Function;
            this.Route = Route;
        }

        public void AddWildCard(String WildCard)
        {
            WildCards.Add(WildCard);
        }
    }
}

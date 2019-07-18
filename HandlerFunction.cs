using System;
using System.Net;

namespace TjWeb
{
    public class HandlerFunction
    {
        //The type of route to accept
        public RouteType Type;

        //The function to run if the route is found
        public Action<RequestObject, ResponseObject> Function;

        //The route to accept
        public String Route;

        /*
         * Method -> Constructor [Sets the values of all the variables]
         * @Param (RouteType) Type -> The type of request to accept
         * @Param (Action<HttpListenerRequest, ResponseObject>) Function -> The Function to run if the route is found
         * @Param (String) Route -> The route to accept
         */
        public HandlerFunction(RouteType Type, Action<RequestObject, ResponseObject> Function, String Route)
        {
            //Set the values
            this.Type = Type;
            this.Function = Function;
            this.Route = Route;
        }
    }
}

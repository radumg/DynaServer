using Nancy;
using System;
using System.Windows;

namespace DynamoServer.Server
{
    /// <summary>
    /// This module only handles requests that target the /test endpoints.
    /// </summary>
    public class TestModule : NancyModule
    {
        public TestModule() : base("/test")
        {
            Get["/index"] = x =>
            {
                //MessageBox.Show("just got a request");
                return Response.AsFile("Views/index.html", "text/html");
            };

            Get["/view"] = _ =>  View["index.html"];

            Get["/time"] = x => { return "Hello, it is now " + DateTime.Now.ToLongTimeString(); };

            // would capture routes like /hello/Dynamo sent as a GET request
            Get["/hello/{name}"] = parameters =>
            {
                return "Hello " + parameters.name;
            };

            // would capture routes like /users/192/add/moderator sent as a POST request
            Post["/users/{id}/add/{category}"] = parameters =>
            {
                return HttpStatusCode.OK;
            };

            // separate interface from implementation
            Get["/favoriteNumber/{value:int}"] = FavoriteNumber;
        }

        private dynamic FavoriteNumber(dynamic parameters)
        {
            return "So your favorite number is " + parameters.value + "?";
        }

    }
}

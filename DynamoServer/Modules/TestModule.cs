using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace DynamoServer
{
    /// <summary>
    /// This module only handles requests that target the /test endpoints.
    /// </summary>
    public class TestModule : NancyModule
    {
        public TestModule() : base("/test")
        {
            Get["/"] = x => { return "Hello, it is now " + DateTime.Now.ToLongTimeString(); };

            // would capture routes like /hello/Dynamo sent as a GET request
            Get["/hello/{name}"] = parameters => {
                return "Hello " + parameters.name;
            };

            // would capture routes like /users/192/add/moderator sent as a POST request
            Post["/users/{id}/add/{category}"] = parameters => {
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

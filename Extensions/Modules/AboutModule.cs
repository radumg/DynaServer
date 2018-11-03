using Nancy;
using System.Reflection;

namespace DynamoServer.Server
{
    /// <summary>
    /// This module only handles requests that target the /about endpoints.
    /// </summary>
    public class AboutModule : NancyModule
    {
        public AboutModule() : base("/about")
        {
            Get["/"] = x =>
            {
                var info =
                    "<h2>DynamoServer</h2></br>" +
                    "Assembly : " + Assembly.GetExecutingAssembly().FullName + "</br>" +
                    "Version : " + Assembly.GetExecutingAssembly().GetName().Version + "</br></br>" +
                    "<h2>Dynamo</h2></br>" +
                    "Version : " + Dynamo.Utilities.AssemblyHelper.GetDynamoVersion();

                return Response.AsText(info,"text/html");
            };
        }
    }
}

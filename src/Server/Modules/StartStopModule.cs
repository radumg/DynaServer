using Nancy;

namespace DynaServer.Server
{
    public class StartStopModule : NancyModule
    {
        public StartStopModule() : base("/")
        {
            Get["/"] = x =>
            {
                return Response.AsFile("extra/start.html", "text/html");
            };

            Get["/stop"] = x =>
            {
                DynamoWebServer.CurrentInstance.Stop();
                return Response.AsFile("extra/stop.html", "text/html");
            };
        }
    }
}

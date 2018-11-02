using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;

namespace DynamoServer
{
    public class DynamoWebServer
    {
        public const string URL_BASE ="http://localhost:1234";

        private NancyHost server;
        public HostConfiguration serverConfig;

        public DynamoWebServer()
        {
            serverConfig = new HostConfiguration();
            serverConfig.UrlReservations.CreateAutomatically = true;

            server = new NancyHost(serverConfig, new Uri(URL_BASE));
        }

        public void Start()
        {
            Console.WriteLine("Starting web service on " + URL_BASE);
            server.Start();
        }

        public void Stop()
        {
            Console.WriteLine("Stopping web service on " + URL_BASE);
            server.Stop();
            server.Dispose();
        }
    }
}

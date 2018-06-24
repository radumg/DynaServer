using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using Topshelf;

namespace DynamoServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServerRunner.Run();
            
            Console.WriteLine("Running on " + DynamoWebServer.URL_BASE);
            Console.WriteLine("Opening web pages...");
            Process.Start(DynamoWebServer.URL_BASE.ToString());
            Process.Start(DynamoWebServer.URL_BASE + "/hello/radu" + DateTime.Now.ToShortTimeString());
            Process.Start(DynamoWebServer.URL_BASE + "/favoriteNumber/13");
            Process.Start(DynamoWebServer.URL_BASE + "/json");

            Console.WriteLine("Press any key to exit...");
            ServerRunner.Stop();
        }
    }
}


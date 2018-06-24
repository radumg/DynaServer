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
            Console.WriteLine("Starting web service on " + DynamoWebServer.URL_BASE);
            Task.Run(async () => { ServiceRunner.Run(); }).Wait();

            Console.WriteLine("DynamoServer terminated at " + DateTime.UtcNow);
        }
    }
}


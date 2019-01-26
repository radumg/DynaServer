using Nancy.Hosting.Self;
using System;
using System.Diagnostics;
using System.IO;

namespace DynaServer.Server
{
    public class DynamoWebServer : IDisposable
    {
        private const string DEFAULT_URL_BASE = "http://localhost:1234";

        private NancyHost server;
        private HostConfiguration serverConfig;

        internal static DynamoWebServer CurrentInstance;

        public string RootPath { get; private set; }
        public string UrlBase { get; private set; }
        public bool IsRunning { get; private set; }
        public bool FailedToStart { get; private set; }


        public DynamoWebServer()
        {
            UrlBase = DEFAULT_URL_BASE;

            serverConfig = new HostConfiguration();
            serverConfig.UrlReservations.CreateAutomatically = true;
            var bootstrapper = new Bootstrapper();

            server = new NancyHost(bootstrapper, serverConfig, new Uri(UrlBase));

            // set initial state
            IsRunning = false;
            FailedToStart = false;
            CurrentInstance = this;
            RootPath = bootstrapper.rootPathProvider.GetRootPath();
        }

        public DynamoWebServer(string urlbase) : base()
        {
            if (!string.IsNullOrWhiteSpace(urlbase)) UrlBase = urlbase;
        }

        public void Start()
        {
            Console.WriteLine("Starting web service on " + UrlBase);
            try
            {
                server.Start();
                IsRunning = true;

                // open the server base url in browser
                Process.Start(UrlBase);
            }
            catch (Exception)
            {
                IsRunning = false;
                FailedToStart = false;
            }
        }

        public void Stop()
        {
            if (!this.IsRunning) return;

            Console.WriteLine("Stopping web service on " + UrlBase);
            try
            {
                server.Stop();
                IsRunning = false;
            }
            catch (Exception)
            {
                // silent ignore for now.
            }
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }

}

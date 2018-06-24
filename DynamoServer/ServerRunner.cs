using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoServer;
using Topshelf;

namespace DynamoServer
{
    public static class ServerRunner
    {
        public static bool Run()
        {
            var result = HostFactory.Run(x =>
            {
                x.Service<DynamoWebServer>(s =>
                {
                    s.ConstructUsing(name => new DynamoWebServer());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("DynamoServer");
                x.SetDescription("Service that hosts the web server required to interact with DynamoServer extension.");
                x.StartManually();
                x.RunAsLocalService();
                x.EnablePauseAndContinue();
                x.EnableShutdown();
            });
            if (result != 0) return false;
            return true;
        }

        public static bool Stop()
        {

            return true;
        }
    }
}

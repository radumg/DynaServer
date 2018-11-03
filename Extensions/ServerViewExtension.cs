using Dynamo.Logging;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynamoServer.Server;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DynamoServer.Extensions
{
    public class ServerViewExtension : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "Dynamo Server View Extension";

        internal ViewLoadedParams viewLoadedParams;
        internal DynamoViewModel dynamoViewModel => viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;
        internal static DynamoLogger DynamoLogger;
        internal static DynamoWebServer Server = null;

        public ServerViewExtension()
        {
            if (Server == null) Server = new DynamoWebServer();
        }

        public void Startup(ViewStartupParams vsp) { }

        public void Loaded(ViewLoadedParams vlp)
        {
            MessageBox.Show($"[ {DateTime.Now} ] : {this.Name} is ready!");

            // hold a reference to the Dynamo params to be used later
            viewLoadedParams = vlp;
            DynamoLogger = dynamoViewModel.Model.Logger;

            Events.RegisterEventHandlers(this);

            // we can now add custom menu items to Dynamo's UI
            UI.MakeMenuItems(this);
        }

        public static async Task StartServerAsync()
        {
            var message = $"[ {DateTime.Now} ] : Starting server on machine {Environment.MachineName}";

            // start server and continue execution
            await Task.Run(() => Server.Start());

            var confirmation = Server.IsRunning == true ? "Started ok" : "Something went wrong, server did not start ok.";
            confirmation = $"[ {DateTime.Now} ] : " + confirmation;
            ServerViewExtension.DynamoLogger.LogNotification("ServerViewExtension", "Dynamo Server START", message, confirmation);
        }

        public static async Task StopServerAsync()
        {
            var message = $"[ {DateTime.Now} ] : Stopping server on machine {Environment.MachineName}";

            // stop server and continue execution
            await Task.Run(() => Server.Stop());

            var confirmation = Server.IsRunning == false ? "Stopped ok" : "Something went wrong, server is still running.";
            confirmation = $"[ {DateTime.Now} ] : " + confirmation;
            ServerViewExtension.DynamoLogger.LogNotification("ServerViewExtension", "Dynamo Server STOP", message, confirmation);
        }

        public static string GetServerStatus()
        {
            return "Server is running : " + Server.IsRunning;
        }

        public void Shutdown()
        {
            ServerViewExtension.StopServerAsync();
            Events.UnregisterEventHandlers(this);
        }

        public void Dispose() { }
    }
}

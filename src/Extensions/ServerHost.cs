using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Dynamo.Logging;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynaServer.Server;

namespace DynaServer.Extensions
{
    public static class ServerHost
    {
        private static ViewLoadedParams dynamoViewLoadedParams = null;
        internal static DynamoViewModel dynamoViewModel = null;
        internal static DynamoLogger DynamoLogger = null;
        internal static Dispatcher dispatcher;
        public static string DynamoVersion => dynamoViewModel.Version;
        public static string DynamoHostVersion => dynamoViewModel.HostVersion;

        internal static DynamoWebServer Server = null;


        static ServerHost()
        {
            if (Server == null) Server = new DynamoWebServer();
        }

        public static void OnViewModelStartup()
        {
            DynamoLogger = dynamoViewModel.Model.Logger;
        }

        public static void OnViewModelReady(ViewLoadedParams viewModelParams, Dispatcher viewModelDispatcher)
        {
            // hold a reference to the Dynamo params to be used later
            if (dynamoViewLoadedParams == null) dynamoViewLoadedParams = viewModelParams;
            dynamoViewModel = dynamoViewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

            Events.RegisterEventHandlers();

            // add Dynamo Server menu to Dynamo UI
            ServerHost.dynamoViewLoadedParams.dynamoMenu.Items.Add(DynaServer.Extensions.UI.DynamoServerMenu);

            // hold reference to thread so we can call methods from web server thread
            ServerHost.dispatcher = Dispatcher.CurrentDispatcher;


        }

        public static async Task StartServerAsync()
        {
            var watch = new Stopwatch();
            watch.Start();
            var message = $"[ {DateTime.Now} ] : Starting server on machine {Environment.MachineName}";

            // start server and continue execution
            await Task.Run(() => Server.Start());
            watch.Stop();

            // log results
            var confirmation = Server.IsRunning == true ? "Started ok" : "Something went wrong, server did not start ok.";
            confirmation = $"[ {DateTime.Now} ] : " + confirmation + ", in " + watch.ElapsedMilliseconds + "ms";
            ServerViewExtension.DynamoLogger.LogNotification("ServerViewExtension", "Dynamo Server START", message, confirmation);
        }

        public static async Task StopServerAsync()
        {
            // first open the shutting down server page.
            // yes, i prioritise UX over speed here, deal with it.
            await Task.Run(() => Process.Start(Server.UrlBase + "/stop"));
            Thread.Sleep(3000);

            var message = $"[ {DateTime.Now} ] : Stopping server on machine {Environment.MachineName}";

            // stop server and continue execution
            await Task.Run(() => Server.Stop());

            // log results
            var confirmation = Server.IsRunning == false ? "Stopped ok" : "Something went wrong, server is still running.";
            confirmation = $"[ {DateTime.Now} ] : " + confirmation;
            ServerViewExtension.DynamoLogger.LogNotification("ServerViewExtension", "Dynamo Server STOP", message, null);
        }

        public static string GetServerStatus()
        {
            var running = Server.IsRunning ? $"RUNNING on machine {Environment.MachineName}" : "NOT RUNNING";
            var message = $"[ {DateTime.Now} ] : Server is {running}";
            ServerViewExtension.DynamoLogger.Log(message);

            return message;
        }

        public async static void Shutdown()
        {
            await StopServerAsync();
            Events.UnregisterEventHandlers();
        }

        #region Utilities

        public static void RunInContext(Action action)
        {
            dispatcher.Invoke(action);
        }

        private static Task<int> RunProcessAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<int>();

            var process = new Process
            {
                StartInfo = { FileName = fileName },
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }

        #endregion

    }
}

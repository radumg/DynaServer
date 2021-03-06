﻿using Dynamo.Logging;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynaServer.Server;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DynaServer.Extensions
{
    public class ServerViewExtension : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "DynaServer View Extension";
        public string DynamoVersion => dynamoViewModel.Version;
        public string DynamoHostVersion => dynamoViewModel.HostVersion;

        internal static ViewLoadedParams viewLoadedParams = null;
        internal static DynamoViewModel dynamoViewModel => viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;
        internal static DynamoLogger DynamoLogger => dynamoViewModel.Model.Logger;
        internal static DynamoWebServer Server = null;
        private static Dispatcher dispatcher;

        public ServerViewExtension()
        {
            if (Server == null) Server = new DynamoWebServer();
        }

        public void Startup(ViewStartupParams vsp) { }

        public void Loaded(ViewLoadedParams vlp)
        {
            // hold a reference to the Dynamo params to be used later
            if (viewLoadedParams == null) viewLoadedParams = vlp;

            Events.RegisterEventHandlers();

            // add Dynamo Server menu to Dynamo UI
            viewLoadedParams.dynamoMenu.Items.Add(DynaServer.Extensions.UI.DynamoServerMenu);

            // hold reference to thread so we can call methods from web server thread
            dispatcher = Dispatcher.CurrentDispatcher;
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
            var message = $"[ {DateTime.Now} ] : Stopping server on machine {Environment.MachineName}";

            // manually open the stop webpage from file since the server will likely be stopped before endpoint response
            Process.Start(Path.Combine(Server.RootPath, "extra/stop.html"));
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

        public async void Shutdown()
        {
            await ServerViewExtension.StopServerAsync();
            Events.UnregisterEventHandlers();
        }

        public void Dispose() { }

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

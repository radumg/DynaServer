using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes.CustomNodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Interfaces;
using Dynamo.Library;
using Dynamo.Logging;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynaServer.Server;

namespace DynaServer.Extensions
{
    public static class ServerHost
    {
        // Dynamo Model properties
        private static ReadyParams dynamoModelReadyParams = null;
        internal static IPreferences DynamoPreferences = null;
        internal static IPathManager DynamoPathManager = null;
        internal static ILibraryLoader DynamoLibraryLoader = null;
        internal static ICustomNodeManager DynamoCustomNodeManager = null;
        internal static IWorkspaceModel DynamoCurrentWorkspaceModel = null;
        internal static ICommandExecutive DynamoCommandExecutive = null;
        public static bool DynamoModelAvailable = false;

        // Dynamo ViewModel properties
        private static ViewLoadedParams dynamoViewLoadedParams = null;
        private static Dispatcher dispatcher=null;
        internal static DynamoModel DynamoModel;
        internal static Menu DynamoMenu = null;
        internal static DynamoViewModel DynamoViewModel = null;
        internal static DynamoLogger DynamoLogger = null;
        internal static IExtensionManager DynamoExtensionManager = null;
        public static string DynamoVersion => DynamoViewModel.Version;
        public static string DynamoHostVersion => DynamoViewModel.HostVersion;
        public static bool DynamoViewModelAvailable = false;

        // Dynamo Server
        internal static DynamoWebServer Server = null;

        static ServerHost()
        {
            if (Server == null) Server = new DynamoWebServer();
        }

        #region Dynamo headless

        public static void OnModelStartup(StartupParams sp)
        {
            DynamoPreferences = sp.Preferences;
            DynamoPathManager = sp.PathManager;
            DynamoLibraryLoader = sp.LibraryLoader;
            DynamoCustomNodeManager = sp.CustomNodeManager;
        }

        public static void OnModelReady(ReadyParams rp)
        {
            dynamoModelReadyParams = rp;
            DynamoCurrentWorkspaceModel = rp.CurrentWorkspaceModel;
            DynamoCommandExecutive = rp.CommandExecutive;
            DynamoModelAvailable = true;
        }

        #endregion

        #region Dynamo with User Interface

        public static void OnViewModelStartup(ViewStartupParams vsp)
        {
            DynamoExtensionManager = vsp.ExtensionManager;
        }

        public static void OnViewModelReady(ViewLoadedParams viewModelParams, Dispatcher viewModelDispatcher)
        {
            // hold a reference to the Dynamo params to be used later
            dynamoViewLoadedParams = viewModelParams;
            DynamoViewModel = dynamoViewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;
            DynamoModel = DynamoViewModel.Model;
            DynamoLogger = DynamoViewModel.Model.Logger;

            // add Dynamo Server menu to Dynamo UI
            DynamoMenu = dynamoViewLoadedParams.dynamoMenu;
            DynamoMenu.Items.Add(UI.DynamoServerMenu);

            // hold reference to thread so we can execute on same thread as Dynamo UI
            ServerHost.dispatcher = Dispatcher.CurrentDispatcher;

            DynamoViewModelAvailable = true;
        }

        internal static void OnViewShutdown()
        {
            throw new NotImplementedException();
        }

        public static void RunInDynamoUIContext(Action action)
        {
            dispatcher.Invoke(action);
        }

        #endregion

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
            DynamoLogger.LogNotification("DynaServer", "Dynamo Server START", message, confirmation);
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
            DynamoLogger.LogNotification("DynaServer", "Dynamo Server STOP", message, null);
        }

        public static string GetServerStatus()
        {
            var running = Server.IsRunning ? $"RUNNING on machine {Environment.MachineName}" : "NOT RUNNING";
            var message = $"[ {DateTime.Now} ] : Server is {running}";
            DynamoLogger.Log(message);

            return message;
        }

        public async static void Shutdown()
        {
            await StopServerAsync();
        }

        #region Utilities

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

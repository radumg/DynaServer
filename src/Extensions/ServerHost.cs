using System;
using System.Diagnostics;
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
        // extensions
        public static ServerViewExtension ViewExtension = null;
        public static ServerExtension Extension = null;

        // Dynamo Model properties
        private static ReadyParams dynamoModelReadyParams = null;
        internal static DynamoModel DynamoModel;
        internal static IPreferences DynamoPreferences = null;
        internal static IPathManager DynamoPathManager = null;
        internal static ILibraryLoader DynamoLibraryLoader = null;
        internal static ICustomNodeManager DynamoCustomNodeManager = null;
        internal static IWorkspaceModel DynamoCurrentWorkspaceModel = null;
        internal static ICommandExecutive DynamoCommandExecutive = null;
        public static bool DynamoModelAvailable = false;

        // Dynamo ViewModel properties
        public static string DynamoVersion { get; private set; }
        public static string DynamoHostVersion { get; private set; }
        public static bool DynamoViewModelAvailable = false;

        // Dynamo Server
        internal static DynamoWebServer Server = null;

        static ServerHost()
        {
            if (Server == null) Server = new DynamoWebServer();
        }

        public static void UpdateViewExtension(ServerViewExtension ext)
        {
            if(ext!=null) ViewExtension = ext;
        }

        public static void UpdateExtension(ServerExtension ext)
        {
            if (ext != null) Extension = ext;
        }

        public static void RunOnDynamoViewModel(Action action)
        {
            ViewExtension.RunInDynamoUIContext(action);
        }

        #region Dynamo headless

        public static void OnModelStartup(StartupParams sp)
        {
            DynamoPreferences = sp.Preferences;
            DynamoLibraryLoader = sp.LibraryLoader;
            DynamoCustomNodeManager = sp.CustomNodeManager;
        }

        public static void OnModelReady(ReadyParams rp)
        {
            dynamoModelReadyParams = rp;
            DynamoCommandExecutive = rp.CommandExecutive;

            if (DynamoModel != null) DynamoModelAvailable = true;
        }

        #endregion

        #region Dynamo with User Interface

        public static void UpdateViewModelStatus(bool status)
        {
           DynamoViewModelAvailable = status;
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
            //DynamoLogger.LogNotification("DynaServer", "Dynamo Server START", message, confirmation);
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
            //DynamoLogger.LogNotification("DynaServer", "Dynamo Server STOP", message, null);
        }

        public static string GetServerStatus()
        {
            var running = Server.IsRunning ? $"RUNNING on machine {Environment.MachineName}" : "NOT RUNNING";
            var message = $"[ {DateTime.Now} ] : Server is {running}";
            //DynamoLogger.Log(message);

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

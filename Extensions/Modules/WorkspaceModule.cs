using Dynamo.Models;
using DynamoServer.Extensions;
using Nancy;
using System;
using System.IO;

namespace DynamoServer.Server
{
    public class DynamoWorkspaceModule : NancyModule
    {
        public DynamoWorkspaceModule() : base("/Workspace")
        {
            Get["/"] = CurrentFile;
            Get["/Current"] = CurrentFile;

            Get["/Open/{filepath}"] = OpenFile;
            Get["/Close"] = CloseFile;
            Get["/Run"] = RunGraph;
            Get["/Save"] = SaveFile;
        }

        private dynamic RunGraph(dynamic parameters)
        {
            ServerViewExtension.RunInContext(() =>
            {
                var vm = ServerViewExtension.dynamoViewModel;
                vm.CurrentSpaceViewModel.RunSettingsViewModel.Model.RunType = RunType.Manual;
                vm.Model.ForceRun();
            });

            return "ran";
        }

        private dynamic CurrentFile(dynamic parameters)
        {
            string file = "";
            ServerViewExtension.RunInContext(() =>
            {
                file = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
            }
            );

            var html = @"<h3>Currently open file</h3></br>" + file;

            return Response.AsText(html, "text/html");
        }

        private dynamic OpenFile(dynamic parameters)
        {
            string result = "";
            string file = Convert.ToString(parameters.filepath);
            if (string.IsNullOrWhiteSpace(file)) return "Invalid filepath supplied";
            if (!File.Exists(file)) return 404;

            ServerViewExtension.RunInContext(() =>
            {
                var vm = ServerViewExtension.dynamoViewModel;

                vm.CloseHomeWorkspaceCommand.Execute(null);
                vm.OpenCommand.Execute(file);
                result = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
            });

            return Response.AsText("Opened file : " + result, "text/html");
        }

        private dynamic CloseFile(dynamic parameters)
        {
            string file = "";

            ServerViewExtension.RunInContext(() =>
                {
                    file = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
                    ServerViewExtension.DynamoLogger.Log("Closed " + file);
                    ServerViewExtension.dynamoViewModel.CloseHomeWorkspaceCommand.Execute(null);
                }
            );

            return "Closed file : " + file;
        }

        private dynamic SaveFile(dynamic parameters)
        {
            string file = "";
            string result = "";

            ServerViewExtension.RunInContext(() =>
            {
                file = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
                if (string.IsNullOrWhiteSpace(file)) result = "No file is open, did not save anything.";
                else
                {
                    ServerViewExtension.DynamoLogger.Log("Saving " + file);
                    ServerViewExtension.dynamoViewModel.SaveCommand.Execute(null);
                    result = "Saved file : " + file;
                }
            }
            );

            return result;
        }

    }
}

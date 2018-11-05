using DynamoServer.Extensions;
using Nancy;
using System.Linq;

namespace DynamoServer.Server
{
    /// <summary>
    /// This module only handles requests that target the /json endpoints.
    /// </summary>
    public class DynamoWorkspaceModule : NancyModule
    {
        public DynamoWorkspaceModule() : base("/Workspace")
        {
            // would capture routes like /hello/nancy sent as a GET request
            Get["/"] = parameters =>
            {
                var feeds = new string[] { "foo", "bar" };
                return Response.AsJson(feeds);
            };

            Get["/Current"] = OpenFile;
            Get["/Open/{filepath}"] = OpenFile;
            Get["/Close"] = CloseFile;

            Get["/Nodes"] = GetNodes;
            Get["/Nodes/Add/{nodename}"] = AddNode;

        }


        private dynamic CurrentFile(dynamic parameters)
        {
            string file = "";
            ServerViewExtension.dispatcher.Invoke(() => {
                file = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
                }
            );

            var html = @"<h3>Currently open file</h3></br>" + file;

            return Response.AsText(html, "text/html");
        }

        private dynamic OpenFile(dynamic parameters)
        {
            return "Your filename is : " + parameters.filepath;
        }

        private dynamic CloseFile(dynamic parameters)
        {
            string file = "";
            ServerViewExtension.dispatcher.Invoke(() => {
                file = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
                ServerViewExtension.DynamoLogger.Log("Closed " + file);
                ServerViewExtension.dynamoViewModel.CloseHomeWorkspaceCommand.Execute(null);
                }
            );

            return "Closed file : " + file;
        }


        private dynamic AddNode(dynamic parameters)
        {
            return "Your node name is : " + parameters.nodename;
        }

        private dynamic GetNodes(dynamic parameters)
        {
            var allNodes = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.Nodes.Select(x => x.Name).ToList();

            string html = "" +
                "<h2>Current workspace nodes : </h2></br>" +
                "<ul></br>";

            foreach (var item in allNodes)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul></br>";

            return Response.AsText(html, "text/html");
        }



    }
}

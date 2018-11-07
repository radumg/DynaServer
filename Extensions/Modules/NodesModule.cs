using DynamoServer.Extensions;
using Nancy;
using System;
using System.Linq;

namespace DynamoServer.Server
{
    public class NodesModule : NancyModule
    {
        public NodesModule() : base("/Nodes")
        {
            Get["/"] = GetNodes;
            Get["/Clear"] = ClearNodes;
            Get["/Add/{nodename}"] = AddNode;
        }

        private dynamic AddNode(dynamic parameters)
        {
            string node = parameters.nodename;
            if (string.IsNullOrWhiteSpace(node)) return "Supplied node was empty";

            string result = "";
            var command = new Dynamo.Models.DynamoModel.CreateNodeCommand(Guid.NewGuid().ToString(), node, 100, 100, true, false);
            var nodes = "";
            ServerViewExtension.RunInContext(() =>
            {
                try
                {
                    int countBefore = ServerViewExtension.dynamoViewModel.Model.CurrentWorkspace.Nodes.Count();

                    ServerViewExtension.dynamoViewModel.Model.ExecuteCommand(command);
                    int countAfter = ServerViewExtension.dynamoViewModel.Model.CurrentWorkspace.Nodes.Count();

                    var n = ServerViewExtension.dynamoViewModel.Model.CurrentWorkspace.Nodes.Select(x => "<li>" + x.Name).ToArray();
                    nodes = string.Join("</li> ", n);

                    if (countBefore + 1 == countAfter)
                        result =
                            "<h3>Nodes on the canvas <h3><hr></br>" +
                            "<ul>" +
                            nodes +
                            "</ul>";
                    else result = $"Could not add your {node} node to canvas.";
                }
                catch (System.Exception e)
                {
                    result = "Something went wrong, error : " + e.Message;
                }
            });
            return Response.AsText(result, "text/html");
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

        private dynamic ClearNodes(dynamic parameters)
        {
            string html = "";
            int nodeCountBefore = 0, nodeCountAfter = 0;

            ServerViewExtension.RunInContext(() =>
            {
                nodeCountBefore = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.Nodes.Count();

                var vm = ServerViewExtension.dynamoViewModel;
                vm.Model.ClearCurrentWorkspace();

                nodeCountAfter = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.Nodes.Count();
            }
            );

            if (nodeCountAfter < nodeCountBefore) html = $"Cleared {nodeCountBefore - nodeCountAfter} nodes from canvas.";
            else html = "Did not clear any nodes from canvas.";

            return Response.AsText(html, "text/html");
        }

    }
}

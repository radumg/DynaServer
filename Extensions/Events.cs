using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Wpf.Extensions;
using System;
using System.Windows;

namespace DynamoServer.Extensions
{
    public static class Events
    {
        public static void RegisterEventHandlers(ServerViewExtension ext)
        {
            ext.viewLoadedParams.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            ext.viewLoadedParams.CurrentWorkspaceModel.NodeAdded += OnNodeAdded;
            ext.viewLoadedParams.CurrentWorkspaceModel.NodeRemoved += OnNodeRemoved;
        }

        public static void UnregisterEventHandlers(ServerViewExtension ext)
        {
            ext.viewLoadedParams.CurrentWorkspaceChanged -= OnCurrentWorkspaceChanged;
            ext.viewLoadedParams.CurrentWorkspaceModel.NodeAdded -= OnNodeAdded;
            ext.viewLoadedParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeRemoved;
        }

        private static void OnCurrentWorkspaceChanged(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            MessageBox.Show($"Congratulations on opening the {obj.Name} workspace!");
        }

        private static void OnNodeAdded(NodeModel node)
        {
            MessageBox.Show($"You just added the {node.Name} node with Id {node.GUID} to the canvas.");
        }

        private static void OnNodeRemoved(NodeModel node)
        {
            MessageBox.Show($"You just removed the {node.Name} node with Id {node.GUID} from the canvas.");
        }

        public static void OnServerStart(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"[ {DateTime.Now} ] : Starting server on machine {Environment.MachineName}");
        }

        public static void OnServerStop(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"[ {DateTime.Now} ] : Stopping server on machine {Environment.MachineName}");
        }
    }
}

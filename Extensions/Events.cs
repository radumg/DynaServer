using Dynamo.Graph.Nodes;
using System;
using System.Windows;

namespace DynamoServer.Extensions
{
    public static class Events
    {
        public static void RegisterEventHandlers(ServerViewExtension extension)
        {
            extension.viewLoadedParams.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            extension.viewLoadedParams.CurrentWorkspaceModel.NodeAdded += OnNodeAdded;
            extension.viewLoadedParams.CurrentWorkspaceModel.NodeRemoved += OnNodeRemoved;
        }

        public static void UnregisterEventHandlers(ServerViewExtension extension)
        {
            extension.viewLoadedParams.CurrentWorkspaceChanged -= OnCurrentWorkspaceChanged;
            extension.viewLoadedParams.CurrentWorkspaceModel.NodeAdded -= OnNodeAdded;
            extension.viewLoadedParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeRemoved;
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

        public static async void OnServerStartAsync(object sender, RoutedEventArgs e)
        {
            await ServerViewExtension.StartServerAsync();
        }

        public static async void OnServerStop(object sender, RoutedEventArgs e)
        {
            await ServerViewExtension.StopServerAsync();
        }

        public static void OnCheckServerStatus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ServerViewExtension.GetServerStatus());
        }
    }
}

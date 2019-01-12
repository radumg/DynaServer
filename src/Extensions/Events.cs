using Dynamo.Graph.Nodes;
using System;
using System.Windows;

namespace DynaServer.Extensions
{
    public static class Events
    {
        public static async void OnServerStartAsync(object sender, RoutedEventArgs e)
        {
            await ServerHost.StartServerAsync();
        }

        public static async void OnServerStop(object sender, RoutedEventArgs e)
        {
            await ServerHost.StopServerAsync();
        }

        public static void OnCheckServerStatus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ServerHost.GetServerStatus());
        }
    }
}

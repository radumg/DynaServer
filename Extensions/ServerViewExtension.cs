using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DynamoServer.Extensions
{
    public class ServerViewExtension : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "Dynamo Server View Extension";

        internal ViewLoadedParams viewLoadedParams;
        internal DynamoViewModel dynamoViewModel => viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

        public void Startup(ViewStartupParams vsp) { }

        public void Loaded(ViewLoadedParams vlp)
        {
            MessageBox.Show($"[ {DateTime.Now} ] : {this.Name} is ready!");

            // hold a reference to the Dynamo params to be used later
            viewLoadedParams = vlp;
            Events.RegisterEventHandlers(this);

            // we can now add custom menu items to Dynamo's UI
            UI.MakeMenuItems(this);
        }


        public void Shutdown()
        {
            Events.UnregisterEventHandlers(this);
        }

        public void Dispose() { }
    }
}

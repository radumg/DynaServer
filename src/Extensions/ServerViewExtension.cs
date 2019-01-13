using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Dynamo.Extensions;
using Dynamo.Logging;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;

namespace DynaServer.Extensions
{
    public class ServerViewExtension : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "DynaServer View Extension";
        private Dispatcher dispatcher = null;

        // Dynamo ViewModel properties
        private ViewLoadedParams dynamoViewLoadedParams = null;
        internal Menu DynamoMenu = null;
        internal DynamoViewModel DynamoViewModel = null;
        internal DynamoLogger DynamoLogger = null;
        internal IExtensionManager DynamoExtensionManager = null;
        internal DynamoModel DynamoModel;


        public void Startup(ViewStartupParams vsp)
        {

        }

        public void Loaded(ViewLoadedParams vlp)
        {
            // hold reference to thread so we can execute on same thread as Dynamo UI
            dispatcher = Dispatcher.CurrentDispatcher;

            //hold a reference to the Dynamo params to be used later
            dynamoViewLoadedParams = vlp;
            DynamoViewModel = dynamoViewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;
            DynamoModel = DynamoViewModel.Model;
            DynamoLogger = DynamoViewModel.Model.Logger;

            // add Dynamo Server menu to Dynamo UI
            DynamoMenu = dynamoViewLoadedParams.dynamoMenu;
            DynamoMenu.Items.Add(UI.DynamoServerMenu);

            ServerHost.UpdateViewModelStatus(this.DynamoViewModel!=null);
            ServerHost.UpdateViewExtension(this);
        }

        public void Dispose() { }

        public void Shutdown()
        {
            //ServerHost.OnViewShutdown();
        }

        internal void RunInDynamoUIContext(Action action)
        {
            dispatcher.Invoke(action);
        }

    }
}

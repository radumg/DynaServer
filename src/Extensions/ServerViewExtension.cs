using System.Windows.Threading;
using Dynamo.Wpf.Extensions;

namespace DynaServer.Extensions
{
    public class ServerViewExtension : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "DynaServer View Extension";

        public void Startup(ViewStartupParams vsp)
        {
            ServerHost.OnViewModelStartup(vsp);
        }

        public void Loaded(ViewLoadedParams vlp)
        {
            ServerHost.OnViewModelReady(vlp, Dispatcher.CurrentDispatcher);
        }

        public void Dispose() { }

        public void Shutdown()
        {
            ServerHost.OnViewShutdown();
        }
    }
}

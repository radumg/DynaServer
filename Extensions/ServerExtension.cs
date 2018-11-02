using Dynamo.Extensions;
using System;
using System.Windows;

namespace DynamoServer.Extensions
{
    /// <summary>
    /// Dynamo extension that controls the underlying Dynamo application but not its UI.
    /// </summary>
    public class ServerExtension : IExtension
    {
        public string UniqueId => "EA3501CF-64AE-4246-8837-EFF7DF7F7067";

        public string Name => "Dynamo Server Extension";

        public void Startup(StartupParams sp) { }

        public void Ready(ReadyParams rp)
        {
            MessageBox.Show($"[ {DateTime.Now} ] : {this.Name} is ready!");
        }

        public void Shutdown()
        {

        }

        public void Dispose() { }
    }
}

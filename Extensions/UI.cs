using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DynamoServer.Extensions
{
    public static class UI
    {
        internal static MenuItem ServerMenu;
        internal static MenuItem StartServerMenuItem;
        internal static MenuItem StopServerMenuItem;
        internal static MenuItem CheckServerStatusMenuItem;

        static UI()
        {
            // let's now create a completely top-level new menu item
            ServerMenu = new MenuItem { Header = "Dynamo Server" };

            // and now we add a new sub-menu item that says hello when clicked
            StartServerMenuItem = new MenuItem { Header = "Start Server" };
            StopServerMenuItem = new MenuItem { Header = "Stop Server" };
            CheckServerStatusMenuItem = new MenuItem { Header = "Check server status" };

            // register event handlers
            StartServerMenuItem.Click += Events.OnServerStartAsync;
            StopServerMenuItem.Click += Events.OnServerStop;
            CheckServerStatusMenuItem.Click += Events.OnCheckServerStatus;

            ServerMenu.Items.Add(StartServerMenuItem);
            ServerMenu.Items.Add(StopServerMenuItem);
            ServerMenu.Items.Add(CheckServerStatusMenuItem);
        }
    }
}

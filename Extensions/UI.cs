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
        internal static MenuItem serverMenu;

        public static void MakeMenuItems(ServerViewExtension extension)
        {
            // let's now create a completely top-level new menu item
            serverMenu = new MenuItem { Header = "Dynamo Server" };

            // and now we add a new sub-menu item that says hello when clicked
            var startServerMenuItem = new MenuItem { Header = "Start Server" };
            var stopServerMenuItem = new MenuItem { Header = "Stop Server" };
            startServerMenuItem.Click += Events.OnServerStartAsync;
            stopServerMenuItem.Click += Events.OnServerStop;
            serverMenu.Items.Add(startServerMenuItem);
            serverMenu.Items.Add(stopServerMenuItem);

            // finally, we need to add our menu to Dynamo
            extension.viewLoadedParams.dynamoMenu.Items.Add(serverMenu);
        }

    }
}

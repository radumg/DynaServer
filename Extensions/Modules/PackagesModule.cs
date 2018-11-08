using Dynamo.Extensions;
using DynamoServer.Extensions;
using Nancy;
using System.Collections.Generic;
using System.Linq;

namespace DynamoServer.Server
{
    public class PackagesModule : NancyModule
    {
        public PackagesModule() : base("/Packages")
        {
            Get["/"] = GetPackages;
            Get["/Install/{packagename}"] = InstallPackage;
            Get["/Remove/{packagename}"] = RemovePackage;
        }

        private dynamic GetPackages(dynamic parameters)
        {
            string html = "";
            List<string> packageNames = new List<string>();
            int packageCountBefore = 0, packageCountAfter = 0;
            IEnumerable<IExtension> packages;

            ServerViewExtension.RunInContext(() =>
            {
                // TODO : get packages and remove specified one
                packages = ServerViewExtension.dynamoViewModel.Model.ExtensionManager.Extensions;
                packageCountBefore = packages.Count();

                packageNames = packages.Select(x => x.Name).ToList();

                packageCountAfter = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.Nodes.Count();
            }
            );

            html = "<h2>Currently installed extensions : </h2></br>" +
                   "<ul></br>";

            foreach (var item in packageNames)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul></br>";

            return Response.AsText(html, "text/html");
        }

        private dynamic InstallPackage(dynamic parameters)
        {
            string packageName = parameters.packagename;
            if (string.IsNullOrWhiteSpace(packageName)) return "Supplied package name was invalid or empty.";

            string result = "";

            ServerViewExtension.RunInContext(() =>
            {
                try
                {
                    // TODO : install package
                }
                catch (System.Exception e)
                {
                    result = "Something went wrong, error : " + e.Message;
                }
            });
            return Response.AsText(result, "text/html");
        }

        private dynamic RemovePackage(dynamic parameters)
        {
            string html = "";
            int packageCountBefore = 0, packageCountAfter = 0;

            ServerViewExtension.RunInContext(() =>
            {
                // TODO : get packages and remove specified one
            }
            );

            if (packageCountAfter < packageCountBefore) html = $"Removed {packageCountBefore - packageCountAfter} packages from Dynamo.";
            else html = "Did not remove any packages from Dynamo.";

            return Response.AsText(html, "text/html");
        }

    }
}

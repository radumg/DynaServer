using Dynamo.Extensions;
using DynamoServer.Extensions;
using Nancy;
using System.Collections.Generic;
using System.Linq;

namespace DynamoServer.Server
{
    /// <summary>
    /// Module to handle packages in Dynamo, allowing you to list them or install new packages or remove existing ones.
    /// </summary>
    public class PackagesModule : NancyModule
    {
        public PackagesModule() : base("/Packages")
        {
            Get["/"] = GetPackages;
            Get["/Install/{packagename}"] = InstallPackage;
            Get["/Remove/{packagename}"] = RemovePackage;
        }

        /// <summary>
        /// Get the list of currently installed packages in Dynamo.
        /// </summary>
        /// <param name="parameters">Not used, can specify null.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic GetPackages(dynamic parameters)
        {
            string html = "";
            List<string> packageNames = new List<string>();
            int packageCountBefore = 0, packageCountAfter = 0;
            IEnumerable<IExtension> packages;

            HashSet<string> uniqueLibs = new List<string>();

            ServerViewExtension.RunInContext(() =>
            {
                // TODO : get packages and remove specified one

                #region List Libraries of loaded Nodes
                var nsm = ServerViewExtension.dynamoViewModel.Model.SearchModel;
                List<Dynamo.Search.SearchElements.NodeSearchElement> nodes = nsm.SearchEntries.ToList();

                List<string> libs = new List<string>();
                foreach (var n in nodes)
                {
                    var cats = n.Categories.ToList();
                    if (cats.Count > 0)
                    {
                        libs.Add(cats[0]);
                    }
                }

                uniqueLibs = libs.Distinct().ToList();
                uniqueLibs.Sort();
                #endregion

                packages = ServerViewExtension.dynamoViewModel.Model.ExtensionManager.Extensions;
                packageCountBefore = packages.Count();

                packageNames = packages.Select(x => x.Name).ToList();

                packageCountAfter = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.Nodes.Count();
            }

            );

            html = "<h2>Currently installed libraries : </h2></br>" +
                 "<ul></br>";

            foreach (var item in uniqueLibs)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul></br>";

            html += "<h2>Currently installed extensions : </h2></br>" +
                   "<ul></br>";

            foreach (var item in packageNames)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul></br>";

            return Response.AsText(html, "text/html");
        }

        /// <summary>
        /// Installs the specified package in Dynamo, using the Package Manager.
        /// </summary>
        /// <param name="parameters">The name of the package to install.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic InstallPackage(dynamic parameters)
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

        /// <summary>
        /// Remove the specified package from Dynamo, using the Package Manager.
        /// </summary>
        /// <param name="parameters">The name of the package to remove.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic RemovePackage(dynamic parameters)
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

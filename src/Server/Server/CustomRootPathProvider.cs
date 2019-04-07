using Nancy;
using System;
using System.IO;

namespace DynaServer.Server
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dynPath = @"\Dynamo\Dynamo Revit\2.0\packages\DynaServer";
            var path = folder + dynPath;
            return path;
        }
    }
}

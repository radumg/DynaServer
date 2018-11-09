using Nancy;
using System.IO;

namespace DynamoServer.Server
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return @"C:\Users\Radu\AppData\Roaming\Dynamo\Dynamo Core\2.0\packages\DynamoServer";
            return Directory.GetCurrentDirectory();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace DynamoServer
{
    /// <summary>
    /// This module only handles requests that target the /about endpoints.
    /// </summary>
    public class AboutModule : NancyModule
    {
        public AboutModule() : base("/about")
        {
            Get["/"] = x =>
            {
                return "Running under " + 
                        Assembly.GetExecutingAssembly().FullName + 
                        ", version " + 
                        Assembly.GetExecutingAssembly().GetName().Version;
            };

            Get["/Dynamo"] = x =>
            {
                return "Running in Dynamo " + Dynamo.Utilities.AssemblyHelper.GetDynamoVersion();
            };
        }
    }
}

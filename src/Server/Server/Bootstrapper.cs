﻿using Nancy;
using Nancy.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaServer.Server
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private const string _password = "dynamo";
        internal IRootPathProvider rootPathProvider;

        protected override IRootPathProvider RootPathProvider
        {
            get {
                rootPathProvider = new CustomRootPathProvider();
                return rootPathProvider;
            }
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = _password }; }
        }
    }
}

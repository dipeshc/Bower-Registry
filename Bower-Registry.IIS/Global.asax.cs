using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using BowerRegistry.PackageRepositories;

namespace BowerRegistry.IIS
{
    public class Global : HttpApplication
    {
        protected void Application_Start(Object sender, EventArgs e)
        {
            // Get settings from config.
            var json = ConfigurationManager.AppSettings ["Json"];
            var xml = ConfigurationManager.AppSettings ["Xml"];

            // Make package reposiroties.
            var packageRepositories = new List<IPackageRepository>();

            if (!string.IsNullOrEmpty(json))
                packageRepositories.Add(new JsonFilePackageRepository(json));

            if (!string.IsNullOrEmpty(xml))
                packageRepositories.Add(new XmlFilePackageRepository(xml));

            if (packageRepositories.Count == 0)
                packageRepositories.Add(new InMemoryPackageRepository());

            // Start app.
            var appHost = new AppHost();
            appHost.Init();
            appHost.Container.Register<IPackageRepository>(_ => new AggregatePackageRepository(packageRepositories));
        }
    }
}
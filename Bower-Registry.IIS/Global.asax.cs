using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using BowerRegistry.PackageRepositories;
using BowerRegistry.IIS.Configuration;

namespace BowerRegistry.IIS
{
	public class Global : HttpApplication
	{
		protected void Application_Start(Object sender, EventArgs e)
		{
			// Make package reposiroties.
			var packageRepositories = new List<IPackageRepository>();

			// Get bowerRegistryConfigurationSection
			var bowerRegistryConfigurationSection = ConfigurationManager.GetSection(BowerRegistryConfigurationSection.SectionName) as BowerRegistryConfigurationSection;
			if (bowerRegistryConfigurationSection != null)
			{
				// Get custom repositories.
				foreach (var packageRepository in bowerRegistryConfigurationSection.Repositories)
				{
					if (packageRepository.GetType() == typeof(InMemory))
						packageRepositories.Add(new InMemoryPackageRepository());

					if (packageRepository.GetType() == typeof(XmlFile))
						packageRepositories.Add(new XmlFilePackageRepository(((XmlFile)packageRepository).FilePath));

					if (packageRepository.GetType() == typeof(JsonFile))
						packageRepositories.Add(new JsonFilePackageRepository(((JsonFile)packageRepository).FilePath));

					if(packageRepository.GetType() == typeof(Stash))
					{
						var stash = packageRepository as Stash; 
						packageRepositories.Add(new StashPackageRepository(stash.BaseUri, stash.ProjectKey, stash.Username, stash.Password, stash.UseSSH));
					}
				}
			}

			// Start app.
			var appHost = new AppHost();
			appHost.Init();
			appHost.Container.Register<IPackageRepository>(_ => new AggregatePackageRepository(packageRepositories));
		}
	}
}
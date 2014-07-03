using System.Configuration;

namespace BowerRegistry.IIS.Configuration
{
	public class BowerRegistryConfigurationSection  : ConfigurationSection
	{
		public const string SectionName = "BowerRegistry";

		[ConfigurationProperty("PackageRepositories")]
		public PackageRepositoriesElementCollection Repositories
		{
			get { return ((PackageRepositoriesElementCollection)(base["PackageRepositories"])); }
			set { base["PackageRepositories"] = value; }
		}
	}
}
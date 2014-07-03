using System.Configuration;

namespace BowerRegistry.IIS.Configuration
{
	public abstract class PackageRepository : ConfigurationElement
	{
		[ConfigurationProperty("id", IsRequired = true)]
		public string Name
		{
			get { return (string)this["id"]; }
			set { this["id"] = value; }
		}
	}
}
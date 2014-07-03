using System.Configuration;

namespace BowerRegistry.IIS.Configuration
{
	public class XmlFile : PackageRepository
	{
		[ConfigurationProperty("filePath", IsRequired = true)]
		public string FilePath
		{
			get { return (string)this["filePath"]; }
			set { this["filePath"] = value; }
		}
	}
}
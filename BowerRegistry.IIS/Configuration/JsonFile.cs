using System.Configuration;

namespace BowerRegistry.IIS.Configuration
{
	public class JsonFile : PackageRepository
	{
		[ConfigurationProperty("filePath", IsRequired = true)]
		public string FilePath
		{
			get { return (string)this["filePath"]; }
			set { this["filePath"] = value; }
		}
	}
}
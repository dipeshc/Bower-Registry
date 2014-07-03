using System.Configuration;

namespace BowerRegistry.IIS.Configuration
{
	public class Stash : PackageRepository
	{
		[ConfigurationProperty("baseUri", IsRequired = true)]
		public string BaseUri
		{
			get { return (string)this["baseUri"]; }
			set { this["baseUri"] = value; }
		}

		[ConfigurationProperty("projectKey", IsRequired = true)]
		public string ProjectKey
		{
			get { return (string)this["projectKey"]; }
			set { this["projectKey"] = value; }
		}

		[ConfigurationProperty("username")]
		public string Username
		{
			get { return (string)this["username"]; }
			set { this["username"] = value; }
		}

		[ConfigurationProperty("password")]
		public string Password
		{
			get { return (string)this["password"]; }
			set { this["password"] = value; }
		}

		[ConfigurationProperty("useSSH")]
		public bool UseSSH
		{
			get { return (bool)this["useSSH"]; }
			set { this["useSSH"] = value; }
		}
	}
}
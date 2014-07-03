using System;
using System.Configuration;
using System.Linq;

namespace BowerRegistry.IIS.Configuration
{
	[ConfigurationCollection(typeof(PackageRepository), CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class PackageRepositoriesElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return null;
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return element;
		}

		protected override ConfigurationElement CreateNewElement(string elementName)
		{
			var matches = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(a => a.DefinedTypes)
				.Where(t => t.Name.Equals(elementName, StringComparison.OrdinalIgnoreCase))
				.Where(t => t.IsSubclassOf(typeof(PackageRepository)))
				.ToList();

			if(!matches.Any())
				throw new ConfigurationErrorsException(string.Format("Unrecognized element '{0}'. Could not find type that matches {0}. Type name must match element name.", elementName));

			if(matches.Count > 1)
				throw new ConfigurationErrorsException(string.Format("More than one matching type for element '{0}'.", elementName));

			return Activator.CreateInstance(matches.First()) as ConfigurationElement;
		}

		protected override bool IsElementName(string elementName)
		{
			return true;
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMap; }
		}
	}
}
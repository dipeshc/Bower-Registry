using System;
using System.Collections.Generic;
using System.Linq;

namespace BowerRegistry.PackageRepositories
{
    public class InMemoryPackageRepository : IPackageRepository
    {
        protected List<Package> Packages = new List<Package>();

        public bool Readonly { get { return false; } }

        public Package[] List()
        {
            return Packages.ToArray();
        }

        public Package Get(string name)
        {
            return Packages.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public Package[] Search(string name)
        {
            return Packages.Where(p => p.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToArray();
        }

        public void Add(Package package)
        {
            var existingPackage = Get(package.Name);

            if (existingPackage == null)
                Packages.Add(package);
            else
                existingPackage.Url = package.Url;
        }
    }
}
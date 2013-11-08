using System;
using System.Collections.Generic;
using System.Linq;

namespace BowerRegistry.PackageRepositories
{
    public abstract class AbstractFilePackageRepository : IPackageRepository
    {
        protected List<Package> Packages;

        public Package[] List()
        {
            Load();
            return Packages.ToArray();
        }

        public Package Get(string name)
        {
            Load();
            return Packages.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public Package[] Search(string name)
        {
            Load();
            return Packages.Where(p => p.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToArray();
        }

        public void Add(Package package)
        {
            Load();
            Packages.Add(package);
            Save();
        }

        protected abstract void Load();
        protected abstract void Save();
    }
}
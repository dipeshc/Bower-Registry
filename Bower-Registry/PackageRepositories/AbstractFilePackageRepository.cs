using System;
using System.Linq;

namespace BowerRegistry
{
    public abstract class AbstractFilePackageRepository : IPackageRepository
    {
        protected abstract Package[] Packages { get; }

        public Package[] List()
        {
            return Packages;
        }

        public Package Get(string name)
        {
            return Packages.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public Package[] Search(string name)
        {
            return Packages.Where(p => p.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToArray();
        }
    }
}
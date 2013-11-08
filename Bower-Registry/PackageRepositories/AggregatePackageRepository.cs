using System;
using System.Linq;
using System.Collections.Generic;

namespace BowerRegistry.PackageRepositories
{
    public class AggregatePackageRepository : IPackageRepository
    {
        protected IPackageRepository[] PackageRepositories;

        public AggregatePackageRepository(IEnumerable<IPackageRepository> packageRepositories)
        {
            PackageRepositories = packageRepositories.ToArray();
        }

        public Package[] List()
        {
            return PackageRepositories.SelectMany(pr => pr.List())
                .GroupBy(p => p.Name.ToLowerInvariant()).Select(Enumerable.First).ToArray();
        }

        public Package Get(string name)
        {
            foreach (var repositories in PackageRepositories)
            {
                var package = repositories.Get(name);
                if (package != null)
                    return package;
            }
            return null;
        }

        public Package[] Search(string name)
        {
            return PackageRepositories.SelectMany(pr => pr.Search(name))
                .GroupBy(p => p.Name.ToLowerInvariant()).Select(Enumerable.First).ToArray();
        }
    }
}
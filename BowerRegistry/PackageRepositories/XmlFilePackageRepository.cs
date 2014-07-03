using System.Collections.Generic;
using System.IO;
using ServiceStack.Text;

namespace BowerRegistry.PackageRepositories
{
    public class XmlFilePackageRepository : AbstractFilePackageRepository
    {
        public readonly string FilePath;

        public XmlFilePackageRepository(string filePath)
        {
            FilePath = filePath;
        }

        protected override void Load()
        {
            using (var reader = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read))
                Packages = XmlSerializer.DeserializeFromStream<List<Package>>(reader);
        }

        protected override void Save()
        {
            using (var writer = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write))
                XmlSerializer.SerializeToStream(Packages, writer);
        }
    }
}
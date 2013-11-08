using System.Collections.Generic;
using System.IO;
using ServiceStack.Text;

namespace BowerRegistry.PackageRepositories
{
    public class JsonFilePackageRepository : AbstractFilePackageRepository
    {
        public readonly string FilePath;

        public JsonFilePackageRepository(string filePath)
        {
            FilePath = filePath;
        }

        protected override void Load()
        {
            using (var reader = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read))
                Packages = JsonSerializer.DeserializeFromStream<List<Package>>(reader);

            if (Packages == null)
            {
                Packages = new List<Package>();
                Save();
            }
        }

        protected override void Save()
        {
            using (var writer = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write))
                JsonSerializer.SerializeToStream(Packages, writer);
        }
    }
}
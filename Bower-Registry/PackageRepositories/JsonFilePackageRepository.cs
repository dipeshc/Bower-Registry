using System.IO;
using ServiceStack.Text;

namespace BowerRegistry.PackageRepositories
{
    public class JsonFilePackageRepository : AbstractFilePackageRepository
    {
        string _filePath;

        public JsonFilePackageRepository(string filePath)
        {
            _filePath = filePath;
        }

        protected override Package[] Packages
        {
            get
            {
                using (var reader = new StreamReader(_filePath))
                    return JsonSerializer.DeserializeFromReader<Package[]>(reader);
            }
        }
    }
}
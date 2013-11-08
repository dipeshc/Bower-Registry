using System.IO;
using ServiceStack.Text;

namespace BowerRegistry
{
    public class XmlFilePackageRepository : AbstractFilePackageRepository
    {
        string _filePath;

        public XmlFilePackageRepository(string filePath)
        {
            _filePath = filePath;
        }

        protected override Package[] Packages
        {
            get
            {
                using (var reader = new StreamReader(_filePath))
                    return XmlSerializer.DeserializeFromReader<Package[]>(reader);
            }
        }
    }
}
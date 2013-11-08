using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace BowerRegistry.Services
{
    [Route("/packages", "GET")]
    public class PackagesRequest : IReturn<Package[]>
    {
    }

    [Route("/packages/{Name}", "GET")]
    public class PackageRequest : IReturn<Package>
    {
        public string Name{ get; set; }
    }

    [Route("/packages/search/{Name}", "GET")]
    public class PackageSearchRequest : IReturn<Package[]>
    {
        public string Name{ get; set; }
    }

    public class PackagesService : Service
    {
        IPackageRepository _packageRepository;

        public PackagesService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public Package[] Get(PackagesRequest request)
        {
            return _packageRepository.List();
        }

        public Package Get(PackageRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                throw HttpError.NotFound("Package name must be provided.");

            var package = _packageRepository.Get(request.Name);
            if(package == null)
                throw HttpError.NotFound("Unable to find package with name, " + request.Name);
            return package;
        }

        public Package[] Get(PackageSearchRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                throw HttpError.NotFound("Package name must be provided.");

            return _packageRepository.Search(request.Name);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace BowerRegistry.PackageRepositories
{
    public class StashPackageRepository : IPackageRepository
    {
        public readonly string BaseUri;
        public readonly string ProjectKey;
        public readonly string Username;
        public readonly string Password;
        public readonly bool SshInsteadOfHttp;
        public readonly int SshPort;

        protected Package[] Packages = new Package[0];

        public bool IsReadonly { get { return true; } }

        public StashPackageRepository(string baseUri, string projectKey, string username="", string password="", bool sshInsteadOfHttp=false, int sshPort=7999)
        {
            BaseUri = baseUri;
            ProjectKey = projectKey;
            Username = username;
            Password = password;
            SshInsteadOfHttp = sshInsteadOfHttp;
            SshPort = sshPort;
        }

        public Package[] List()
        {
            var client = new RestClient(BaseUri)
            {
                Authenticator = new HttpBasicAuthenticator(Username, Password)
            };
            var request = new RestRequest(string.Format("/rest/api/1.0/projects/{0}/repos", ProjectKey), Method.GET);
            
            IRestResponse<StashProjectRepos> response;
            try
            {
                response = client.Get<StashProjectRepos>(request);
            }
            catch (Exception)
            {
                return new Package[0];
            }
            
            if (response == null || response.Data == null || response.Data.Values == null || !response.Data.Values.Any())
            {
                return new Package[0];
            }

            Packages = response.Data.Values.Select(repo => new Package
            {
                Name = repo.Slug,
                Url = SshInsteadOfHttp ? MakeSshUri(repo.Slug) : MakeHttpUri(repo.Slug)
            }).ToArray();

            return Packages;
        }

        public Package Get(string name)
        {
            // Try to find based on local cache first.
            var package = Packages.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            // If could not find based on cache, then reload via list and try again.
            if (package == null)
            {
                List();
                package = Packages.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            }

            return package;
        }

        public Package[] Search(string name)
        {
            // Try to find based on local cache first.
            var package = Packages.Where(p => p.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToArray();

            // If could not find based on cache, then reload via list and try again.
            if (!package.Any())
            {
                List();
                package = Packages.Where(p => p.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToArray();
            }

            return package;
        }

        public void Add(Package package)
        {
            throw new InvalidOperationException("StashPackageRepository is readonly, cannot Add package.");
        }

        protected string MakeSshUri(string repoSlug)
        {
            var baseEndpoint = BaseUri.ToLower().Replace("http://", "").Replace("https://", "");
            return string.Format("ssh://git@{0}:{1}/{2}/{3}.git", baseEndpoint, SshPort, ProjectKey, repoSlug);
        }

        protected string MakeHttpUri(string repoSlug)
        {
            return string.Format("{0}/scm/{1}/{2}.git", BaseUri, ProjectKey, repoSlug);
        }

        public class StashProjectRepos
        {
            public int Start { get; set; }
            public List<StashRepos> Values { get; set; }
        }

        public class StashRepos
        {
            public int Id { get; set; }
            public string Slug { get; set; }
            public string Name { get; set; }
        }
    }
}
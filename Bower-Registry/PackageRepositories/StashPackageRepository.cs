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
        public readonly int SshPort;

        protected Package[] Packages = new Package[0];

        public StashPackageRepository(string baseUri, string projectKey, string username="", string password="", int sshPort=7999)
        {
            BaseUri = baseUri;
            ProjectKey = projectKey;
            SshPort = sshPort;
        }

        public Package[] List()
        {
            var client = new RestClient(BaseUri);
            var request = new RestRequest(string.Format("/rest/api/1.0/projects/{0}/repos", ProjectKey), Method.GET);
            var response = client.Get<StashProjectRepos>(request);

            Packages = response.Data.Repos.Select(repo => new Package
            {
                Name = repo.Slug,
                Url = MakeSSHUri(repo.Slug)
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
            if (package == null)
            {
                List();
                package = Packages.Where(p => p.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToArray();
            }

            return package;
        }

        public void Add(Package package)
        {
            // This repository is readonly, to add a new package it must be added via the regular stash repo mechanism.
            throw new UnauthorizedAccessException();
        }

        protected string MakeSSHUri(string repoSlug)
        {
            return string.Format("ssh://{0}:{1}/{2}/{3}.git", BaseUri, SshPort, ProjectKey, repoSlug);
        }

        public class StashProjectRepos
        {
            public int Start { get; set; }
            public List<StashRepos> Repos { get; set; }
        }

        public class StashRepos
        {
            public int Id { get; set; }
            public string Slug { get; set; }
            public string Name { get; set; }
        }
    }
}
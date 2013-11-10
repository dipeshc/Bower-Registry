using System;
using System.Collections.Generic;
using System.Threading;
using BowerRegistry.PackageRepositories;
using ManyConsole;

namespace BowerRegistry.Console
{
    public class ServerCommand : ConsoleCommand
    {
        public int Port = 80;
        public string Json;
        public string Xml;

        public string StashBaseUri;
        public string StashProjectKey;
        public string StashUsername;
        public string StashPassword;

        public ServerCommand()
        {
            IsCommand("server", "hosts the bower registry.");
            HasOption("p|port=", "Specifies the port that the server will listen on [default 80].", o => Port = int.Parse(o));

            HasOption("json=", "Path to json document containing serialized package repository.", o => Json = o);
            HasOption("xml=", "Path to xml document containing serialized package repository.", o => Xml = o);

            HasOption("stashBaseUri=", "Stash base endpoint (e.g. http://stash.atlassian.com", o => StashBaseUri = o);
            HasOption("stashProjectKey=", "Stash root project key.", o => Xml = o);
            HasOption("stashUsername=", "Stash username", o => Xml = o);
            HasOption("stashPassword=", "Stash password.", o => Xml = o);

            SkipsCommandSummaryBeforeRunning();
        }

        public override int Run(string[] remainingArguments)
        {
            var packageRepositories = new List<IPackageRepository>();

            if (!string.IsNullOrEmpty(Json))
                packageRepositories.Add(new JsonFilePackageRepository(Json));

            if (!string.IsNullOrEmpty(Xml))
                packageRepositories.Add(new XmlFilePackageRepository(Xml));

            if (!string.IsNullOrEmpty(StashBaseUri) && !string.IsNullOrEmpty(StashProjectKey))
                packageRepositories.Add(new StashPackageRepository(StashBaseUri, StashProjectKey, StashUsername, StashPassword));

            if (packageRepositories.Count == 0)
                packageRepositories.Add(new InMemoryPackageRepository());

            var listener = string.Format("http://*:{0}/", Port);

            var appHost = new AppHost();
            appHost.Init();
            appHost.Container.Register<IPackageRepository>(_ => new AggregatePackageRepository(packageRepositories));

            System.Console.WriteLine("Listening on {0}", listener);
            appHost.Start(listener);

            Thread.Sleep(Timeout.Infinite);

            return 0;
        }
    }
}
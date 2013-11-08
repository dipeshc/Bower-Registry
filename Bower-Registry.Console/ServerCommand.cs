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

        public ServerCommand()
        {
            IsCommand("server");
            HasOption("p|port=", "Specifies the port that the server will listen on [default 80].", o => Port = int.Parse(o));

            HasOption("j|json=", "Path to json document containing serialized package reposiroty.", o => Json = o);
            HasOption("x|xml=", "Path to xml document containing serialized package repository.", o => Xml = o);

            SkipsCommandSummaryBeforeRunning();
        }

        public override int Run(string[] remainingArguments)
        {
            var packageRepositories = new List<IPackageRepository>();

            if (!string.IsNullOrEmpty(Json))
                packageRepositories.Add(new JsonFilePackageRepository(Json));

            if(!string.IsNullOrEmpty(Xml))
                packageRepositories.Add(new XmlFilePackageRepository(Xml));

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
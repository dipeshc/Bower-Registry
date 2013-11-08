using System;
using Funq;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace BowerRegistry
{
    public class AppHost : AppHostHttpListenerBase
    {
        public AppHost() : base(AppDomain.CurrentDomain.FriendlyName, AppDomain.CurrentDomain.GetAssemblies())
        {
        }

        public override void Configure(Container container)
        {
            JsConfig.EmitCamelCaseNames = true;
        }
    }
}
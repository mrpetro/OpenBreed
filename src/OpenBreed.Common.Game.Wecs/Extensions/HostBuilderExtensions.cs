using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Extensions;

namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupGameCommonComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
            hostBuilder.SetupAssemblyComponentFactories();
        }
    }
}

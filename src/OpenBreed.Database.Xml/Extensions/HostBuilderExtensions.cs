using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Database.Xml.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupXmlDatabase(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<XmlDatabaseMan, XmlDatabaseMan>();
            });
        }
    }
}

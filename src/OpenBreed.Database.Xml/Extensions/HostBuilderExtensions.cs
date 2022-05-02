using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface;
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
                services.AddSingleton<XmlDatabaseMan>();
            });
        }

        public static void SetupXmlReadonlyDatabase(this IHostBuilder hostBuilder, string dbFilePath = null)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IRepositoryProvider>((sp) => new XmlReadonlyDatabaseMan(sp.GetService<IVariableMan>(), dbFilePath));
            });
        }
    }
}

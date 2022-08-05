using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface;
using OpenBreed.Database.EFCore;
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
        public static void SetupXmlUnitOfWork(this IHostBuilder hostBuilder, Action<XmlUnitOfWork, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IUnitOfWork, XmlUnitOfWork>((sp) =>
                {
                    var unitofWork = new XmlUnitOfWork(sp.GetService<IDatabase>());
                    action.Invoke(unitofWork, sp);
                    return unitofWork;
                });
            });
        }

        public static void SetupXmlDatabase(this IHostBuilder hostBuilder, Action<XmlDatabaseMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IDatabase>((sp) =>
                {
                    var databaseMan = new XmlDatabaseMan(sp.GetService<IServiceScopeFactory>(), sp.GetService<IVariableMan>());
                    action.Invoke(databaseMan, sp);
                    return databaseMan;
                });
            });
        }

        public static void SetupXmlReadonlyDatabase(this IHostBuilder hostBuilder, string dbFilePath = null)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IRepositoryProvider>((sp) => new XmlReadonlyDatabaseMan(
                    sp.GetService<OpenBreedDbContext>(),
                    sp.GetService<IVariableMan>(), dbFilePath));
            });
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenBreed.Common.Interface;
using OpenBreed.Database.EFCore;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Database.Xml.Repositories;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Database.Xml.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

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

            hostBuilder.SetupXmlUnitOfWork((unitOfWork, sp) =>
            {
                var database = sp.GetRequiredService<IDatabase>();
                unitOfWork.RegisterRepository(new XmlDataSourcesRepository(database.GetTable<XmlDbDataSourceTableDef>()));
                unitOfWork.RegisterRepository(new XmlTileAtlasRepository(database.GetTable<XmlDbTileAtlasTableDef>()));
                unitOfWork.RegisterRepository(new XmlTileStampsRepository(database.GetTable<XmlDbTileStampTableDef>()));
                unitOfWork.RegisterRepository(new XmlSpriteAtlasRepository(database.GetTable<XmlDbSpriteAtlasTableDef>()));
                unitOfWork.RegisterRepository(new XmlActionSetsRepository(database.GetTable<XmlDbActionSetTableDef>()));
                unitOfWork.RegisterRepository(new XmlImagesRepository(database.GetTable<XmlDbImageTableDef>()));
                unitOfWork.RegisterRepository(new XmlPalettesRepository(database.GetTable<XmlDbPaletteTableDef>()));
                unitOfWork.RegisterRepository(new XmlTextsRepository(database.GetTable<XmlDbTextTableDef>()));
                unitOfWork.RegisterRepository(new XmlMapsRepository(database.GetTable<XmlDbMapTableDef>()));
                unitOfWork.RegisterRepository(new XmlSoundsRepository(database.GetTable<XmlDbSoundTableDef>()));
                unitOfWork.RegisterRepository(new XmlSongsRepository(database.GetTable<XmlDbSongTableDef>()));
                unitOfWork.RegisterRepository(new XmlScriptsRepository(database.GetTable<XmlDbScriptTableDef>()));
                unitOfWork.RegisterRepository(new XmlAnimationsRepository(database.GetTable<XmlDbAnimationTableDef>()));
                unitOfWork.RegisterRepository(new XmlEntityTemplatesRepository(database.GetTable<XmlDbEntityTemplateTableDef>()));
                unitOfWork.RegisterRepository(new XmlEntityClassesRepository(database.GetTable<XmlDbEntityClassTableDef>()));
            });
        }

        public static void SetupXmlReadonlyDatabase(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IRepositoryProvider>((sp) => new XmlReadonlyDatabaseMan(
                    sp.GetService<OpenBreedDbContext>(),
                    sp.GetService<IVariableMan>(),
                    sp.GetService<IOptions<XmlDbSettings>>()));
            });
        }

        #endregion Public Methods

        #region Internal Methods

        internal static void SetupXmlUnitOfWork(this IHostBuilder hostBuilder, Action<XmlUnitOfWork, IServiceProvider> action)
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

        #endregion Internal Methods
    }
}
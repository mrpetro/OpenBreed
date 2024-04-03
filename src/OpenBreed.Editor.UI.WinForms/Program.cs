using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.VM.Extensions;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.VM.Tiles;
using System;
using OpenBreed.Common.Database.Xml.Extensions;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Database.Xml.Repositories;
using OpenBreed.Database.Xml.Tables;
using OpenBreed.Database.EFCore;
using OpenBreed.Database.EFCore.Extensions;
using OpenBreed.Editor.UI.WinForms.Extensions;
using OpenBreed.Editor.UI.Wpf.Extensions;

namespace OpenBreed.Editor.UI.WinForms
{
    internal static class Program
    {
        #region Private Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new GLTestForm());

            var builder = new HostBuilder();

            builder.SetupABFormats();
            builder.SetupDataProviders();
            builder.SetupModelProvider();

            builder.ConfigurePcmPlayer();

            builder.ConfigureServices((hostContext, services) =>
                 {
                     services.AddSingleton<ILogger>(
                         (sp) => new DefaultLogger());

                     services.AddSingleton<IVariableMan>(
                         (sp) => new VariableMan(
                             sp.GetService<ILogger>()));

                     services.AddSingleton<SettingsMan>(
                         (sp) => new SettingsMan(
                             sp.GetService<IVariableMan>(),                                                         
                             sp.GetService<ILogger>()));

                     services.AddSingleton<DbEntryFactory>(
                         (sp) => new DbEntryFactory());

                     services.AddSingleton<EditorApplication>(
                         (sp) => new EditorApplication(
                             sp,
                             sp.GetService<DataSourceProvider>()));

                     services.AddSingleton<IDialogProvider>(
                         (sp) => new DialogProvider(
                             sp.GetService<EditorApplication>()));

                     services.AddSingleton<IWorkspaceMan>(
                         (sp) => new EditorWorkspaceMan(
                             sp.GetService<IDatabase>(),                                                              
                             sp.GetService<ILogger>()));

                     services.AddSingleton<IRepositoryProvider>(
                         (sp) => sp.GetService<IWorkspaceMan>());

                 });

            builder.SetupEFDatabaseContext((efDatabaseContext, sp) =>
            {
            });

            builder.SetupXmlDatabase((databaseMan, sp) =>
            {
            });

            builder.SetupXmlUnitOfWork((unitOfWork, sp) =>
            {
                var database = sp.GetService<IDatabase>();
                var context = sp.GetService<OpenBreedDbContext>();

                //unitOfWork.RegisterRepository(new DataSourcesRepository(context, database.GetTable<XmlDbDataSourceTableDef>()));
                //unitOfWork.RegisterRepository(new AssetsRepository(context, database.GetTable<XmlDbAssetTableDef>()));
                //unitOfWork.RegisterRepository(new TileAtlasRepository(context, database.GetTable<XmlDbTileAtlasTableDef>()));

                unitOfWork.RegisterRepository(new XmlDataSourcesRepository(database.GetTable<XmlDbDataSourceTableDef>()));
                unitOfWork.RegisterRepository(new XmlAssetsRepository(database.GetTable<XmlDbAssetTableDef>()));
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
            });

            builder.SetupCommonViewModels();
            builder.SetupDbEntryEditors();
            builder.SetupDbEntryEditorFactory();
            builder.SetupDbEntrySubEditorFactory();
            builder.ConfigureControlFactory();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var application = services.GetService<EditorApplication>();
                    application.Run();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error Occured");
                }
            }
        }

        #endregion Private Methods
    }
}
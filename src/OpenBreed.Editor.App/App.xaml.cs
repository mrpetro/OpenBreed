using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Database;
using System.Configuration;
using System.Data;
using System.Windows;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Database.Xml.Extensions;
using OpenBreed.Database.Xml.Repositories;
using OpenBreed.Database.Xml.Tables;
using OpenBreed.Database.EFCore;
using OpenBreed.Database.EFCore.Extensions;
using OpenBreed.Editor.VM.Extensions;
using OpenBreed.Editor.UI.Wpf.Extensions;
using OpenBreed.Common.Windows.Extensions;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Windows.Dialog;
using OpenBreed.Editor.Cfg.Managers;
using OpenBreed.Editor.VM.Options;
using OpenBreed.Editor.UI.Wpf.Options;
using OpenBreed.Editor.VM.Tools;
using OpenBreed.Editor.UI.Wpf.Tools;
using OpenBreed.Common.Tools.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using OpenBreed.Common.Logging;
using OpenBreed.Editor.VM.Logging;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Core.Extensions;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Wecs.Components.Animation.Extensions;
using OpenBreed.Wecs.Components.Rendering.Extensions;
using OpenBreed.Wecs.Components.Physics.Extensions;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Common.Game.Extensions;
using OpenBreed.Common.Interface.Tools;
using OpenBreed.Gui.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;

namespace OpenBreed.Editor.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;

        public App()
        {
            ThreadTools.Initialize();

            var hostBuilder = new HostBuilder();

            hostBuilder.SetupDefaultLogger();
            hostBuilder.SetupDataHandlers();
            hostBuilder.ConfigureUIDispatcher(this.Dispatcher);
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                //Add business services as needed
                services.AddScoped<EditorApplicationVM>();

                services.AddSingleton((sp) =>
                {
                    var mainWindow = new MainWindow();
                    var vm = sp.GetRequiredService<EditorApplicationVM>();

                    vm.ExitAction = mainWindow.Close;
                    vm.ShowOptionsAction = () => sp.GetRequiredService<OptionsForm>().ShowDialog();
                    vm.ShowAbtaPasswordGeneratorAction = () => sp.GetRequiredService<AbtaPasswordGeneratorForm>().ShowDialog();

                    mainWindow.DataContext = vm;

                    return mainWindow;
                });
            });

            hostBuilder.SetupCommonGameServices(isEditor: true);
            hostBuilder.SetupCommonGameWecsServices(isEditor: true);

            hostBuilder.SetupDataLoaderFactory((dataLoaderFactory, sp) =>
            {
                dataLoaderFactory.RegisterGraphicsDataLoader(sp);
                dataLoaderFactory.SetupAnimationDataLoader<IEntity>(sp);
                dataLoaderFactory.SetupSoundSampleDataLoader(sp);
                dataLoaderFactory.SetupScriptDataLoader(sp);
            });

            hostBuilder.ConfigureInteraction();

            hostBuilder.ConfigureAbtaPasswordGeneratorForm();
            hostBuilder.ConfigureOptionsForm();

            hostBuilder.ConfigurePcmPlayer();

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                //services.AddSingleton<ILogger, DefaultLogger>();
                services.AddSingleton<IVariableMan, VariableMan>();
                services.AddSingleton<SettingsMan>();
                services.AddSingleton<DbEntryFactory>();
                services.AddSingleton<IDialogProvider, DialogProvider>();

                services.AddSingleton<IWorkspaceMan>(
                    (sp) => new EditorWorkspaceMan(
                        sp.GetService<IDatabase>(),
                        sp.GetService<ILogger>()));

                services.AddSingleton<IRepositoryProvider>(
                    (sp) => sp.GetService<IWorkspaceMan>());

            });

            hostBuilder.SetupEFDatabaseContext((efDatabaseContext, sp) =>
            {
            });

            hostBuilder.SetupXmlDatabase((databaseMan, sp) =>
            {
            });

            hostBuilder.SetupXmlUnitOfWork((unitOfWork, sp) =>
            {
                var database = sp.GetRequiredService<IDatabase>();
                //var context = sp.GetRequiredService<OpenBreedDbContext>();

                //unitOfWork.RegisterRepository(new DataSourcesRepository(context, database.GetTable<XmlDbDataSourceTableDef>()));
                //unitOfWork.RegisterRepository(new AssetsRepository(context, database.GetTable<XmlDbAssetTableDef>()));
                //unitOfWork.RegisterRepository(new TileAtlasRepository(context, database.GetTable<XmlDbTileAtlasTableDef>()));

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
            });

            hostBuilder.ConfigureCommonTools();
            hostBuilder.SetupCommonViewModels();
            hostBuilder.SetupEditorViewModels();
            hostBuilder.ConfigureEditorUIMvc();
            hostBuilder.SetupDbEntryEditorFactory();

            hostBuilder.SetupWindowsDrawingContext();

            host = hostBuilder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var mainWindow = services.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
            }
        }
    }

}

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

            var builder = new HostBuilder();

            builder.SetupDefaultLogger();
            builder.SetupDataHandlers();

            builder.ConfigureServices((hostContext, services) =>
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

            builder.SetupDataProviders();
            builder.SetupModelProvider();

            builder.SetupOpenALManagers();
            builder.SetupOpenGLManagers();

            builder.ConfigureAbtaPasswordGeneratorForm();
            builder.ConfigureOptionsForm();

            builder.ConfigurePcmPlayer();

            builder.ConfigureServices((hostContext, services) =>
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

            builder.SetupEFDatabaseContext((efDatabaseContext, sp) =>
            {
            });

            builder.SetupXmlDatabase((databaseMan, sp) =>
            {
            });

            builder.SetupXmlUnitOfWork((unitOfWork, sp) =>
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

            builder.ConfigureCommonTools();
            builder.SetupCommonViewModels();
            builder.SetupEditorViewModels();
            builder.SetupDbEntryEditorFactory();

            builder.SetupWindowsDrawingContext();

            host = builder.Build();

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

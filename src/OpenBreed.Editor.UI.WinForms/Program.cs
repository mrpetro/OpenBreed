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
using OpenBreed.Editor.UI.WinForms.Extensions;
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

            var managerCollection = new DefaultManagerCollection();

            managerCollection.SetupABFormats();

            managerCollection.AddSingleton<ILogger>(() => new DefaultLogger());

            managerCollection.AddSingleton<IVariableMan>(() => new VariableMan(managerCollection.GetManager<ILogger>()));

            managerCollection.AddSingleton<SettingsMan>(() => new SettingsMan(managerCollection.GetManager<IVariableMan>(),
                                                                              managerCollection.GetManager<ILogger>()));
            managerCollection.AddSingleton<DbEntryFactory>(() => new DbEntryFactory());

            managerCollection.AddSingleton<EditorApplication>(() => new EditorApplication(managerCollection));

            managerCollection.AddSingleton<XmlDatabaseMan>(() => new XmlDatabaseMan(managerCollection.GetManager<IVariableMan>()));

            managerCollection.AddSingleton<IDialogProvider>(() => new DialogProvider(managerCollection.GetManager<EditorApplication>()));

            managerCollection.AddSingleton<IWorkspaceMan>(() => new EditorWorkspaceMan(managerCollection.GetManager<XmlDatabaseMan>(),
                                                                                       managerCollection.GetManager<ILogger>()));

            managerCollection.AddSingleton<DataProvider>(() => new DataProvider(managerCollection.GetManager<IWorkspaceMan>(),
                                                                                managerCollection.GetManager<ILogger>(), 
                                                                                managerCollection.GetManager<IVariableMan>(),
                                                                                managerCollection.GetManager<DataFormatMan>()));

            managerCollection.AddSingleton<IDataProvider>(managerCollection.GetManager<DataProvider>());



            managerCollection.SetupDbEntryEditorFactory();

            var application = managerCollection.GetManager<EditorApplication>();
            application.Run();
        }

        #endregion Private Methods
    }
}
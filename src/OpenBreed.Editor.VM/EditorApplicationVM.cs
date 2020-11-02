using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Tiles;
using System.Drawing;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Sprites;
using System.Diagnostics;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Editor.Cfg;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Common.Formats;
using OpenBreed.Model.Actions;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Database.Entries;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Editor.VM.Logging;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Common.Tools;

namespace OpenBreed.Editor.VM
{
    public enum EditorState
    {
        Active,
        Exiting,
        Exited
    }

    public class EditorApplicationVM : BaseViewModel, IApplicationInterface
    {

        #region Private Fields

        private EditorState _state;

        #endregion Private Fields

        #region Public Constructors

        private EditorApplication application;

        public EditorApplicationVM(EditorApplication application)
        {
            this.application = application;

            Logger = new LoggerVM(application.Logger);

            DialogProvider = application.GetInterface<IDialogProvider>();

            DbEditor = new DbEditorVM(application);
            //PaletteEditor = new PaletteEditorVM();
            //SpriteViewer = new SpriteViewerVM(this);
            DataSourceProvider.ExpandGlobalVariables = application.Variables.ExpandVariables;
        }

        public void ShowOptions()
        {
            ShowOptionsAction?.Invoke(application.Settings);
        }

        #endregion Public Constructors

        #region Public Properties

        public DbEditorVM DbEditor { get; }
        public IDialogProvider DialogProvider { get; }
        public LoggerVM Logger { get; }
        public Action<LoggerVM> ShowLoggerAction { get; set; }
        public Action<SettingsMan> ShowOptionsAction { get; set; }

        public EditorState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Run()
        {
            try
            {
                Initialize();
                DialogProvider.ShowEditorView(this);
            }
            catch (Exception ex)
            {
                DialogProvider.ShowMessage("Critical exception: " + ex, "Open Breed Editor critial exception");
            }
        }

        public void ShowLogger()
        {
            ShowLoggerAction?.Invoke(Logger);
        }

        public bool TryExit()
        {
            if (application.UnitOfWork != null)
            {
                if (DbEditor.IsModified)
                {
                    var answer = DialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before exiting?",
                                                                               "Save database before exiting?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                        return false;
                    else if (answer == DialogAnswer.Yes)
                        DbEditor.Save();
                }
            }

            return true;
        }

        public void TryRunABTAGame()
        {
            Other.TryAction(RunABTAGame);
        }

        #endregion Public Methods

        #region Internal Methods

        internal bool TrySaveDatabase()
        {
            throw new NotImplementedException();
        }

        #endregion Internal Methods

        #region Private Methods

        private void Initialize()
        {
            var dbTableSelectorConnector = new DbTableSelectorConnector(DbEditor.DbTablesEditor.DbTableSelector);
            dbTableSelectorConnector.ConnectTo(DbEditor);
            var dbTableEditorConnector = new DbTableEditorConnector(DbEditor.DbTablesEditor.DbTableEditor);
            dbTableEditorConnector.ConnectTo(DbEditor.DbTablesEditor.DbTableSelector);
        }
        private void RunABTAGame()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = application.Settings.Cfg.Options.ABTA.GameRunFilePath;
            proc.StartInfo.Arguments = application.Settings.Cfg.Options.ABTA.GameRunFileArgs;
            proc.StartInfo.WorkingDirectory = application.Settings.Cfg.Options.ABTA.GameFolderPath;
            proc.Start();
        }

        #endregion Private Methods

    }
}

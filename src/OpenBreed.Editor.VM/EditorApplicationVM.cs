﻿using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;

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

        private EditorApplication application;

        private LoggerVM logger;

        #endregion Private Fields

        #region Public Constructors

        public EditorApplicationVM(EditorApplication application)
        {
            this.application = application;

            DialogProvider = application.GetInterface<IDialogProvider>();
            DbEditor = new DbEditorVM(application);

            MenuItems = new BindingList<MenuItemVM>();

            //MenuItems =
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<MenuItemVM> MenuItems { get; }

        public DbEditorVM DbEditor { get; }

        public IDialogProvider DialogProvider { get; }

        public Action<LoggerVM, bool> ToggleLoggerAction { get; set; }



        public Action<SettingsMan> ShowOptionsAction { get; set; }

        public EditorState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void ShowOptions()
        {
            ShowOptionsAction?.Invoke(application.Settings);
        }

        public void Run()
        {
            try
            {
                DialogProvider.ShowEditorView(this);
            }
            catch (Exception ex)
            {
                DialogProvider.ShowMessage("Critical exception: " + ex, "Open Breed Editor critial exception");
            }
        }

        public void ToggleLogger(bool toggle)
        {
            if (logger == null)
                logger = application.CreateLoggerVm();

            ToggleLoggerAction?.Invoke(logger, toggle);
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

        #region Protected Methods



        #endregion Protected Methods

        #region Private Methods



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
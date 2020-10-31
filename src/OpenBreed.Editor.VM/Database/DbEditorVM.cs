﻿using OpenBreed.Common;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Entries;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using System.IO;
using OpenBreed.Common.Tools;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEditorVM : BaseViewModel
    {

        #region Private Fields

        private readonly Dictionary<string, EntryEditorVM> _openedEntryEditors = new Dictionary<string, EntryEditorVM>();
        private DatabaseVM _editable;

        #endregion Private Fields

        #region Public Constructors
        private EditorApplication application;
        public DbEditorVM(EditorApplication application)
        {
            this.application = application;

            DbTablesEditor = new DbTablesEditorVM(application);
        }

        #endregion Public Constructors

        #region Public Properties

        public DbTablesEditorVM DbTablesEditor { get; }

        public DatabaseVM Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
        }

        public Action<EntryEditorVM> EntryEditorOpeningAction { get; set; }

        public bool IsModified { get; internal set; }

        #endregion Public Properties

        #region Public Methods

        public void CloseAllEditors()
        {
            var toClose = _openedEntryEditors.Values.ToArray();

            foreach (var entryEditor in toClose)
                entryEditor.Close();
        }

        public void EditModel(IUnitOfWork model)
        {
            //Unsubscribe to previous edited item changes
            if (Editable != null)
                Editable.PropertyChanged -= CurrentDb_PropertyChanged;

            var vm = new DatabaseVM();
            UpdateVM(model, vm);
            Editable = vm;
            Editable.PropertyChanged += CurrentDb_PropertyChanged;


        }

        private bool TryCloseDatabaseInternal()
        {
            if (Editable == null)
                throw new InvalidOperationException("Expected current database");

            if (Editable.IsModified)
            {
                var answer = application.GetInterface<IDialogProvider>().ShowMessageWithQuestion("Current database has been modified. Do you want to save it before closing?",
                                                                           "Save database before closing?", QuestionDialogButtons.YesNoCancel);

                if (answer == DialogAnswer.Cancel)
                    return false;
                else if (answer == DialogAnswer.Yes)
                    Save();
            }

            Editable.Dispose();
            Editable = null;

            return true;
        }

        public bool TryCloseDatabase()
        {
            if (TryCloseDatabaseInternal())
            {
                application.CloseDatabase();

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// This checks if database is opened already,
        /// If it is then it asks of it can be closed
        /// </summary>
        /// <returns>True if no database was opened or if previous one was closed, false otherwise</returns>
        internal bool CheckCloseCurrentDatabase(DbEditorVM dbEditor, string newDatabaseFilePath)
        {
            if (dbEditor.Editable != null)
            {
                if (IOHelper.GetNormalizedPath(newDatabaseFilePath) == IOHelper.GetNormalizedPath(dbEditor.Editable.Name))
                {
                    //Root.Logger.Warning("Database already opened.");
                    return false;
                }

                var answer = application.GetInterface<IDialogProvider>().ShowMessageWithQuestion($"Another database ({dbEditor.Editable.Name}) is already opened. Do you want to close it?",
                                                                "Close current database?",
                                                                QuestionDialogButtons.OKCancel);
                if (answer != DialogAnswer.OK)
                    return false;

                if (!dbEditor.TryCloseDatabase())
                    return false;
            }

            return true;
        }

        public bool TryOpenXmlDatabase()
        {
            var openFileDialog = application.GetInterface<IDialogProvider>().OpenFileDialog();
            openFileDialog.Title = "Select an Open Breed Editor Database file to open...";
            openFileDialog.Filter = "Open Breed Editor Database files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = XmlDatabase.DefaultDirectoryPath;
            openFileDialog.Multiselect = false;

            var answer = openFileDialog.Show();

            if (answer != DialogAnswer.OK)
                return false;

            string databaseFilePath = openFileDialog.FileName;

            if (!CheckCloseCurrentDatabase(this, databaseFilePath))
                return false;

            var unitOfWork = application.OpenXmlDatabase(databaseFilePath);

            EditModel(unitOfWork);
            return true;
        }

        public void TrySaveDatabase()
        {
            if (Editable == null)
                throw new InvalidOperationException("Expected current database");

            Save();
        }

        #endregion Public Methods

        #region Internal Methods

        internal EntryEditorVM OpenEntryEditor(IRepository repository, string entryId)
        {
            string entryEditorKey = $"{repository.Name}#{entryId}";

            EntryEditorVM entryEditor = null;
            if (!_openedEntryEditors.TryGetValue(entryEditorKey, out entryEditor))
            {
                entryEditor = application.GetInterface<DbEntryEditorFactory>().CreateEditor(repository);
                _openedEntryEditors.Add(entryEditorKey, entryEditor);
                entryEditor.ClosedAction = () => OnEntryEditorClosed(entryEditor);
                entryEditor.EditEntry(entryId);
                EntryEditorOpeningAction?.Invoke(entryEditor);
            }
            else
                entryEditor.Activate();

            return entryEditor;
        }

        internal void Save()
        {
            ServiceLocator.Instance.GetService<DataProvider>().Save();
            //_edited.Save();
        }

        #endregion Internal Methods

        #region Private Methods

        private void CurrentDb_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void OnEntryEditorClosed(EntryEditorVM editor)
        {
            //TODO: This can be done better
            var foundItem = _openedEntryEditors.FirstOrDefault(item => item.Value == editor);       
            _openedEntryEditors.Remove(foundItem.Key);
        }

        private void UpdateVM(IUnitOfWork source, DatabaseVM target)
        {
            target.Name = source.Name;
        }

        #endregion Private Methods

    }
}

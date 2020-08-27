using OpenBreed.Common;
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

namespace OpenBreed.Editor.VM.Database
{
    public class DbEditorVM : BaseViewModel
    {

        #region Private Fields

        private readonly Dictionary<string, EntryEditorVM> _openedEntryEditors = new Dictionary<string, EntryEditorVM>();
        private DatabaseVM _editable;
        private IUnitOfWork _edited;
        private readonly EntryEditorVM _activeEntryEditor;

        #endregion Private Fields

        #region Public Constructors

        public DbEditorVM()
        {
            DbTablesEditor = new DbTablesEditorVM();
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

            _edited = model;

            ServiceLocator.Instance.RegisterService<IUnitOfWork>(model);
            ServiceLocator.Instance.RegisterService<DataProvider>(new DataProvider(model));

            var vm = new DatabaseVM();
            UpdateVM(model, vm);
            Editable = vm;
            Editable.PropertyChanged += CurrentDb_PropertyChanged;


        }

        public bool TryCloseDatabase()
        {
            if (DbEditorVMHelper.TryCloseDatabase(this))
            {
                _edited = null;
                ServiceLocator.Instance.UnregisterService<DataProvider>();
                ServiceLocator.Instance.UnregisterService<IUnitOfWork>();
                return true;
            }
            else
                return false;
        }

        public void TryOpenXmlDatabase()
        {
            DbEditorVMHelper.TryOpenXmlDatabase(this);
        }

        public void TrySaveDatabase()
        {
            DbEditorVMHelper.TrySaveDatabase(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal EntryEditorVM OpenEntryEditor(IRepository repository, string entryId)
        {
            string entryEditorKey = $"{repository.Name}#{entryId}";

            EntryEditorVM entryEditor = null;
            if (!_openedEntryEditors.TryGetValue(entryEditorKey, out entryEditor))
            {
                entryEditor = ServiceLocator.Instance.GetService<DbEntryEditorFactory>().CreateEditor(repository);
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

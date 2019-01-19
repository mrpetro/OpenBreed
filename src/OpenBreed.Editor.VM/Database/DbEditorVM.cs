using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Editor.VM.Assets;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Items;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEditorVM : BaseViewModel
    {

        #region Private Fields

        private readonly Dictionary<string, EntryEditorVM> _openedEntryEditors = new Dictionary<string, EntryEditorVM>();
        private DatabaseVM _editable;
        private IUnitOfWork _edited;
        private EntryEditorVM _activeEntryEditor;

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

        internal void OpenEntryEditor(IRepository repository, string entryName)
        {
            string entryEditorKey = $"{repository.Name}#{entryName}";

            EntryEditorVM entryEditor = null;
            if (!_openedEntryEditors.TryGetValue(entryEditorKey, out entryEditor))
            {
                entryEditor = ServiceLocator.Instance.GetService<DbEntryEditorFactory>().CreateEditor(repository);
                _openedEntryEditors.Add(entryEditorKey, entryEditor);
                entryEditor.ClosedAction = () => OnEntryEditorClosed(entryEditor);
                EntryEditorOpeningAction?.Invoke(entryEditor);
                entryEditor.EditEntry(entryName);
            }
            else
                entryEditor.Activate();
        }

        internal void Save()
        {
            _editable.Save();
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddNewEntryEditor()
        {

        }
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

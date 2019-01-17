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

        private DatabaseVM _editable;
        private IUnitOfWork _edited;

        #endregion Private Fields

        #region Public Constructors

        public DbEditorVM()
        {
            DbTablesEditor = new DbTablesEditorVM();




            DbEntryEditors = new BindingList<EntryEditorVM>();
            DbEntryEditors.ListChanged += (s, a) => OnPropertyChanged(nameof(DbEntryEditors));

        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<EntryEditorVM> DbEntryEditors { get; }

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

        internal void OpenEntryEditor(DbEntryVM entry)
        {
            //Check if entry editor is already opened. If yes then focus on this entry editor.
            //var openedDbEntryEditor = DbEntryEditors.FirstOrDefault(item => item.)

            var entryType = entry.GetType();
            var editor = ServiceLocator.Instance.GetService<EntryEditorFactory>().CreateEditor(entryType);
            DbEntryEditors.Add(editor);

            EntryEditorOpeningAction?.Invoke(editor);

            editor.ClosedAction = () => OnEntryEditorClosed(editor);

            editor.OpenEntry(entry.Name);
        }

        internal void Save()
        {
            _editable.Save();
        }

        #endregion Internal Methods

        #region Private Methods

        private void CurrentDb_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void OnEntryEditorClosed(EntryEditorVM editor)
        {
            DbEntryEditors.Remove(editor);
        }

        private void UpdateVM(IUnitOfWork source, DatabaseVM target)
        {
            target.Name = source.Name;
        }

        public void CloseAllEditors()
        {
            var toClose = DbEntryEditors.ToArray();

            foreach (var entryEditor in toClose)
                entryEditor.Close();
        }

        #endregion Private Methods

    }
}

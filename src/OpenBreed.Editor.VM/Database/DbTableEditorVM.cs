using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Entries;
using OpenBreed.Editor.VM.Database.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableEditorVM : BaseViewModel
    {
        private DbTableNewEntryCreatorVM _newEntryCreator;

        #region Private Fields

        private DbEntryVM _currentItem;
        private DbTableVM _editable;
        private IRepository _edited;
        private string _title;

        #endregion Private Fields

        #region Internal Constructors

        private readonly EditorApplication application;

        internal DbTableEditorVM(EditorApplication application)
        {
            this.application = application;
        }

        #endregion Internal Constructors



        #region Public Properties

        public DbEntryVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public DbTableVM Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
        }

        public string EditorName { get { return "Table Editor"; } }

        public Action<DbTableNewEntryCreatorVM> OpenNewEntryCreatorAction { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Public Methods


        private string GetUniqueId()
        {
            string uniqueId = $"{Editable.Name}.{DateTime.Now.Ticks}";

            return uniqueId;
        }

        public void OpenNewEntryCreator()
        {
            _newEntryCreator = new DbTableNewEntryCreatorVM();
            _newEntryCreator.CreateAction = OnNewEntryCreate;
            _newEntryCreator.ValidateNewIdFunc = OnValidateNewId;

            _edited.EntryTypes.ForEach(item => _newEntryCreator.EntryTypes.Add(new EntryTypeVM(item)));

            _newEntryCreator.EntryType = _newEntryCreator.EntryTypes.FirstOrDefault();
            _newEntryCreator.NewId = GetUniqueId();
            OpenNewEntryCreatorAction?.Invoke(_newEntryCreator);
        }

        /// <summary>
        /// This validation function will check if entry id is unique in repository and its not empty
        /// </summary>
        /// <param name="id">Id to validate</param>
        /// <returns>true if given id is valid, false otherwise</returns>
        private bool OnValidateNewId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            return !_edited.Entries.Any(item => item.Id == id);
        }

        private void OnNewEntryCreate()
        {
            var newEntryId = _newEntryCreator.NewId;
            var newEntryType = _newEntryCreator.EntryType.Type;

            _newEntryCreator.Close();

            var entry = _edited.New(newEntryId, newEntryType);

            var dbEntryFactory = application.GetInterface<DbEntryFactory>();
            var dbEntry = dbEntryFactory.Create(entry);
            dbEntry.Load(entry);
            Editable.Entries.Add(dbEntry);
            EditEntry(newEntryId);
        }

        public void EditEntry(string entryId)
        {

            //Check if entry editor is already opened. If yes then focus on this entry editor.
            //var openedDbEntryEditor = DbEntryEditors.FirstOrDefault(item => item.)
            var dbEditor = application.GetInterface<EditorApplicationVM>().DbEditor;

            var entryEditor = dbEditor.OpenEntryEditor(_edited, entryId);
            entryEditor.CommitedAction = OnEntryCommited;
        }

        public void EditRepository(IRepository model)
        {
            //Unsubscribe to previous edited item changes
            if (Editable != null)
                Editable.PropertyChanged -= Editable_PropertyChanged;

            _edited = model;

            var vm = application.GetInterface<DbTableFactory>().CreateTable(_edited);
            UpdateVM(model, vm);
            Editable = vm;
            Editable.PropertyChanged += Editable_PropertyChanged;
            UpdateTitle();

            if (_newEntryCreator != null)
            {
                _newEntryCreator.Close();
                _newEntryCreator = null;
            }
        }

        public void SetModel(string modelName)
        {
            var repository = application.DataProvider.GetRepository(modelName);

            if (repository == null)
                throw new InvalidOperationException($"Repository with name '{modelName}' not found.");

            EditRepository(repository);
        }

        public void SetNoModel()
        {
            if (Editable != null)
                Editable.PropertyChanged -= Editable_PropertyChanged;

            _edited = null;
            Editable = null;

            UpdateTitle();
        }

        #endregion Public Methods

        //public void OnStore()
        //{
        //    UpdateEntry(_editable, _edited);

        #region Protected Methods

        protected void UpdateEntry(DbTableVM source, IRepository target)
        {

        }

        protected void UpdateVM(IRepository source, DbTableVM target)
        {
            var dbEntryFactory = application.GetInterface<DbEntryFactory>();

            target.Entries.UpdateAfter(() =>
            {
                target.Entries.Clear();

                foreach (var entry in _edited.Entries)
                {
                    var dbEntry = dbEntryFactory.Create(entry);
                    dbEntry.Load(entry);
                    target.Entries.Add(dbEntry);
                }
            });
        }

        #endregion Protected Methods

        #region Private Methods

        private void Editable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //    //if (EditMode)
        //    //    _repo.Update(_edited);
        //    //else
        //    //    _repo.Add(_edited);
        //    //Done();
        //}
        private void OnEntryCommited(string entryId)
        {
            var entryVM = _editable.Entries.FirstOrDefault(item => item.Id == entryId);

            if (entryVM != null)
                entryVM.Load(entryVM.Entry);
        }
        private void UpdateTitle()
        {
            if (Editable == null)
                Title = $"{EditorName} - no entry to edit";
            else
                Title = $"{EditorName} - {Editable.Name}";
        }

        #endregion Private Methods

    }
}
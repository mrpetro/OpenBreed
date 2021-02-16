using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Database
{
    internal delegate EntryEditorVM EntryEditorOpener(IRepository repository, string entryId);

    public class DbTableEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;
        private readonly DbEntryFactory dbEntryFactory;
        private DbTableNewEntryCreatorVM _newEntryCreator;

        private DbEntryVM _currentItem;
        private IRepository _edited;
        private string _title;
        private string tableName;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableEditorVM(IWorkspaceMan workspaceMan, DbEntryFactory dbEntryFactory)
        {
            this.workspaceMan = workspaceMan;
            this.dbEntryFactory = dbEntryFactory;
            Entries = new BindingList<Entries.DbEntryVM>();
            Entries.ListChanged += (s, a) => OnPropertyChanged(nameof(Entries));
        }

        #endregion Internal Constructors

        #region Public Properties

        public BindingList<DbEntryVM> Entries { get; }

        public DbEntryVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public string EditorName { get { return "Table Editor"; } }

        public Action<DbTableNewEntryCreatorVM> OpenNewEntryCreatorAction { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string TableName
        {
            get { return tableName; }
            set { SetProperty(ref tableName, value); }
        }

        #endregion Public Properties

        #region Internal Properties

        internal EntryEditorOpener EntryEditorOpener { get; set; }

        #endregion Internal Properties

        #region Public Methods

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

        public void EditEntry(string entryId)
        {
            //Check if entry editor is already opened. If yes then focus on this entry editor.
            //var openedDbEntryEditor = DbEntryEditors.FirstOrDefault(item => item.)
            var entryEditor = EntryEditorOpener.Invoke(_edited, entryId);
            entryEditor.CommitedAction = OnEntryCommited;
        }

        public void EditRepository(IRepository model)
        {
            _edited = model;

            //var vm = application.GetInterface<DbTableFactory>().CreateTable(_edited);
            UpdateVM(model);

            UpdateTitle();

            if (_newEntryCreator != null)
            {
                _newEntryCreator.Close();
                _newEntryCreator = null;
            }
        }

        public void SetModel(string modelName)
        {
            var repository = workspaceMan.UnitOfWork.GetRepository(modelName);

            if (repository == null)
                throw new InvalidOperationException($"Repository with name '{modelName}' not found.");

            EditRepository(repository);
        }

        public void SetNoModel()
        {
            Entries.Clear();

            UpdateTitle();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void UpdateEntry(IRepository target)
        {
        }

        protected void UpdateVM(IRepository source)
        {
            Entries.UpdateAfter(() =>
            {
                Entries.Clear();

                foreach (var entry in _edited.Entries)
                {
                    var dbEntry = dbEntryFactory.Create(entry);
                    dbEntry.Load(entry);
                    Entries.Add(dbEntry);
                }
            });

            TableName = source.Name;
        }

        #endregion Protected Methods

        #region Private Methods

        private string GetUniqueId()
        {
            string uniqueId = $"{TableName}.{DateTime.Now.Ticks}";

            return uniqueId;
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
            var dbEntry = dbEntryFactory.Create(entry);
            dbEntry.Load(entry);
            Entries.Add(dbEntry);
            EditEntry(newEntryId);
        }

        private void OnEntryCommited(string entryId)
        {
            var entryVM = Entries.FirstOrDefault(item => item.Id == entryId);

            if (entryVM != null)
                entryVM.Load(entryVM.Entry);
        }

        private void UpdateTitle()
        {
            Title = $"{EditorName} - {TableName}";
        }

        #endregion Private Methods
    }
}
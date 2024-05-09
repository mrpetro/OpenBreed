using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Database
{
    internal delegate EntryEditorVM EntryEditorOpener(string tableName, string entryId);

    public class DbTableEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;
        private readonly DbEntryFactory dbEntryFactory;

        private DbEntryVM _currentItem;
        private IRepository _edited;
        private string _title;
        private string tableName;
        private string tablePresentationName;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableEditorVM(IWorkspaceMan workspaceMan, DbEntryFactory dbEntryFactory)
        {
            this.workspaceMan = workspaceMan;
            this.dbEntryFactory = dbEntryFactory;
            Entries = new BindingList<Entries.DbEntryVM>();
            Entries.ListChanged += (s, a) => OnPropertyChanged(nameof(Entries));

            OpenEntryCommand = new Command(() => EditEntry(CurrentItem.Id));
            StartNewEntryCommand = new Command(() => StartNewEntry());

            NewEntryCreator = new DbTableNewEntryCreatorVM();
            NewEntryCreator.CreateAction = OnNewEntryCreate;
            NewEntryCreator.ValidateNewIdFunc = OnValidateNewId;
        }

        #endregion Internal Constructors

        #region Public Properties

        public DbTableNewEntryCreatorVM NewEntryCreator { get; }

        public BindingList<DbEntryVM> Entries { get; }

        public DbEntryVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public string EditorName
        { get { return "Table Editor"; } }

        public Action<DbTableNewEntryCreatorVM> OpenNewEntryCreatorAction { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string TablePresentationName
        {
            get { return tablePresentationName; }
            set { SetProperty(ref tablePresentationName, value); }
        }

        public ICommand OpenEntryCommand { get; }
        public ICommand StartNewEntryCommand { get; }

        #endregion Public Properties

        #region Internal Properties

        internal EntryEditorOpener EntryEditorOpener { get; set; }

        #endregion Internal Properties

        #region Public Methods

        public void StartNewEntry()
        {
            NewEntryCreator.IsVisible = true;
            NewEntryCreator.NewId = GetUniqueId();


            NewEntryCreator.EntryTypes.Clear();
            _edited.EntryTypes.ForEach(item => NewEntryCreator.EntryTypes.Add(new EntryTypeVM(item)));
            NewEntryCreator.SelectedEntryType = NewEntryCreator.EntryTypes.FirstOrDefault();
        }

        public void EditEntry(string entryId)
        {
            //Check if entry editor is already opened. If yes then focus on this entry editor.
            //var openedDbEntryEditor = DbEntryEditors.FirstOrDefault(item => item.)
            var entryEditor = EntryEditorOpener.Invoke(tableName, entryId);
            entryEditor.CommitedAction = OnEntryCommited;
        }

        public void SetModel(string modelName)
        {
            var repository = workspaceMan.GetRepository(modelName);

            if (repository == null)
                throw new InvalidOperationException($"Repository with name '{modelName}' not found.");

            _edited = repository;

            //var vm = application.GetInterface<DbTableFactory>().CreateTable(_edited);
            UpdateVM(_edited);

            tableName = modelName;
            TablePresentationName = modelName;

            UpdateTitle();

            NewEntryCreator.IsVisible = false;
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
        }

        #endregion Protected Methods

        #region Private Methods

        private string GetUniqueId()
        {
            string uniqueId = $"{TablePresentationName}.{DateTime.Now.Ticks}";

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
            var newEntryId = NewEntryCreator.NewId;
            var newEntryType = NewEntryCreator.SelectedEntryType.Type;

            NewEntryCreator.IsVisible = false;

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
            Title = $"{EditorName} - {TablePresentationName}";
        }

        internal void Refresh()
        {
        }

        #endregion Private Methods
    }
}
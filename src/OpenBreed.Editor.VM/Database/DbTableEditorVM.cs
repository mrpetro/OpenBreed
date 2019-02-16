using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Entries;
using OpenBreed.Editor.VM.Database.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableEditorVM : BaseViewModel
    {
        private DbTableNewEntryCreatorVM _newEntryCreator;

        //public MyIcommand CancelCommand { get; private set; }

        #region Private Fields

        private DbEntryVM _currentItem;
        private DbTableVM _editable;
        private IRepository _edited;
        private string _title;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableEditorVM()
        {
            //Items = new BindingList<Items.DbEntryVM>();
            //Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Internal Constructors

        //public BindingList<DbEntryVM> Items { get; }


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

        public Action<string, string> EditEntryAction { get; set; }

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
            _newEntryCreator.NewId = GetUniqueId();
            OpenNewEntryCreatorAction?.Invoke(_newEntryCreator);
        }

        private void OnNewEntryCreate()
        {
            var newEntryId = _newEntryCreator.NewId;
            _newEntryCreator.Close();

            var entry = _edited.New(newEntryId);

            var dbEntryFactory = ServiceLocator.Instance.GetService<DbEntryFactory>();
            var dbEntry = dbEntryFactory.Create(entry);
            dbEntry.Load(entry);
            Editable.Entries.Add(dbEntry);
            EditEntry(newEntryId);
        }

        public void EditEntry(string entryId)
        {

            //Check if entry editor is already opened. If yes then focus on this entry editor.
            //var openedDbEntryEditor = DbEntryEditors.FirstOrDefault(item => item.)
            var dbEditor = ServiceLocator.Instance.GetService<EditorVM>().DbEditor;

            var entryEditor = dbEditor.OpenEntryEditor(_edited, entryId);
            entryEditor.CommitedAction = OnEntryCommited;
        }

        public void EditRepository(IRepository model)
        {
            //Unsubscribe to previous edited item changes
            if (Editable != null)
                Editable.PropertyChanged -= Editable_PropertyChanged;

            _edited = model;

            var vm = ServiceLocator.Instance.GetService<DbTableFactory>().CreateTable(_edited);
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
            var repository = ServiceLocator.Instance.GetService<IUnitOfWork>().GetRepository(modelName);

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
            var dbEntryFactory = ServiceLocator.Instance.GetService<DbEntryFactory>();

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
            var entry = _edited.Entries.FirstOrDefault(item => item.Id == entryId);

            if (entryVM != null && entry != null)
                entryVM.Load(entry);
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
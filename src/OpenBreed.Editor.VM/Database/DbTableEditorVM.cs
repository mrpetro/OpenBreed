using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Items;
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


        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DbEntryVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        #region Public Properties

        public DbTableVM Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void SetNoModel()
        {
            if (Editable != null)
                Editable.PropertyChanged -= Editable_PropertyChanged;

            _edited = null;
            Editable = null;

            UpdateTitle();
        }

        public void SetModel(string modelName)
        {
            var repository = ServiceLocator.Instance.GetService<IUnitOfWork>().GetRepository(modelName);

            if (repository == null)
                throw new InvalidOperationException($"Repository with name '{modelName}' not found.");

            EditRepository(repository);
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
        }

        public string EditorName { get { return "Table Editor"; } }

        public Action<string, string> EditEntryAction { get; set; }

        //public void OnStore()
        //{
        //    UpdateEntry(_editable, _edited);

        //    //if (EditMode)
        //    //    _repo.Update(_edited);
        //    //else
        //    //    _repo.Add(_edited);
        //    //Done();
        //}

        public void EditEntity(string entryName)
        {

            //Check if entry editor is already opened. If yes then focus on this entry editor.
            //var openedDbEntryEditor = DbEntryEditors.FirstOrDefault(item => item.)
            var dbEditor = ServiceLocator.Instance.GetService<EditorVM>().DbEditor;

            var entryEditor = dbEditor.OpenEntryEditor(_edited, entryName);
            entryEditor.CommitedAction = OnEntryCommited;
        }

        private void OnEntryCommited(string entryId)
        {
            var entryVM = _editable.Entries.FirstOrDefault(item => item.Id == entryId);
            var entry = _edited.Entries.FirstOrDefault(item => item.Id == entryId);

            if (entryVM != null && entry != null)
                entryVM.Load(entry);
        }

        #endregion Public Methods

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
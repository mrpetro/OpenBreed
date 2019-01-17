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
        //private IRepository<M> _edited;
        private string _title;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableEditorVM()
        {
            Items = new BindingList<Items.DbEntryVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
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

        public BindingList<DbEntryVM> Items { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Public Methods

        //public void EditModel(IRepository<M> model)
        //{
        //    //Unsubscribe to previous edited item changes
        //    if (Editable != null)
        //        Editable.PropertyChanged -= Editable_PropertyChanged;

        //    _edited = model;

        //    var vm = new DbTable();
        //    UpdateVM(model, vm);
        //    Editable = vm;
        //    Editable.PropertyChanged += Editable_PropertyChanged;
        //    UpdateTitle();
        //}

        //public void OnStore()
        //{
        //    UpdateEntry(_editable, _edited);

        //    //if (EditMode)
        //    //    _repo.Update(_edited);
        //    //else
        //    //    _repo.Add(_edited);
        //    //Done();
        //}

        public void OpenEntity(DbEntryVM item)
        {
            ServiceLocator.Instance.GetService<EditorVM>().DbEditor.OpenEntryEditor(item);
        }

        #endregion Public Methods

        #region Protected Methods

        //protected void UpdateEntry(DbTableVM source, IRepository<M> target)
        //{

        //}

        //protected void UpdateVM(IRepository<M> source, DbTableVM target)
        //{
        //    //target.Name


        //}

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
//using OpenBreed.Common;
//using OpenBreed.Editor.VM.Base;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Editor.VM.Database
//{
//    public class DbTableEditorExVM<M> : BaseViewModel where M : IEntry
//    {

//        #region Private Fields

//        private DbTable _editable;
//        private IRepository<M> _edited;
//        private string _title;

//        #endregion Private Fields

//        #region Public Properties

//        public DbTable Editable
//        {
//            get { return _editable; }
//            set { SetProperty(ref _editable, value); }
//        }

//        public string EditorName { get { return "Table Editor"; } }

//        public string Title
//        {
//            get { return _title; }
//            set { SetProperty(ref _title, value); }
//        }

//        #endregion Public Properties

//        #region Public Methods

//        public void EditModel(IRepository<M> model)
//        {
//            //Unsubscribe to previous edited item changes
//            if (Editable != null)
//                Editable.PropertyChanged -= Editable_PropertyChanged;

//            _edited = model;

//            var vm = new DbTable();
//            UpdateVM(model, vm);
//            Editable = vm;
//            Editable.PropertyChanged += Editable_PropertyChanged;
//            UpdateTitle();
//        }

//        public void OnStore()
//        {
//            UpdateEntry(_editable, _edited);

//            //if (EditMode)
//            //    _repo.Update(_edited);
//            //else
//            //    _repo.Add(_edited);
//            //Done();
//        }

//        #endregion Public Methods

//        #region Protected Methods

//        protected void UpdateEntry(DbTable source, IRepository<M> target)
//        {

//        }

//        protected void UpdateVM(IRepository<M> source, DbTable target)
//        {
//            //target.Name

            
//        }

//        #endregion Protected Methods

//        #region Private Methods

//        private void Editable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            //throw new NotImplementedException();
//        }

//        private void UpdateTitle()
//        {
//            if (Editable == null)
//                Title = $"{EditorName} - no entry to edit";
//            else
//                Title = $"{EditorName} - {Editable.Name}";
//        }

//        #endregion Private Methods

//    }
//}

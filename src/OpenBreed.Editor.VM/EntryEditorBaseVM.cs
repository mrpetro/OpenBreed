using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public abstract class EntryEditorBaseVM<E,VM> : EntryEditorVM where VM : EditableEntryVM, new() where E : IEntry
    {

        #region Private Fields

        private VM _editable;
        private E _edited;
        private IRepository<E> _repository;

        #endregion Private Fields

        #region Public Properties

        public VM Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public EntryEditorBaseVM()
        {
            _repository = ServiceLocator.Instance.GetService<IUnitOfWork>().GetRepository<E>();
        }

        protected EntryEditorBaseVM(IRepository repository)
        {
            _repository = (IRepository<E>)repository;
        }

        public void EditModel(E model)
        {
            //Unsubscribe to previous edited item changes
            if (Editable != null)
                Editable.PropertyChanged -= Editable_PropertyChanged;

            _edited = model;

            var vm = new VM();
            UpdateVM(model, vm);
            Editable = vm;
            Editable.PropertyChanged += Editable_PropertyChanged;
            UpdateTitle();
        }

        public override void OnStore()
        {
            UpdateEntry(_editable, _edited);

            //if (EditMode)
            //    _repo.Update(_edited);
            //else
            //    _repo.Add(_edited);
            //Done();
        }

        public override void EditEntry(string name)
        {
            var entry = GetEntry(name);
            EditModel(entry);
        }

        public override void OpenNextEntry()
        {
            throw new NotImplementedException();
        }

        public override void OpenPreviousEntry()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Protected Methods

        protected E GetEntry(string name)
        {
            return _repository.GetByName(name);
        }

        protected abstract void UpdateEntry(VM source, E target);

        protected abstract void UpdateVM(E source, VM target);

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

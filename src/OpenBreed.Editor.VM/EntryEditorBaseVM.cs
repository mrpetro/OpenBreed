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
        private string _editableName;
        private E _edited;
        private E _next;
        private E _previous;
        private IRepository<E> _repository;

        #endregion Private Fields

        #region Public Constructors

        public EntryEditorBaseVM()
        {
            _repository = ServiceLocator.Instance.GetService<IUnitOfWork>().GetRepository<E>();
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected EntryEditorBaseVM(IRepository repository)
        {
            _repository = (IRepository<E>)repository;
        }

        #endregion Protected Constructors

        #region Public Properties

        public VM Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
        }

        public override string EditableName
        {
            get { return _editableName; }
            set { SetProperty(ref _editableName, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public override void EditEntry(string name)
        {
            var entry = _repository.GetByName(name);
            EditModel(entry);
        }

        public void EditModel(E model)
        {
            //Unsubscribe to previous edited item changes
            if (Editable != null)
                Editable.PropertyChanged -= Editable_PropertyChanged;

            _edited = model;
            _next = _repository.GetNextTo(_edited);
            _previous = _repository.GetPreviousTo(_edited);

            var vm = CreateVM(_edited);

            UpdateVM(model, vm);
            Editable = vm;
            Editable.PropertyChanged += Editable_PropertyChanged;
            Update();
        }

        public override void EditNextEntry()
        {
            if (_next == null)
                throw new InvalidOperationException("No next entry available");

            EditModel(_next);
        }

        public override void EditPreviousEntry()
        {
            if (_previous == null)
                throw new InvalidOperationException("No previous entry available");

            EditModel(_previous);
        }

        public override void Store()
        {
            UpdateEntry(_editable, _edited);

            //if (EditMode)
            //    _repo.Update(_edited);
            //else
            //    _repo.Add(_edited);
            //Done();
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual VM CreateVM(E model)
        {
            return new VM();
        }
        protected virtual void UpdateEntry(VM source, E target)
        {
        }

        protected virtual void UpdateVM(E source, VM target)
        {
            target.Name = source.Name;
        }

        #endregion Protected Methods

        #region Private Methods

        private void Editable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Update()
        {
            if (Editable == null)
            {
                Title = $"{EditorName} - no entry to edit";
                EditableName = null;
            }
            else
            {
                Title = $"{EditorName} - {Editable.Name}";
                EditableName = Editable.Name;
            }

            NextAvailable = _next != null;
            PreviousAvailable = _previous != null;
        }

        #endregion Private Methods

    }
}

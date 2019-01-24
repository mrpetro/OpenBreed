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

        //private VM _editable;
        //private string _editableName;
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

        public new VM Editable
        {
            get { return (VM)_editable; }
            set { base.SetProperty(ref _editable, value); }
        }

        //public override string EditableName
        //{
        //    get { return _editableName; }
        //    set { SetProperty(ref _editableName, value); }
        //}

        #endregion Public Properties

        #region Public Methods

        public override void Commit()
        {
            string originalName = _edited.Id;

            UpdateEntry((VM)_editable, _edited);

            if (EditMode)
                _repository.Update(_edited);
            else
                _repository.Add(_edited);

            CommitEnabled = false;
            RevertEnabled = true;
            EditMode = true;

            CommitedAction?.Invoke(originalName);
        }

        public override void Revert()
        {
            ServiceLocator.Instance.GetService<IDialogProvider>().ShowMessage("Function not implemented yet.", "Not implemented");
        }

        public override void EditEntry(string name)
        {
            var entry = _repository.GetById(name);
            EditEntry(entry);
            EditMode = true;
        }

        public override void EditNextEntry()
        {
            if (_next == null)
                throw new InvalidOperationException("No next entry available");

            EditEntry(_next);
        }

        public override void EditPreviousEntry()
        {
            if (_previous == null)
                throw new InvalidOperationException("No previous entry available");

            EditEntry(_previous);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual VM CreateVM(E model)
        {
            return new VM();
        }

        protected virtual void UpdateEntry(VM source, E target)
        {
            source.ToEntry(target);
        }

        protected virtual void UpdateVM(E source, VM target)
        {
            target.FromEntry(source);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Editable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CommitEnabled = true;
        }

        private void EditEntry(E entry)
        {
            //Unsubscribe to previous edited item changes
            if (Editable != null)
                Editable.PropertyChanged -= Editable_PropertyChanged;

            _edited = entry;
            _next = _repository.GetNextTo(_edited);
            _previous = _repository.GetPreviousTo(_edited);

            var vm = CreateVM(_edited);

            UpdateVM(entry, vm);
            Editable = vm;
            Editable.PropertyChanged += Editable_PropertyChanged;
            Update();
        }

        private void Update()
        {
            if (Editable == null)
                Title = $"{EditorName} - no entry to edit";
            else
                Title = $"{EditorName} - {Editable.Id}";

            NextAvailable = _next != null;
            PreviousAvailable = _previous != null;
        }

        #endregion Private Methods

    }
}

using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;

namespace OpenBreed.Editor.VM
{
    public abstract class EntryEditorBaseVM<E, VM> : EntryEditorVM where VM : EditableEntryVM, new() where E : IEntry
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

        #endregion Public Properties

        //public override string EditableName
        //{
        //    get { return _editableName; }
        //    set { SetProperty(ref _editableName, value); }
        //}

        #region Public Methods

        public override void Commit()
        {
            var originalId = _edited.Id;

            UpdateEntry((VM)_editable, _edited);

            if (EditMode)
                _repository.Update(_edited);
            else
                _repository.Add(_edited);

            UpdateControls();
            CommitEnabled = false;
            RevertEnabled = true;
            EditMode = true;

            CommitedAction?.Invoke(originalId);
        }

        public override void Revert()
        {
            ServiceLocator.Instance.GetService<IDialogProvider>().ShowMessage("Function not implemented yet.", "Not implemented");
        }

        public override void EditEntry(string id)
        {
            var entry = _repository.GetById(id);
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

        protected virtual void OnEditablePropertyChanged(string propertyName)
        {
            var canCommit = true;

            switch (propertyName)
            {
                case nameof(Editable.Id):
                    canCommit = IsIdUnique();
                    break;

                default:
                    break;
            }

            CommitEnabled = canCommit;
        }

        #endregion Protected Methods

        #region Private Methods

        private bool IsIdUnique()
        {
            var foundEntry = _repository.Find(Editable.Id);

            if (foundEntry == null)
                return true;

            return false;
        }

        private void Editable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnEditablePropertyChanged(e.PropertyName);
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
            UpdateControls();

            CommitEnabled = false;
        }

        private void UpdateControls()
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
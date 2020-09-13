using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Runtime.CompilerServices;

namespace OpenBreed.Editor.VM
{
    public abstract class EntryEditorBaseExVM<E> : EntryEditorVM where E : IEntry
    {
        #region Private Fields

        private E _edited;
        private E _next;
        private E _previous;
        private IRepository<E> _repository;

        private string _id;

        private string _description;

        #endregion Private Fields

        #region Public Constructors

        public EntryEditorBaseExVM()
        {
            _repository = ServiceLocator.Instance.GetService<IUnitOfWork>().GetRepository<E>();
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected EntryEditorBaseExVM(IRepository repository)
        {
            _repository = (IRepository<E>)repository;
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Commit()
        {
            var originalId = _edited.Id;

            UpdateEntry(_edited);

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

        protected virtual void UpdateEntry(E target)
        {
            target.Id = Id;
            target.Description = Description;
        }

        protected virtual void UpdateVM(E source)
        {
            Id = source.Id;
            Description = source.Description;
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            var propertyChanged = base.SetProperty(ref storage, value, propertyName);

            if (propertyChanged)
                OnEditablePropertyChanged(propertyName);

            return propertyChanged;
        }

        protected virtual void OnEditablePropertyChanged(string propertyName)
        {
            var canCommit = true;

            switch (propertyName)
            {
                case nameof(Id):
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
            var foundEntry = _repository.Find(Id);

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
            _edited = entry;
            _next = _repository.GetNextTo(_edited);
            _previous = _repository.GetPreviousTo(_edited);

            UpdateVM(entry);

            UpdateControls();

            CommitEnabled = false;
        }

        private void UpdateControls()
        {
            if (_edited == null)
                Title = $"{EditorName} - no entry to edit";
            else
                Title = $"{EditorName} - {Id}";

            NextAvailable = _next != null;
            PreviousAvailable = _previous != null;
        }

        #endregion Private Methods
    }
}
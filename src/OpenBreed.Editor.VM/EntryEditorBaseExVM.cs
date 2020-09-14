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

        protected override void OnPropertyChanged(string name)
        {
            var canCommit = true;

            switch (name)
            {
                case nameof(Id):
                    canCommit = IsIdUnique();
                    break;

                default:
                    break;
            }

            CommitEnabled = canCommit;

            base.OnPropertyChanged(name);
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

        protected virtual void DisableChangesTracking()
        {
        }

        protected virtual void EnableChangesTracking()
        {
        }

        private void EditEntry(E entry)
        {
            DisableChangesTracking();

            _edited = entry;
            _next = _repository.GetNextTo(_edited);
            _previous = _repository.GetPreviousTo(_edited);

            UpdateVM(entry);

            EnableChangesTracking();

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
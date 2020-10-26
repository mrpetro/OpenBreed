using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM
{
    public abstract class EntryEditorBaseVM<E> : EntryEditorVM where E : IEntry
    {
        #region Private Fields

        private static readonly HashSet<string> propertyNamesIgnoredForChanges = new HashSet<string>();
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

        protected static void IgnoreProperty(string propertyName)
        {
            propertyNamesIgnoredForChanges.Add(propertyName);
        }

        protected virtual void UpdateEntry(E target)
        {
            target.Id = Id;
            target.Description = Description;
        }

        static EntryEditorBaseVM()
        {
            IgnoreProperty(nameof(CommitEnabled));
            IgnoreProperty(nameof(RevertEnabled));
        }

        protected virtual void UpdateVM(E source)
        {
            Id = source.Id;
            Description = source.Description;
        }

        protected override void OnPropertyChanged(string name)
        {
            if (!propertyNamesIgnoredForChanges.Contains(name) && changesTrackingEnabled)
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
            }

            base.OnPropertyChanged(name);
        }

        private bool changesTrackingEnabled;

        protected virtual void DisableChangesTracking()
        {
            changesTrackingEnabled = false;
        }

        protected virtual void EnableChangesTracking()
        {
            changesTrackingEnabled = true;
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

        private void EditEntry(E entry)
        {
            DisableChangesTracking();

            _edited = entry;
            _next = _repository.GetNextTo(_edited);
            _previous = _repository.GetPreviousTo(_edited);

            UpdateVM(entry);

            UpdateControls();

            EditMode = true;
            CommitEnabled = false;
            EnableChangesTracking();
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
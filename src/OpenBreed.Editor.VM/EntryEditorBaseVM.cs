using OpenBreed.Common.Data;
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
        private readonly IDialogProvider dialogProvider;
        private readonly IRepository<E> repository;
        private E edited;
        private E next;
        private E previous;
        private bool changesTrackingEnabled;

        #endregion Private Fields

        #region Public Constructors

        static EntryEditorBaseVM()
        {
            IgnoreProperty(nameof(CommitEnabled));
            IgnoreProperty(nameof(RevertEnabled));
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected EntryEditorBaseVM(IWorkspaceMan workspaceMan, IDialogProvider dialogProvider)
        {
            WorkspaceMan = workspaceMan;
            this.dialogProvider = dialogProvider;
            repository = WorkspaceMan.GetRepository<E>();
        }

        #endregion Protected Constructors

        #region Internal Properties

        internal IWorkspaceMan WorkspaceMan { get; }

        internal IDialogProvider DialogProvider => dialogProvider;

        #endregion Internal Properties

        #region Public Methods

        public override void Commit()
        {
            var originalId = edited.Id;

            UpdateEntry(edited);

            if (EditMode)
                repository.Update(edited);
            else
                repository.Add(edited);

            UpdateControls();
            CommitEnabled = false;
            RevertEnabled = true;
            EditMode = true;

            CommitedAction?.Invoke(originalId);
        }

        public override void Revert()
        {
            dialogProvider.ShowMessage("Function not implemented yet.", "Not implemented");
        }

        public override void EditEntry(string id)
        {
            var entry = repository.GetById(id);
            EditEntry(entry);
        }

        public override void EditNextEntry()
        {
            if (next == null)
                throw new InvalidOperationException("No next entry available");

            EditEntry(next);
        }

        public override void EditPreviousEntry()
        {
            if (previous == null)
                throw new InvalidOperationException("No previous entry available");

            EditEntry(previous);
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
            var foundEntry = repository.Find(Id);

            if (foundEntry == null)
                return true;

            return false;
        }

        private void EditEntry(E entry)
        {
            DisableChangesTracking();

            edited = entry;
            next = repository.GetNextTo(edited);
            previous = repository.GetPreviousTo(edited);

            UpdateVM(entry);

            UpdateControls();

            EditMode = true;
            CommitEnabled = false;
            EnableChangesTracking();
        }

        private void UpdateControls()
        {
            if (edited == null)
                Title = $"{EditorName} - no entry to edit";
            else
                Title = $"{EditorName} - {Id}";

            NextAvailable = next != null;
            PreviousAvailable = previous != null;
        }

        #endregion Private Methods
    }
}
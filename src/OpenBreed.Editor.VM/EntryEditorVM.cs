using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public class EntryEditorVM<E> : EntryEditorVM where E : IDbEntry
    {
        #region Private Fields

        private static readonly HashSet<string> propertyNamesIgnoredForChanges = new HashSet<string>();
        private readonly IDialogProvider dialogProvider;
        private readonly DbEntryEditorFactory dbEntryEditorFactory;
        private readonly IRepository<E> repository;
        private E edited;
        private E next;
        private E previous;
        private bool changesTrackingEnabled;

        #endregion Private Fields

        #region Public Constructors

        static EntryEditorVM()
        {
            IgnoreProperty(nameof(CommitEnabled));
            IgnoreProperty(nameof(RevertEnabled));
        }

        public EntryEditorVM(
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            DbEntryEditorFactory dbEntryEditorFactory)
        {
            WorkspaceMan = workspaceMan;
            this.dialogProvider = dialogProvider;
            this.dbEntryEditorFactory = dbEntryEditorFactory;
            repository = WorkspaceMan.GetRepository<E>();
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get; }

        #endregion Public Properties

        #region Internal Properties

        internal IWorkspaceMan WorkspaceMan { get; }

        internal IDialogProvider DialogProvider => dialogProvider;

        #endregion Internal Properties

        #region Public Methods

        public override bool IsEdited(string repositoryName, string entryId)
        {
            return repository.Name == repositoryName && Id == entryId;
        }

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
            if (next is null)
            {
                throw new InvalidOperationException("No next entry available");
            }

            EditEntry(next);
        }

        public override void EditPreviousEntry()
        {
            if (previous is null)
            {
                throw new InvalidOperationException("No previous entry available");
            }

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
            SpecificsEditor.UpdateEntry(target);

            target.Id = Id;
            target.Description = Description;
        }

        protected virtual void UpdateVM(E source)
        {
            Id = source.Id;
            Description = source.Description;

            SpecificsEditor = dbEntryEditorFactory.CreateSpecific(source);

            SpecificsEditor.UpdateVM(source);
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

            return foundEntry is null;
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

            OnPropertyChanged(nameof(SpecificsEditor));
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

    public abstract class EntryEditorVM : BaseViewModel
    {
        #region Private Fields

        private bool _commitEnabled;
        private bool _editMode;
        private bool _nextAvailable;
        private bool _previousAvailable;
        private bool _revertEnabled;
        private string _title;
        private string _id;
        private string _description;

        private EntrySpecificEditorVM _specificsEditor;

        #endregion Private Fields

        #region Protected Constructors

        protected EntryEditorVM()
        {
            CommitCommand = new Command(() => Commit());
            RevertCommand = new Command(() => Revert());
            EditPreviousCommand = new Command(() => EditPreviousEntry());
            EditNextCommand = new Command(() => EditNextEntry());
        }

        #endregion Protected Constructors

        #region Public Properties

        public Action ActivatingAction { get; set; }
        public Action<string> CommitedAction { get; set; }
        public Action ClosingAction { get; set; }

        public EntrySpecificEditorVM SpecificsEditor
        {
            get { return _specificsEditor; }
            protected set { SetProperty(ref _specificsEditor, value); }
        }

        public bool CommitEnabled
        {
            get { return _commitEnabled; }
            protected set { SetProperty(ref _commitEnabled, value); }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public abstract string EditorName { get; }

        public bool NextAvailable
        {
            get { return _nextAvailable; }
            protected set { SetProperty(ref _nextAvailable, value); }
        }

        public bool PreviousAvailable
        {
            get { return _previousAvailable; }
            protected set { SetProperty(ref _previousAvailable, value); }
        }

        public bool RevertEnabled
        {
            get { return _revertEnabled; }
            protected set { SetProperty(ref _revertEnabled, value); }
        }

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

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public Command CommitCommand { get; }
        public Command RevertCommand { get; }
        public Command EditPreviousCommand { get; }
        public Command EditNextCommand { get; }

        #endregion Public Properties

        #region Public Methods

        public void Activate()
        {
            ActivatingAction?.Invoke();
        }

        public bool Close()
        {
            ClosingAction?.Invoke();

            return true;
        }

        public abstract void Commit();

        public abstract void Revert();

        public abstract void EditEntry(string entryId);

        public abstract void EditNextEntry();

        public abstract void EditPreviousEntry();

        public abstract bool IsEdited(string repositoryName, string entryId);

        #endregion Public Methods
    }
}
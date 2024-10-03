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
using System.Xml.Linq;

namespace OpenBreed.Editor.VM
{
    public class EntryEditorVM<E> : EntryEditorVM where E : IDbEntry
    {
        #region Private Fields

        private readonly IDialogProvider dialogProvider;
        private readonly DbEntryEditorFactory dbEntryEditorFactory;
        private readonly IRepository<E> repository;
        private E edited;
        private string editedId;

        private E next;
        private E previous;

        private bool isUnique;

        private bool dataChanged;

        #endregion Private Fields

        #region Public Constructors

        static EntryEditorVM()
        {
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
            return repository.Name == repositoryName && edited.Id == entryId;
        }

        public override void Commit()
        {
            SpecificsEditor.UpdateEntry();

            if (EditMode)
            {
                var newEditedId = edited.Id;
                edited.Id = editedId;
                repository.Update(edited);
                edited.Id = newEditedId;
            }
            else
                repository.Add(edited);

            UpdateControls();
            CommitEnabled = false;
            RevertEnabled = true;
            EditMode = true;

            CommitedAction?.Invoke(editedId);
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

        #region Private Methods

        private void OnIdChanged(string newId)
        {
            isUnique = IsIdUnique(newId);
            CommitEnabled = dataChanged && isUnique;
        }

        private void OnDataChanged()
        {
            dataChanged = true;
            CommitEnabled = dataChanged && isUnique;
        }

        private bool IsIdUnique(string id)
        {
            var foundEntry = repository.Find(id);

            return foundEntry is null;
        }

        private void EditEntry(E entry)
        {
            edited = (E)entry.Copy();
            editedId = edited.Id;

            next = repository.GetNextTo(edited);
            previous = repository.GetPreviousTo(edited);

            SpecificsEditor = dbEntryEditorFactory.CreateSpecific(edited);
            SpecificsEditor.IdChangedCallback = OnIdChanged;
            SpecificsEditor.DataChangedCallback = OnDataChanged;

            SpecificsEditor.UpdateVM();

            UpdateControls();

            EditMode = true;
            CommitEnabled = false;
            isUnique = true;
            dataChanged = false;

            OnPropertyChanged(nameof(SpecificsEditor));
        }

        private void UpdateControls()
        {
            if (edited == null)
                Title = $"{EditorName} - no entry to edit";
            else
                Title = $"{EditorName} - {edited.Id}";

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
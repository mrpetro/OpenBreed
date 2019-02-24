using OpenBreed.Common;
using OpenBreed.Common.Data;
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
    public abstract class EntryEditorVM : BaseViewModel
    {

        #region Private Fields

        protected EditableEntryVM _editable;
        private bool _commitEnabled;
        private bool _editMode;
        private bool _nextAvailable;
        private bool _previousAvailable;
        private bool _revertEnabled;
        private string _title;

        #endregion Private Fields

        #region Public Properties

        public Action ActivatingAction { get; set; }
        public Action ClosedAction { get; set; }
        public Action<string> CommitedAction { get; set; }
        public Action ClosingAction { get; set; }
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

        public EditableEntryVM Editable
        {
            get { return _editable; }
            set { SetProperty(ref _editable, value); }
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

        #endregion Public Properties

        #region Internal Properties

        internal DataProvider DataProvider { get { return ServiceLocator.Instance.GetService<DataProvider>(); } }

        #endregion Internal Properties

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

        public abstract void EditEntry(string name);
        public abstract void EditNextEntry();
        public abstract void EditPreviousEntry();

        #endregion Public Methods
    }
}

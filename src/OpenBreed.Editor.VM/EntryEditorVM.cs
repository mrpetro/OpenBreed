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

        private bool _commitEnabled;
        private bool _editMode;
        private bool _nextAvailable;
        private bool _previousAvailable;
        private bool _revertEnabled;
        private string _title;
        private string _id;
        private string _description;
        private object _innerCtrl;

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
        public Action ClosedAction { get; set; }
        public Action<string> CommitedAction { get; set; }
        public Action ClosingAction { get; set; }

        public BaseViewModel DataEditor { get; }

        public object InnerCtrl
        {
            get { return _innerCtrl; }
            set { SetProperty(ref _innerCtrl, value); }
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

        public abstract void EditEntry(string name);

        public abstract void EditNextEntry();

        public abstract void EditPreviousEntry();

        #endregion Public Methods
    }
}
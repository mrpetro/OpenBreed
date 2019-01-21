using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Items;
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

        private bool _EditMode;
        private bool _nextAvailable;
        private bool _previousAvailable;
        private string _title;

        #endregion Private Fields

        #region Public Properties

        public Action ActivatingAction { get; set; }

        public Action ClosedAction { get; set; }
        public Action ClosingAction { get; set; }
        public abstract string EditableName { get; set; }

        public bool EditMode
        {
            get { return _EditMode; }
            set { SetProperty(ref _EditMode, value); }
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

        public abstract void EditEntry(string name);
        public abstract void EditNextEntry();
        public abstract void EditPreviousEntry();

        public abstract void Store();

        #endregion Public Methods
    }
}

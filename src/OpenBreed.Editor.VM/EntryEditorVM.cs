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
        private string _title;

        #endregion Private Fields

        #region Public Properties

        public Action ActivatingAction { get; set; }

        public Action ClosedAction { get; set; }
        public Action ClosingAction { get; set; }
        public bool EditMode
        {
            get { return _EditMode; }
            set { SetProperty(ref _EditMode, value); }
        }

        public abstract string EditorName { get; }
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

        public abstract void OnStore();
        public abstract void OpenNextEntry();

        public abstract void OpenPreviousEntry();

        #endregion Public Methods
    }
}

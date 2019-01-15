using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
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

        internal DataProvider DataProvider { get { return ServiceLocator.Instance.GetService<DataProvider>(); } }

        public Action CloseAction { get; set; }

        private string _title;
        private bool _EditMode;

        #endregion Private Fields

        #region Public Properties

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool EditMode
        {
            get { return _EditMode; }
            set { SetProperty(ref _EditMode, value); }
        }
        public abstract string EditorName { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract void OnStore();

        public abstract void OpenEntry(string name);

        public abstract void OpenNextEntry();

        public abstract void OpenPreviousEntry();

        public bool TryClose()
        {
            if (CloseAction != null)
                CloseAction();

            return true;
        }

        #endregion Public Methods
    }
}

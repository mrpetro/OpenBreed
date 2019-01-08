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

        private string _editableName;
        private bool _EditMode;

        #endregion Private Fields

        #region Public Constructors

        public EntryEditorVM(EditorVM root)
        {
            Root = root;
        }

        #endregion Public Constructors

        #region Public Properties

        public string EditableName
        {
            get { return _editableName; }
            set { SetProperty(ref _editableName, value); }
        }

        public bool EditMode
        {
            get { return _EditMode; }
            set { SetProperty(ref _EditMode, value); }
        }
        public abstract string EditorName { get; }
        public EditorVM Root { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract void OnStore();

        public abstract void OpenEntry(string name);

        public abstract void OpenNextEntry();

        public abstract void OpenPreviousEntry();

        public void TryClose()
        {

        }

        #endregion Public Methods
    }
}

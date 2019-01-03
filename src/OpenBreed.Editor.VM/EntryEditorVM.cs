using OpenBreed.Editor.VM.Base;
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

        private string _editableName;

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

        public abstract string EditorName { get; }
        public EditorVM Root { get; }

        #endregion Public Properties

        #region Public Methods

        public void TryClose()
        {

        }

        #endregion Public Methods

        #region Internal Methods

        public abstract void OpenEntry(string name);

        public abstract void OpenNextEntry();

        public abstract void OpenPreviousEntry();

        #endregion Internal Methods

    }
}

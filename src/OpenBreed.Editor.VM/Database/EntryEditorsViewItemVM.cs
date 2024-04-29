using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Database
{
    public class EntryEditorsViewItemVM : BaseViewModel
    {
        #region Private Fields

        private string content;

        #endregion Private Fields

        #region Public Constructors

        public EntryEditorsViewItemVM(EntryEditorVM entryEditor)
        {
            Editor = entryEditor;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Title => Editor.Title;

        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }

        public EntryEditorVM Editor { get; }

        #endregion Public Properties
    }
}
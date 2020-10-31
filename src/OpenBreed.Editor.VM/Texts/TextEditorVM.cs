using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Texts;
using System;

namespace OpenBreed.Editor.VM.Texts
{
    public class TextEditorVM : ParentEntryEditor<ITextEntry>
    {
        #region Public Constructors

        static TextEditorVM()
        {
            RegisterSubeditor<ITextEmbeddedEntry>((parent) => new TextEmbeddedEditorVM(parent));
            RegisterSubeditor<ITextFromMapEntry>((parent) => new TextFromMapEditorVM(parent));
        }

        public TextEditorVM(EditorApplication application, IRepository repository) : base(application, repository, "Text Editor")
        {
        }

        #endregion Public Constructors
    }
}
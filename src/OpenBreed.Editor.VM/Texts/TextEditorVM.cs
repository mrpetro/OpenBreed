using OpenBreed.Common;
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
            RegisterSubeditor<ITextEmbeddedEntry, ITextEntry>();
            RegisterSubeditor<ITextFromMapEntry, ITextEntry>();
        }

        public TextEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dialogProvider, "Text Editor")
        {
        }

        #endregion Public Constructors
    }
}
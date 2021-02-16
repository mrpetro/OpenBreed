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
            RegisterSubeditor<ITextEmbeddedEntry>((parent) => new TextEmbeddedEditorVM(parent.DataProvider.Texts));
            RegisterSubeditor<ITextFromMapEntry>((parent) => new TextFromMapEditorVM(parent.DataProvider.Texts,
                                                                                     parent.DataProvider));
        }

        public TextEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider, "Text Editor")
        {
        }

        #endregion Public Constructors
    }
}
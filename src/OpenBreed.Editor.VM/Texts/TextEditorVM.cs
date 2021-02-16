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
            RegisterSubeditor<ITextEmbeddedEntry>((workspaceMan, dataProvider, dialogProvider) => new TextEmbeddedEditorVM(dataProvider.Texts));
            RegisterSubeditor<ITextFromMapEntry>((workspaceMan, dataProvider, dialogProvider) => new TextFromMapEditorVM(dataProvider.Texts,
                                                                                     dataProvider));
        }

        public TextEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dataProvider, dialogProvider, "Text Editor")
        {
        }

        #endregion Public Constructors
    }
}
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using System;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetEditorVM : ParentEntryEditor<ISpriteSetEntry>
    {
        #region Public Constructors

        public SpriteSetEditorVM(DbEntrySubEditorFactory subEditorFactory, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(subEditorFactory, workspaceMan, dialogProvider, "Sprite Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
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

        static SpriteSetEditorVM()
        {
            RegisterSubeditor<ISpriteSetFromSprEntry, ISpriteSetEntry>();
            RegisterSubeditor<ISpriteSetFromImageEntry, ISpriteSetEntry>();
        }

        public SpriteSetEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dialogProvider, "Sprite Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
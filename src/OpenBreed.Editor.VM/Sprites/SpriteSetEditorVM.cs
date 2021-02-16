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
            RegisterSubeditor<ISpriteSetFromSprEntry>((parent) => new SpriteSetFromSprEditorVM(parent.DataProvider.SpriteSets,
                                                                                               parent.DataProvider.Palettes,
                                                                                               parent.DataProvider));
            RegisterSubeditor<ISpriteSetFromImageEntry>((parent) => new SpriteSetFromImageEditorVM(parent.DataProvider.SpriteSets,
                                                                                                   parent.DataProvider.Palettes,
                                                                                                   parent.DataProvider));
        }

        public SpriteSetEditorVM(IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(workspaceMan, dataProvider, dialogProvider, "Sprite Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
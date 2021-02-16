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
            RegisterSubeditor<ISpriteSetFromSprEntry>((workspaceMan, dataProvider, dialogProvider) => new SpriteSetFromSprEditorVM(dataProvider.SpriteSets,
                                                                                               dataProvider.Palettes,
                                                                                               dataProvider));
            RegisterSubeditor<ISpriteSetFromImageEntry>((workspaceMan, dataProvider, dialogProvider) => new SpriteSetFromImageEditorVM(dataProvider.SpriteSets,
                                                                                                   dataProvider.Palettes,
                                                                                                   dataProvider));
        }

        public SpriteSetEditorVM(IManagerCollection managerCollection, IWorkspaceMan workspaceMan, DataProvider dataProvider, IDialogProvider dialogProvider) : base(managerCollection, workspaceMan, dataProvider, dialogProvider, "Sprite Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
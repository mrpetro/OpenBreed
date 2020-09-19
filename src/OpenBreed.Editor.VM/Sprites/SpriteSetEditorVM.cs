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
            RegisterSubeditor<ISpriteSetFromSprEntry>((parent) => new SpriteSetFromSprEditorVM(parent));
            RegisterSubeditor<ISpriteSetFromImageEntry>((parent) => new SpriteSetFromImageEditorVM(parent));
        }

        public SpriteSetEditorVM(IRepository repository) : base(repository, "Sprite Set Editor")
        {
        }

        #endregion Public Constructors
    }
}
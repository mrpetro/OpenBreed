using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Sprites;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromSprEditorVM : SpriteSetEditorBaseVM<IDbSpriteAtlasFromSpr>
    {
        private readonly IBitmapProvider bitmapProvider;
        #region Private Fields

        private int currentSpriteIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromSprEditorVM(
            IDbSpriteAtlasFromSpr dbEntry,
            ILogger logger,
            SpriteAtlasDataProvider spriteSetsDataProvider,
            PalettesDataProvider palettesDataProvider,
            IModelsProvider modelsProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IBitmapProvider bitmapProvider) : base(dbEntry, logger, spriteSetsDataProvider, palettesDataProvider, modelsProvider, workspaceMan, dialogProvider)
        {
            Items = new BindingList<SpriteVM>();
            this.bitmapProvider = bitmapProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<SpriteVM> Items { get; }

        public int CurrentSpriteIndex
        {
            get { return currentSpriteIndex; }
            set { SetProperty(ref currentSpriteIndex, value); }
        }

        public override string EditorName => "SPR Sprite Set Editor";

        #endregion Public Properties

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Palette):

                    Palette.SetColors(64, Palette.Data.Skip(16).Take(16).ToArray());

                    foreach (var item in Items)
                        bitmapProvider.SetPaletteColors(item.Image, Palette.Data);

                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        protected override void ProtectedUpdateEntry()
        {
            base.ProtectedUpdateEntry();
        }

        protected override void ProtectedUpdateVM()
        {
            var model = spriteAtlasDataProvider.GetSpriteAtlas(Entry);

            if (model != null)
            {
                FromModel(model);
            }

            CurrentSpriteIndex = 0;

            foreach (var item in Items)
            {
                bitmapProvider.SetPaletteColors(item.Image, Palette.Data);
            }

            base.ProtectedUpdateVM();
        }

        #endregion Protected Methods

        #region Private Methods

        private void FromModel(SpriteSetModel spriteSet)
        {
            SetupSprites(spriteSet.Sprites);
        }

        private void SetupSprites(List<SpriteModel> sprites)
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();

                foreach (var sprite in sprites)
                    Items.Add(SpriteVM.Create(sprite, bitmapProvider));
            });
        }

        #endregion Private Methods
    }
}
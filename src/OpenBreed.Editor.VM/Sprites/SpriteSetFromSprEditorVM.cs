using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Sprites;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromSprEditorVM : SpriteSetEditorExVM
    {
        #region Private Fields

        private int currentSpriteIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromSprEditorVM(SpriteAtlasDataProvider spriteSetsDataProvider,
                                          PalettesDataProvider palettesDataProvider,
                                          IModelsProvider dataProvider) : base(spriteSetsDataProvider, palettesDataProvider, dataProvider)
        {
            Items = new BindingList<SpriteVM>();
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<SpriteVM> Items { get; }

        public int CurrentSpriteIndex
        {
            get { return currentSpriteIndex; }
            set { SetProperty(ref currentSpriteIndex, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public override void UpdateEntry(IDbSpriteAtlas entry)
        {
            base.UpdateEntry(entry);
            UpdateEntry((IDbSpriteAtlasFromSpr)entry);
        }

        public override void UpdateVM(IDbSpriteAtlas entry)
        {
            base.UpdateVM(entry);
            UpdateVM((IDbSpriteAtlasFromSpr)entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Palette):

                    Palette.SetColors(64, Palette.Data.Skip(16).Take(16).ToArray());

                    foreach (var item in Items)
                        BitmapHelper.SetPaletteColors(item.Image, Palette.Data);


                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateEntry(IDbSpriteAtlasFromSpr entry)
        {
        }

        private void UpdateVM(IDbSpriteAtlasFromSpr entry)
        {
            var model = spriteAtlasDataProvider.GetSpriteSet(entry.Id);

            if (model != null)
                FromModel(model);

            CurrentSpriteIndex = 0;

            foreach (var item in Items)
                BitmapHelper.SetPaletteColors(item.Image, Palette.Data);
        }

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
                    Items.Add(SpriteVM.Create(sprite));
            });
        }

        #endregion Private Methods
    }
}
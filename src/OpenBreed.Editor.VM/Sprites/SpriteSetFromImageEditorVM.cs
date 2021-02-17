using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Sprites;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromImageEditorVM : SpriteSetEditorExVM
    {
        #region Private Fields

        private int currentSpriteIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromImageEditorVM(SpriteSetsDataProvider spriteSetsDataProvider,
                                          PalettesDataProvider palettesDataProvider,
                                          IDataProvider dataProvider) : base(spriteSetsDataProvider, palettesDataProvider, dataProvider)
        {
            SpriteEditor = new SpriteFromImageEditorVM(this);
            Items = new BindingList<SpriteFromImageVM>();
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<SpriteFromImageVM> Items { get; }

        public SpriteFromImageEditorVM SpriteEditor { get; }
        public Bitmap SourceImage { get; private set; }

        public Action RefreshAction { get; set; }

        public int CurrentSpriteIndex
        {
            get { return currentSpriteIndex; }
            set { SetProperty(ref currentSpriteIndex, value); }
        }

        public SpriteFromImageVM CurrentSprite { get { return Items[currentSpriteIndex]; } }

        #endregion Public Properties

        #region Public Methods

        public void RemoveSprite()
        {
            if (Items.Any())
                Items.RemoveAt(CurrentSpriteIndex);
        }

        public void AddSprite()
        {
            var newSprite = new SpriteFromImageVM();
            newSprite.Id = Items.Count;
            newSprite.SourceRectangle = new Rectangle(0, 0, 8, 8);
            var bytes = BitmapHelper.ToBytes(SourceImage, newSprite.SourceRectangle);
            newSprite.UpdateBitmap(8, 8, bytes);
            BitmapHelper.SetPaletteColors(newSprite.Image, SourceImage.Palette.Entries);

            Items.Add(newSprite);
            CurrentSpriteIndex = Items.Count - 1;
        }

        public void Connect()
        {
        }

        public override void UpdateEntry(ISpriteSetEntry entry)
        {
            base.UpdateEntry(entry);
            UpdateEntry((ISpriteSetFromImageEntry)entry);
        }

        public override void UpdateVM(ISpriteSetEntry entry)
        {
            UpdateVM((ISpriteSetFromImageEntry)entry);
            base.UpdateVM(entry);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void UpdateSpriteImage(SpriteVM sprite, Rectangle cutout)
        {
            var bytes = BitmapHelper.ToBytes(SourceImage, cutout);
            var originalPalette = sprite.Image.Palette;
            sprite.UpdateBitmap(cutout.Width, cutout.Height, bytes);
            sprite.Image.Palette = originalPalette;

            RefreshAction?.Invoke();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Palette):
                    BitmapHelper.SetPaletteColors(SourceImage, Palette.Data);

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

        private void UpdateEntry(ISpriteSetFromImageEntry entry)
        {
            entry.ClearCoords();

            for (int i = 0; i < Items.Count; i++)
            {
                var coords = Items[i].SourceRectangle;
                entry.AddCoords(coords.X, coords.Y, coords.Width, coords.Height);
            }
        }

        private void UpdateVM(ISpriteSetFromImageEntry entry)
        {
            SourceImage = dataProvider.GetData<Bitmap>(entry.DataRef);

            Items.UpdateAfter(() =>
            {
                Items.Clear();

                for (int i = 0; i < entry.Sprites.Count; i++)
                {
                    var spriteDef = entry.Sprites[i];

                    var spriteBuilder = SpriteBuilder.NewSprite();
                    spriteBuilder.SetIndex(i);
                    spriteBuilder.SetSize(spriteDef.Width, spriteDef.Height);
                    var cutout = new Rectangle(spriteDef.X, spriteDef.Y, spriteDef.Width, spriteDef.Height);
                    var bytes = BitmapHelper.ToBytes(SourceImage, cutout);
                    spriteBuilder.SetData(bytes);

                    var sprite = spriteBuilder.Build();

                    var spriteVM = SpriteFromImageVM.Create(sprite, cutout);
                    Items.Add(spriteVM);
                }
            });

            CurrentSpriteIndex = 0;
        }

        #endregion Private Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Helpers;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Sprites.Builders;
using OpenBreed.Editor.VM.Base;
using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromImageVM : SpriteSetVM
    {
        #region Private Fields

        private int currentSpriteIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromImageVM()
        {
            SpriteEditor = new SpriteFromImageEditorVM(this);

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public SpriteFromImageEditorVM SpriteEditor { get; }
        public Bitmap SourceImage { get; private set; }

        public Action RefreshAction { get; set; }

        public int CurrentSpriteIndex
        {
            get { return currentSpriteIndex; }
            set { SetProperty(ref currentSpriteIndex, value); }
        }

        public SpriteFromImageVM CurrentSprite { get { return (SpriteFromImageVM)Items[currentSpriteIndex]; } }

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

        #endregion Public Methods

        #region Internal Methods

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((ISpriteSetFromImageEntry)entry);
        }

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((ISpriteSetFromImageEntry)entry);
        }

        internal void UpdateSpriteImage(SpriteVM sprite, Rectangle cutout)
        {
            var bytes = BitmapHelper.ToBytes(SourceImage, cutout);
            var originalPalette = sprite.Image.Palette;
            sprite.UpdateBitmap(cutout.Width, cutout.Height, bytes);
            sprite.Image.Palette = originalPalette;

            RefreshAction?.Invoke();
        }

        #endregion Internal Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette):
                    BitmapHelper.SetPaletteColors(SourceImage, Palette.Data);
                    break;

                default:
                    break;
            }
        }

        private void ToEntry(ISpriteSetFromImageEntry entry)
        {
            entry.ClearCoords();

            for (int i = 0; i < Items.Count; i++)
            {
                var coords = ((SpriteFromImageVM)Items[i]).SourceRectangle;
                entry.AddCoords(coords.X, coords.Y, coords.Width, coords.Height);
            }
        }

        private void FromEntry(ISpriteSetFromImageEntry entry)
        {
            var dataProvicer = ServiceLocator.Instance.GetService<DataProvider>();

            SourceImage = dataProvicer.GetData(entry.DataRef) as Bitmap;

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
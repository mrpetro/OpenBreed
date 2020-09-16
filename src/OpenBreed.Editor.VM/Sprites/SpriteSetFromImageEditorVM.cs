using OpenBreed.Common.Data;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using OpenBreed.Common;
using OpenBreed.Model.Sprites;
using OpenBreed.Model;
using OpenBreed.Common.Tools;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromImageEditorVM : BaseViewModel, IEntryEditor<ISpriteSetEntry>
    {
        #region Private Fields

        private int currentSpriteIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromImageEditorVM(SpriteSetEditorVM parent)
        {
            Parent = parent;

            SpriteEditor = new SpriteFromImageEditorVM(this);
            Items = new BindingList<SpriteFromImageVM>();

            Parent.PropertyChanged += Parent_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public SpriteSetEditorVM Parent { get; }
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

        public void UpdateEntry(ISpriteSetEntry entry)
        {
            var spriteSetEntry = (ISpriteSetFromImageEntry)entry;

            spriteSetEntry.ClearCoords();

            for (int i = 0; i < Items.Count; i++)
            {
                var coords = Items[i].SourceRectangle;
                spriteSetEntry.AddCoords(coords.X, coords.Y, coords.Width, coords.Height);
            }
        }

        public void UpdateVM(ISpriteSetEntry entry)
        {
            var spriteSetEntry = (ISpriteSetFromImageEntry)entry;
            var dataProvicer = ServiceLocator.Instance.GetService<DataProvider>();

            SourceImage = dataProvicer.GetData<Bitmap>(spriteSetEntry.DataRef);

            Items.UpdateAfter(() =>
            {
                Items.Clear();

                for (int i = 0; i < spriteSetEntry.Sprites.Count; i++)
                {
                    var spriteDef = spriteSetEntry.Sprites[i];

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

        #region Private Methods

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.Palette):
                    BitmapHelper.SetPaletteColors(SourceImage, Parent.Palette.Data);

                    foreach (var item in Items)
                        BitmapHelper.SetPaletteColors(item.Image, Parent.Palette.Data);
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
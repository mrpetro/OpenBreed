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
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromImageEditorVM : SpriteSetEditorBaseVM<IDbSpriteAtlasFromImage>
    {
        #region Private Fields

        private int currentSpriteIndex = -1;
        private readonly IBitmapProvider bitmapProvider;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromImageEditorVM(
            ILogger logger,
            SpriteAtlasDataProvider spriteAtlasDataProvider,                             
            PalettesDataProvider palettesDataProvider,
            IModelsProvider dataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IDrawingFactory drawingFactory,
            IBitmapProvider bitmapProvider) : base(logger, spriteAtlasDataProvider, palettesDataProvider, dataProvider, workspaceMan, dialogProvider)
        {
            SpriteEditor = new SpriteFromImageEditorVM(this, drawingFactory);
            Items = new BindingList<SpriteFromImageVM>();

            AddSpriteCommand = new Command(() => AddSprite());
            RemoveSpriteCommand = new Command(() => RemoveSprite());
            this.bitmapProvider = bitmapProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<SpriteFromImageVM> Items { get; }

        public SpriteFromImageEditorVM SpriteEditor { get; }
        public IBitmap SourceImage { get; private set; }

        public Action RefreshAction { get; set; }

        public ICommand AddSpriteCommand { get; }
        public ICommand RemoveSpriteCommand { get; }

        public int CurrentSpriteIndex
        {
            get { return currentSpriteIndex; }
            set { SetProperty(ref currentSpriteIndex, value); }
        }

        public SpriteFromImageVM CurrentSprite { get { return Items[currentSpriteIndex]; } }

        public override string EditorName => "Image Sprite Set Editor";

        #endregion Public Properties

        #region Public Methods

        public void RemoveSprite()
        {
            if (Items.Any())
                Items.RemoveAt(CurrentSpriteIndex);
        }

        public void AddSprite()
        {
            var newSprite = new SpriteFromImageVM(bitmapProvider);
            newSprite.Id = Items.Count;
            newSprite.SourceRectangle = new MyRectangle(0, 0, 8, 8);
            var bytes = bitmapProvider.ToBytes(SourceImage, newSprite.SourceRectangle);
            newSprite.UpdateBitmap(8, 8, bytes);
            bitmapProvider.SetPaletteColors(newSprite.Image, SourceImage.Palette.Entries);

            Items.Add(newSprite);
            CurrentSpriteIndex = Items.Count - 1;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void UpdateSpriteImage(SpriteVM sprite, MyRectangle cutout)
        {
            var bytes = bitmapProvider.ToBytes(SourceImage, cutout);
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
                    bitmapProvider.SetPaletteColors(SourceImage, Palette.Data);

                    foreach (var item in Items)
                        bitmapProvider.SetPaletteColors(item.Image, Palette.Data);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        protected override void UpdateEntry(IDbSpriteAtlasFromImage entry)
        {
            entry.ClearCoords();

            for (int i = 0; i < Items.Count; i++)
            {
                var coords = Items[i].SourceRectangle;
                entry.AddCoords(coords.X, coords.Y, coords.Width, coords.Height);
            }
        }

        protected override void UpdateVM(IDbSpriteAtlasFromImage entry)
        {
            SourceImage = dataProvider.GetModel<IBitmap>(entry.DataRef);

            Items.UpdateAfter(() =>
            {
                Items.Clear();

                for (int i = 0; i < entry.Sprites.Count; i++)
                {
                    var spriteDef = entry.Sprites[i];

                    var spriteBuilder = SpriteBuilder.NewSprite();
                    spriteBuilder.SetIndex(i);
                    spriteBuilder.SetSize(spriteDef.Width, spriteDef.Height);
                    var cutout = new MyRectangle(spriteDef.X, spriteDef.Y, spriteDef.Width, spriteDef.Height);
                    var bytes = bitmapProvider.ToBytes(SourceImage, cutout);
                    spriteBuilder.SetData(bytes);

                    var sprite = spriteBuilder.Build();

                    var spriteVM = SpriteFromImageVM.Create(bitmapProvider, sprite, cutout);
                    Items.Add(spriteVM);
                }
            });

            CurrentSpriteIndex = 0;
        }

        #endregion Private Methods
    }
}
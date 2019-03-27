using OpenBreed.Common.Data;
using OpenBreed.Common.Drawing;
using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetVM : EditableEntryVM
    {
        #region Private Fields

        private PaletteVM _palette;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetVM()
        {
            Items = new BindingList<SpriteVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));
            PropertyChanged += SpriteSetVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<SpriteVM> Items { get; private set; }
        public string Name { get { return null; } }

        public PaletteVM Palette
        {
            get { return _palette; }
            set
            {
                var prevPalette = _palette;
                if (SetProperty(ref _palette, value))
                {
                    if (prevPalette != null)
                        prevPalette.PropertyChanged -= Palette_PropertyChanged;

                    _palette.PropertyChanged += Palette_PropertyChanged;
                }
            }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void SetupSprites(List<SpriteModel> sprites)
        {
            foreach (var sprite in sprites)
                Items.Add(SpriteVM.Create(sprite));
        }

        #endregion Internal Methods

        #region Private Methods

        private void Palette_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //case nameof(Palette.Colors):
                //    Palette = Root.LevelEditor.PaletteSelector.CurrentItem;
                //    break;
                default:
                    break;
            }
        }

        private void Palettes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //case nameof(Root.LevelEditor.PaletteSelector.CurrentItem):
                //    Palette = Root.LevelEditor.PaletteSelector.CurrentItem;
                //    break;
                default:
                    break;
            }
        }

        private void SpriteSetVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette):
                    foreach (var item in Items)
                        BitmapHelper.SetPaletteColors(item.Bitmap, Palette.Colors.ToArray());
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
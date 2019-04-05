using OpenBreed.Common.Data;
using OpenBreed.Common.Drawing;
using OpenBreed.Common.Palettes;
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

        private PaletteModel _palette;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetVM()
        {
            Items = new BindingList<SpriteVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<SpriteVM> Items { get; private set; }


        internal virtual void FromModel(SpriteSetModel spriteSet)
        {

        }

        public PaletteModel Palette
        {
            get { return _palette; }
            set { SetProperty(ref _palette, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void SetupSprites(List<SpriteModel> sprites)
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();

                foreach (var sprite in sprites)
                    Items.Add(SpriteVM.Create(sprite));
            });
        }

        #endregion Internal Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette):
                    foreach (var item in Items)
                        BitmapHelper.SetPaletteColors(item.Bitmap, Palette.Data);
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
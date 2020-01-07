using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Helpers;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Sprites;
using System.ComponentModel;

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

        public BindingList<SpriteVM> Items { get; }

        public PaletteModel Palette
        {
            get { return _palette; }
            set { SetProperty(ref _palette, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        #endregion Internal Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette):
                    foreach (var item in Items)
                        BitmapHelper.SetPaletteColors(item.Image, Palette.Data);
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
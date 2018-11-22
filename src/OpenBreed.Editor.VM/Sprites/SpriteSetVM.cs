using OpenBreed.Common.Drawing;
using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Sources;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetVM : BaseViewModel
    {
        private PaletteVM _palette;

        private SpriteSetsVM _owner;

        #region Private Fields

        private SpriteVM _currentItem;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetVM(SpriteSetsVM owner)
        {
            Owner = owner;

            Items = new BindingList<SpriteVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));
            Owner.Root.Palettes.PropertyChanged += Palettes_PropertyChanged;

            PropertyChanged += SpriteSetVM_PropertyChanged;
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

        private void Palette_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette.Colors):
                    Palette = Owner.Root.Palettes.CurrentItem;
                    break;
                default:
                    break;
            }
        }

        private void Palettes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Owner.Root.Palettes.CurrentItem):
                    Palette = Owner.Root.Palettes.CurrentItem;
                    break;
                default:
                    break;
            }
        }

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

        public SpriteSetsVM Owner
        {
            get { return _owner; }
            set { SetProperty(ref _owner, value); }
        }

        #endregion Public Constructors

        #region Public Properties

        public SpriteVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public BindingList<SpriteVM> Items { get; private set; }

        public string Name { get { return Source.Name; } }

        public BaseSource Source { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static SpriteSetVM Create(SpriteSetsVM owner, BaseSource source)
        {
            var model = source.Load() as SpriteSetModel;

            var newSpriteSet = new SpriteSetVM(owner);
            newSpriteSet.Source = source;

            foreach (var sprite in model.Sprites)
                newSpriteSet.Items.Add(SpriteVM.Create(sprite));

            newSpriteSet.CurrentItem = newSpriteSet.Items.FirstOrDefault();

            return newSpriteSet;
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateSprites()
        {
        }

        #endregion Private Methods

    }
}
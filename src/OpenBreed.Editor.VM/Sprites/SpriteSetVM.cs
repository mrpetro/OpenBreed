using OpenBreed.Common.Drawing;
using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Common.Sources;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetVM : BaseViewModel
    {
        #region Private Fields

        public EditorVM Root { get; private set; }
        private PaletteVM _palette;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetVM(EditorVM root)
        {
            Root = root;

            Items = new BindingList<SpriteVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));
            Root.Palettes.PropertyChanged += Palettes_PropertyChanged;

            PropertyChanged += SpriteSetVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<SpriteVM> Items { get; private set; }

        public string Name { get { return Source.Name; } }

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

        public BaseSource Source { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static SpriteSetVM Create(EditorVM root, BaseSource source)
        {
            var model = source.Load() as SpriteSetModel;

            var newSpriteSet = new SpriteSetVM(root);
            newSpriteSet.Source = source;

            foreach (var sprite in model.Sprites)
                newSpriteSet.Items.Add(SpriteVM.Create(sprite));

            return newSpriteSet;
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateSprites()
        {
        }

        private void Palette_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette.Colors):
                    Palette = Root.Palettes.CurrentItem;
                    break;
                default:
                    break;
            }
        }

        private void Palettes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Root.Palettes.CurrentItem):
                    Palette = Root.Palettes.CurrentItem;
                    break;
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
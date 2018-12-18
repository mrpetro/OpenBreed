using OpenBreed.Common.Drawing;
using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Common.Sources;
using System.ComponentModel;
using System.Linq;
using OpenBreed.Common.Database.Items.Sources;
using System;
using OpenBreed.Common.Database.Items.Sprites;

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
            Root.PaletteViewer.PropertyChanged += Palettes_PropertyChanged;

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

        #endregion Public Methods

        #region Private Methods

        private void Palette_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette.Colors):
                    Palette = Root.PaletteViewer.CurrentItem;
                    break;
                default:
                    break;
            }
        }

        private void Palettes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Root.PaletteViewer.CurrentItem):
                    Palette = Root.PaletteViewer.CurrentItem;
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

        internal void Load(SpriteSetDef spriteSetDef)
        {
            var sourceDef = Root.Database.GetSourceDef(spriteSetDef.SourceRef);
            if (sourceDef == null)
                throw new Exception("No Source definition found with name: " + spriteSetDef.SourceRef);

            var source = Root.SourceMan.GetSource(sourceDef);
            if (source == null)
                throw new Exception("SpriteSet source error: " + sourceDef);

            var model = Root.FormatMan.Load(source, spriteSetDef.Format) as SpriteSetModel;
            Source = source;

            foreach (var sprite in model.Sprites)
                Items.Add(SpriteVM.Create(sprite));
        }

        #endregion Private Methods
    }
}
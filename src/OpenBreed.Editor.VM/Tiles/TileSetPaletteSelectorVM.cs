using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetPaletteSelectorVM : BaseViewModel
    {
        private PaletteVM _currentPalette;

        public TileSetViewerVM Viewer { get; private set; }

        public TileSetPaletteSelectorVM(TileSetViewerVM viewer)
        {
            Viewer = viewer;

            Viewer.PropertyChanged += Viewer_PropertyChanged;
        }

        private void Viewer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Viewer.CurrentTileSet):

                    break;
                default:
                    break;
            }
        }

        public PaletteVM CurrentPalette
        {
            get { return _currentPalette; }
            set { SetProperty(ref _currentPalette, value); }
        }
    }
}

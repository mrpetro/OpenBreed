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
        #region Private Fields

        private PaletteVM _currentPalette;

        #endregion Private Fields

        #region Public Constructors

        public TileSetPaletteSelectorVM(TileSetViewerVM viewer)
        {
            Viewer = viewer;

            Viewer.PropertyChanged += Viewer_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public PaletteVM CurrentPalette
        {
            get { return _currentPalette; }
            set { SetProperty(ref _currentPalette, value); }
        }

        public TileSetViewerVM Viewer { get; private set; }

        #endregion Public Properties

        #region Private Methods

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

        #endregion Private Methods
    }
}

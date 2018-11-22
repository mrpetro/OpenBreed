using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteEditorVM : BaseViewModel
    {
        #region Private Fields

        private Color _currentColor = Color.Empty;
        private int _currentColorIndex = -1;
        private PaletteVM _currentPalette = null;

        #endregion Private Fields

        #region Public Constructors

        public PaletteEditorVM(PalettesVM palettes)
        {
            Palettes = palettes;

            Palettes.PropertyChanged += Palettes_PropertyChanged;
        }

        private void Palettes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palettes.CurrentItem):
                    CurrentPalette = Palettes.CurrentItem;
                    CurrentColorIndex = 0;
                    break;
                default:
                    break;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public Color CurrentColor
        {
            get { return CurrentPalette.Colors[CurrentColorIndex]; }
            set
            {
                if (CurrentPalette.Colors[CurrentColorIndex] == value)
                    return;

                CurrentPalette.Colors[CurrentColorIndex] = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        }

        public int CurrentColorIndex
        {
            get { return _currentColorIndex; }
            set { SetProperty(ref _currentColorIndex, value); }
        }

        public PaletteVM CurrentPalette
        {
            get { return _currentPalette; }
            set { SetProperty(ref _currentPalette, value); }
        }

        public PalettesVM Palettes { get; private set; }

        #endregion Public Properties
    }
}

using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using OpenBreed.Editor.VM.Palettes;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorPalettesToolVM : MapEditorToolVM
    {

        #region Private Fields

        private int _currentIndex = -1;
        private PaletteVM _currentItem = null;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorPalettesToolVM(MapEditorVM parent)
        {
            Parent = parent;

            PropertyChanged += LevelPaletteSelectorVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { SetProperty(ref _currentIndex, value); }
        }

        public PaletteVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public MapEditorVM Parent { get; }

        #endregion Public Properties

        #region Private Methods

        private void LevelPaletteSelectorVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentIndex):
                    UpdateCurrentItem();
                    break;
                case nameof(CurrentItem):
                    UpdateCurrentIndex();
                    break;
                default:
                    break;
            }
        }
        private void UpdateCurrentIndex()
        {
            CurrentIndex = Parent.Editable.Palettes.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = Parent.Editable.Palettes[CurrentIndex];
        }

        #endregion Private Methods

    }
}
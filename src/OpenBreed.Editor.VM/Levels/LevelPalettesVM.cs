using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using OpenBreed.Editor.VM.Palettes;

namespace OpenBreed.Editor.VM.Levels
{
    public class PalettesVM : BaseViewModel
    {
        #region Private Fields

        private int _currentIndex = -1;
        private PaletteVM _currentItem = null;

        #endregion Private Fields

        #region Public Constructors

        public PaletteEditorVM Editor { get; private set; }

        public PalettesVM(EditorVM root)
        {
            Root = root;

            Editor = new PaletteEditorVM(this);

            Items = new BindingList<PaletteVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                if (SetProperty(ref _currentIndex, value))
                    CurrentItem = Items[value];
            }
        }

        public PaletteVM CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (SetProperty(ref _currentItem, value))
                    CurrentIndex = Items.IndexOf(value);
            }
        }

        public EditorVM Root { get; private set; }
        public BindingList<PaletteVM> Items { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Restore(List<PaletteModel> palettes)
        {
            Items.RaiseListChangedEvents = false;
            Items.Clear();

            foreach (var palette in palettes)
            {
                var paletteVM = new PaletteVM(Root);
                paletteVM.Restore(palette);
                Items.Add(paletteVM);
            }

            Items.RaiseListChangedEvents = true;
            Items.ResetBindings();
        }

        #endregion Public Methods

    }
}
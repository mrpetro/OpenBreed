using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Palettes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;

namespace OpenBreed.Editor.VM.Palettes
{
    public class ColorEditorVM : BaseViewModel
    {
        #region Private Fields

        private int _index = 0;
        private Color _color = Color.Empty;
        private Color _colorNegative = Color.Empty;
        private byte r;
        private byte g;
        private byte b;
        private readonly IList<ColorSelectionVM> colors;

        #endregion Private Fields

        #region Public Constructors

        public ColorEditorVM(IList<ColorSelectionVM> colors)
        {
            this.colors = colors;
        }

        #endregion Public Constructors

        #region Public Properties

        public Color Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        public Color ColorNegative
        {
            get { return _colorNegative; }
            set { SetProperty(ref _colorNegative, value); }
        }

        public byte R
        {
            get { return r; }
            set { SetProperty(ref r, value); }
        }

        public byte G
        {
            get { return g; }
            set { SetProperty(ref g, value); }
        }

        public byte B
        {
            get { return b; }
            set { SetProperty(ref b, value); }
        }

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Edit(int index)
        {
            Index = index;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Index):
                    var color = colors[Index].Color;
                    R = color.R;
                    G = color.G;
                    B = color.B;
                    break;
                case nameof(R):
                case nameof(G):
                case nameof(B):
                    Color = Color.FromArgb(R, G, B);
                    ColorNegative = Color.FromArgb(Color.ToArgb() ^ 0xffffff);
                    colors[Index].Color = Color;
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods
    }

    public class ColorSelectionVM : BaseViewModel
    {
        #region Private Fields

        private Color _color = Color.Empty;

        #endregion Private Fields

        #region Public Constructors

        public ColorSelectionVM(Color color, Action<ColorSelectionVM> selectColorCallback)
        {
            Color = color;
            SelectCommand = new Command(() => selectColorCallback.Invoke(this));
        }

        #endregion Public Constructors

        #region Public Properties

        public Color Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

        public ICommand SelectCommand { get; }

        #endregion Public Properties
    }

    public class PaletteEditorExVM : BaseViewModel, IEntryEditor<IDbPalette>
    {
        #region Protected Fields

        protected readonly PalettesDataProvider palettesDataProvider;
        protected readonly IModelsProvider dataProvider;

        #endregion Protected Fields

        #region Private Fields

        private int _currentColorIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public PaletteEditorExVM(PalettesDataProvider palettesDataProvider,
                                 IModelsProvider dataProvider)
        {
            this.palettesDataProvider = palettesDataProvider;
            this.dataProvider = dataProvider;
            Colors = new ObservableCollection<ColorSelectionVM>();

            ColorEditor = new ColorEditorVM(Colors);

            Initialize();

            Colors.CollectionChanged += (s, a) => OnPropertyChanged(nameof(Colors));
        }

        #endregion Public Constructors

        #region Public Properties

        public ColorEditorVM ColorEditor { get; }

        public ObservableCollection<ColorSelectionVM> Colors { get; }

        public int CurrentColorIndex
        {
            get { return _currentColorIndex; }
            set { SetProperty(ref _currentColorIndex, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public virtual void UpdateVM(IDbPalette entry)
        {
        }

        public virtual void UpdateEntry(IDbPalette target)
        {
            var model = palettesDataProvider.GetPalette(target.Id);

            for (int i = 0; i < model.Length; i++)
                model.Data[i] = Colors[i].Color;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void UpdateVMColors(PaletteModel model)
        {
            for (int i = 0; i < model.Data.Length; i++)
            {
                Colors[i] = new ColorSelectionVM(model.Data[i], OnColorSelected);
            }

            CurrentColorIndex = 0;
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentColorIndex):
                    ColorEditor.Index = CurrentColorIndex;
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnColorSelected(ColorSelectionVM colorSelection)
        {
            CurrentColorIndex = Colors.IndexOf(colorSelection);
        }

        private void ColorChangeCallBack(Color color)
        {
            //Colors[CurrentColorIndex].Color = color;
        }

        private void Initialize()
        {
            for (int i = 0; i < 256; i++)
            {
                Colors.Add(new ColorSelectionVM(Color.FromArgb(255, i, i, i), OnColorSelected));
            }

            CurrentColorIndex = 0;
        }

        #endregion Private Methods
    }
}
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Palettes;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Palettes
{
    public class ColorEditorVM : BaseViewModel
    {
        #region Private Fields

        private Color _color = Color.Empty;
        private Color _colorNegative = Color.Empty;
        private byte r;
        private byte g;
        private byte b;
        private Action<Color> colorChangeCallBack;

        #endregion Private Fields

        #region Public Constructors

        public ColorEditorVM(Action<Color> colorChangeCallBack)
        {
            this.colorChangeCallBack = colorChangeCallBack;
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

        #endregion Public Properties

        #region Public Methods

        public void Set(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            Color = Color.FromArgb(R, G, B);
            ColorNegative = Color.FromArgb(Color.ToArgb() ^ 0xffffff);

            colorChangeCallBack.Invoke(Color.FromArgb(R, G, B));

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods
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
            Colors = new BindingList<Color>();
            ColorEditor = new ColorEditorVM(ColorChangeCallBack);

            Initialize();

            Colors.ListChanged += (s, a) => OnPropertyChanged(nameof(Colors));
        }

        #endregion Public Constructors

        #region Public Properties

        public ColorEditorVM ColorEditor { get; }

        public BindingList<Color> Colors { get; }

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
                model.Data[i] = Colors[i];
        }

        #endregion Public Methods

        #region Protected Methods

        protected void UpdateVMColors(PaletteModel model)
        {
            Colors.UpdateAfter(() =>
            {
                for (int i = 0; i < model.Data.Length; i++)
                    Colors[i] = model.Data[i];
            });

            CurrentColorIndex = 0;
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentColorIndex):
                    ColorEditor.Set(Colors[CurrentColorIndex]);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void ColorChangeCallBack(Color color)
        {
            Colors[CurrentColorIndex] = color;
        }

        private void Initialize()
        {
            Colors.UpdateAfter(() =>
            {
                for (int i = 0; i < 256; i++)
                    Colors.Add(Color.FromArgb(255, i, i, i));
            });

            CurrentColorIndex = 0;
        }

        #endregion Private Methods
    }
}
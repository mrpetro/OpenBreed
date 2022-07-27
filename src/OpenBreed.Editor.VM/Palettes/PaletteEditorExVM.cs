using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Palettes;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteEditorExVM : BaseViewModel, IEntryEditor<IDbPalette>
    {
        #region Private Fields

        private Color _currentColor = Color.Empty;
        private int _currentColorIndex = -1;
        protected readonly PalettesDataProvider palettesDataProvider;
        protected readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public PaletteEditorExVM(PalettesDataProvider palettesDataProvider,
                                 IModelsProvider dataProvider)
        {
            this.palettesDataProvider = palettesDataProvider;
            this.dataProvider = dataProvider;
            Colors = new BindingList<Color>();
            Initialize();

            Colors.ListChanged += (s, a) => OnPropertyChanged(nameof(Colors));
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<Color> Colors { get; }

        public Color CurrentColor
        {
            get { return CurrentColorIndex == -1 ? Color.Empty : Colors[CurrentColorIndex]; }

            set
            {
                if (Colors[CurrentColorIndex] == value)
                    return;

                Colors[CurrentColorIndex] = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        }

        public int CurrentColorIndex
        {
            get { return _currentColorIndex; }
            set { SetProperty(ref _currentColorIndex, value); }
        }

        public MapEditorPalettesToolVM Palettes { get; private set; }

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

        #endregion Protected Methods

        #region Private Methods

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
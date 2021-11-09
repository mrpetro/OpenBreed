using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetFromBlkEditorVM : BaseViewModel, IEntryEditor<IDbTileAtlas>
    {
        #region Private Fields

        private string currentPaletteRef = null;
        private int _currentPaletteIndex = -1;

        private TileSetModel model;
        private readonly TileAtlasDataProvider tileSetsDataProvider;
        private readonly PalettesDataProvider palettesDataProvider;

        #endregion Private Fields

        #region Public Constructors

        public TileSetFromBlkEditorVM(TileAtlasDataProvider tileSetsDataProvider,
                                      PalettesDataProvider palettesDataProvider)
        {
            PaletteIds = new BindingList<string>();
            this.tileSetsDataProvider = tileSetsDataProvider;
            this.palettesDataProvider = palettesDataProvider;

            Viewer = new TileSetViewerVM();
        }

        #endregion Public Constructors

        public TileSetViewerVM Viewer { get; set; }

        #region Public Properties

        public BindingList<string> PaletteIds { get; }

        public string CurrentPaletteRef
        {
            get { return currentPaletteRef; }
            set { SetProperty(ref currentPaletteRef, value); }
        }

        public int CurrentPaletteIndex
        {
            get { return _currentPaletteIndex; }
            set { SetProperty(ref _currentPaletteIndex, value); }
        }

        #endregion Public Properties

        #region Internal Properties

        internal PaletteModel CurrentPalette { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public virtual void UpdateEntry(IDbTileAtlas entry)
        {
        }

        public virtual void UpdateVM(IDbTileAtlas entry)
        {
            model = tileSetsDataProvider.GetTileAtlas(entry.Id);

            if (model == null)
                return;

            Viewer.FromModel(model);

            SetupPaletteIds(entry.PaletteRefs);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetupPaletteIds(List<string> paletteRefs)
        {
            PaletteIds.UpdateAfter(() =>
            {
                PaletteIds.Clear();
                paletteRefs.ForEach(item => PaletteIds.Add(item));
            });

            CurrentPaletteRef = PaletteIds.FirstOrDefault();
            Viewer.Palette = CurrentPalette;
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentPaletteRef):
                    UpdateCurrentPaletteIndex();
                    SwitchPalette();
                    Viewer.Palette = CurrentPalette;
                    break;

                case nameof(CurrentPaletteIndex):
                    UpdateCurrentPaletteRef();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SwitchPalette()
        {
            CurrentPalette = palettesDataProvider.GetPalette(CurrentPaletteRef);
            BitmapHelper.SetPaletteColors(model.Bitmap, CurrentPalette.Data);
        }

        private void UpdateCurrentPaletteRef()
        {
            if (CurrentPaletteIndex == -1)
                CurrentPaletteRef = null;
            else
                CurrentPaletteRef = PaletteIds[CurrentPaletteIndex];
        }

        private void UpdateCurrentPaletteIndex()
        {
            CurrentPaletteIndex = PaletteIds.IndexOf(CurrentPaletteRef);
        }

        #endregion Private Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
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
    public class TileSetFromBlkEditorVM : EntryEditorBaseVM<IDbTileAtlas>
    {
        #region Private Fields

        private readonly TileAtlasDataProvider tileSetsDataProvider;
        private readonly PalettesDataProvider palettesDataProvider;
        private string currentPaletteRef = null;
        private int _currentPaletteIndex = -1;

        private TileSetModel model;

        #endregion Private Fields

        #region Public Constructors

        public TileSetFromBlkEditorVM(
            TileAtlasDataProvider tileSetsDataProvider,
            PalettesDataProvider palettesDataProvider,
            IWorkspaceMan workspaceMan,
            IDialogProvider dialogProvider,
            IControlFactory controlFactory) : base(workspaceMan, dialogProvider, controlFactory)
        {
            PaletteIds = new BindingList<string>();
            this.tileSetsDataProvider = tileSetsDataProvider;
            this.palettesDataProvider = palettesDataProvider;

            Viewer = new TileSetViewerVM();
        }

        #endregion Public Constructors

        #region Public Properties

        public TileSetViewerVM Viewer { get; set; }

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

        public override string EditorName => "Tileset Editor";

        #endregion Public Properties

        #region Internal Properties

        internal PaletteModel CurrentPalette { get; private set; }

        #endregion Internal Properties

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

        protected void UpdateEntry(IDbTileAtlasFromBlk entry)
        {
        }

        protected void UpdateVM(IDbTileAtlasFromBlk entry)
        {
            model = tileSetsDataProvider.GetTileAtlas(entry.Id);

            if (model == null)
                return;

            Viewer.FromModel(model);

            SetupPaletteIds(entry.PaletteRefs);
        }

        protected override void UpdateEntry(IDbTileAtlas entry)
        {
            UpdateEntry((IDbTileAtlasFromBlk)entry);

            base.UpdateEntry(entry);
        }

        protected override void UpdateVM(IDbTileAtlas entry)
        {
            base.UpdateVM(entry);

            UpdateVM((IDbTileAtlasFromBlk)entry);
        }

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
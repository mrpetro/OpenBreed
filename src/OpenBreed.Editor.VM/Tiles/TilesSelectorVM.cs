using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Model.Palettes;
using OpenBreed.Rendering.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TilesSelectorVM : BaseViewModel
    {
        #region Private Fields

        private readonly TileAtlasDataProvider tileSetsDataProvider;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IRepositoryProvider repositoryProvider;
        private string currentTileSetRef;

        #endregion Private Fields

        #region Public Constructors

        public TilesSelectorVM(
            TileAtlasDataProvider tileSetsDataProvider,
            TileSetViewerVM tileSetViewerVm,
            PalettesDataProvider palettesDataProvider,
            IRepositoryProvider repositoryProvider)
        {
            this.tileSetsDataProvider = tileSetsDataProvider;
            Viewer = tileSetViewerVm;
            this.palettesDataProvider = palettesDataProvider;
            this.repositoryProvider = repositoryProvider;

            RefreshTileSetRefs();

            Viewer.SelectionChanged += Viewer_SelectionChanged;
        }

        private void Viewer_SelectionChanged(object sender, TilesSelectionChangedEventArgs e)
        {
            TilesSelectionChanged?.Invoke(this, e);
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<string> TileSetIds { get; } = new ObservableCollection<string>();

        public string CurrentTileSetId
        {
            get { return currentTileSetRef; }
            set { SetProperty(ref currentTileSetRef, value); }
        }

        public TileSetViewerVM Viewer { get; }

        #endregion Public Properties

        #region Protected Methods

        public event EventHandler<string> TileSetChanged;
        public event EventHandler<TilesSelectionChangedEventArgs> TilesSelectionChanged;

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentTileSetId):
                    RefreshTileSetViewer();
                    TileSetChanged?.Invoke(this, CurrentTileSetId);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RefreshTileSetRefs()
        {
            var repository = repositoryProvider.GetRepository<IDbTileAtlas>();

            TileSetIds.Clear();
            repository.Entries.ForEach(item => TileSetIds.Add(item.Id));

            if (!TileSetIds.Contains(CurrentTileSetId))
            {
                CurrentTileSetId = TileSetIds.FirstOrDefault();
            }
        }
        public static Color4 ToColor4(MyColor color)
        {
            return new Color4(
                color.R / 255.0f,
                color.G / 255.0f,
                color.B / 255.0f,
                color.A / 255.0f);
        }

        //private IPalette palette;

        //private PaletteModel GetPalette()
        //{
        //    var commonPaletteModel = palettesDataProvider.GetPalette("Palettes.COMMON");

        //    var builder = paletteMan.CreatePalette()
        //        .SetLength(256)
        //        .SetName("GamePalette")
        //        .SetColors(commonPaletteModel.Data.Select(color => ToColor4(color)).ToArray());

        //    var cb = commonPaletteModel[0];
        //    builder.SetColor(0, new Color4(cb.R / 255.0f, cb.G / 255.0f, cb.B / 255.0f, 0.0f));

        //    palette = builder.Build();
        //}

        private void RefreshTileSetViewer()
        {
            var model = tileSetsDataProvider.GetTileAtlas(CurrentTileSetId);

            if (model is null)
            {
                return;
            }

            Viewer.FromModel(model);
            Viewer.Palette = PaletteModel.NullPalette;
            Viewer.Palette = palettesDataProvider.GetPalette("Palettes.COMMON");
        }

        #endregion Private Methods
    }
}
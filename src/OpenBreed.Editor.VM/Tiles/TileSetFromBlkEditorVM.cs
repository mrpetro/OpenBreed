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
    public class TileSetFromBlkEditorVM : BaseViewModel, IEntryEditor<ITileSetEntry>
    {
        #region Private Fields

        private string currentPaletteRef = null;
        private int _currentPaletteIndex = -1;

        private int _tileSize;

        private TileSetModel model;
        private readonly TileSetsDataProvider tileSetsDataProvider;
        private readonly PalettesDataProvider palettesDataProvider;

        #endregion Private Fields

        #region Public Constructors

        public TileSetFromBlkEditorVM(TileSetsDataProvider tileSetsDataProvider,
                                      PalettesDataProvider palettesDataProvider)
        {
            PaletteIds = new BindingList<string>();
            Items = new BindingList<TileVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));
            this.tileSetsDataProvider = tileSetsDataProvider;
            this.palettesDataProvider = palettesDataProvider;

            //Viewer = new TileSetViewerVM();
        }

        #endregion Public Constructors

        //public TileSetViewerVM Viewer { get; set; }

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

        public BindingList<TileVM> Items { get; private set; }

        public int TileSize
        {
            get { return _tileSize; }
            set { SetProperty(ref _tileSize, value); }
        }

        public int TilesNoX => model.TilesNoY;
        public int TilesNoY => model.TilesNoY;
        public int Width => model.Bitmap.Width;
        public int Height => model.Bitmap.Height;

        #endregion Public Properties

        #region Internal Properties

        internal PaletteModel CurrentPalette { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public virtual void UpdateEntry(ITileSetEntry entry)
        {
        }

        public virtual void UpdateVM(ITileSetEntry entry)
        {
            model = tileSetsDataProvider.GetTileSet(entry.Id);

            if (model == null)
                return;

            TileSize = model.TileSize;
            SetupTiles(model.Tiles);
            SetupPaletteIds(entry.PaletteRefs);
        }

        public void Draw(Graphics gfx)
        {
            int xMax = TilesNoX;
            int yMax = TilesNoY;

            for (int j = 0; j < yMax; j++)
            {
                for (int i = 0; i < xMax; i++)
                {
                    int gfxId = i + xMax * j;
                    DrawTile(gfx, gfxId, i * TileSize, j * TileSize, TileSize);
                }
            }
        }

        public void DrawTile(Graphics gfx, int tileId, float x, float y, int tileSize)
        {
            if (tileId >= Items.Count)
                return;

            var tileRect = Items[tileId].Rectangle;
            gfx.DrawImage(model.Bitmap, (int)x, (int)y, tileRect, GraphicsUnit.Pixel);
        }

        public Point GetIndexCoords(Point point)
        {
            return new Point(point.X / TileSize, point.Y / TileSize);
        }

        public Point GetSnapCoords(Point point)
        {
            int x = point.X / TileSize;
            int y = point.Y / TileSize;

            return new Point(x * TileSize, y * TileSize);
        }

        public void LoadDefaultTiles()
        {
            RebuildTiles();
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
        }

        internal void SetupTiles(List<TileModel> tiles)
        {
            RebuildTiles();
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

        private void RebuildTiles()
        {
            Items.UpdateAfter(() => 
            {
                Items.Clear();

                var tilesCount = TilesNoX * TilesNoY;

                for (int tileId = 0; tileId < tilesCount; tileId++)
                {
                    int tileIndexX = tileId % TilesNoX;
                    int tileIndexY = tileId / TilesNoX;
                    var rectangle = new Rectangle(tileIndexX * TileSize, tileIndexY * TileSize, TileSize, TileSize);
                    Items.Add(new TileVM(tileId, rectangle));
                }
            });
        }

        #endregion Private Methods
    }
}
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Common.Sources;
using OpenBreed.Editor.VM.Tiles.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenBreed.Editor.VM.Palettes;
using System.Linq;
using OpenBreed.Common.Drawing;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetVM : BaseViewModel
    {

        #region Private Fields

        private string _name;
        private PaletteVM _palette;

        #endregion Private Fields

        #region Public Constructors

        public TileSetVM(EditorVM root)
        {
            Root = root;

            Items = new BindingList<TileVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));

            PropertyChanged += TileSetVM_PropertyChanged;
            Root.Palettes.PropertyChanged += Palettes_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public Bitmap Bitmap { get; private set; }
        public BindingList<TileVM> Items { get; private set; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public PaletteVM Palette
        {
            get { return _palette; }
            set
            {
                var prevPalette = _palette;
                if (SetProperty(ref _palette, value))
                {
                    if (prevPalette != null)
                        prevPalette.PropertyChanged -= Palette_PropertyChanged;

                    _palette.PropertyChanged += Palette_PropertyChanged;
                }
            }
        }

        public EditorVM Root { get; private set; }
        public BaseSource Source { get; private set; }

        public int TileSize { get; private set; }

        public int TilesNoX { get; private set; }

        public int TilesNoY { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static TileSetVM Create(EditorVM root, BaseSource source)
        {
            var model = source.Load() as TileSetModel;

            var newTileSet = new TileSetVM(root);
            newTileSet.Source = source;
            newTileSet.Name = source.Name;
            newTileSet.TileSize = model.TileSize;
            newTileSet.Bitmap = newTileSet.ToBitmap(model.Tiles);
            newTileSet.SetupTiles();

            return newTileSet;
        }

        public void Dispose()
        {
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
            gfx.DrawImage(Bitmap, (int)x, (int)y, tileRect, GraphicsUnit.Pixel);
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

        public List<int> GetTileIdList(Rectangle rectangle)
        {
            int left = rectangle.Left;
            int right = rectangle.Right;
            int top = rectangle.Top;
            int bottom = rectangle.Bottom;

            if (left < 0)
                left = 0;

            if (right > Bitmap.Width)
                right = Bitmap.Width;

            if (top < 0)
                top = 0;

            if (bottom > Bitmap.Height)
                bottom = Bitmap.Height;

            rectangle = new Rectangle(left, top, right - left, bottom - top);

            List<int> tileIdList = new List<int>();
            int xFrom = rectangle.Left / TileSize;
            int xTo = rectangle.Right / TileSize;
            int yFrom = rectangle.Top / TileSize;
            int yTo = rectangle.Bottom / TileSize;

            for (int xIndex = xFrom; xIndex < xTo; xIndex++)
            {
                for (int yIndex = yFrom; yIndex < yTo; yIndex++)
                {
                    int gfxId = xIndex + TilesNoX * yIndex;
                    tileIdList.Add(gfxId);
                }
            }

            return tileIdList;
        }

        public void LoadDefaultTiles()
        {
            CreateDefaultBitmap();
            SetupTiles();
        }

        public void LoadFromBLK()
        {
        }

        public Bitmap ToBitmap(List<TileModel> tiles)
        {
            int bmpWidth = 320;
            TilesNoX = bmpWidth / TileSize;
            TilesNoY = tiles.Count / TilesNoX;
            int bmpHeight = TilesNoY * TileSize;
            Bitmap bitmap = new Bitmap(bmpWidth, bmpHeight, PixelFormat.Format8bppIndexed);

            for (int j = 0; j < TilesNoY; j++)
            {
                for (int i = 0; i < TilesNoX; i++)
                {
                    //Create a BitmapData and Lock all pixels to be written
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(i * TileSize, j * TileSize, TileSize, TileSize),
                                                         ImageLockMode.WriteOnly, bitmap.PixelFormat);

                    //Copy the data from the byte array into BitmapData.Scan0
                    for (int k = 0; k < TileSize; k++)
                        Marshal.Copy(tiles[i + j * TilesNoX].Data, k * TileSize, bmpData.Scan0 + k * bmpData.Stride, TileSize);

                    //Unlock the pixels
                    bitmap.UnlockBits(bmpData);
                }
            }

            return bitmap;
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateDefaultBitmap()
        {
            Bitmap = new Bitmap(320, 768, PixelFormat.Format32bppArgb);

            TileSize = 16;
            TilesNoX = Bitmap.Width / TileSize;
            TilesNoY = Bitmap.Height / TileSize;

            using (Graphics gfx = Graphics.FromImage(Bitmap))
            {
                Font font = new Font("Arial", 5);

                for (int j = 0; j < TilesNoY; j++)
                {
                    for (int i = 0; i < TilesNoX; i++)
                    {
                        int tileId = i + j * TilesNoX;

                        var rectangle = new Rectangle(i * TileSize, j * TileSize, TileSize - 1, TileSize - 1);

                        Color c = Color.Black;
                        Pen tileColor = new Pen(c);
                        Brush brush = new SolidBrush(c);

                        gfx.FillRectangle(brush, rectangle);

                        c = Color.White;
                        tileColor = new Pen(c);
                        brush = new SolidBrush(c);

                        gfx.DrawRectangle(tileColor, rectangle);
                        gfx.DrawString(string.Format("{0,2:D2}", tileId / 100), font, brush, i * TileSize + 2, j * TileSize + 1);
                        gfx.DrawString(string.Format("{0,2:D2}", tileId % 100), font, brush, i * TileSize + 2, j * TileSize + 7);
                    }
                }
            }
        }

        private void Palette_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette.Colors):
                    BitmapHelper.SetPaletteColors(Bitmap, Palette.Colors.ToArray());
                    break;
                default:
                    break;
            }
        }

        private void Palettes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Root.Palettes.CurrentItem):
                    Palette = Root.Palettes.CurrentItem;
                    break;
                default:
                    break;
            }
        }

        private void SetupTiles()
        {
            Items.Clear();

            var tilesCount = TilesNoX * TilesNoY;

            for (int tileId = 0; tileId < tilesCount; tileId++)
            {
                int tileIndexX = tileId % TilesNoX;
                int tileIndexY = tileId / TilesNoX;
                var rectangle = new Rectangle(tileIndexX * TileSize, tileIndexY * TileSize, TileSize, TileSize);
                Items.Add(new TileVM(this, tileId, rectangle));
            }
        }

        private void TileSetVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Palette):
                    BitmapHelper.SetPaletteColors(Bitmap, Palette.Colors.ToArray());
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
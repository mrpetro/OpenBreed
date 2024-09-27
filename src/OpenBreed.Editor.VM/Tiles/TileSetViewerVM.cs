using OpenBreed.Common.Data;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Tiles.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using OpenBreed.Model;
using OpenBreed.Common.Tools;
using System.Windows.Input;
using System;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Renderer;
using System.Drawing;
using OpenBreed.Editor.UI.Mvc.Models;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TilesSelectionChangedEventArgs : EventArgs
    {
        #region Public Constructors

        public TilesSelectionChangedEventArgs(TileSelection[] selections)
        {
            Selections = selections;
        }

        #endregion Public Constructors

        #region Public Properties

        public TileSelection[] Selections { get; }

        #endregion Public Properties
    }

    public class TileSetViewerVM : EditableEntryVM
    {
        #region Private Fields

        private readonly IDrawingFactory drawingFactory;
        private readonly IDrawingContextProvider drawingContextProvider;
        private readonly IBitmapProvider bitmapProvider;
        private string info;
        private IBitmap _bitmap;
        private PaletteModel _palette;
        private int _width;
        private int _height;

        #endregion Private Fields

        #region Public Constructors

        public TileSetViewerVM(IDrawingFactory drawingFactory, IDrawingContextProvider drawingContextProvider, IBitmapProvider bitmapProvider)
        {
            Selector = new TilesSelector(this, drawingFactory);
            Selector.InfoChanged += (s, a) => Info = Selector.Info;

            Items = new BindingList<TileVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));

            Bitmap = drawingFactory.CreateBitmap(1, 1, MyPixelFormat.Format8bppIndexed);
            OnMouseLeftButtonDownCommand = new Command(OnLeft);
            OnMouseRightButtonDownCommand = new Command(OnRight);
            this.drawingFactory = drawingFactory;
            this.drawingContextProvider = drawingContextProvider;
            this.bitmapProvider = bitmapProvider;

            Selector.SelectionChanged += (s, a) => SelectionChanged?.Invoke(this, a);
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<TilesSelectionChangedEventArgs> SelectionChanged;

        #endregion Public Events

        #region Public Properties

        public ICommand OnMouseLeftButtonDownCommand { get; }

        public ICommand OnMouseRightButtonDownCommand { get; }

        public IBitmap Bitmap
        {
            get { return _bitmap; }
            set { SetProperty(ref _bitmap, value); }
        }

        public int Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        public BindingList<TileVM> Items { get; }

        public PaletteModel Palette
        {
            get { return _palette; }
            set { SetProperty(ref _palette, value); }
        }

        public string Info
        {
            get { return info; }
            set { SetProperty(ref info, value); }
        }

        public TilesSelector Selector { get; }

        public int TileSize { get; set; }

        public int TilesNoX { get; private set; }

        public int TilesNoY { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Draw(IDrawingContext gfx)
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

        public void DrawTile(IDrawingContext gfx, int tileId, float x, float y, int tileSize)
        {
            if (tileId >= Items.Count)
                return;

            var tileRect = Items[tileId].GetBox(TileSize);
            gfx.DrawImage(Bitmap, (int)x, (int)y, tileRect);
        }

        public MyPoint GetIndexCoords(MyPoint point)
        {
            return new MyPoint(point.X / TileSize, point.Y / TileSize);
        }

        public MyPoint GetSnapCoords(MyPoint point)
        {
            int x = point.X / TileSize;
            int y = point.Y / TileSize;

            return new MyPoint(x * TileSize, y * TileSize);
        }

        public List<int> GetTileIdList(MyRectangle rectangle)
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

            rectangle = new MyRectangle(left, top, right - left, bottom - top);

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
            RebuildTiles();
        }

        public IBitmap ToBitmap(List<TileModel> tiles)
        {
            int bmpWidth = 320;
            TilesNoX = bmpWidth / TileSize;
            TilesNoY = tiles.Count / TilesNoX;
            int bmpHeight = TilesNoY * TileSize;
            var bitmap = drawingFactory.CreateBitmap(bmpWidth, bmpHeight, MyPixelFormat.Format8bppIndexed);

            for (int j = 0; j < TilesNoY; j++)
            {
                for (int i = 0; i < TilesNoX; i++)
                {
                    //Create a BitmapData and Lock all pixels to be written
                    var bmpData = bitmap.LockBits(new MyRectangle(i * TileSize, j * TileSize, TileSize, TileSize),
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

        #region Internal Methods

        internal void FromModel(TileSetModel tileSet)
        {
            TileSize = tileSet.TileSize;
            SetupTiles(tileSet.Tiles);
        }

        internal void SetupTiles(List<TileModel> tiles)
        {
            Bitmap = ToBitmap(tiles);
            Width = Bitmap.Width;
            Height = Bitmap.Height;
            RebuildTiles();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(Palette):
                    bitmapProvider.SetPaletteColors(Bitmap, Palette.Data);
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnLeft()
        {
            if (Selector.SelectMode != SelectModeEnum.Nothing)
            {
                return;
            }

            //Selector.StartSelection(SelectModeEnum.Select, e.Location);
        }

        private void OnRight()
        {
            if (Selector.SelectMode != SelectModeEnum.Nothing)
            {
                return;
            }

            //Selector.StartSelection(SelectModeEnum.Deselect, e.Location);
        }

        private void CreateDefaultBitmap()
        {
            Bitmap = drawingFactory.CreateBitmap(320, 768, MyPixelFormat.Format32bppArgb);
            Width = 320;
            Height = 768;
            TileSize = 16;
            TilesNoX = Bitmap.Width / TileSize;
            TilesNoY = Bitmap.Height / TileSize;

            using (var gfx = drawingContextProvider.FromImage(Bitmap))
            {
                var font = drawingFactory.CreateFont("Arial", 5);

                for (int j = 0; j < TilesNoY; j++)
                {
                    for (int i = 0; i < TilesNoX; i++)
                    {
                        int tileId = i + j * TilesNoX;

                        var rectangle = new MyRectangle(i * TileSize, j * TileSize, TileSize - 1, TileSize - 1);

                        var c = MyColor.Black;
                        var tileColor = drawingFactory.CreatePen(c);
                        var brush = drawingFactory.CreateSolidBrush(c);

                        gfx.FillRectangle(brush, rectangle);

                        c = MyColor.White;
                        tileColor = drawingFactory.CreatePen(c);
                        brush = drawingFactory.CreateSolidBrush(c);

                        gfx.DrawRectangle(tileColor, rectangle);
                        gfx.DrawString(string.Format("{0,2:D2}", tileId / 100), font, brush, i * TileSize + 2, j * TileSize + 1);
                        gfx.DrawString(string.Format("{0,2:D2}", tileId % 100), font, brush, i * TileSize + 2, j * TileSize + 7);
                    }
                }
            }
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
                    Items.Add(new TileVM(tileId, new MyPoint(tileIndexX, tileIndexY)));
                }
            });
        }

        #endregion Private Methods
    }
}
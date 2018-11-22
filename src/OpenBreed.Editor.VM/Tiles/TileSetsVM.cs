using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Base;
using System;
using System.ComponentModel;
using System.Linq;
using OpenBreed.Common.Maps;
using System.Drawing;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetsVM : BaseViewModel
    {
        #region Private Fields

        private int _currentIndex = -1;
        private TileSetVM _currentItem;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public TileSetsVM(EditorVM root)
        {
            Root = root;

            TileSetViewer = new TileSetViewerVM(this);

            Items = new BindingList<TileSetVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));

            PropertyChanged += TileSetsVM_PropertyChanged;
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

        public TileSetVM CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (SetProperty(ref _currentItem, value))
                    CurrentIndex = Items.IndexOf(value);
            }
        }

        public EditorVM Root { get; private set; }

        public BindingList<TileSetVM> Items { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public TileSetViewerVM TileSetViewer { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void AddTileSet(string tileSetRef)
        {
            var tileSetSourceDef = Root.CurrentDatabase.GetSourceDef(tileSetRef);
            if (tileSetSourceDef == null)
                throw new Exception("No TileSetSource definition found with name: " + tileSetRef);

            var source = Root.Sources.GetSource(tileSetSourceDef);

            if (source == null)
                throw new Exception("TileSet source error: " + tileSetRef);

            Items.Add(TileSetVM.Create(this, source));

            CurrentItem = Items.LastOrDefault();
        }

        internal void DrawTile(Graphics gfx, TileRef tileRef, float x, float y, int tileSize)
        {
            if (tileRef.TileSetId < Items.Count)
                Items[tileRef.TileSetId].DrawTile(gfx, tileRef.TileId, x, y, tileSize);
            else
                DrawDefaultTile(gfx, tileRef, x, y, tileSize);
        }

        private void DrawDefaultTile(Graphics gfx, TileRef tileRef, float x, float y, int tileSize)
        {
            Font font = new Font("Arial", 5);

            var rectangle = new Rectangle((int)x, (int)y, tileSize, tileSize);

            Color c = Color.Black;
            Pen tileColor = new Pen(c);
            Brush brush = new SolidBrush(c);

            gfx.FillRectangle(brush, rectangle);

            c = Color.White;
            tileColor = new Pen(c);
            brush = new SolidBrush(c);

            gfx.DrawRectangle(tileColor, rectangle);
            gfx.DrawString(string.Format("{0,2:D2}", tileRef.TileId / 100), font, brush, x + 2, y + 1);
            gfx.DrawString(string.Format("{0,2:D2}", tileRef.TileId % 100), font, brush, x + 2, y + 7);
        }

        #endregion Public Methods

        #region Private Methods

        private void TileSetsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentItem):
                    if (CurrentItem == null)
                        Title = "Tile sets - <no current tile set>";
                    else
                        Title = "Tile sets - " + CurrentItem.Source.Name;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
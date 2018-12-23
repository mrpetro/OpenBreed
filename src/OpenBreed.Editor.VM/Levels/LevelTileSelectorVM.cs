using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Base;
using System;
using System.ComponentModel;
using System.Linq;
using OpenBreed.Common.Maps;
using System.Drawing;
using OpenBreed.Editor.VM.Levels;

namespace OpenBreed.Editor.VM.Tiles
{
    public class LevelTileSelectorVM : BaseViewModel
    {

        #region Private Fields

        private int _currentIndex = -1;
        private TileSetVM _currentItem = null;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public LevelTileSelectorVM(LevelEditorVM parent)
        {
            Parent = parent;

            PropertyChanged += LevelTileSelectorVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { SetProperty(ref _currentIndex, value); }
        }

        public TileSetVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public LevelEditorVM Parent { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void Connect()
        {
        }

        internal void DrawTile(Graphics gfx, TileRef tileRef, float x, float y, int tileSize)
        {
            if (tileRef.TileSetId < Parent.Root.LevelEditor.CurrentLevel.TileSets.Count)
                Parent.Root.LevelEditor.CurrentLevel.TileSets[tileRef.TileSetId].DrawTile(gfx, tileRef.TileId, x, y, tileSize);
            else
                DrawDefaultTile(gfx, tileRef, x, y, tileSize);
        }

        #endregion Internal Methods

        #region Private Methods

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

        private void LevelTileSelectorVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentIndex):
                    UpdateCurrentItem();
                    break;
                case nameof(CurrentItem):
                    UpdateCurrentIndex();
                    break;
                default:
                    break;
            }
        }

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

        private void UpdateCurrentIndex()
        {
            CurrentIndex = Parent.CurrentLevel.TileSets.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = Parent.CurrentLevel.TileSets[CurrentIndex];
        }

        #endregion Private Methods

    }
}
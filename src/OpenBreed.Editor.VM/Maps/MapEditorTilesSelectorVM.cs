using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Base;
using System;
using System.ComponentModel;
using System.Linq;
using OpenBreed.Common.Maps;
using System.Drawing;
using OpenBreed.Editor.VM.Maps;
using System.Collections.Generic;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Tiles;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorTilesSelectorVM : BaseViewModel
    {

        #region Public Fields

        public Point CenterCoord;

        public Point MaxCoord;

        public Point MinCoord;

        #endregion Public Fields

        #region Private Fields

        private TileSetVM _currentTileSet;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTilesSelectorVM(MapEditorTilesToolVM parent)
        {
            Parent = parent;

            SelectedIndexes = new List<int>();
            SelectionRectangle = new SelectionRectangle();
            SelectMode = SelectModeEnum.Nothing;
            MultiSelect = false;

            PropertyChanged += TileSetViewerVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public TileSetVM CurrentTileSet
        {
            get { return _currentTileSet; }
            set { SetProperty(ref _currentTileSet, value); }
        }

        public bool IsEmpty { get { return SelectedIndexes.Count == 0; } }
        public bool MultiSelect { get; set; }
        public MapEditorTilesToolVM Parent { get; }
        public EditorVM Root { get; }

        public List<int> SelectedIndexes { get; }

        public SelectionRectangle SelectionRectangle { get; }

        public SelectModeEnum SelectMode { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void AddSelection(List<int> tileIdList)
        {
            foreach (int tileId in tileIdList)
                AddSelection(tileId);
        }

        public void AddSelection(int tileId)
        {
            if (!SelectedIndexes.Contains(tileId))
                SelectedIndexes.Add(tileId);
        }

        public void ClearSelection()
        {
            SelectedIndexes.Clear();
        }

        public void DrawSelection(Graphics gfx)
        {
            Pen selectedPen = new Pen(Color.LightGreen);
            Pen selectPen = new Pen(Color.LightBlue);
            Pen deselectPen = new Pen(Color.Red);

            for (int index = 0; index < SelectedIndexes.Count; index++)
            {
                Rectangle rectangle = CurrentTileSet.Items[SelectedIndexes[index]].Rectangle;
                gfx.DrawRectangle(selectedPen, rectangle);
            }

            if (SelectMode == SelectModeEnum.Select)
                gfx.DrawRectangle(selectPen, SelectionRectangle.GetRectangle(CurrentTileSet.TileSize));
            else if (SelectMode == SelectModeEnum.Deselect)
                gfx.DrawRectangle(deselectPen, SelectionRectangle.GetRectangle(CurrentTileSet.TileSize));
        }

        public void FinishSelection(Point point)
        {
            SelectionRectangle.SetFinish(GetIndexCoords(point));

            Rectangle selectionRectangle = SelectionRectangle.GetRectangle(CurrentTileSet.TileSize);
            List<int> selectionList = CurrentTileSet.GetTileIdList(selectionRectangle);

            if (SelectMode == SelectModeEnum.Select)
            {
                if (!MultiSelect)
                    ClearSelection();

                AddSelection(selectionList);
            }
            else if (SelectMode == SelectModeEnum.Deselect)
            {
                RemoveSelection(selectionList);
            }

            if (!IsEmpty)
                CalculateSelectionCenter();

            SelectMode = SelectModeEnum.Nothing;
        }

        public void RemoveSelection(List<int> tileIdList)
        {
            foreach (int tileId in tileIdList)
                RemoveSelection(tileId);
        }

        public void RemoveSelection(int tileId)
        {
            SelectedIndexes.Remove(tileId);
        }

        public void StartSelection(SelectModeEnum selectMode, Point point)
        {
            SelectMode = selectMode;
            SelectionRectangle.SetStart(GetIndexCoords(point));

            if (!MultiSelect && selectMode == SelectModeEnum.Select)
                ClearSelection();
        }

        public void UpdateSelection(Point point)
        {
            SelectionRectangle.Update(GetIndexCoords(point));
        }

        #endregion Public Methods

        #region Internal Methods

        internal Point GetIndexCoords(Point point)
        {
            return CurrentTileSet.GetIndexCoords(point);
        }

        #endregion Internal Methods

        #region Private Methods

        private void CalculateSelectionCenter()
        {
            Rectangle rectangle = CurrentTileSet.Items[SelectedIndexes[0]].Rectangle;

            MinCoord.X = rectangle.Left;
            MaxCoord.X = rectangle.Right;
            MinCoord.Y = rectangle.Bottom;
            MaxCoord.Y = rectangle.Top;

            for (int i = 1; i < SelectedIndexes.Count; i++)
            {
                rectangle = CurrentTileSet.Items[SelectedIndexes[i]].Rectangle;

                MinCoord.X = Math.Min(MinCoord.X, rectangle.Left);
                MaxCoord.X = Math.Max(MaxCoord.X, rectangle.Right);
                MinCoord.Y = Math.Min(MinCoord.Y, rectangle.Bottom);
                MaxCoord.Y = Math.Max(MaxCoord.Y, rectangle.Top);
            }

            CenterCoord = CurrentTileSet.GetSnapCoords(new Point((MinCoord.X + MaxCoord.X) / 2, (MinCoord.Y + MaxCoord.Y) / 2));
        }

        private void TileSetViewerVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentTileSet):
                    SelectedIndexes.Clear();
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
using OpenBreed.Editor.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Tiles.Helpers
{
    public class TilesSelector
    {
        internal TileSetViewerVM Viewer;

        #region Public Fields

        public Point CenterCoord;

        public Point MaxCoord;

        public Point MinCoord;

        #endregion Public Fields

        #region Public Constructors

        public TilesSelector(TileSetViewerVM viewer)
        {
            Viewer = viewer;

            SelectedIndexes = new List<int>();
            SelectionRectangle = new SelectionRectangle();
            SelectMode = SelectModeEnum.Nothing;
            MultiSelect = false;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsEmpty { get { return SelectedIndexes.Count == 0; } }

        public bool MultiSelect { get; set; }

        public EditorApplicationVM Root { get; }

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
                Rectangle rectangle = Viewer.Items[SelectedIndexes[index]].Rectangle;
                gfx.DrawRectangle(selectedPen, rectangle);
            }

            if (SelectMode == SelectModeEnum.Select)
                gfx.DrawRectangle(selectPen, SelectionRectangle.GetRectangle(Viewer.TileSize));
            else if (SelectMode == SelectModeEnum.Deselect)
                gfx.DrawRectangle(deselectPen, SelectionRectangle.GetRectangle(Viewer.TileSize));
        }

        public void FinishSelection(Point point)
        {
            SelectionRectangle.SetFinish(GetIndexCoords(point));

            Rectangle selectionRectangle = SelectionRectangle.GetRectangle(Viewer.TileSize);
            List<int> selectionList = Viewer.GetTileIdList(selectionRectangle);

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
            return Viewer.GetIndexCoords(point);
        }

        #endregion Internal Methods

        #region Private Methods

        private void CalculateSelectionCenter()
        {
            Rectangle rectangle = Viewer.Items[SelectedIndexes[0]].Rectangle;

            MinCoord.X = rectangle.Left;
            MaxCoord.X = rectangle.Right;
            MinCoord.Y = rectangle.Bottom;
            MaxCoord.Y = rectangle.Top;

            for (int i = 1; i < SelectedIndexes.Count; i++)
            {
                rectangle = Viewer.Items[SelectedIndexes[i]].Rectangle;

                MinCoord.X = Math.Min(MinCoord.X, rectangle.Left);
                MaxCoord.X = Math.Max(MaxCoord.X, rectangle.Right);
                MinCoord.Y = Math.Min(MinCoord.Y, rectangle.Bottom);
                MaxCoord.Y = Math.Max(MaxCoord.Y, rectangle.Top);
            }

            CenterCoord = Viewer.GetSnapCoords(new Point((MinCoord.X + MaxCoord.X) / 2, (MinCoord.Y + MaxCoord.Y) / 2));
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Viewer.Items):
                    SelectedIndexes.Clear();
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
using OpenBreed.Common.Interface.Drawing;
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
        private readonly IDrawingFactory drawingFactory;

        #region Public Fields

        public MyPoint CenterCoord;

        public MyPoint MaxCoord;

        public MyPoint MinCoord;

        #endregion Public Fields

        #region Public Constructors

        public TilesSelector(TileSetViewerVM viewer, IDrawingFactory drawingFactory)
        {
            Viewer = viewer;
            this.drawingFactory = drawingFactory;
            SelectedIndexes = new List<int>();
            SelectionRectangle = new SelectionRectangle();
            SelectMode = SelectModeEnum.Nothing;
            MultiSelect = false;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsEmpty { get { return SelectedIndexes.Count == 0; } }

        public event EventHandler InfoChanged;

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

        public string Info { get; private set; }

        private void UpdateInfo()
        {
            if (SelectedIndexes.Count == 1)
                Info = $"Index: {SelectedIndexes[0]}";
            else
                Info = "";

            InfoChanged?.Invoke(this, new EventArgs());
        }

        public void AddSelection(int tileId)
        {
            if (!SelectedIndexes.Contains(tileId))
            {
                SelectedIndexes.Add(tileId);
                UpdateInfo();
            }
        }

        public void ClearSelection()
        {
            SelectedIndexes.Clear();
            UpdateInfo();
        }

        public void DrawSelection(IDrawingContext gfx)
        {
            var selectedPen = drawingFactory.CreatePen(MyColor.LightGreen);
            var selectPen = drawingFactory.CreatePen(MyColor.LightBlue);
            var deselectPen = drawingFactory.CreatePen(MyColor.Red);

            for (int index = 0; index < SelectedIndexes.Count; index++)
            {
                var rectangle = Viewer.Items[SelectedIndexes[index]].Rectangle;
                gfx.DrawRectangle(selectedPen, rectangle);
            }

            if (SelectMode == SelectModeEnum.Select)
                gfx.DrawRectangle(selectPen, SelectionRectangle.GetRectangle(Viewer.TileSize));
            else if (SelectMode == SelectModeEnum.Deselect)
                gfx.DrawRectangle(deselectPen, SelectionRectangle.GetRectangle(Viewer.TileSize));
        }

        public void FinishSelection(MyPoint point)
        {
            SelectionRectangle.SetFinish(GetIndexCoords(point));

            var selectionRectangle = SelectionRectangle.GetRectangle(Viewer.TileSize);
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

        public void StartSelection(SelectModeEnum selectMode, MyPoint point)
        {
            SelectMode = selectMode;
            SelectionRectangle.SetStart(GetIndexCoords(point));

            if (!MultiSelect && selectMode == SelectModeEnum.Select)
                ClearSelection();
        }

        public void UpdateSelection(MyPoint point)
        {
            SelectionRectangle.Update(GetIndexCoords(point));
        }

        #endregion Public Methods

        #region Internal Methods

        internal MyPoint GetIndexCoords(MyPoint point)
        {
            return Viewer.GetIndexCoords(point);
        }

        #endregion Internal Methods

        #region Private Methods

        private void CalculateSelectionCenter()
        {
            var rectangle = Viewer.Items[SelectedIndexes[0]].Rectangle;

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

            CenterCoord = Viewer.GetSnapCoords(new MyPoint((MinCoord.X + MaxCoord.X) / 2, (MinCoord.Y + MaxCoord.Y) / 2));
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
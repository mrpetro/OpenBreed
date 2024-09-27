using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Editor.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Tiles.Helpers
{
    public class TilesSelector
    {
        #region Public Fields

        #endregion Public Fields

        #region Internal Fields

        internal TileSetViewerVM Viewer;

        #endregion Internal Fields

        #region Private Fields

        private readonly IDrawingFactory drawingFactory;

        #endregion Private Fields

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

        #region Public Events

        public event EventHandler InfoChanged;

        public event EventHandler<TilesSelectionChangedEventArgs> SelectionChanged;

        #endregion Public Events

        #region Public Properties

        public bool IsEmpty
        { get { return SelectedIndexes.Count == 0; } }
        public bool MultiSelect { get; set; }

        public List<int> SelectedIndexes { get; }

        public SelectionRectangle SelectionRectangle { get; }

        public SelectModeEnum SelectMode { get; private set; }
        public string Info { get; private set; }

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

            var tileSize = Viewer.TileSize;

            for (int index = 0; index < SelectedIndexes.Count; index++)
            {
                var rectangle = Viewer.Items[SelectedIndexes[index]].GetBox(tileSize);
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
            {
                CalculateSelection();
            }

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

        private void UpdateInfo()
        {
            if (SelectedIndexes.Count == 1)
                Info = $"Index: {SelectedIndexes[0]}";
            else
                Info = "";

            InfoChanged?.Invoke(this, new EventArgs());
        }

        private void OnSelectionChanged(TileSelection[] selection)
        {
            SelectionChanged?.Invoke(this, new TilesSelectionChangedEventArgs(selection));
        }


        private void CalculateSelection()
        {

            var extent = new MyExtent();

            var selections = new List<TileSelection>();

            for (int i = 0; i < SelectedIndexes.Count; i++)
            {
                var selectedItem = Viewer.Items[SelectedIndexes[i]];

                var pos = selectedItem.IndexPosition;

                extent.Expand(pos);

                selections.Add(new TileSelection()
                {
                    Index = selectedItem.Index,
                    Position = selectedItem.IndexPosition
                });
            }

            var selectionCenter = extent.Center;

            var flipY = new MyPoint(1, -1);

            for (int i = 0; i < selections.Count; i++)
            {
                selections[i] = new TileSelection()
                {
                    Index = selections[i].Index,
                    Position = (selections[i].Position - selectionCenter) * flipY
                };
            }

            OnSelectionChanged(selections.ToArray());
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
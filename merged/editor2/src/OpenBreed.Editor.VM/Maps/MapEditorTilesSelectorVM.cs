using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Renderer;
using OpenBreed.Model.Tiles;
using System;
using System.Collections.Generic;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorTilesSelectorVM : BaseViewModel
    {
        #region Public Fields

        public MyPoint CenterCoord;

        public MyPoint MaxCoord;

        public MyPoint MinCoord;

        #endregion Public Fields

        #region Private Fields

        private string currentTileSetRef;
        private TilesSelectorRenderer renderer;

        #endregion Private Fields

        #region Public Constructors

        public IRenderTarget RenderTarget { get; }

        public MapEditorTilesSelectorVM(MapEditorTilesToolVM parent, TilesSelectorRenderer renderer, IRenderTarget renderTarget)
        {
            Parent = parent;
            this.renderer = renderer;
            RenderTarget = renderTarget;
            SelectedIndexes = new List<int>();
            SelectionRectangle = new SelectionRectangle();
            SelectMode = SelectModeEnum.Nothing;
            MultiSelect = false;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapEditorTilesToolVM Parent { get; }

        public string CurrentTileSetRef
        {
            get { return currentTileSetRef; }
            set { SetProperty(ref currentTileSetRef, value); }
        }

        public void Resize(int width, int height)
        {
            RenderTarget.Resize(width, height);
        }

        public bool IsEmpty { get { return SelectedIndexes.Count == 0; } }

        public bool MultiSelect { get; set; }

        public EditorApplicationVM Root { get; }

        public List<int> SelectedIndexes { get; }

        public SelectionRectangle SelectionRectangle { get; }

        public SelectModeEnum SelectMode { get; private set; }

        public Action<string> ModelChangeAction { get; internal set; }

        #endregion Public Properties

        #region Internal Properties

        internal TileSetModel CurrentTileSet => Parent.Parent.TileSet;

        #endregion Internal Properties

        #region Public Methods

        public void Render(IDrawingContext graphics)
        {
            if (Parent.Parent.Model is null)
                return;

            renderer.Render(this);
            RenderTarget.Flush(graphics);
        }

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

        public List<int> GetTileIdList(MyRectangle rectangle)
        {
            var bitmap = Parent.Parent.CurrentTilesBitmap;
            var tileSize = CurrentTileSet.TileSize;
            var tilesNoX = CurrentTileSet.TilesNoX;

            int left = rectangle.Left;
            int right = rectangle.Right;
            int top = rectangle.Top;
            int bottom = rectangle.Bottom;

            if (left < 0)
                left = 0;

            if (right > bitmap.Width)
                right = bitmap.Width;

            if (top < 0)
                top = 0;

            if (bottom > bitmap.Height)
                bottom = bitmap.Height;

            rectangle = new MyRectangle(left, top, right - left, bottom - top);

            List<int> tileIdList = new List<int>();
            int xFrom = rectangle.Left / tileSize;
            int xTo = rectangle.Right / tileSize;
            int yFrom = rectangle.Top / tileSize;
            int yTo = rectangle.Bottom / tileSize;

            for (int xIndex = xFrom; xIndex < xTo; xIndex++)
            {
                for (int yIndex = yFrom; yIndex < yTo; yIndex++)
                {
                    int gfxId = xIndex + tilesNoX * yIndex;
                    tileIdList.Add(gfxId);
                }
            }

            return tileIdList;
        }

        public void FinishSelection(MyPoint point)
        {
            SelectionRectangle.SetFinish(GetIndexCoords(point));

            var selectionRectangle = SelectionRectangle.GetRectangle(CurrentTileSet.TileSize);
            List<int> selectionList = GetTileIdList(selectionRectangle);

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
            return CurrentTileSet.GetIndexCoords(point);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentTileSetRef):
                    UpdateModel();
                    SelectedIndexes.Clear();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateModel()
        {
            ModelChangeAction?.Invoke(CurrentTileSetRef);
        }

        private void CalculateSelectionCenter()
        {
            var rectangle = CurrentTileSet.Tiles[SelectedIndexes[0]].Rectangle;

            MinCoord.X = rectangle.Left;
            MaxCoord.X = rectangle.Right;
            MinCoord.Y = rectangle.Bottom;
            MaxCoord.Y = rectangle.Top;

            for (int i = 1; i < SelectedIndexes.Count; i++)
            {
                rectangle = CurrentTileSet.Tiles[SelectedIndexes[i]].Rectangle;

                MinCoord.X = Math.Min(MinCoord.X, rectangle.Left);
                MaxCoord.X = Math.Max(MaxCoord.X, rectangle.Right);
                MinCoord.Y = Math.Min(MinCoord.Y, rectangle.Bottom);
                MaxCoord.Y = Math.Max(MaxCoord.Y, rectangle.Top);
            }

            CenterCoord = CurrentTileSet.GetSnapCoords(new MyPoint((MinCoord.X + MaxCoord.X) / 2, (MinCoord.Y + MaxCoord.Y) / 2));
        }

        #endregion Private Methods
    }
}
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Maps.Commands;
using OpenBreed.Editor.VM.Renderer;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public enum InsertModeEnum
    {
        Nothing,
        Point,
        Line,
        Draw
    }

    public class MapEditorTilesToolVM : MapEditorToolVM
    {
        #region Private Fields

        private MapEditorTileInsertOperation[,] InsertBuffer;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTilesToolVM(MapEditorVM parent, IWorkspaceMan workspaceMan)
        {
            Parent = parent;

            RefIdEditor = new EntryRefIdEditorVM(workspaceMan, typeof(IDbTileAtlas));

            TilesCursor = new List<MapEditorTileInsertOperation>();
            //Inserter = new MapEditorTilesInserter(Parent);


            TileSetSelector = new MapEditorTileSetSelectorVM(this);

            var mapViewRenderTarget = new RenderTarget(1, 1);
            var renderer = new TilesSelectorRenderer(this, mapViewRenderTarget);
            TilesSelector = new MapEditorTilesSelectorVM(this, renderer, mapViewRenderTarget);

            RefIdEditor.PropertyChanged += EntryRef_PropertyChanged;
        }

        internal void SetValue(Point tileCoords, int value)
        {
            var oldValue = Layout.GetCellValue(LayerIndex, tileCoords.X, tileCoords.Y);

            if (oldValue == value)
                return;

            Layout.SetCellValue(LayerIndex, tileCoords.X, tileCoords.Y, value);

            Parent.IsModified = true;
        }

        #endregion Public Constructors

        #region Public Properties

        public InsertModeEnum Mode { get; private set; }

        public MapLayoutModel Layout { get; private set; }
        public int LayerIndex { get; private set; }

        public MapEditorVM Parent { get; }
        public MapEditorTileSetSelectorVM TileSetSelector { get; }
        public MapEditorTilesSelectorVM TilesSelector { get; }
        public EntryRefIdEditorVM RefIdEditor { get; }

        private void EntryRef_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(RefIdEditor.RefId):
                    TilesSelector.CurrentTileSetRef = RefIdEditor.RefId;
                    break;

                default:
                    break;
            }
        }


        public List<MapEditorTileInsertOperation> TilesCursor { get; }

        #endregion Public Properties

        #region Public Methods

        public Func<List<TileSetModel>> UpdateVMAction { get; internal set; }

        #endregion Public Methods

        #region Internal Methods

        internal override void OnCursor(MapViewCursorVM cursor)
        {
            switch (cursor.Action)
            {
                case CursorActions.Hover:
                    break;
                case CursorActions.Leave:
                    break;
                case CursorActions.Move:
                    UpdateInsertion();
                    UpdateInserting();
                    break;
                case CursorActions.Click:
                    break;
                case CursorActions.Up:
                    if (Mode == InsertModeEnum.Nothing)
                        return;
                    if (cursor.Buttons.HasFlag(CursorButtons.Left))
                        FinishInserting();
                    break;
                case CursorActions.Down:
                    if (Mode != InsertModeEnum.Nothing)
                        return;
                    if (cursor.Buttons.HasFlag(CursorButtons.Left))
                        StartInserting(InsertModeEnum.Point);
                    break;
                default:
                    break;
            }
        }

        #endregion Internal Methods

        public void CommitInsertion()
        {
            List<MapEditorTileInsertOperation> tileInserts = new List<MapEditorTileInsertOperation>();

            for (int indexY = 0; indexY < InsertBuffer.GetLength(1); indexY++)
            {
                for (int indexX = 0; indexX < InsertBuffer.GetLength(0); indexX++)
                {
                    var tileReplacement = InsertBuffer[indexX, indexY];

                    if (tileReplacement == null)
                        continue;

                    tileInserts.Add(tileReplacement);
                    InsertBuffer[indexX, indexY] = null;
                }
            }

            if (tileInserts.Count > 0)
            {
                var cmd = new CmdTilesInsert(this, tileInserts);
                cmd.Execute();
                //Layer.Map.Commands.ExecuteCommand(new CmdTilesInsert(this, tileInserts));
            }
        }

        #region Private Methods

        internal void StartInserting(InsertModeEnum insertMode)
        {
            if (TilesSelector.IsEmpty)
                return;

            Mode = insertMode;

            if (Mode == InsertModeEnum.Point)
                InsertSelection();
        }

        internal void FinishInserting()
        {
            CommitInsertion();
            Mode = InsertModeEnum.Nothing;
        }

        private void UpdateInsertion()
        {
            TilesCursor.Clear();

            var indexCoords = Parent.MapView.Cursor.WorldIndexCoords;
            var selectionCenter = TilesSelector.GetIndexCoords(TilesSelector.CenterCoord);

            for (int i = 0; i < TilesSelector.SelectedIndexes.Count; i++)
            {
                int tileId = TilesSelector.SelectedIndexes[i];
                var rectangle = TilesSelector.CurrentTileSet.Tiles[tileId].Rectangle;
                int tileInsertIndexX = indexCoords.X + rectangle.X / rectangle.Width;
                int tileInsertIndexY = indexCoords.Y + rectangle.Y / rectangle.Height;
                tileInsertIndexX -= selectionCenter.X;
                tileInsertIndexY -= selectionCenter.Y;

                if (tileInsertIndexX < 0 || tileInsertIndexX >= Layout.Width)
                    continue;

                if (tileInsertIndexY < 0 || tileInsertIndexY >= Layout.Height)
                    continue;

                TilesCursor.Add(new MapEditorTileInsertOperation(new Point(tileInsertIndexX, tileInsertIndexY), 0, tileId));
            }
        }

        internal void UpdateInserting()
        {
            if (Mode == InsertModeEnum.Point)
            {
                InsertSelection();
            }
        }

        internal void UpdateVM()
        {   
            TileSetSelector.CurrentItem = TileSetSelector.TileSetNames.FirstOrDefault();

            Layout = Parent.Layout;
            LayerIndex = Layout.GetLayerIndex(MapLayerType.Gfx);
            InsertBuffer = new MapEditorTileInsertOperation[Layout.Width, Layout.Height];
        }

        public void DrawBuffer(RenderTarget renderTarget, int tileSize)
        {
            for (int indexY = 0; indexY < InsertBuffer.GetLength(1); indexY++)
            {
                for (int indexX = 0; indexX < InsertBuffer.GetLength(0); indexX++)
                {
                    var tileReplacement = InsertBuffer[indexX, indexY];

                    if (tileReplacement == null)
                        continue;

                    Parent.DrawTile(renderTarget, tileReplacement.TileIdAfter, indexX * tileSize, indexY * tileSize, tileSize);
                }
            }
        }

        private void InsertSelection()
        {
            var indexCoords = Parent.MapView.Cursor.WorldIndexCoords;
            var selectionCenter = TilesSelector.GetIndexCoords(TilesSelector.CenterCoord);

            for (int i = 0; i < TilesSelector.SelectedIndexes.Count; i++)
            {
                int tileId = TilesSelector.SelectedIndexes[i];
                var rectangle = TilesSelector.CurrentTileSet.Tiles[tileId].Rectangle;
                int tileInsertIndexX = indexCoords.X + rectangle.X / rectangle.Width;
                int tileInsertIndexY = indexCoords.Y + rectangle.Y / rectangle.Height;
                tileInsertIndexX -= selectionCenter.X;
                tileInsertIndexY -= selectionCenter.Y;

                if (tileInsertIndexX < 0 || tileInsertIndexX >= Layout.Width)
                    continue;

                if (tileInsertIndexY < 0 || tileInsertIndexY >= Layout.Height)
                    continue;

                var tileReplacement = InsertBuffer[tileInsertIndexX, tileInsertIndexY];
                if (tileReplacement == null)
                {
                    int oldTileId = Layout.GetCellValue(LayerIndex, tileInsertIndexX, tileInsertIndexY);

                    if (oldTileId != tileId)
                        InsertBuffer[tileInsertIndexX, tileInsertIndexY] = new MapEditorTileInsertOperation(new Point(tileInsertIndexX, tileInsertIndexY), oldTileId, tileId);
                }
                else
                {
                    if (tileReplacement.TileIdAfter != tileId)
                        InsertBuffer[tileInsertIndexX, tileInsertIndexY] = new MapEditorTileInsertOperation(new Point(tileInsertIndexX, tileInsertIndexY), tileReplacement.TileIdBefore, tileId);
                }
            }
        }

        #endregion Private Methods

    }
}

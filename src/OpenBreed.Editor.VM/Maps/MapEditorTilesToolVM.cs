using OpenBreed.Common;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Maps.Commands;
using OpenBreed.Editor.VM.Maps.Layers;
using OpenBreed.Editor.VM.Tiles;
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

        public MapEditorTilesToolVM(MapEditorVM parent)
        {
            Parent = parent;

            RefIdEditor = new EntryRefIdEditorVM(typeof(ITileSetEntry));

            TilesCursor = new List<MapEditorTileInsertOperation>();
            //Inserter = new MapEditorTilesInserter(Parent);
            TileSetSelector = new MapEditorTileSetSelectorVM(this);
            TilesSelector = new MapEditorTilesSelectorVM(this);

            RefIdEditor.PropertyChanged += EntryRef_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties



        public InsertModeEnum Mode { get; private set; }
        //public MapEditorTilesInserter Inserter { get; }
        public MapLayerGfxVM Layer { get; private set; }
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

                if (tileInsertIndexX < 0 || tileInsertIndexX >= Layer.Size.Width)
                    continue;

                if (tileInsertIndexY < 0 || tileInsertIndexY >= Layer.Size.Height)
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
            Layer = Parent.Layout.Layers.OfType<MapLayerGfxVM>().FirstOrDefault();
            InsertBuffer = new MapEditorTileInsertOperation[Layer.Size.Width, Layer.Size.Height];
        }

        public void DrawBuffer(Graphics gfx, int tileSize)
        {
            for (int indexY = 0; indexY < InsertBuffer.GetLength(1); indexY++)
            {
                for (int indexX = 0; indexX < InsertBuffer.GetLength(0); indexX++)
                {
                    var tileReplacement = InsertBuffer[indexX, indexY];

                    if (tileReplacement == null)
                        continue;

                    Parent.DrawTile(gfx, tileReplacement.TileIdAfter, indexX * tileSize, indexY * tileSize, tileSize);
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

                if (tileInsertIndexX < 0 || tileInsertIndexX >= Layer.Size.Width)
                    continue;

                if (tileInsertIndexY < 0 || tileInsertIndexY >= Layer.Size.Height)
                    continue;

                var tileReplacement = InsertBuffer[tileInsertIndexX, tileInsertIndexY];
                if (tileReplacement == null)
                {
                    int oldTileId = Layer.GetCell(tileInsertIndexX, tileInsertIndexY).TileId;

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

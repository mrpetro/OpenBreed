//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using OpenBreed.Editor.VM.Maps.Layers;
//using OpenBreed.Editor.VM.Maps.Commands;

//namespace OpenBreed.Editor.VM.Maps
//{
//    public enum InsertModeEnum
//    {
//        Nothing,
//        Point,
//        Line,
//        Draw
//    }

//    public class MapEditorTilesInserter
//    {

//        #region Private Fields

//        private MapEditorTileInsertOperation[,] _insertBuffer;

//        #endregion Private Fields

//        #region Public Constructors

//        public MapEditorTilesInserter(MapEditorVM editor)
//        {
//            Editor = editor;
//        }

//        public void Connect()
//        {
//            Selector = Editor.TilesTool.TilesSelector;
//        }



//        #endregion Public Constructors

//        #region Public Properties

//        public MapEditorVM Editor { get; }
//        public MapLayerGfxVM Layer { get; private set; }
//        public InsertModeEnum Mode { get; private set; }
//        public MapEditorTilesSelectorVM Selector { get; private set; }

//        #endregion Public Properties

//        #region Public Methods

//        public void CommitInsertion()
//        {
//            List<MapEditorTileInsertOperation> tileInserts = new List<MapEditorTileInsertOperation>();

//            for (int indexY = 0; indexY < _insertBuffer.GetLength(1); indexY++)
//            {
//                for (int indexX = 0; indexX < _insertBuffer.GetLength(0); indexX++)
//                {
//                    var tileReplacement = _insertBuffer[indexX, indexY];

//                    if (tileReplacement == null)
//                        continue;

//                    tileInserts.Add(tileReplacement);
//                    _insertBuffer[indexX, indexY] = null;
//                }
//            }

//            if (tileInserts.Count > 0)
//            {
//                var cmd = new CmdTilesInsert(this, tileInserts);
//                cmd.Execute();
//                //Layer.Map.Commands.ExecuteCommand(new CmdTilesInsert(this, tileInserts));
//            }
//        }

//        public void DrawInsertion(Graphics gfx, int tileSize)
//        {
//            var indexCoords = Editor.MapView.Cursor.WorldIndexCoords;

//            DrawBuffer(gfx, tileSize);

//            for (int i = 0; i < Selector.SelectedIndexes.Count; i++)
//            {
//                int tileId = Selector.SelectedIndexes[i];
//                var rectangle = Selector.CurrentTileSet.Items[tileId].Rectangle;
//                int tileInsertIndexX = indexCoords.X + rectangle.X / rectangle.Width;
//                int tileInsertIndexY = indexCoords.Y + rectangle.Y / rectangle.Height;

//                if (tileInsertIndexX < 0 || tileInsertIndexX >= Layer.Size.Width)
//                    continue;

//                if (tileInsertIndexY < 0 || tileInsertIndexY >= Layer.Size.Height)
//                    continue;

//                Selector.DrawTile(gfx, tileId, tileInsertIndexX * rectangle.Width, tileInsertIndexY * rectangle.Height, tileSize);
//            }
//        }

//        #endregion Public Methods

//        #region Internal Methods

//        internal void FinishInserting()
//        {
//            CommitInsertion();
//            Mode = InsertModeEnum.Nothing;
//        }

//        internal void StartInserting(InsertModeEnum insertMode)
//        {
//            if (Selector.IsEmpty)
//                return;

//            Mode = insertMode;

//            if (Mode == InsertModeEnum.Point)
//                InsertSelection();
//        }

//        internal void UpdateInserting()
//        {
//            if (Mode == InsertModeEnum.Point)
//            {
//                InsertSelection();
//            }
//        }

//        #endregion Internal Methods

//        #region Private Methods


//        private void DrawBuffer(Graphics gfx, int tileSize)
//        {
//            for (int indexY = 0; indexY < _insertBuffer.GetLength(1); indexY++)
//            {
//                for (int indexX = 0; indexX < _insertBuffer.GetLength(0); indexX++)
//                {
//                    var tileReplacement = _insertBuffer[indexX, indexY];

//                    if (tileReplacement == null)
//                        continue;

//                    //Selector.DrawTile(gfx, tileReplacement.TileIdAfter, indexX * tileSize, indexY * tileSize, tileSize);
//                }
//            }
//        }


//        #endregion Private Methods

//        //private void model_CurrentMapBodyChanged(object sender, CurrentMapBodyChangedEventArgs e)
//        //{
//        //    if (e.MapBody != null)
//        //        _insertBuffer = new TileInsertOperation[Layer.Size.Width, Layer.Size.Height];
//        //}

//    }
//}

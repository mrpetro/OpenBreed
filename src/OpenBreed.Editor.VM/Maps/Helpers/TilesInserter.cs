//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using OpenBreed.Editor.VM.Tiles.Helpers;
//using OpenBreed.Editor.VM.Levels.Commands;

//namespace OpenBreed.Editor.VM.Levels.Helpers
//{
//    public enum InsertModeEnum
//    {
//        Nothing,
//        Point,
//        Line,
//        Draw
//    }

//    public class TilesInserter
//    {
//        private readonly MapBodyVM m_Model;
//        private readonly TilesSelector m_TilesSource;
//        private TileInsertOperation[,] m_InsertBuffer;
//        private Point m_CursorIndexCoords;
//        private InsertModeEnum m_Mode;

//        public MapBodyVM Model { get { return m_Model; } }
//        public InsertModeEnum Mode { get { return m_Mode; } }

//        public TilesInserter(MapBodyVM model, TilesSelector tilesSource)
//        {
//            m_Model = model;
//            m_TilesSource = tilesSource;

//            model.CurrentMapBodyChanged += new CurrentMapBodyChangedEventHandler(model_CurrentMapBodyChanged);
//        }

//        void model_CurrentMapBodyChanged(object sender, CurrentMapBodyChangedEventArgs e)
//        {
//            if(e.MapBody != null)
//                m_InsertBuffer = new TileInsertOperation[m_Model.Size.Width, m_Model.Size.Height];
//        }

//        public void CommitInsertion()
//        {
//            List<TileInsertOperation> tileInserts = new List<TileInsertOperation>();

//            for (int indexY = 0; indexY < m_InsertBuffer.GetLength(1); indexY++)
//            {
//                for (int indexX = 0; indexX < m_InsertBuffer.GetLength(0); indexX++)
//                {
//                    var tileReplacement = m_InsertBuffer[indexX, indexY];

//                    if (tileReplacement == null)
//                        continue;

//                    tileInserts.Add(tileReplacement);
//                    m_InsertBuffer[indexX, indexY] = null;
//                }
//            }

//            if (tileInserts.Count > 0)
//                m_Model.Map.Commands.ExecuteCommand(new CmdTilesInsert(this, tileInserts));
//        }

//        public void DrawInsertion(Graphics gfx, int tileSize)
//        {
//            DrawBuffer(gfx, tileSize);

//            for (int i = 0; i < m_TilesSource.Selected.Count; i++)
//            {
//                int tileId = m_TilesSource.Selected[i];
//                Rectangle rectangle = m_TilesSource.VM.Items[tileId].Rectangle;
//                int tileInsertIndexX = m_CursorIndexCoords.X + rectangle.X / rectangle.Width;
//                int tileInsertIndexY = m_CursorIndexCoords.Y + rectangle.Y / rectangle.Height;

//                if (tileInsertIndexX < 0 || tileInsertIndexX >= m_Model.Size.Width)
//                    continue;

//                if (tileInsertIndexY < 0 || tileInsertIndexY >= m_Model.Size.Height)
//                    continue;

//                m_TilesSource.VM.DrawTile(gfx, tileId, tileInsertIndexX * rectangle.Width, tileInsertIndexY * rectangle.Height, tileSize);
//            }
//        }

//        private void DrawBuffer(Graphics gfx, int tileSize)
//        {
//            for (int indexY = 0; indexY < m_InsertBuffer.GetLength(1); indexY++)
//            {
//                for (int indexX = 0; indexX < m_InsertBuffer.GetLength(0); indexX++)
//                {
//                    var tileReplacement = m_InsertBuffer[indexX, indexY];

//                    if (tileReplacement == null)
//                        continue;

//                    m_TilesSource.VM.DrawTile(gfx, tileReplacement.TileIdAfter, indexX * tileSize, indexY * tileSize, tileSize);
//                }
//            }
//        }

//        internal void StartInserting(InsertModeEnum insertMode)
//        {
//            if (m_TilesSource.IsEmpty)
//                return;

//            m_Mode = insertMode;

//            if (m_Mode == InsertModeEnum.Point)
//                InsertSelection();
//        }

//        internal void UpdateInserting()
//        {
//            if (m_Mode == InsertModeEnum.Point)
//            {
//                InsertSelection();
//            }
//        }

//        internal void FinishInserting()
//        {
//            CommitInsertion();
//            m_Mode = InsertModeEnum.Nothing;
//        }

//        internal bool UpdateCursor(Point point)
//        {
//            Point oldCursorCoords = m_CursorIndexCoords;

//            m_CursorIndexCoords = ToMapIndexCoords(point);

//            return oldCursorCoords != m_CursorIndexCoords;
//        }

//        private void InsertSelection()
//        {
//            for (int i = 0; i < m_TilesSource.Selected.Count; i++)
//            {
//                int tileId = m_TilesSource.Selected[i];
//                Rectangle rectangle = m_TilesSource.VM.Items[tileId].Rectangle;
//                Point indexCoords = m_CursorIndexCoords;
//                indexCoords.X += rectangle.X / rectangle.Width;
//                indexCoords.Y += rectangle.Y / rectangle.Height;

//                if (indexCoords.X < 0 || indexCoords.X >= m_Model.Size.Width)
//                    continue;

//                if (indexCoords.Y < 0 || indexCoords.Y >= m_Model.Size.Height)
//                    continue;


//                var tileReplacement = m_InsertBuffer[indexCoords.X, indexCoords.Y];
//                if (tileReplacement == null)
//                {
//                    //int oldTileId = m_Model.GetCell(indexCoords.X, indexCoords.Y).GfxId;

//                    //if (oldTileId != tileId)
//                    //    m_InsertBuffer[indexCoords.X, indexCoords.Y] = new TileInsertOperation(indexCoords, oldTileId, tileId);
//                }
//                else
//                {
//                    if (tileReplacement.TileIdAfter != tileId)
//                        m_InsertBuffer[indexCoords.X, indexCoords.Y] = new TileInsertOperation(indexCoords, tileReplacement.TileIdBefore, tileId);
//                }
//            }
//        }

//        private Point ToMapIndexCoords(Point point)
//        {
//            point = m_Model.GetWorldCoords(point);
//            point = m_TilesSource.VM.GetSnapCoords(point);
//            point.Offset(-m_TilesSource.CenterCoord.X, -m_TilesSource.CenterCoord.Y);
//            return m_TilesSource.VM.GetIndexCoords(point);
//        }

//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using OpenBreed.Editor.VM.Levels.Commands;

//namespace OpenBreed.Editor.VM.Levels.Helpers
//{
//    public class PropertyInserter
//    {
//        private readonly MapBodyVM m_Model;
//        private PropertyInsertOperation[,] m_InsertBuffer;
//        private Point m_CursorIndexCoords;
//        private InsertModeEnum m_Mode;

//        public MapBodyVM Model { get { return m_Model; } }
//        public InsertModeEnum Mode { get { return m_Mode; } }


//        public PropertyInserter(MapBodyVM model)
//        {
//            m_Model = model;

//            model.CurrentMapBodyChanged += new CurrentMapBodyChangedEventHandler(model_CurrentMapBodyChanged);
//        }

//        void model_CurrentMapBodyChanged(object sender, CurrentMapBodyChangedEventArgs e)
//        {
//            if(e.MapBody != null)
//                m_InsertBuffer = new PropertyInsertOperation[m_Model.Size.Width, m_Model.Size.Height];
//        }

//        public void CommitInsertion()
//        {
//            List<PropertyInsertOperation> propertyInserts = new List<PropertyInsertOperation>();

//            for (int indexY = 0; indexY < m_InsertBuffer.GetLength(1); indexY++)
//            {
//                for (int indexX = 0; indexX < m_InsertBuffer.GetLength(0); indexX++)
//                {
//                    var tileReplacement = m_InsertBuffer[indexX, indexY];

//                    if (tileReplacement == null)
//                        continue;

//                    propertyInserts.Add(tileReplacement);
//                    m_InsertBuffer[indexX, indexY] = null;
//                }
//            }

//            if (propertyInserts.Count > 0)
//                m_Model.Map.Commands.ExecuteCommand(new CmdPropertiesInsert(this, propertyInserts));
//        }

//        public void DrawInsertion(Graphics gfx, int tileSize)
//        {
//            DrawBuffer(gfx, tileSize);

//            int tileInsertIndexX = m_CursorIndexCoords.X;
//            int tileInsertIndexY = m_CursorIndexCoords.Y;

//            if (tileInsertIndexX < 0 || tileInsertIndexX >= m_Model.Size.Width)
//                return;

//            if (tileInsertIndexY < 0 || tileInsertIndexY >= m_Model.Size.Height)
//                return;

//            //m_Source.Model.DrawProperty(gfx, propertyId, tileInsertIndexX * tileSize, tileInsertIndexY * tileSize, tileSize);
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

//                    //m_Source.Model.DrawProperty(gfx, tileReplacement.PropertyIdAfter, indexX * tileSize, indexY * tileSize, tileSize);
//                }
//            }
//        }

//        internal void StartInserting(InsertModeEnum insertMode)
//        {
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
//            //int propertyId = m_Source.Selected;
//            //Point indexCoords = m_CursorIndexCoords;

//            //if (indexCoords.X < 0 || indexCoords.X >= m_Model.Size.Width)
//            //    return;

//            //if (indexCoords.Y < 0 || indexCoords.Y >= m_Model.Size.Height)
//            //    return;


//            //var tileReplacement = m_InsertBuffer[indexCoords.X, indexCoords.Y];
//            //if (tileReplacement == null)
//            //{
//            //    //int oldPropertyId = m_Model.GetCell(indexCoords.X, indexCoords.Y).PropertyId;
//            //    //if (oldPropertyId != propertyId)
//            //    //    m_InsertBuffer[indexCoords.X, indexCoords.Y] = new PropertyInsertOperation(indexCoords, oldPropertyId, propertyId);
//            //}
//            //else
//            //{
//            //    if (tileReplacement.PropertyIdAfter != propertyId)
//            //        m_InsertBuffer[indexCoords.X, indexCoords.Y] = new PropertyInsertOperation(indexCoords, tileReplacement.PropertyIdBefore, propertyId);
//            //}
//        }

//        private Point ToMapIndexCoords(Point point)
//        {
//            point = m_Model.GetWorldCoords(point);
//            return m_Model.GetIndexCoords(point);
//        }

//    }
//}

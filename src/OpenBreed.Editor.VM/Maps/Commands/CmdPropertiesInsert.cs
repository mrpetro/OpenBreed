//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using OpenBreed.Editor.VM.Levels.Helpers;
//using OpenBreed.Common.Commands;

//namespace OpenBreed.Editor.VM.Levels.Commands
//{
//    public class CmdPropertiesInsert : ICommand
//    {
//        private readonly PropertyInserter m_Inserter;
//        private readonly List<PropertyInsertOperation> m_Operations;

//        public CmdPropertiesInsert(PropertyInserter inserter, List<PropertyInsertOperation> operations)
//        {
//            m_Inserter = inserter;
//            m_Operations = operations;
//        }

//        public void Execute()
//        {
//            for (int i = 0; i < m_Operations.Count; i++)
//            {
//                Point tileCoords = m_Operations[i].IndexCoords;
//                int propertyId = m_Operations[i].PropertyIdAfter;

//                m_Inserter.Model.SetTileProperty(tileCoords.X, tileCoords.Y, propertyId);
//            }

//            m_Inserter.Model.Update();
//        }

//        public void UnExecute()
//        {
//            for (int i = 0; i < m_Operations.Count; i++)
//            {
//                Point tileCoords = m_Operations[i].IndexCoords;
//                int propertyId = m_Operations[i].PropertyIdBefore;

//                m_Inserter.Model.SetTileProperty(tileCoords.X, tileCoords.Y, propertyId);
//            }

//            m_Inserter.Model.Update();
//        }
//    }
//}

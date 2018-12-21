//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using OpenBreed.Editor.VM.Levels.Helpers;
//using OpenBreed.Common.Commands;

//namespace OpenBreed.Editor.VM.Levels.Commands
//{
//    public class CmdTilesInsert : ICommand
//    {
//        private readonly TilesInserter m_Inserter;
//        private readonly List<TileInsertOperation> m_Operations;

//        public CmdTilesInsert(TilesInserter inserter, List<TileInsertOperation> operations)
//        {
//            m_Inserter = inserter;
//            m_Operations = operations;
//        }

//        public void Execute()
//        {
//            for (int i = 0; i < m_Operations.Count; i++)
//            {
//                Point tileCoords = m_Operations[i].IndexCoords;
//                int tileId = m_Operations[i].TileIdAfter;

//                m_Inserter.Model.SetTileGfx(tileCoords.X, tileCoords.Y, tileId);
//            }

//            m_Inserter.Model.Update();
//        }

//        public void UnExecute()
//        {
//            for (int i = 0; i < m_Operations.Count; i++)
//            {
//                Point tileCoords = m_Operations[i].IndexCoords;
//                int tileId = m_Operations[i].TileIdBefore;

//                m_Inserter.Model.SetTileGfx(tileCoords.X, tileCoords.Y, tileId);
//            }

//            m_Inserter.Model.Update();
//        }
//    }
//}

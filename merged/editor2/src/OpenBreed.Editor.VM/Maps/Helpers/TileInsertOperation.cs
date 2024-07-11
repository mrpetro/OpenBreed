using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenBreed.Editor.VM.Maps.Helpers
{
    public class TileInsertOperation
    {
        private readonly Point m_IndexCoords;
        private readonly int m_TileIdBefore;
        private readonly int m_TileIdAfter;

        public Point IndexCoords { get { return m_IndexCoords; } }
        public int TileIdBefore { get { return m_TileIdBefore; } }
        public int TileIdAfter { get { return m_TileIdAfter; } }

        public TileInsertOperation(Point indexCoords, int tileIdBefore, int tileIdAfter)
        {
            m_IndexCoords = indexCoords;
            m_TileIdBefore = tileIdBefore;
            m_TileIdAfter = tileIdAfter;
        }
    }
}

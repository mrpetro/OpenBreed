using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenBreed.Editor.VM.Maps.Helpers
{
    public class PropertyInsertOperation
    {
        private readonly Point m_IndexCoords;
        private readonly int m_PropertyIdBefore;
        private readonly int m_PropertyIdAfter;

        public Point IndexCoords { get { return m_IndexCoords; } }
        public int PropertyIdBefore { get { return m_PropertyIdBefore; } }
        public int PropertyIdAfter { get { return m_PropertyIdAfter; } }

        public PropertyInsertOperation(Point indexCoords, int propertyBefore, int propertyIdAfter)
        {
            m_IndexCoords = indexCoords;
            m_PropertyIdBefore = propertyBefore;
            m_PropertyIdAfter = propertyIdAfter;
        }
    }
}

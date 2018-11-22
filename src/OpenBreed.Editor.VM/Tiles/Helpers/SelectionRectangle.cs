using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenBreed.Editor.VM.Tiles.Helpers
{
    public class SelectionRectangle
    {
        private Point m_SelectStartPos;
        private Point m_SelectEndPos;

        public SelectionRectangle()
        {
        }

        public void SetStart(Point coordinates)
        {
            m_SelectStartPos = coordinates;
            m_SelectEndPos = m_SelectStartPos;
        }

        public void Update(Point coordinates)
        {
            m_SelectEndPos = coordinates;
        }

        public void SetFinish(Point coordinates)
        {
            m_SelectEndPos = coordinates;
        }

        public Rectangle GetRectangle(int size)
        {
            int startX = m_SelectStartPos.X;
            int startY = m_SelectStartPos.Y;
            int endX = m_SelectEndPos.X;
            int endY = m_SelectEndPos.Y;

            if (m_SelectStartPos.X <= m_SelectEndPos.X)
                endX++;
            else
                startX++;

            if (m_SelectStartPos.Y <= m_SelectEndPos.Y)
                endY++;
            else
                startY++;

            int xFrom = Math.Min(startX, endX);
            int xTo = Math.Max(startX, endX);
            int yFrom = Math.Min(startY, endY);
            int yTo = Math.Max(startY, endY);

            return new Rectangle(xFrom * size, yFrom * size, (xTo - xFrom) * size, (yTo - yFrom) * size);
        }
    }
}

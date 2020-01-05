using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpenBreed.Editor.VM.Common
{
    public class SelectionRectangle
    {
        private Point selectStartPos;
        private Point selectEndPos;

        public SelectionRectangle()
        {
        }

        public void SetStart(Point coordinates)
        {
            selectStartPos = coordinates;
            selectEndPos = selectStartPos;
        }

        public void Update(Point coordinates)
        {
            selectEndPos = coordinates;
        }

        public void SetFinish(Point coordinates)
        {
            selectEndPos = coordinates;
        }

        public Rectangle GetRectangle()
        {
            int startX = selectStartPos.X;
            int startY = selectStartPos.Y;
            int endX = selectEndPos.X;
            int endY = selectEndPos.Y;

            int xFrom = Math.Min(startX, endX);
            int xTo = Math.Max(startX, endX);
            int yFrom = Math.Min(startY, endY);
            int yTo = Math.Max(startY, endY);

            return new Rectangle(xFrom, yFrom, xTo - xFrom, yTo - yFrom);

        }

        public Rectangle GetRectangle(int size)
        {
            int startX = selectStartPos.X;
            int startY = selectStartPos.Y;
            int endX = selectEndPos.X;
            int endY = selectEndPos.Y;

            if (selectStartPos.X <= selectEndPos.X)
                endX++;
            else
                startX++;

            if (selectStartPos.Y <= selectEndPos.Y)
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

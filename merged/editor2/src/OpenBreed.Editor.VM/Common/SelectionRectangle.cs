using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Common.Interface.Drawing;

namespace OpenBreed.Editor.VM.Common
{
    public class SelectionRectangle
    {
        private MyPoint selectStartPos;
        private MyPoint selectEndPos;

        public SelectionRectangle()
        {
        }

        public void SetStart(MyPoint coordinates)
        {
            selectStartPos = coordinates;
            selectEndPos = selectStartPos;
        }

        public void Update(MyPoint coordinates)
        {
            selectEndPos = coordinates;
        }

        public void SetFinish(MyPoint coordinates)
        {
            selectEndPos = coordinates;
        }

        public MyRectangle GetRectangle()
        {
            int startX = selectStartPos.X;
            int startY = selectStartPos.Y;
            int endX = selectEndPos.X;
            int endY = selectEndPos.Y;

            int xFrom = Math.Min(startX, endX);
            int xTo = Math.Max(startX, endX);
            int yFrom = Math.Min(startY, endY);
            int yTo = Math.Max(startY, endY);

            return new MyRectangle(xFrom, yFrom, xTo - xFrom, yTo - yFrom);

        }

        public MyRectangle GetRectangle(int size)
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

            return new MyRectangle(xFrom * size, yFrom * size, (xTo - xFrom) * size, (yTo - yFrom) * size);
        }
    }
}

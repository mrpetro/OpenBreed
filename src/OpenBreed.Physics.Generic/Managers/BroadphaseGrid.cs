using OpenBreed.Physics.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class BroadphaseGrid
    {
        #region Private Fields

        private readonly BroadphaseCell[] cells;

        #endregion Private Fields

        #region Internal Constructors

        internal BroadphaseGrid(int width, int height, int cellSize)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;

            cells = CreateArray();
        }

        #endregion Internal Constructors

        #region Public Properties

        public int CellSize { get; }
        public int Height { get; }
        public int Width { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void InsertItem(BroadphaseItem item)
        {
            int leftIndex;
            int rightIndex;
            int bottomIndex;
            int topIndex;

            GetAabbIndices(item.Aabb, out leftIndex, out rightIndex, out bottomIndex, out topIndex);
            InsertToCells(leftIndex, rightIndex, bottomIndex, topIndex, item.ItemId);
        }

        internal HashSet<int> QueryStatic(Box2 aabb)
        {
            int leftIndex;
            int rightIndex;
            int bottomIndex;
            int topIndex;

            GetAabbIndices(aabb, out leftIndex, out rightIndex, out bottomIndex, out topIndex);

            //Collect all unique aabb boxes
            var result = new HashSet<int>();
            for (int yIndex = bottomIndex; yIndex < topIndex; yIndex++)
            {
                for (int xIndex = leftIndex; xIndex < rightIndex; xIndex++)
                {
                    cells[xIndex + Width * yIndex].InsertTo(result);
                }
            }

            return result;
        }

        internal void RemoveItem(BroadphaseItem item)
        {
            int leftIndex;
            int rightIndex;
            int bottomIndex;
            int topIndex;

            GetAabbIndices(item.Aabb, out leftIndex, out rightIndex, out bottomIndex, out topIndex);
            RemoveFromCells(leftIndex, rightIndex, bottomIndex, topIndex, item.ItemId);
        }

        #endregion Internal Methods

        #region Private Methods

        private BroadphaseCell[] CreateArray()
        {
            var cells = new BroadphaseCell[Width * Height];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = new BroadphaseCell();

            return cells;
        }

        private void GetAabbIndices(Box2 aabb, out int left, out int right, out int bottom, out int top)
        {
            int xMod = (int)aabb.Max.X % CellSize;
            int yMod = (int)aabb.Max.Y % CellSize;

            left = (int)aabb.Min.X >> 4;
            right = (int)aabb.Max.X >> 4;
            bottom = (int)aabb.Min.Y >> 4;
            top = (int)aabb.Max.Y >> 4;

            if (xMod > 0)
                right++;

            if (yMod > 0)
                top++;

            if (left < 0)
                left = 0;

            if (bottom < 0)
                bottom = 0;

            if (right > Width - 1)
                right = Width - 1;

            if (top > Height - 1)
                top = Height - 1;
        }

        private void InsertToCells(int leftIndex, int rightIndex, int bottomIndex, int topIndex, int itemId)
        {
            for (int j = bottomIndex; j < topIndex; j++)
            {
                var gridIndex = Width * j + leftIndex;
                for (int i = leftIndex; i < rightIndex; i++)
                {
                    cells[gridIndex].AddItem(itemId);
                    gridIndex++;
                }
            }
        }

        private void RemoveFromCells(int leftIndex, int rightIndex, int bottomIndex, int topIndex, int itemId)
        {
            for (int j = bottomIndex; j < topIndex; j++)
            {
                var gridIndex = Width * j + leftIndex;
                for (int i = leftIndex; i < rightIndex; i++)
                {
                    cells[gridIndex].RemoveItem(itemId);
                    gridIndex++;
                }
            }
        }

        #endregion Private Methods
    }
}
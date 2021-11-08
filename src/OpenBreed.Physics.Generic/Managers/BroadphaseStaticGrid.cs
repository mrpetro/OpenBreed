using OpenBreed.Physics.Interface;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class BroadphaseCell
    {
        #region Private Fields

        private readonly List<int> items = new List<int>();

        #endregion Private Fields

        #region Internal Methods

        internal void AddItem(int itemId)
        {
            items.Add(itemId);
        }

        internal void RemoveItem(int itemId)
        {
            items.Remove(itemId);
        }

        internal void InsertTo(HashSet<int> result)
        {
            foreach (var item in items)
                result.Add(item);
        }

        #endregion Internal Methods
    }

    internal class BroadphaseStaticGrid : IBroadphaseStatic
    {
        #region Private Fields

        private readonly BroadphaseCell[] cells;
        private readonly Dictionary<int, Box2> items = new Dictionary<int, Box2>();

        #endregion Private Fields

        #region Public Constructors

        public BroadphaseStaticGrid(int width, int height, int cellSize)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;

            cells = CreateArray();
        }

        #endregion Public Constructors

        #region Public Properties

        public int Width { get; }

        public int Height { get; }

        public int CellSize { get; }

        #endregion Public Properties

        #region Public Methods

        public Vector2 GetCellCenter(int xIndex, int yIndex)
        {
            return new Vector2(xIndex * CellSize + CellSize / 2, yIndex * CellSize + CellSize / 2);
        }

        public Box2 GetAabb(int itemId)
        {
            if (!items.TryGetValue(itemId, out Box2 aabb))
                throw new InvalidOperationException($"Item with ID '{itemId}' was not inserted.");

            return aabb;
        }

        public void InsertItem(int itemId, Box2 aabb)
        {
            if (items.ContainsKey(itemId))
                throw new InvalidOperationException($"Item with ID '{itemId}' was already inserted.");

            int leftIndex;
            int rightIndex;
            int bottomIndex;
            int topIndex;

            GetAabbIndices(aabb, out leftIndex, out rightIndex, out bottomIndex, out topIndex);
            InsertItemToGrid(leftIndex, rightIndex, bottomIndex, topIndex, itemId);

            items.Add(itemId, aabb);
        }

        public bool ContainsItem(int itemId) => items.ContainsKey(itemId);

        public void RemoveStatic(int itemId)
        {
            int leftIndex;
            int rightIndex;
            int bottomIndex;
            int topIndex;

            if (!items.TryGetValue(itemId, out Box2 aabb))
                throw new InvalidOperationException($"Item with ID '{itemId}' was not inserted.");

            GetAabbIndices(aabb, out leftIndex, out rightIndex, out bottomIndex, out topIndex);
            RemoveItemFromGrid(leftIndex, rightIndex, bottomIndex, topIndex, itemId);

            items.Remove(itemId);
        }

        public HashSet<int> QueryStatic(Box2 aabb)
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

        #endregion Public Methods

        #region Private Methods

        private void InsertItemToGrid(int leftIndex, int rightIndex, int bottomIndex, int topIndex, int itemId)
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

        private void RemoveItemFromGrid(int leftIndex, int rightIndex, int bottomIndex, int topIndex, int itemId)
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

        private BroadphaseCell[] CreateArray()
        {
            var cells = new BroadphaseCell[Width * Height];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = new BroadphaseCell();

            return cells;
        }

        private void GetAabbIndices(Box2 aabb, out int left, out int right, out int bottom, out int top)
        {
            int xMod = (int)aabb.Right % CellSize;
            int yMod = (int)aabb.Top % CellSize;

            left = (int)aabb.Left >> 4;
            right = (int)aabb.Right >> 4;
            bottom = (int)aabb.Bottom >> 4;
            top = (int)aabb.Top >> 4;

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

        #endregion Private Methods
    }
}
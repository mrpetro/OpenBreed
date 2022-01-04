using OpenBreed.Physics.Interface;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Physics.Generic.Managers
{
    //internal class BroadphaseCell
    //{
    //    #region Private Fields

    //    private readonly List<int> items = new List<int>();

    //    #endregion Private Fields

    //    #region Internal Methods

    //    internal void AddItem(int itemId)
    //    {
    //        items.Add(itemId);
    //    }

    //    internal void RemoveItem(int itemId)
    //    {
    //        items.Remove(itemId);
    //    }

    //    internal void InsertTo(HashSet<int> result)
    //    {
    //        foreach (var item in items)
    //            result.Add(item);
    //    }

    //    #endregion Internal Methods
    //}

    internal class BroadphaseDynamicSwepAndPrune : IBroadphaseDynamic
    {
        #region Private Fields

        private readonly List<BroadphaseDynamicElement> cells = new List<BroadphaseDynamicElement>();
        private readonly Dictionary<int, BroadphaseDynamicElement> items = new Dictionary<int, BroadphaseDynamicElement>();

        #endregion Private Fields

        #region Public Constructors

        public BroadphaseDynamicSwepAndPrune()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public Box2 GetAabb(int itemId)
        {
            if (!items.TryGetValue(itemId, out BroadphaseDynamicElement cell))
                throw new InvalidOperationException($"Item with ID '{itemId}' was not inserted.");

            return cell.Aabb;
        }

        public bool ContainsItem(int itemId) => items.ContainsKey(itemId);

        public void InsertItem(int itemId, Box2 aabb)
        {
            if (items.ContainsKey(itemId))
                throw new InvalidOperationException($"Item with ID '{itemId}' was already inserted.");

            var newCell = new BroadphaseDynamicElement(itemId, aabb);

            cells.Add(newCell);
            items.Add(itemId, newCell);
        }

        public void UpdateItem(int itemId, Box2 aabb)
        {
            if (!items.TryGetValue(itemId, out BroadphaseDynamicElement cell))
                throw new InvalidOperationException($"Item with ID '{itemId}' was not inserted.");

            cell.Aabb = aabb;
        }

        public void RemoveItem(int itemId)
        {
            if (!items.TryGetValue(itemId, out BroadphaseDynamicElement cell))
                throw new InvalidOperationException($"Item with ID '{itemId}' was not inserted.");

            cells.Remove(cell);
            items.Remove(itemId);
        }

        public void Solve(Action<BroadphaseDynamicElement, float> staticPhase, Action<BroadphaseDynamicElement, BroadphaseDynamicElement, float> narrowPhase, float dt)
        {
            var xActiveList = new List<BroadphaseDynamicElement>();
            BroadphaseDynamicElement nextCollider;

            cells.Sort(Xcomparison);

            for (int i = 0; i < cells.Count - 1; i++)
            {
                var activeDynamic = cells[i];

                staticPhase(activeDynamic, dt);

                nextCollider = cells[i + 1];

                xActiveList.Add(cells[i]);

                for (int j = 0; j < xActiveList.Count; j++)
                {
                    var currentCollider = xActiveList[j];

                    if (nextCollider.Aabb.Min.X < currentCollider.Aabb.Max.X)
                    {
                        if (nextCollider.Aabb.Min.Y <= currentCollider.Aabb.Max.Y && nextCollider.Aabb.Max.Y > currentCollider.Aabb.Min.Y)
                        {
                            narrowPhase(nextCollider, currentCollider, dt);
                        }
                    }
                    else
                    {
                        xActiveList.RemoveAt(j);
                        j--;
                    }
                }
            }

            if (cells.Count > 0)
            {
                staticPhase(cells.Last(), dt);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private int Xcomparison(BroadphaseDynamicElement x, BroadphaseDynamicElement y)
        {
            var xAabb = x.Aabb;
            var yAabb = y.Aabb;

            if (xAabb.Min.X < yAabb.Min.X)
                return -1;
            if (xAabb.Min.X == yAabb.Min.X)
                return 0;
            else
                return 1;
        }

        #endregion Private Methods
    }
}
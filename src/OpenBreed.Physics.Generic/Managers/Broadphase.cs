using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class Broadphase : IBroadphase
    {
        #region Private Fields

        private readonly BroadphaseGrid grid;
        private readonly List<BroadphaseItem> dynamicItems = new List<BroadphaseItem>();
        private readonly Dictionary<int, BroadphaseItem> items = new Dictionary<int, BroadphaseItem>();

        #endregion Private Fields

        #region Public Constructors

        public Broadphase(int width, int height, int cellSize)
        {
            grid = new BroadphaseGrid(width, height, cellSize);
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IBroadphaseItem> DynamicItems => dynamicItems;


        #endregion Public Properties

        #region Public Methods

        public bool ContainsItem(int itemId) => items.ContainsKey(itemId);

        public Box2 GetAabb(int itemId)
        {
            if (!items.TryGetValue(itemId, out BroadphaseItem cell))
                throw new InvalidOperationException($"Item with ID '{itemId}' was not inserted.");

            return cell.Aabb;
        }

        public void InsertItem(int itemId, Box2 aabb, BroadphaseItemType type)
        {
            if (items.ContainsKey(itemId))
                throw new InvalidOperationException($"Item with ID '{itemId}' was already inserted.");

            var item = new BroadphaseItem(itemId, aabb, type);

            items.Add(itemId, item);

            switch (type)
            {
                case BroadphaseItemType.Static:
                    InsertStaticItem(item);
                    break;

                case BroadphaseItemType.Dynamic:
                    InsertDynamicItem(item);
                    break;

                default:
                    throw new NotImplementedException($"{typeof(BroadphaseItemType)} '{type}' not implemented.");
            }
        }

        public HashSet<int> QueryStatic(Box2 aabb) => grid.QueryStatic(aabb);

        public void RemoveItem(int itemId)
        {
            if (!items.TryGetValue(itemId, out BroadphaseItem item))
                throw new InvalidOperationException($"Item with ID '{itemId}' was not inserted.");

            switch (item.Type)
            {
                case BroadphaseItemType.Static:
                    RemoveStaticItem(item);
                    break;

                case BroadphaseItemType.Dynamic:
                    RemoveDynamicItem(item);
                    break;

                default:
                    throw new NotImplementedException($"{typeof(BroadphaseItemType)} '{item.Type}' not implemented.");
            }

            items.Remove(itemId);
        }

        public void Solve(
            Action<IBroadphase, BroadphaseItem, BroadphaseResult, float> staticPhase,
            Action<BroadphaseItem, BroadphaseItem, BroadphaseResult, float> narrowPhase,
            BroadphaseResult result,
            float dt)
        {
            var xActiveList = new List<BroadphaseItem>();
            BroadphaseItem nextCollider;

            dynamicItems.Sort(Xcomparison);

            var prevCount = 0;

            for (int i = 0; i < dynamicItems.Count - 1; i++)
            {
                var activeDynamic = dynamicItems[i];
                prevCount = dynamicItems.Count;

                staticPhase(this, activeDynamic, result, dt);

                nextCollider = dynamicItems[i + 1];

                xActiveList.Add(dynamicItems[i]);

                for (int j = 0; j < xActiveList.Count; j++)
                {
                    var currentCollider = xActiveList[j];

                    if (nextCollider.Aabb.Min.X < currentCollider.Aabb.Max.X)
                    {
                        if (nextCollider.Aabb.Min.Y <= currentCollider.Aabb.Max.Y && nextCollider.Aabb.Max.Y > currentCollider.Aabb.Min.Y)
                        {
                            narrowPhase(nextCollider, currentCollider, result, dt);
                        }
                    }
                    else
                    {
                        xActiveList.RemoveAt(j);
                        j--;
                    }
                }
            }

            if (dynamicItems.Count > 0)
            {
                staticPhase(this, dynamicItems.Last(), result, dt);
            }
        }

        public void UpdateItem(int itemId, Box2 aabb)
        {
            if (!items.TryGetValue(itemId, out BroadphaseItem item))
                throw new InvalidOperationException($"Item with ID '{itemId}' was not inserted.");

            if (item.Type != BroadphaseItemType.Dynamic)
            {
                throw new InvalidOperationException("Only dynamic items can be updated.");
            }

            item.Aabb = aabb;
        }

        public void UpdateItems(UpdateDynamicDelegate updater)
        {
            foreach (var item in dynamicItems)
            {
                item.Aabb = updater.Invoke(item.ItemId);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void InsertDynamicItem(BroadphaseItem item)
        {
            dynamicItems.Add(item);
        }

        private void RemoveDynamicItem(BroadphaseItem item)
        {
            dynamicItems.Remove(item);
        }

        private void InsertStaticItem(BroadphaseItem item)
        {
            grid.InsertItem(item);
        }

        private void RemoveStaticItem(BroadphaseItem item)
        {
            grid.RemoveItem(item);
        }

        private int Xcomparison(BroadphaseItem x, BroadphaseItem y)
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
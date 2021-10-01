using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Physics.Interface
{
    public class BroadphaseDynamicCell
    {
        public BroadphaseDynamicCell(int itemId, Box2 aabb)
        {
            ItemId = itemId;
            Aabb = aabb;
        }

        public int ItemId { get; }
        public Box2 Aabb { get; set; }
    }


    /// <summary>
    /// Interface for dynamic collision detection
    /// </summary>
    public interface IBroadphaseDynamic
    {
        #region Public Methods

        void InsertItem(int itemId, Box2 aabb);

        void UpdateItem(int itemId, Box2 aabb);

        void RemoveItem(int itemId);

        Box2 GetAabb(int itemId);


        void Solve(Action<BroadphaseDynamicCell, float> staticPhase, Action<BroadphaseDynamicCell, BroadphaseDynamicCell, float> narrowPhase, float dt);

        #endregion Public Methods
    }
}
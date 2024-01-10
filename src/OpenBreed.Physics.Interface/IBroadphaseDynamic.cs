using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Physics.Interface
{
    public class BroadphaseDynamicElement
    {
        public BroadphaseDynamicElement(int itemId, Box2 aabb)
        {
            ItemId = itemId;
            Aabb = aabb;
        }

        public int ItemId { get; }
        public Box2 Aabb { get; set; }
    }


    public delegate Box2 UpdateDynamicDelegate(int itemId);

    /// <summary>
    /// Interface for dynamic collision detection
    /// </summary>
    public interface IBroadphaseDynamic
    {
        #region Public Methods

        void InsertItem(int itemId, Box2 aabb);

        void UpdateItem(int itemId, Box2 aabb);

        void UpdateItems(UpdateDynamicDelegate updater);

        void RemoveItem(int itemId);

        Box2 GetAabb(int itemId);

        void Solve(Action<BroadphaseDynamicElement, float> staticPhase, Action<BroadphaseDynamicElement, BroadphaseDynamicElement, float> narrowPhase, float dt);
 
        bool ContainsItem(int itemId);

        #endregion Public Methods
    }
}
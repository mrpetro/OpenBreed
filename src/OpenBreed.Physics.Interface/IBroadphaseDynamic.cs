using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Physics.Interface
{
    public delegate Box2 UpdateDynamicDelegate(int itemId);

    /// <summary>
    /// Interface for dynamic collision detection
    /// </summary>
    public interface IBroadphaseDynamic
    {
        #region Public Properties

        IEnumerable<IBroadphaseDynamicElement> Items { get; }

        #endregion Public Properties

        #region Public Methods

        bool ContainsItem(int itemId);

        Box2 GetAabb(int itemId);

        void InsertItem(int itemId, Box2 aabb);

        void RemoveItem(int itemId);

        void Solve(Action<BroadphaseDynamicElement, float> staticPhase, Action<BroadphaseDynamicElement, BroadphaseDynamicElement, float> narrowPhase, float dt);

        void UpdateItem(int itemId, Box2 aabb);

        void UpdateItems(UpdateDynamicDelegate updater);

        #endregion Public Methods
    }

    public interface IBroadphaseDynamicElement
    {
        #region Public Properties

        Box2 Aabb { get; }
        int ItemId { get; }

        #endregion Public Properties
    }

    public class BroadphaseDynamicElement : IBroadphaseDynamicElement
    {
        #region Public Constructors

        public BroadphaseDynamicElement(int itemId, Box2 aabb)
        {
            ItemId = itemId;
            Aabb = aabb;
        }

        #endregion Public Constructors

        #region Public Properties

        public Box2 Aabb { get; set; }
        public int ItemId { get; }

        #endregion Public Properties
    }
}
using OpenBreed.Physics.Interface.Managers;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Physics.Interface
{
    public class ContactPair
    {
        public ContactPair(
            int itemA,
            int itemB,
            List<CollisionContact> contacts)
        {
            ItemA = itemA;
            ItemB = itemB;
            Contacts = contacts;
        }

        public int ItemA { get; }
        public int ItemB { get; }
        public List<CollisionContact> Contacts { get; }
    }

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

        public void Solve(
            Action<IBroadphaseStatic, BroadphaseDynamicElement, List<ContactPair>, float> staticPhase,
            Action<BroadphaseDynamicElement, BroadphaseDynamicElement, List<ContactPair>, float> narrowPhase,
            IBroadphaseStatic grid,
            List<ContactPair> result,
            float dt);

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
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
    /// Interface for collision detection
    /// </summary>
    public interface IBroadphase
    {
        #region Public Properties

        IEnumerable<IBroadphaseItem> DynamicItems { get; }

        #endregion Public Properties

        #region Public Methods

        bool ContainsItem(int itemId);

        Box2 GetAabb(int itemId);

        void InsertItem(int itemId, Box2 aabb, BroadphaseItemType type);

        void RemoveItem(int itemId);

        HashSet<int> QueryStatic(Box2 aabb);

        public void Solve(
            Action<IBroadphase, BroadphaseItem, List<ContactPair>, float> staticPhase,
            Action<BroadphaseItem, BroadphaseItem, List<ContactPair>, float> narrowPhase,
            List<ContactPair> result,
            float dt);

        void UpdateItem(int itemId, Box2 aabb);

        void UpdateItems(UpdateDynamicDelegate updater);

        #endregion Public Methods
    }

    public interface IBroadphaseItem
    {
        #region Public Properties

        Box2 Aabb { get; }
        int ItemId { get; }
        BroadphaseItemType Type { get; }

        #endregion Public Properties
    }

    public enum BroadphaseItemType
    {
        Static,
        Dynamic
    }

    public class BroadphaseItem : IBroadphaseItem
    {
        #region Public Constructors

        public BroadphaseItem(
            int itemId,
            Box2 aabb,
            BroadphaseItemType type)
        {
            ItemId = itemId;
            Aabb = aabb;
            Type = type;
        }

        #endregion Public Constructors

        #region Public Properties

        public Box2 Aabb { get; set; }
        public BroadphaseItemType Type { get; }
        public int ItemId { get; }

        #endregion Public Properties
    }
}
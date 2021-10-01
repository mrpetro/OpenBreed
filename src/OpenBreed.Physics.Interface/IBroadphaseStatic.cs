using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Physics.Interface
{
    /// <summary>
    /// Interface for static collision detection broadphase
    /// </summary>
    public interface IBroadphaseStatic
    {
        #region Public Methods

        void InsertItem(int itemId, Box2 aabb);

        void RemoveStatic(int itemId);

        Box2 GetAabb(int itemId);

        HashSet<int> QueryStatic(Box2 aabb);

        #endregion Public Methods
    }
}
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Physics.Interface
{
    /// <summary>
    /// Interface for collision detection broadphase grid
    /// </summary>
    public interface IBroadphaseGrid
    {
        #region Public Methods

        void InsertStatic(int itemId, Box2 aabb);

        void RemoveStatic(int itemId, Box2 aabb);

        HashSet<int> QueryStatic(Box2 aabb);

        #endregion Public Methods
    }
}
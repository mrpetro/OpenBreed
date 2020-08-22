using OpenBreed.Core.Common.Components;
using System.Collections.Generic;
using System.Data;

namespace OpenBreed.Core.Modules.Physics.Components
{
    public class CollisionComponent : IEntityComponent
    {
        #region Public Constructors

        public CollisionComponent()
        {
            ColliderTypes = new List<int>();
        }

        public CollisionComponent(int collisionTypeId)
        {
            ColliderTypes = new List<int>(new int[] { collisionTypeId });
        }

        #endregion Public Constructors

        #region Public Properties

        public List<int> ColliderTypes { get; }

        #endregion Public Properties
    }
}
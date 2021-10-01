using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Data;
using OpenTK;

namespace OpenBreed.Wecs.Components.Physics
{
    public class ColliderComponent : IEntityComponent
    {
        #region Public Constructors

        public ColliderComponent()
        {
            ColliderTypes = new List<int>();
        }

        public ColliderComponent(int collisionTypeId)
        {
            ColliderTypes = new List<int>(new int[] { collisionTypeId });
        }

        #endregion Public Constructors

        #region Public Properties

        public List<int> ColliderTypes { get; }

        #endregion Public Properties
    }
}
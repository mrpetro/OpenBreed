using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Data;
using OpenTK;

namespace OpenBreed.Wecs.Components.Physics
{
    public class CollisionContact
    {
        public CollisionContact(int entityId, Vector2 projection)
        {
            EntityId = entityId;
            Projection = projection;
        }

        public int EntityId { get; }
        public Vector2 Projection { get; }
    }

    public class CollisionComponent : IEntityComponent
    {
        #region Public Constructors

        public CollisionComponent()
        {
            Contacts = new List<CollisionContact>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<CollisionContact> Contacts { get; }

        #endregion Public Properties
    }
}
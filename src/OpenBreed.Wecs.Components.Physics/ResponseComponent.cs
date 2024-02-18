using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Data;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Components.Physics
{
    public class CollisionContact
    {
        #region Public Constructors

        public CollisionContact(int entityId, Vector2 projection)
        {
            EntityId = entityId;
            Projection = projection;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public Vector2 Projection { get; }

        #endregion Public Properties
    }

    public class ResponseComponent : IEntityComponent
    {
        #region Public Constructors

        public ResponseComponent()
        {
            Contacts = new List<CollisionContact>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<CollisionContact> Contacts { get; }

        #endregion Public Properties
    }
}
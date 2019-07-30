using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Physics.Events
{
    public class CollisionEvent : IEvent
    {
        #region Public Fields

        public const string TYPE = "COLLISION_OCCURED";

        #endregion Public Fields

        #region Public Constructors

        public CollisionEvent(IEntity entity)
        {
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}
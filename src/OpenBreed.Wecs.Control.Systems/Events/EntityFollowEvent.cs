using OpenBreed.Wecs.Abstractions.Events;

namespace OpenBreed.Wecs.Control.Systems.Events
{
    /// <summary>
    /// Entity event on follower
    /// </summary>
    public class EntityFollowEvent : EntityEvent
    {
        #region Public Constructors

        public EntityFollowEvent(int entityId, int followerId)
            : base(entityId)
        {
            FollowerId = followerId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int FollowerId { get; }

        #endregion Public Properties
    }
}
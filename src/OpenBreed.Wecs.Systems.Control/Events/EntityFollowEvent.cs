using OpenBreed.Wecs.Events;

namespace OpenBreed.Wecs.Systems.Control.Events
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
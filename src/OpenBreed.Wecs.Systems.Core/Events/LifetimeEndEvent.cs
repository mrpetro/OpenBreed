using OpenBreed.Wecs.Events;

namespace OpenBreed.Wecs.Systems.Core.Events
{
    /// <summary>
    /// Event fired when entity emits another entity
    /// </summary>
    public class LifetimeEndEvent : EntityEvent
    {
        #region Public Constructors

        public LifetimeEndEvent(int entityId)
            : base(entityId)
        {
        }

        #endregion Public Constructors
    }
}
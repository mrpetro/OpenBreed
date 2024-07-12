using OpenBreed.Wecs.Events;

namespace OpenBreed.Sandbox.Wecs.Events
{
    /// <summary>
    /// Event fired when entity is about to be resurrected
    /// </summary>
    public class ResurrectEvent : EntityEvent
    {
        #region Public Constructors

        public ResurrectEvent(int entityId)
            : base(entityId)
        {
        }

        #endregion Public Constructors
    }
}
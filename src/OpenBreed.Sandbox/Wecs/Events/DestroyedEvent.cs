using OpenBreed.Wecs.Events;

namespace OpenBreed.Sandbox.Wecs.Events
{
    public class DestroyedEvent : EntityEvent
    {
        #region Public Constructors

        public DestroyedEvent(int entityId)
            : base(entityId)
        {

        }

        #endregion Public Constructors

    }
}
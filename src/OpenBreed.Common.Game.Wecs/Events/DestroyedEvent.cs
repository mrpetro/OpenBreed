using OpenBreed.Wecs.Events;

namespace OpenBreed.Common.Game.Wecs.Events
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
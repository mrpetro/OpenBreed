using OpenBreed.Wecs.Events;

namespace OpenBreed.Common.Game.Wecs.Events
{
    public class LivesChangedEvent : EntityEvent
    {
        #region Public Constructors

        public LivesChangedEvent(int entityId, int livesNo)
            : base(entityId)
        {
            LivesNo = livesNo;
        }

        #endregion Public Constructors

        #region Public Properties

        public int LivesNo { get; }

        #endregion Public Properties
    }
}
using OpenBreed.Wecs.Events;

namespace OpenBreed.Wecs.Systems.Audio.Events
{
    /// <summary>
    /// Event fired when sound has been played on particular entity
    /// </summary>
    public class SoundPlayEvent : EntityEvent
    {
        #region Public Constructors

        public SoundPlayEvent(int entityId, int soundId)
            : base(entityId)
        {
            SoundId = soundId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int SoundId { get; }

        #endregion Public Properties
    }
}
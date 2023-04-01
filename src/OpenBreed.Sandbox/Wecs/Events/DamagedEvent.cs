using OpenBreed.Wecs.Events;

namespace OpenBreed.Sandbox.Wecs.Events
{
    /// <summary>
    /// Event fired when entity has been damaged
    /// </summary>
    public class DamagedEvent : EntityEvent
    {
        #region Public Constructors

        public DamagedEvent(int entityId, int damage, int damagingEntityId)
            : base(entityId)
        {
            Damage = damage;
            DamagingEntityId = damagingEntityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Damage { get; }
        public int DamagingEntityId { get; }

        #endregion Public Properties
    }
}
using OpenBreed.Wecs.Events;

namespace OpenBreed.Wecs.Systems.Gui.Events
{
    /// <summary>
    /// Entity event when cursor is pressed
    /// </summary>
    public class CursorKeyPressedEntityEvent : EntityEvent
    {
        #region Public Constructors

        public CursorKeyPressedEntityEvent(int entityId, int keyId)
            : base(entityId)
        {
            KeyId = keyId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int KeyId { get; }

        #endregion Public Properties
    }
}
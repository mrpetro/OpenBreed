using OpenBreed.Wecs.Events;

namespace OpenBreed.Wecs.Systems.Gui.Events
{
    /// <summary>
    /// Entity event when cursor is moved
    /// </summary>
    public class CursorMovedEntityEvent : EntityEvent
    {
        #region Public Constructors

        public CursorMovedEntityEvent(int entityId)
            : base(entityId)
        {
        }

        #endregion Public Constructors
    }
}
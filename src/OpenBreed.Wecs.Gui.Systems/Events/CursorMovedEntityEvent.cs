using OpenBreed.Wecs.Abstractions.Events;

namespace OpenBreed.Wecs.Gui.Systems.Events
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
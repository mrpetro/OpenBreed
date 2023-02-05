using OpenBreed.Wecs.Events;

namespace OpenBreed.Wecs.Systems.Control.Events
{
    /// <summary>
    /// Entity event on action 
    /// </summary>
    public class EntityActionEvent : EntityEvent
    {
        #region Public Constructors

        public EntityActionEvent(int entityId, int actionId)
            : base(entityId)
        {
            ActionId = actionId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int ActionId { get; }

        #endregion Public Properties
    }
}
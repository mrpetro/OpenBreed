using OpenBreed.Wecs.Events;

namespace OpenBreed.Wecs.Systems.Control.Events
{
    /// <summary>
    /// Entity event on action 
    /// </summary>
    public class EntityActionEvent : EntityEvent
    {
        #region Public Constructors

        public EntityActionEvent(int entityId, string actionType)
            : base(entityId)
        {
            ActionType = actionType;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ActionType { get; }

        #endregion Public Properties
    }
}
using OpenBreed.Wecs.Events;
using System;

namespace OpenBreed.Wecs.Systems.Control.Events
{
    /// <summary>
    /// Entity event on action 
    /// </summary>
    public class EntityActionEvent<TActionType> : EntityEvent where TActionType : Enum
    {
        #region Public Constructors

        public EntityActionEvent(int entityId, TActionType actionCode)
            : base(entityId)
        {
            ActionCode = actionCode;
        }

        #endregion Public Constructors

        #region Public Properties

        public TActionType ActionCode { get; }

        #endregion Public Properties
    }
}
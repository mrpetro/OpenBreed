using OpenBreed.Wecs.Events;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Systems.Physics.Events
{
    public class VelocityChangedEvent : EntityEvent
    {
        #region Public Constructors

        public VelocityChangedEvent(int entityId, Vector2 value)
            : base(entityId)
        {
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Value { get; }

        #endregion Public Properties
    }
}
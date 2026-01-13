using OpenBreed.Wecs.Abstractions.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Physics.Systems.Events
{
    public class PositionChangedEvent : EntityEvent
    {
        #region Public Constructors

        public PositionChangedEvent(int entityId, Vector2 position)
            : base(entityId)
        {
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Position { get; }

        #endregion Public Properties
    }
}

using OpenBreed.Wecs.Events;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Events
{
    public class DirectionSetEvent : EntityEvent
    {
        #region Public Constructors

        public DirectionSetEvent(int entityId, Vector2 direction)
            : base(entityId)
        {
            Direction = direction;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Direction { get; }

        #endregion Public Properties
    }
}

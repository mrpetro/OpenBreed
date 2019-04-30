using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Game.Common;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;

namespace OpenBreed.Game.Control.Components
{
    public enum MovementDirection
    {
        Right,
        Up,
        Left,
        Down
    }

    public class MovementControl : IControlComponent
    {
        private Transformation transformation;

        public Type SystemType { get { return typeof(ControlSystem); } }

        public void Move(MovementDirection direction)
        {

        }

        public void Deinitialize(IEntity entity)
        {
            //throw new NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
            transformation = entity.Components.OfType<Transformation>().First();

        }
    }
}

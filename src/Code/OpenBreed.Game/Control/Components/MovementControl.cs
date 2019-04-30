using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Game.Common;
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
            //throw new NotImplementedException();
        }
    }
}

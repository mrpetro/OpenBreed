using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public enum MovementState
    {
        Standing,
        Walking
    }

    public enum MovementImpulse
    {
        Stop,
        Walk
    }
}

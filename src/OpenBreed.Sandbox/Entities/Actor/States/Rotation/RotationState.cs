using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public enum RotationState
    {
        Idle,
        Rotating
    }

    public enum RotationImpulse
    {
        Stop,
        Rotate
    }
}

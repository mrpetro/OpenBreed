using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Door.States
{
    public enum FunctioningState
    {
        Closed,
        Closing,
        Opened,
        Opening
    }

    public enum FunctioningImpulse
    {
        Close,
        StopClosing,
        Open,
        StopOpening
    }
}

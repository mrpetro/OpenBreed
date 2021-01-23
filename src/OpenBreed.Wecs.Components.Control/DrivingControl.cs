using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Control
{
    /// <summary>
    /// Placeholder class for car controls
    /// </summary>
    public class DrivingControl : BaseControl
    {
        public bool Gas { get; }
        public bool Break { get; }
        public bool TurnLeft { get; }
        public bool TurnRight { get; }
    }
}

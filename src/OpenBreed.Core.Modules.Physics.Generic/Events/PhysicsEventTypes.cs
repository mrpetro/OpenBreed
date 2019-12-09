using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Physics related event types
    /// </summary>
    public static class PhysicsEventTypes
    {
        /// <summary>
        /// Occurs when entity body collision disables
        /// </summary>
        public const string BODY_OFF = "BODY_OFF";

        /// <summary>
        /// Occurs when entity body collision enables
        /// </summary>
        public const string BODY_ON = "BODY_ON";

        /// <summary>
        /// Occurs when two entities are colliding
        /// </summary>
        public const string COLLISION_OCCURRED = "COLLISION_OCCURRED";
    }
}

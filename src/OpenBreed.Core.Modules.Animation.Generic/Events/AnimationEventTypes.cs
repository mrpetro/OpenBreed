using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Animation.Events
{
    /// <summary>
    /// Animation related event types
    /// </summary>
    public class AnimationEventTypes
    {
        /// <summary>
        /// Occurs when animation frame is changed
        /// </summary>
        public const string ANIMATION_CHANGED = "ANIMATION_CHANGED";

        /// <summary>
        /// Occurs when animation is stopped
        /// </summary>
        public const string ANIMATION_STOPPED = "ANIMATION_STOPPED";
    }
}

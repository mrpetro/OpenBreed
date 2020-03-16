using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Events
{
    /// <summary>
    /// Graphics related event types
    /// </summary>
    public static class GfxEventTypes
    {
        /// <summary>
        /// Occurs when rendering client screen was resized
        /// </summary>
        public const string CLIENT_RESIZED = "CLIENT_RESIZED";

        /// <summary>
        /// Occurs when rendering viewport was resized
        /// </summary>
        public const string VIEWPORT_RESIZED = "VIEWPORT_RESIZED";

        /// <summary>
        /// Occurs when rendering viewport was clicked with cursor
        /// </summary>
        public const string VIEWPORT_CLICKED = "VIEWPORT_CLICKED";
    }
}

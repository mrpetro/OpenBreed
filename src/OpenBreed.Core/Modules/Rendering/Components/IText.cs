using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Text render component interface
    /// </summary>
    public interface IText : IEntityComponent
    {
        /// <summary>
        /// Id of text font
        /// </summary>
        int FontId { get; set; }

        /// <summary>
        /// Offset position of text
        /// </summary>
        Vector2 Offset { get; set; }

        /// <summary>
        /// Actual text of this component
        /// </summary>
        string Value { get; set; }
    }
}

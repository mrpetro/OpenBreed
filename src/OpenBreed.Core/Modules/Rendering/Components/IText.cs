using OpenBreed.Core.Systems.Common.Components;
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
    public interface IText : IRenderComponent
    {
        /// <summary>
        /// Id of text font
        /// </summary>
        int FontId { get; set; }

        /// <summary>
        /// Text position component reference
        /// </summary>
        Position Position { get; }

        /// <summary>
        /// Actual text of this component
        /// </summary>
        string Value { get; set; }
    }
}

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
        /// Actual text of this component
        /// </summary>
        string Value { get; set; }
    }
}

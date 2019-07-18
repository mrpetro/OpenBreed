using OpenBreed.Core.Modules.Rendering.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Common.Systems
{
    /// <summary>
    /// System that state will be rendered to parricular viewport during core render phase
    /// </summary>
    public interface IRenderableSystem : IWorldSystem
    {
        /// <summary>
        /// Render this system to given viewport using certain time step
        /// </summary>
        /// <param name="viewport">Rendered viewport</param>
        /// <param name="dt">Time step</param>
        void Render(IViewport viewport, float dt);
    }
}

using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Rendering.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Rendering.Components
{
    /// <summary>
    /// Component interface which is dedicated for graphics rendering
    /// </summary>
    public interface IRenderComponent : IEntityComponent
    {
        /// <summary>
        /// Draw this component to given viewport
        /// </summary>
        /// <param name="viewport">Viewport which this component will be rendered to</param>
        void Draw(Viewport viewport);
    }
}

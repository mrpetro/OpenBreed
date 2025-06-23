using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL
{
    internal class RenderLayer : IRenderLayer
    {
        private ViewRenderHandler renderHandler;

        public RenderLayer(ViewRenderHandler renderHandler)
        {
            this.renderHandler = renderHandler ?? throw new ArgumentNullException(nameof(renderHandler));
        }

        public void Render(IRenderView renderView, float dt)
        {
            renderHandler.Invoke(renderView, dt);
        }
    }
}

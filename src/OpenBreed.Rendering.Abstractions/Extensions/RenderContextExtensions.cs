using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions.Extensions
{
    public static class RenderContextExtensions
    {
        public static IRenderView CreateView(this IRenderContext renderContext, Box2 boundaryBox)
        {
            return renderContext.CreateView(boundaryBox.Min.X, boundaryBox.Min.Y, boundaryBox.Size.X, boundaryBox.Size.Y);
        }
    }
}

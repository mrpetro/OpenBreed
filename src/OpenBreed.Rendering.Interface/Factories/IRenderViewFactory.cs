using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Factories
{
    public interface IRenderViewFactory
    {
        TRenderView CreateView<TRenderView>(IRenderContext renderContext, Box2 viewBox) where TRenderView : IRenderView;
    }
}

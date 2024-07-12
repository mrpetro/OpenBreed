using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public interface IRenderer<T>// where T : IRenderable
    {
        void Render(T renderable);
    }
}

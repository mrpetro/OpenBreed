using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public abstract class RendererBase<T> : IRenderer<T>
    {
        public IRenderTarget Target { get; }

        protected RendererBase(IRenderTarget target)
        {
            Target = target;
        }

        public abstract void Render(T renderable);
    }
}

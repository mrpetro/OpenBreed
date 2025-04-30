using OpenBreed.Rendering.Abstractions.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions.Renderers
{
    public interface IRenderer<TObject>
    {
        void Render(TObject obj, IRenderView view);
    }
}

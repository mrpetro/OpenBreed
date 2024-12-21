using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Rendering
{
    public interface IInteractionRenderer
    {
        void Render(IInteractionCore interactionCore, IRenderView renderView);
    }
}

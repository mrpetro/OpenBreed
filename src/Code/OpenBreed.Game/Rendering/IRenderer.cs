using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Rendering
{
    public interface IRenderer
    {
        void Render(Viewport viewport);
    }
}

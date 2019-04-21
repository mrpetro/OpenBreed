using OpenBreed.Game.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Components
{
    public interface IRenderComponent : IEntityComponent
    {
        void Draw(Viewport viewport);
    }
}

using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Rendering.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Rendering.Components
{
    public interface IRenderComponent : IEntityComponent
    {
        void Draw(Viewport viewport);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public interface ICameraSystem
    {
        void Render(float left, float bottom, float right, float top, float dt);
    }
}

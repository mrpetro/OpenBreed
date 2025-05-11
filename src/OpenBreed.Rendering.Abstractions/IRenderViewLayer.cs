using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions
{
    public interface IRenderLayer
    {
        #region Public Methods

        void Render(IRenderView renderView, float dt);

        #endregion Public Methods
    }
}
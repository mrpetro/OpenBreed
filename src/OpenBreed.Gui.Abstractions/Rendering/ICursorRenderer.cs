using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Abstractions.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Rendering
{
    public interface ICursorRenderer
    {
        #region Public Methods

        void Render(IInteractionCursor cursor, IRenderView view);

        #endregion Public Methods
    }
}
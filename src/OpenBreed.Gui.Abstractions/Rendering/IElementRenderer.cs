using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Rendering
{
    public interface IElementRenderer
    {
        #region Public Methods

        Type ElementType { get; }

        void Render(IElement element, IRenderView view);

        #endregion Public Methods
    }
}
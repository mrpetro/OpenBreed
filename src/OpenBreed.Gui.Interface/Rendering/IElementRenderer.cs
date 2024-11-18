using OpenBreed.Gui.Interface.Elements;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Rendering
{
    public interface IElementRenderer
    {
        #region Public Methods

        Type ElementType { get; }

        void Render(IElement element, IRenderView view);

        #endregion Public Methods
    }
}
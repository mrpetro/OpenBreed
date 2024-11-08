using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Elements;

namespace OpenBreed.Gui.Interface
{
    public interface IInteractionFactory
    {
        #region Public Methods

        IInteractiveLabel CreateBox(float centerX, float centerY, float width, float height);

        #endregion Public Methods
    }
}
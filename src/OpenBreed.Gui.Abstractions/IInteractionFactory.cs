using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Abstractions.Elements;

namespace OpenBreed.Gui.Abstractions
{
    public interface IInteractionFactory
    {
        #region Public Methods

        ILabel CreateBox(float centerX, float centerY, float width, float height);

        #endregion Public Methods
    }
}
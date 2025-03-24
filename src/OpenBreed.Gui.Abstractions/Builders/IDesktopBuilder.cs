using OpenBreed.Gui.Abstractions.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface IDesktopBuilder : IElementBuilder
    {
        #region Public Methods

        IDesktop Build();

        #endregion Public Methods
    }
}
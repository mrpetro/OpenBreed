using OpenBreed.Gui.Abstractions.Elements;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions
{
    public interface IElementFactory
    {
        #region Public Methods

        TElement Create<TElement>(string blueprintName, object model) where TElement : IElement;

        #endregion Public Methods

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Presentations
{
    public interface ITextFieldPresentation : IElementPresentation
    {
        #region Public Properties

        string FontName { get; }
        int FontSize { get; }

        #endregion Public Properties
    }
}
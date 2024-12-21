using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface IContainerBuilder : IElementBuilder
    {
        #region Public Methods

        IElementBuilder AddChild(IElementBuilder childBuilder);

        #endregion Public Methods
    }
}
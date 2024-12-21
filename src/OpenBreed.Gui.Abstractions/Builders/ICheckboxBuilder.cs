using OpenBreed.Gui.Abstractions.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface ICheckboxBuilder : IDockPanelBuilder
    {
        #region Public Methods

        void BindIsChecked(PropertyBinding<bool> binding);

        void SetLabel(string text);

        #endregion Public Methods
    }
}
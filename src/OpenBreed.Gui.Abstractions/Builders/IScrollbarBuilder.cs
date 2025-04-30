using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface IScrollbarBuilder : IElementBuilder
    {
        #region Public Methods

        void SetMode(ScrollbarMode mode);

        void SetValue(float value);

        void SetMinimumValue(float value);

        void SetMaximumValue(float value);

        void SetValueUnit(float valueUnit);

        void BindValue(IValueBinding<float>? binding);

        #endregion Public Methods
    }
}
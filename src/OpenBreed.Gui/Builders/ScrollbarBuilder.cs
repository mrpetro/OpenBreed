using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Builders
{
    internal class ScrollbarBuilder : ElementBuilder<IScrollbar>, IScrollbarBuilder
    {
        internal float DefaultValue { get; private set; }
        internal float MaximumValue { get; private set; }
        internal float MinimumValue { get; private set; }
        internal float ValueUnit { get; private set; }
        internal IValueBinding<float>? ValueBinding { get; private set; }
        internal ScrollbarMode Mode { get; private set; }

        public ScrollbarBuilder(IElementInputController<IScrollbar> inputController) : base(inputController)
        {
        }

        public void SetMode(ScrollbarMode mode)
        {
            Mode = mode;
        }

        public void SetValue(float value)
        {
            DefaultValue = value;
        }

        public void SetMaximumValue(float value)
        {
            MaximumValue = value;
        }

        public void SetMinimumValue(float value)
        {
            MinimumValue = value;
        }

        public void SetValueUnit(float valueUnit)
        {
            ValueUnit = valueUnit;
        }

        public void BindValue(IValueBinding<float>? binding)
        {
            ValueBinding = binding;
        }

        public override IScrollbar Build()
        {
            return new Scrollbar(this);
        }
    }
}

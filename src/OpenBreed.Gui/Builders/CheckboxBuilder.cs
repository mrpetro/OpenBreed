using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Extensions;
using OpenBreed.Gui.Elements;
using OpenBreed.Gui.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Builders
{
    internal class CheckboxBuilder : DockPanelBuilder, ICheckboxBuilder
    {

        #region Private Fields

        private readonly IInteractionFactory factory;
        private PropertyBinding<bool>? isCheckedBinding;
        private string? labelText;

        #endregion Private Fields

        #region Public Constructors

        public CheckboxBuilder(IInteractionFactory factory)
        {
            SetTag("Checkbox");
            SetSize(300, 40);
            SetMinimumSize(40, 40);
            SetMaximumSize(float.MaxValue, 40);
            SetPadding(4);
            this.factory = factory;
        }

        #endregion Public Constructors

        #region Public Methods

        public void BindIsChecked(PropertyBinding<bool> binding)
        {
            isCheckedBinding = binding;
        }

        public void SetLabel(string text)
        {
            labelText = text;
        }

        #endregion Public Methods

        #region Internal Methods

        public override IDockPanel Build()
        {
            var element = base.Build();

            element.AddChild(factory.CreateStatebox((builder) =>
            {
                builder.SetDockMode(ElementDockMode.Left);
                builder.SetChecked(isChecked: true);
                builder.SetTag("Checkbox");
                builder.SetMargin(4);
                builder.SetMaximumSize(32, 32);
                builder.SetMinimumSize(32, 32);
                builder.BindIsChecked(isCheckedBinding);
            }));

            element.AddChild(factory.CreateLabel((builder) =>
            {
                builder.SetDockMode(ElementDockMode.Fill);
                builder.SetHitTestable(false);
                builder.SetHorizontalAlignment(HorizontalAlignment.Left);
                builder.SetVerticalAlignment(VerticalAlignment.Center);
                builder.SetText(labelText);
            }));

            return element;
        }

        #endregion Internal Methods
    }
}
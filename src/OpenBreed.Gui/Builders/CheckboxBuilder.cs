using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Gui.Abstractions.Controllers;
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
        private string? labelText;
        private bool value;
        private PropertyBinding<bool>? valueBinding;

        #endregion Private Fields

        #region Public Constructors

        public CheckboxBuilder(IElementInputController inputController, IInteractionFactory factory) : base(inputController)
        {
            SetTag("Checkbox");
            SetSize(300, 16);
            SetMinimumSize(16, 16);
            SetMaximumSize(float.MaxValue, 16);
            SetPadding(1);
            this.factory = factory;
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetValue(bool value)
        {
            this.value = value;
        }

        public void BindValue(PropertyBinding<bool> binding)
        {
            valueBinding = binding;
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

            element.AddChild(factory.CreateCheckField((builder) =>
            {
                builder.SetDockMode(ElementDockMode.Left);
                builder.SetChecked(value);
                builder.SetTag("Checkbox");
                builder.SetMargin(1);
                builder.SetMaximumSize(14, 14);
                builder.SetMinimumSize(14, 14);
                builder.BindProperty(valueBinding);
            }));

            element.AddChild(factory.CreateLabelField((builder) =>
            {
                builder.SetFontSize(8);
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
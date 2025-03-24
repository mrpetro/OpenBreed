using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Extensions
{
    public static class InteractionFactoryExtensions
    {
        #region Public Methods

        public static ILabel CreateLabel(this IInteractionFactory factory, Action<ILabelBuilder> setter)
        {
            var fontMan = factory.View.Context.Fonts;

            var builder = new LabelBuilder(fontMan);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static ITextBox CreateTextBox(this IInteractionFactory factory, Action<ITextBoxBuilder> setter)
        {
            var fontMan = factory.View.Context.Fonts;

            var builder = new TextBoxBuilder(fontMan);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static ICheckboxBuilder CreateCheckbox(this IInteractionFactory factory, Action<ICheckboxBuilder> setter)
        {
            var builder = new CheckboxBuilder(factory);

            setter.Invoke(builder);

            return builder;
        }

        public static IDockPanel CreateDockPanel(this IInteractionFactory factory, Action<IDockPanelBuilder> setter)
        {
            var builder = new DockPanelBuilder();

            setter.Invoke(builder);

            return builder.Build();
        }

        public static IButton CreateButton(this IInteractionFactory factory, Action<IButtonBuilder> setter)
        {
            var builder = new ButtonBuilder();

            setter.Invoke(builder);

            return builder.Build();
        }

        public static IStatebox CreateStatebox(this IInteractionFactory factory, Action<IStateboxBuilder> setter)
        {
            var builder = new StateboxBuilder();

            setter.Invoke(builder);

            return builder.Build();
        }

        public static IGridPanel CreateGridPanel(this IInteractionFactory factory, Action<IGridPanelBuilder> setter)
        {
            var builder = new GridPanelBuilder();

            setter.Invoke(builder);

            return builder.Build();
        }

        #endregion Public Methods
    }
}
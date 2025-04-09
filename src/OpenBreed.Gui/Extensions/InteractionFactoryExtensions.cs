using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Controllers;
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
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ILabel>>();

            var fontMan = factory.View.Context.Fonts;

            var builder = new LabelBuilder(inputController, fontMan);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static ITextField CreateTextField(this IInteractionFactory factory, Action<ITextFieldBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ITextField>>();

            var fontMan = factory.View.Context.Fonts;

            var builder = new TextFieldBuilder(inputController, fontMan);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static ICheckboxBuilder CreateCheckbox(this IInteractionFactory factory, Action<ICheckboxBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ICheckbox>>();

            var builder = new CheckboxBuilder(inputController, factory);

            setter.Invoke(builder);

            return builder;
        }

        public static IDockPanel CreateDockPanel(this IInteractionFactory factory, Action<IDockPanelBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ILabel>>();

            var builder = new DockPanelBuilder(inputController);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static IButton CreateButton(this IInteractionFactory factory, Action<IButtonBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<IButton>>();

            var builder = new ButtonBuilder(inputController);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static IStatebox CreateStatebox(this IInteractionFactory factory, Action<IStateboxBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ILabel>>();

            var builder = new StateboxBuilder(inputController);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static IGridPanel CreateGridPanel(this IInteractionFactory factory, Action<IGridPanelBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<IGridPanel>>();

            var builder = new GridPanelBuilder(inputController);

            setter.Invoke(builder);

            return builder.Build();
        }

        #endregion Public Methods
    }
}
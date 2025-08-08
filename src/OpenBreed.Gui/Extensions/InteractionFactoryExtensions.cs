using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Controllers;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;
using OpenBreed.Rendering.Abstractions.Managers;
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

        public static ILabelField CreateLabelField(this IInteractionFactory factory, Action<ILabelFieldBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController>();

            var fontMan = factory.ServiceProvider.GetRequiredService<IFontMan>();

            var builder = new LabelFieldBuilder(inputController, fontMan);

            setter.Invoke(builder);

            return builder.Build();
        }

        //public static IContainer CreateTextBox(this IInteractionFactory factory, Action<ITextFieldBuilder> setter)
        //{
        //    var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ITextField>>();

        //    var fontMan = factory.View.Context.Fonts;

        //    var builder = new TextBoxBuilder(inputController, fontMan);

        //    setter.Invoke(builder);

        //    return builder.Build();
        //}

        public static ITextField CreateTextField(this IInteractionFactory factory, Action<ITextFieldBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ITextField>>();
            var fontMan = factory.ServiceProvider.GetRequiredService<IFontMan>();

            var builder = new TextFieldBuilder(inputController, fontMan);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static IContainer CreateCheckbox(this IInteractionFactory factory, Action<ICheckboxBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController>();

            var builder = new CheckboxBuilder(inputController, factory);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static IDockPanel CreateDockPanel(this IInteractionFactory factory, Action<IDockPanelBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ILabelField>>();

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

        public static IScrollbar CreateScrollbar(this IInteractionFactory factory, Action<IScrollbarBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<IScrollbar>>();

            var builder = new ScrollbarBuilder(inputController);

            setter.Invoke(builder);

            return builder.Build();
        }

        public static ICheckField CreateCheckField(this IInteractionFactory factory, Action<ICheckFieldBuilder> setter)
        {
            var inputController = factory.ServiceProvider.GetRequiredService<IElementInputController<ICheckField>>();

            var builder = new CheckFieldBuilder(inputController);

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
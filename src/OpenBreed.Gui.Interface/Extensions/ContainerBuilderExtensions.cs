using OpenBreed.Gui.Interface.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Extensions
{
    public static class ContainerBuilderExtensions
    {
        #region Public Methods

        public static IElementBuilder AddLabel(this IContainerBuilder container, Action<ILabelBuilder> setter)
        {
            var builder = new LabelBuilder(container);

            setter.Invoke(builder);

            return container.AddChild(builder);
        }

        public static IElementBuilder AddDockPanel(this IContainerBuilder container, Action<IDockPanelBuilder> setter)
        {
            var builder = new DockPanelBuilder(container);

            setter.Invoke(builder);

            return container.AddChild(builder);
        }

        public static IElementBuilder AddGridPanel(this IContainerBuilder container, Action<IGridPanelBuilder> setter)
        {
            var builder = new GridPanelBuilder(container);

            setter.Invoke(builder);

            return container.AddChild(builder);
        }

        public static IElementBuilder AddButton(this IContainerBuilder container, Action<IButtonBuilder> setter)
        {
            var builder = new ButtonBuilder(container);

            setter.Invoke(builder);

            return container.AddChild(builder);
        }

        public static IElementBuilder AddCheckbox(this IContainerBuilder container, Action<ICheckboxBuilder> setter)
        {
            var builder = new CheckboxBuilder(container);

            setter.Invoke(builder);

            return container.AddChild(builder);
        }

        #endregion Public Methods
    }
}
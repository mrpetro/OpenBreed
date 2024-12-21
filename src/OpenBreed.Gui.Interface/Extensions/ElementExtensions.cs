using OpenBreed.Gui.Interface.Builders;
using OpenBreed.Gui.Interface.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Extensions
{
    public static class ElementExtensions
    {
        public static bool TryGetOption<TElementOption>(this IElement element, out TElementOption? option) where TElementOption : class, IElementOption
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            option = element.Options.OfType<TElementOption>().FirstOrDefault();

            if (option is null)
            {
                option = null;
                return false;
            }

            return true;
        }

        public static ElementDockMode GetDockMode(this IElement element)
        {
            if (element.TryGetOption<IDockPanelParentOption>(out IDockPanelParentOption? option) && option is not null)
            {
                return option.Mode;
            }

            return ElementDockMode.None;
        }

        public static IGridPanelParentOption GetGridPosition(this IElement element)
        {
            if (element.TryGetOption<IGridPanelParentOption>(out IGridPanelParentOption? option) && option is not null)
            {
                return option;
            }

            return new GridPanelParentOption(0, 0);
        }
    }
}

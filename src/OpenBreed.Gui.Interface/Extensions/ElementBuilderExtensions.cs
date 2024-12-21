using OpenBreed.Gui.Interface.Builders;
using OpenBreed.Gui.Interface.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Extensions
{
    public static class ElementBuilderExtensions
    {
        public static void SetDockMode(this IElementBuilder elementBuilder, ElementDockMode dockMode)
        {
            elementBuilder.SetOption(new DockPanelParentOption(dockMode));
        }

        public static void SetGridPosition(this IElementBuilder elementBuilder, int column, int row, int columnSpan = 1, int rowSpan = 1)
        {
            elementBuilder.SetOption(new GridPanelParentOption(column, row, columnSpan, rowSpan));
        }
    }
}

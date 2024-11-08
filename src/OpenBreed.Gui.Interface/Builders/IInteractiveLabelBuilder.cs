using OpenBreed.Gui.Interface.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    public interface IInteractiveLabelBuilder : IInteractiveElementBuilder
    {
        void SetHorizontalAlignment(HorizontalAlignment horizontalAlignment);
        void SetVerticalAlignment(VerticalAlignment verticalAlignment);
        void SetText(string text);
    }
}

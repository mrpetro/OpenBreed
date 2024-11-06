using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    public interface IInteractivePanelBuilder : IInteractiveElementBuilder
    {
        void SetBorderColor(Color4 color);
        void SetFillColor(Color4 color);

    }
}

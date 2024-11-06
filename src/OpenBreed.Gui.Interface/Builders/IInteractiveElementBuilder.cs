using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Interface.Builders
{
    public interface IInteractiveElementBuilder
    {
        IInteractiveLabelBuilder BeginLabel();
        IInteractivePanelBuilder BeginPanel();
        IInteractiveElementBuilder SetClickCallback(Action<IInteractiveElement> callback);
        IInteractiveElementBuilder SetPosition(float x, float y);
        IInteractiveElementBuilder SetSize(int width, int height);
        IInteractiveElementBuilder SetTag(string tag);
        IInteractiveElementBuilder FinishElement();
        IInteractiveElement Build();
    }
}

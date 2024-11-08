using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Gui.Interface.Elements;

namespace OpenBreed.Gui.Interface.Builders
{
    public interface IInteractiveElementBuilder
    {
        #region Public Methods

        IInteractiveElementBuilder AddLabel(Action<IInteractiveLabelBuilder> setter);

        IInteractiveElementBuilder AddPanel(Action<IInteractivePanelBuilder> setter);

        void SetClickCallback(Action<IInteractiveElement> callback);

        void SetPosition(float x, float y);

        void SetSize(int width, int height);

        void SetTag(string tag);

        IInteractiveElement Build();

        #endregion Public Methods
    }
}
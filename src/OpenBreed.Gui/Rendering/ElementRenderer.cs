using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Abstractions.Rendering;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Gui.Rendering
{
    public abstract class ElementRenderer<TElement> : IElementRenderer where TElement : IElement
    {
        #region Public Properties

        public Type ElementType => typeof(TElement);

        #endregion Public Properties

        #region Public Methods

        public void Render(IElement element, IRenderView view) => Render((TElement)element, view);

        #endregion Public Methods

        #region Protected Methods

        protected abstract void Render(TElement element, IRenderView view);

        #endregion Protected Methods
    }
}
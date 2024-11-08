using OpenTK.Mathematics;

namespace OpenBreed.Gui.Interface.Elements
{
    public interface IInteractivePanel : IInteractiveElement
    {
        #region Public Properties

        Color4 FillColor { get; }
        Color4 BorderColor { get; }

        #endregion Public Properties
    }
}
using OpenBreed.Gui.Abstractions.Constants;
using OpenBreed.Rendering.Abstractions;

namespace OpenBreed.Gui.Abstractions.Elements
{

    public interface ILabelField : IElement
    {
        #region Public Properties

        HorizontalAlignment HorizontalAlignment { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }

        string Text { get; set; }

        IFont Font { get; }

        #endregion Public Properties
    }
}
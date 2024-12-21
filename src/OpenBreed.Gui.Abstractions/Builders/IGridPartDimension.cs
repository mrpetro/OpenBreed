using OpenBreed.Gui.Abstractions.Enums;

namespace OpenBreed.Gui.Abstractions.Builders
{
    public interface IGridPartDimension
    {
        #region Public Properties

        float Value { get; }
        DimmensionType Type { get; }

        #endregion Public Properties
    }
}
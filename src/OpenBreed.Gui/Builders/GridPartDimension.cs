using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Enums;

namespace OpenBreed.Gui.Builders
{
    public class GridPartDimension : IGridPartDimension
    {
        #region Public Constructors

        public GridPartDimension(float value, DimmensionType dimmensionType)
        {
            Value = value;
            Type = dimmensionType;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Value { get; }
        public DimmensionType Type { get; }

        #endregion Public Properties
    }
}
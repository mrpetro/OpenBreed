using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Core.Components
{
    public interface IPreviousPositionComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        float X { get; }
        float Y { get; }

        #endregion Public Properties
    }

    public sealed class PreviousPositionComponent : IEntityComponent
    {
        #region Public Constructors

        public PreviousPositionComponent(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        public PreviousPositionComponent(Vector2 value)
        {
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static PreviousPositionComponent Create(Vector2 value)
        {
            return new PreviousPositionComponent(value);
        }

        public static PreviousPositionComponent Create(float x, float y)
        {
            return new PreviousPositionComponent(x, y);
        }

        #endregion Public Methods
    }
}
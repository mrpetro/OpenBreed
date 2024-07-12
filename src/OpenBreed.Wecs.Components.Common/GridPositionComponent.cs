using OpenTK;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IGridPositionComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int X { get; }
        int Y { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Position entity component class that can be used to store entity current position information
    /// Example: Actor is standing somewhere in the world at current position
    /// </summary>
    public sealed class GridPositionComponent : IEntityComponent
    {
        #region Private Constructors

        /// <summary>
        /// Constructor with passed initial position values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private GridPositionComponent(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// X index coordinate in grid
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y index coordinate in grid
        /// </summary>
        public int Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static GridPositionComponent Create(int x, int y)
        {
            return new GridPositionComponent(x, y);
        }

        #endregion Public Methods
    }

    public sealed class GridPositionComponentFactory : ComponentFactoryBase<IGridPositionComponentTemplate>
    {
        #region Internal Constructors

        public GridPositionComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IGridPositionComponentTemplate template)
        {
            return GridPositionComponent.Create(template.X, template.Y);
        }

        #endregion Protected Methods
    }
}
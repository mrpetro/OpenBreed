using OpenBreed.Core.Common.Components.Builders;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Common.Components
{
    /// <summary>
    /// Grid position entity component class that can be used to store entity grid position information
    /// Example: Tile is being placed at X and Y grid coordinates
    /// </summary>
    public class GridPosition : IEntityComponent
    {
        #region Public Constructors

        public GridPosition(GridPositionBuilder builder)
        {
            X = builder.X + builder.OffsetX;
            Y = builder.Y + builder.OffsetY;
        }

        #endregion Public Constructors

        #region Private Constructors

        /// <summary>
        /// Constructor with passed initial grid indices values
        /// </summary>
        /// <param name="value">Initial values</param>
        private GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion Private Constructors

        #region Public Properties

        public int X { get; }
        public int Y { get; }

        #endregion Public Properties

        #region Public Methods

        public static GridPosition Create(int x, int y)
        {
            return new GridPosition(x, y);
        }
        
        public static void Register(EntityMan entities)
        {
            //EntityMan.AddSetter("GridPosition.Offset",(o) => 
        }

        #endregion Public Methods
    }
}
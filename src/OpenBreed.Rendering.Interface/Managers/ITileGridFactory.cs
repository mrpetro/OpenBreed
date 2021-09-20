using OpenTK;

namespace OpenBreed.Rendering.Interface.Managers
{
    /// <summary>
    /// Tile Grid Factory interface for handling rendering of tile grids 
    /// </summary>
    public interface ITileGridFactory
    {
        #region Public Methods

        /// <summary>
        /// Create a tile grid giving it's properties
        /// </summary>
        /// <param name="width">Width of grid</param>
        /// <param name="height">Height of grid</param>
        /// <param name="layersNo">Number of grid layers</param>
        /// <param name="cellSize">Grid cell size</param>
        /// <returns>New tile grid instance</returns>
        ITileGrid CreateGrid(int width, int height, int layersNo, int cellSize);

        #endregion Public Methods
    }
}
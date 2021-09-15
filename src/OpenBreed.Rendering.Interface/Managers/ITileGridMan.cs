using OpenTK;

namespace OpenBreed.Rendering.Interface.Managers
{
    /// <summary>
    /// Tile Grid Manager interface for handling rendering of tile grids 
    /// </summary>
    public interface ITileGridMan
    {
        #region Public Methods

        /// <summary>
        ///  Render tile grid with specific ID using clipping box limits
        /// </summary>
        /// <param name="tileGridId">ID of tile gird to render</param>
        /// <param name="clipBox">Clipping box used to limit rendering</param>
        void Render(int tileGridId, Box2 clipBox);

        /// <summary>
        /// Create a grid giving it's properties and return it's ID
        /// </summary>
        /// <param name="width">Width of grid</param>
        /// <param name="height">Height of grid</param>
        /// <param name="layersNo">Number of grid layers</param>
        /// <param name="cellSize">Grid cell size</param>
        /// <returns></returns>
        int CreateGrid(int width, int height, int layersNo, int cellSize);

        /// <summary>
        /// Modify single tile grid cell with new tile data
        /// </summary>
        /// <param name="tileGridId">ID of tile grid to modify</param>
        /// <param name="pos">Position of tile grid cell to modify</param>
        /// <param name="tileAtlasId">ID of tile atlas which will be set on found cell</param>
        /// <param name="tileImageId">ID of tile image which will be set on found cell</param>
        void ModifyTile(int tileGridId, Vector2 pos, int tileAtlasId, int tileImageId);

        /// <summary>
        /// Modify multiple tile grid cells with tile stamp
        /// </summary>
        /// <param name="tileGridId">ID of tile grid to modify</param>
        /// <param name="pos">Position of first tile grid cell to modify</param>
        /// <param name="stampId">ID of tile stamp which will be applied to cells</param>
        void ModifyTiles(int tileGridId, Vector2 pos, int stampId);

        #endregion Public Methods
    }
}
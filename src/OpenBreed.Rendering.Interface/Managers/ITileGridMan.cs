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
        /// Get tile grid by it's ID
        /// </summary>
        /// <param name="atlasId">Id of tile grid to get</param>
        /// <returns>Tile grid object</returns>
        ITileGrid GetById(int id);

        /// <summary>
        /// Get tile grid by it's name
        /// </summary>
        /// <param name="tileGridName">Name of tile grid to get</param>
        /// <returns>Tile grid object</returns>
        ITileGrid GetByName(string tileGridName);

        /// <summary>
        /// Checks if tile grid with given name already exists
        /// </summary>
        /// <param name="tileGridId">Name of tile grid to check</param>
        /// <returns>True if exits, false otherwise</returns>
        bool Contains(string tileGridId);

        /// <summary>
        /// Get tile grid name based on it's ID
        /// </summary>
        /// <param name="tileGridId">ID of tile grid</param>
        /// <returns>Tile grid name</returns>
        string GetName(int tileGridId);

        /// <summary>
        ///  Render tile grid with specific ID using clipping box limits
        /// </summary>
        /// <param name="tileGridId">ID of tile gird to render</param>
        /// <param name="clipBox">Clipping box used to limit rendering</param>
        void Render(int tileGridId, Box2 clipBox);

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
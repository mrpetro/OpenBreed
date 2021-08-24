namespace OpenBreed.Rendering.Interface
{
    /// <summary>
    /// Interface for tile atlas builder
    /// </summary>
    public interface ITileAtlasBuilder
    {
        #region Public Methods

        /// <summary>
        /// Sets created atlas name
        /// </summary>
        /// <param name="name">Proposed atlas name</param>
        /// <returns>This builder instance</returns>
        ITileAtlasBuilder SetName(string name);

        /// <summary>
        /// Sets created atlas tile size
        /// </summary>
        /// <param name="tileSize">Atlas tile size</param>
        /// <returns>This builder instance</returns>
        ITileAtlasBuilder SetTileSize(int tileSize);

        /// <summary>
        /// Sets atlas texture
        /// </summary>
        /// <param name="textureId">Texture ID</param>
        /// <returns>This builder instance</returns>
        ITileAtlasBuilder SetTexture(int textureId);

        /// <summary>
        /// Appends multiple tile coordinates based on given grid parameters
        /// </summary>
        /// <param name="columnsNo">Number of cell columns in tile grid</param>
        /// <param name="rowsNo">Number of cell rows in tile grid</param>
        /// <param name="offsetX">X coordinate offset for first cell</param>
        /// <param name="offsetY">Y coordinate offset for first cell</param>
        /// <returns>This builder instance</returns>
        ITileAtlasBuilder AppendCoordsFromGrid(int columnsNo, int rowsNo, int offsetX = 0, int offsetY = 0);

        /// <summary>
        /// Appends single tile coordinates based on given U and V texture coordinates
        /// </summary>
        /// <param name="u">U texture coordinate of tile</param>
        /// <param name="v">V texture coordinate of tile</param>
        /// <returns>This builder instance</returns> 
        ITileAtlasBuilder AppendCoords(int u, int v);

        /// <summary>
        /// Build tile atlas
        /// </summary>
        /// <returns>Tile atlas</returns>
        ITileAtlas Build();


        #endregion Public Methods
    }
}
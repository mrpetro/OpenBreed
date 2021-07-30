namespace OpenBreed.Rendering.Interface
{
    public interface ISpriteAtlasBuilder
    {
        #region Public Methods

        /// <summary>
        /// Sets created atlas name
        /// </summary>
        /// <param name="name">Proposed atlas name</param>
        /// <returns>This builder instance</returns>
        ISpriteAtlasBuilder SetName(string name);

        /// <summary>
        /// Sets atlas texture
        /// </summary>
        /// <param name="textureId">Texture ID</param>
        /// <returns>This builder instance</returns>
        ISpriteAtlasBuilder SetTexture(int textureId);

        /// <summary>
        /// Appends multiple sprite coordinates based on given grid parameters
        /// </summary>
        /// <param name="cellWidth">Width of each cell in sprite grid</param>
        /// <param name="cellHeight">Height of each cell in sprite grid</param>
        /// <param name="columnsNo">Number of cell columns in sprite grid</param>
        /// <param name="rowsNo">Number of cell rows in sprite grid</param>
        /// <param name="offsetX">X coordinate offset for first cell</param>
        /// <param name="offsetY">Y coordinate offset for first cell</param>
        /// <returns>This builder instance</returns>
        ISpriteAtlasBuilder AppendCoordsFromGrid(int cellWidth, int cellHeight, int columnsNo, int rowsNo, int offsetX = 0, int offsetY = 0);

        /// <summary>
        /// Build sprite atlas
        /// </summary>
        /// <returns>Sprite atlas</returns>
        ISpriteAtlas Build();

        #endregion Public Methods
    }
}
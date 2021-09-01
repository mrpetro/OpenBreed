namespace OpenBreed.Rendering.Interface
{
    public interface ITileGridBuilder
    {
        #region Public Methods

        ITileGridBuilder SetSize(int width, int height);

        ITileGridBuilder SetLayersNo(int layersNo);

        ITileGridBuilder SetCellSize(int cellSize);

        TileCell[] CreateTileArray();

        /// <summary>
        /// Build tile grid
        /// </summary>
        /// <returns>tile grid</returns>
        ITileGrid Build();

        #endregion Public Methods
    }
}
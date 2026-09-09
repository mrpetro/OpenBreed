namespace OpenBreed.Rendering.Abstractions
{
    /// <summary>
    /// Tile system cell data
    /// </summary>
    public interface ITileCell
    {
        /// <summary>
        /// Id of tile atlas.
        /// </summary>
        public int AtlasId { get; }

        /// <summary>
        /// Id of tile image from the atlas.
        /// </summary>
        public int ImageId { get; }

        /// <summary>
        /// Checks if cell is empty.
        /// </summary>
        public bool IsEmpty { get; }
    }
}
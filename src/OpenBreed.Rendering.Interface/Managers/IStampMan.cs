namespace OpenBreed.Rendering.Interface.Managers
{
    /// <summary>
    /// Stamp manager interface
    /// </summary>
    public interface IStampMan
    {
        /// <summary>
        /// Get stamp it's ID
        /// </summary>
        /// <param name="id">ID of stamp to get</param>
        /// <returns>Stamp object</returns>
        ITileStamp GetById(int id);

        /// <summary>
        /// Try to get stamp by it's name.
        /// </summary>
        /// <param name="name">Name of stamp to get.</param>
        /// <param name="tileStamp">Tile stamp object.</param>
        /// <returns>True if tile stamp was found, false otherwise.</returns>
        bool TryGetByName(string name, out ITileStamp tileStamp);

        /// <summary>
        /// Get stamp by it's name
        /// </summary>
        /// <param name="name">Name of stamp to get</param>
        /// <returns>Sprite atlas object</returns>
        ITileStamp GetByName(string name);

        /// <summary>
        /// Checks if stamp with specified name exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if exists, false otherwise</returns>
        bool Contains(string name);

        /// <summary>
        /// Create a stamp
        /// </summary>
        /// <returns>Stamp builder</returns>
        IStampBuilder Create();
    }
}
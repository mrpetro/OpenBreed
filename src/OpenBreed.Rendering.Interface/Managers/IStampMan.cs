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
        /// Get stamp by it's name
        /// </summary>
        /// <param name="name">Name of stamp to get</param>
        /// <returns>Sprite atlas object</returns>
        ITileStamp GetByName(string name);

        /// <summary>
        /// Create a stamp
        /// </summary>
        /// <returns>Stamp builder</returns>
        IStampBuilder Create();
    }
}
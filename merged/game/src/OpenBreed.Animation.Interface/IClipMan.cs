namespace OpenBreed.Animation.Interface
{
    /// <summary>
    /// Animation clip manager interface
    /// </summary>
    /// <typeparam name="TObject">Type of object which is animated</typeparam>
    public interface IClipMan<TObject>
    {
        #region Public Methods

        /// <summary>
        /// Creates a clip with given name and length
        /// </summary>
        /// <param name="name">Name of clip to create</param>
        /// <param name="length">Length of clip to create</param>
        /// <returns></returns>
        IClip<TObject> CreateClip(string name, float length);

        /// <summary>
        /// Get animation clip by it's ID
        /// </summary>
        /// <param name="id">ID of animation clip</param>
        /// <returns>Animation clip</returns>
        IClip<TObject> GetById(int id);

        /// <summary>
        /// Get animation clip by it's name
        /// </summary>
        /// <param name="name">Name of animation clip</param>
        /// <returns>Animation clip</returns>
        IClip<TObject> GetByName(string name);

        /// <summary>
        /// Try to get animation clip by it's name
        /// </summary>
        /// <param name="name">Name of clip to find</param>
        /// <param name="clip">Resulting animation clip</param>
        /// <returns>True if clip was found, false otherwise</returns>
        bool TryGetByName(string name, out IClip<TObject> clip);

        #endregion Public Methods
    }
}
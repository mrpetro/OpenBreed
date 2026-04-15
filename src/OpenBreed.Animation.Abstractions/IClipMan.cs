using OpenBreed.Animation.Abstractions.Builders;
using System;

namespace OpenBreed.Animation.Abstractions
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
        /// <returns>Clip builder instance.</returns>
        IReadOnlyClipBuilder<TObject> NewClip(string name, float length);

        /// <summary>
        /// Registers given clip
        /// </summary>
        /// <param name="clip">Clip to register</param>
        /// <returns>True if clip was registered successfully, false otherwise.</returns>
        bool Register(IReadOnlyClip<TObject> clip);

        /// <summary>
        /// Get animation clip by it's ID
        /// </summary>
        /// <param name="id">ID of animation clip</param>
        /// <returns>Animation clip</returns>
        IReadOnlyClip<TObject> GetById(int id);

        /// <summary>
        /// Get animation clip by it's name
        /// </summary>
        /// <param name="name">Name of animation clip</param>
        /// <returns>Animation clip</returns>
        IReadOnlyClip<TObject> GetByName(string name);

        /// <summary>
        /// Get animation clip ID by it's name
        /// </summary>
        /// <param name="name">Name of clip to find</param>
        /// <returns>Clip ID</returns>
        /// Throws when not found
        int GetId(string clipName);

        /// <summary>
        /// Try to get animation clip ID by it's name
        /// </summary>
        /// <param name="name">Name of clip to find</param>
        /// <param name="clip">Clip ID</param>
        /// <returns>True if clip was found, false otherwise.</returns>
        bool TryGetId(string name, out int clipId);

        /// <summary>
        /// Try to get animation clip by it's name
        /// </summary>
        /// <param name="name">Name of clip to find</param>
        /// <param name="clip">Resulting animation clip</param>
        /// <returns>True if clip was found, false otherwise</returns>
        bool TryGetByName(string name, out IReadOnlyClip<TObject> clip);

        #endregion Public Methods
    }
}
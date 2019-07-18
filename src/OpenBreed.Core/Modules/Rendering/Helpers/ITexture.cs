using System;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Basic texture interface
    /// </summary>
    public interface ITexture : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Internal Id of this texture, used for OpenGL for example
        /// </summary>
        int InternalId { get; }

        /// <summary>
        /// Texture manager Id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Width of this texture in pixels
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of this texture in pixels
        /// </summary>
        int Height { get; }

        #endregion Public Properties
    }
}
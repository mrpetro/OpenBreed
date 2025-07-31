using System;

namespace OpenBreed.Rendering.Abstractions
{
    public enum TextureDataMode
    {
        Rgba,
        Index,
    }

    /// <summary>
    /// Basic texture interface
    /// </summary>
    public interface ITexture
    {
        #region Public Properties

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

        /// <summary>
        /// Texture color mode
        /// </summary>
        TextureDataMode DataMode { get; }

        /// <summary>
        /// Palette index to be used as invisible
        /// Used only when DataMode is set to Index
        /// </summary>
        int MaskIndex { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Unload this texture from render context
        /// </summary>
        /// <param name="renderContext">Render context</param>
        void Unload(IRenderContext context);

        /// <summary>
        /// Uses  this texture on render context
        /// </summary>
        /// <param name="renderContext">Render context</param>
        void Use(IRenderContext renderContext);

        #endregion Public Methods
    }
}
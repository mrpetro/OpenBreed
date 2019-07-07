using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Textures manager class  which handles creating textures from various sources
    /// Or just retrieving them by ID or name.
    /// </summary>
    internal class TextureMan : ITextureMan
    {
        #region Private Fields

        private readonly List<ITexture> items = new List<ITexture>();

        #endregion Private Fields

        #region Internal Constructors

        internal TextureMan(OpenGLModule module)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal OpenGLModule Module { get; }

        #endregion Internal Properties

        #region Public Methods

        /// <summary>
        /// Creates texture object from image file path and return it
        /// If id parameter is not set, texture ID will be set to file path
        /// </summary>
        /// <param name="filePath">File path to image file</param>
        /// <param name="id">Optional ID of texture to create</param>
        /// <returns>ITexture object</returns>
        public ITexture Create(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            return AddFrom(filePath);
        }

        /// <summary>
        /// Unloads all textures
        /// </summary>
        public void UnloadAll()
        {
            foreach (var texture in items)
                texture.Dispose();

            items.Clear();
        }

        /// <summary>
        /// Get texture object by it's Id
        /// </summary>
        /// <param name="id">Given Id of texture</param>
        /// <returns>Return ITexture object if found, false otherwise</returns>
        public ITexture GetById(int id)
        {
            return items[id];
        }

        /// <summary>
        /// Creates texture object from given bitmap and return it
        /// </summary>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <returns>ITexture object</returns>
        public ITexture Create(Bitmap bitmap)
        {
            var texture = Texture.CreateFromBitmap(bitmap);
            texture.Id = items.Count;
            items.Add(texture);
            return texture;
        }

        #endregion Public Methods

        #region Internal Methods

        internal ITexture AddFrom(string filePath)
        {
            if (!File.Exists(filePath))
                throw new InvalidOperationException($"File '{filePath}' doesn't exist.");

            using (var bitmap = new Bitmap(filePath))
                return Create(bitmap);
        }

        #endregion Internal Methods
    }
}
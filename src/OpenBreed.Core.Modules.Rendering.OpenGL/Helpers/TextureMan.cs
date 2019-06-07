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
    public class TextureMan : ITextureMan
    {
        #region Private Fields

        private Dictionary<string, ITexture> textures = new Dictionary<string, ITexture>();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Creates texture object from image file path and return it
        /// If id parameter is not set, texture ID will be set to file path
        /// </summary>
        /// <param name="filePath">File path to image file</param>
        /// <param name="id">Optional ID of texture to create</param>
        /// <returns>ITexture object</returns>
        public ITexture Load(string filePath, string id = null)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var fullPath = Path.GetFullPath(filePath);

            ITexture texture = null;

            if (textures.TryGetValue(fullPath, out texture))
                return texture;

            return AddFrom(fullPath);
        }

        /// <summary>
        /// Unloads all textures
        /// </summary>
        public void UnloadAll()
        {
            foreach (var texture in textures.Values)
                texture.Dispose();

            textures.Clear();
        }

        /// <summary>
        /// Get texture object by it's ID
        /// </summary>
        /// <param name="id">Given ID of texture</param>
        /// <returns>Return ITexture object if found, false otherwise</returns>
        public ITexture GetById(string id)
        {
            ITexture texture = null;

            if (textures.TryGetValue(id, out texture))
                return texture;
            else
                return null;
        }

        /// <summary>
        /// Creates texture object from given bitmap and return it
        /// </summary>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <param name="id">Obligatory ID of texture to create</param>
        /// <returns>ITexture object</returns>
        public ITexture Load(Bitmap bitmap, string id)
        {
            ITexture texture;

            if (textures.TryGetValue(id, out texture))
                return texture;

            texture = Texture.CreateFromBitmap(bitmap);
            textures.Add(id, texture);
            return texture;
        }

        #endregion Public Methods

        #region Internal Methods

        internal ITexture AddFrom(string filePath)
        {
            if (!File.Exists(filePath))
                throw new InvalidOperationException($"File '{filePath}' doesn't exist.");

            using (var bitmap = new Bitmap(filePath))
                return Load(bitmap, filePath);
        }

        #endregion Internal Methods
    }
}
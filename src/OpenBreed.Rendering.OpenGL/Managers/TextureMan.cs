using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    /// <summary>
    /// Textures manager class which handles creating textures from various sources
    /// Or just retrieving them by ID or name.
    /// </summary>
    internal class TextureMan : ITextureMan
    {
        #region Private Fields

        private readonly Dictionary<string, ITexture> aliases = new Dictionary<string, ITexture>();
        private readonly List<ITexture> items = new List<ITexture>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal TextureMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        /// <summary>
        /// Unloads all textures
        /// </summary>
        public void UnloadAll()
        {
            foreach (var texture in items)
                texture.Dispose();

            items.Clear();
            aliases.Clear();
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

        public ITexture GetByAlias(string alias)
        {
            ITexture result = null;
            aliases.TryGetValue(alias, out result);
            return result;
        }

        /// <summary>
        /// Creates texture object from image file path and return it
        /// If id parameter is not set, texture ID will be set to file path
        /// </summary>
        /// <param name="alias">Alias name to access the texture</param>
        /// <param name="filePath">File path to image file</param>
        /// <returns>ITexture object</returns>
        public ITexture Create(string alias, string filePath)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(alias), "Alias is empty!");
            Debug.Assert(!string.IsNullOrWhiteSpace(filePath), "File path is empty!");

            ITexture result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            if (!File.Exists(filePath))
                throw new InvalidOperationException($"File '{filePath}' doesn't exist.");

            using (var bitmap = new Bitmap(filePath))
                return InternalCreate(alias, bitmap);
        }

        /// <summary>
        /// Creates texture object from given bitmap and return it
        /// </summary>
        /// <param name="alias">Alias name to access the texture</param>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <returns>ITexture object</returns>
        public ITexture Create(string alias, Bitmap bitmap)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(alias), "Alias is empty!");
            Debug.Assert(bitmap != null, "Bitmap is null!");

            ITexture result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            return InternalCreate(alias, bitmap);
        }

        #endregion Public Methods

        #region Internal Methods

        internal ITexture InternalCreate(string name, Bitmap bitmap)
        {
            var texture = Texture.CreateFromBitmap(bitmap);
            texture.Id = items.Count;
            items.Add(texture);
            aliases.Add(name, texture);

            logger.Verbose($"Texture '{name}' created with ID {texture.Id}.");

            return texture;
        }

        #endregion Internal Methods
    }
}
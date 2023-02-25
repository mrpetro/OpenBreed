using OpenBreed.Common.Interface.Logging;
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

        private readonly List<ITexture> items = new List<ITexture>();
        private readonly ILogger logger;
        private readonly Dictionary<string, ITexture> names = new Dictionary<string, ITexture>();

        #endregion Private Fields

        #region Public Constructors

        public TextureMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public ITexture Create(string name, int width, int height, byte[] data)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(name), "Name is empty!");
            Debug.Assert(width > 0, "Width is zero!");
            Debug.Assert(height > 0, "Height is zero!");
            Debug.Assert(data is not null, "Bitmap is null!");

            ITexture result;
            if (names.TryGetValue(name, out result))
                return result;

            var texture = Texture.CreateFromIndexArray(width, height, data);
            texture.Id = items.Count;
            items.Add(texture);
            names.Add(name, texture);

            logger.Verbose($"Texture '{name}' created with ID {texture.Id}.");

            return texture;
        }

        /// <summary>
        /// Creates texture object from image file path and return it
        /// If id parameter is not set, texture ID will be set to file path
        /// </summary>
        /// <param name="name">Name to access the texture</param>
        /// <param name="filePath">File path to image file</param>
        /// <returns>ITexture object</returns>
        public ITexture Create(string name, string filePath)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(name), "Alias is empty!");
            Debug.Assert(!string.IsNullOrWhiteSpace(filePath), "File path is empty!");

            ITexture result;
            if (names.TryGetValue(name, out result))
                return result;

            if (!File.Exists(filePath))
                throw new InvalidOperationException($"File '{filePath}' doesn't exist.");

            using (var bitmap = new Bitmap(filePath))
            {
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                return CreateFromBitmap(name, bitmap);
            }
        }

        /// <summary>
        /// Creates texture object from given bitmap and return it
        /// </summary>
        /// <param name="name">Name to access the texture</param>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <returns>ITexture object</returns>
        public ITexture Create(string name, Bitmap bitmap)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(name), "Alias is empty!");
            Debug.Assert(bitmap != null, "Bitmap is null!");

            ITexture result;
            if (names.TryGetValue(name, out result))
                return result;

            return CreateFromBitmap(name, bitmap);
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

        public ITexture GetByName(string name)
        {
            ITexture result = null;
            names.TryGetValue(name, out result);
            return result;
        }

        /// <summary>
        /// Unloads all textures
        /// </summary>
        public void UnloadAll()
        {
            foreach (var texture in items)
                texture.Dispose();

            items.Clear();
            names.Clear();
        }

        #endregion Public Methods

        #region Private Methods

        private ITexture CreateFromBitmap(string name, Bitmap bitmap)
        {
            var texture = Texture.CreateFromBitmap(bitmap);
            texture.Id = items.Count;
            items.Add(texture);
            names.Add(name, texture);

            logger.Verbose($"Texture '{name}' created with ID {texture.Id}.");

            return texture;
        }

        #endregion Private Methods
    }
}
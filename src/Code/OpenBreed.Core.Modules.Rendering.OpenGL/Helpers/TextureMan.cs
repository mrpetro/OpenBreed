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
        /// Find ITexture object by it's name,
        /// Return it if found, if not then create it from image under filePath
        /// </summary>
        /// <param name="filePath">Image file path</param>
        /// <param name="name">Name of texture object to return</param>
        /// <returns>ITexture object</returns>
        public ITexture Load(string filePath, string name = null)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var fullPath = Path.GetFullPath(filePath);

            ITexture texture = null;

            if (textures.TryGetValue(fullPath, out texture))
                return texture;

            return AddFrom(fullPath);
        }

        public void UnloadAll()
        {
            foreach (var texture in textures.Values)
                texture.Dispose();

            textures.Clear();
        }

        public ITexture GetByName(string name)
        {
            ITexture texture = null;

            if (textures.TryGetValue(name, out texture))
                return texture;
            else
                return null;
        }

        public ITexture GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find ITexture object by it's name,
        /// Return it if found, if not then create it from bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <param name="name">Name of texture object to return</param>
        /// <returns>ITexture object</returns>
        public ITexture Load(Bitmap bitmap, string name)
        {
            ITexture texture;

            if (textures.TryGetValue(name, out texture))
                return texture;

            texture = Texture.CreateFromBitmap(bitmap);
            textures.Add(name, texture);
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
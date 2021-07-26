using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal struct SpriteData
    {
        #region Public Fields

        /// <summary>
        /// U coordinate of sprite on texture
        /// </summary>
        public int U;

        /// <summary>
        /// V coordinate of sprite on texture
        /// </summary>
        public int V;

        /// <summary>
        /// Width of sprite on texture
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of sprite on texture
        /// </summary>
        public int Height;

        #endregion Public Fields

        #region Internal Fields

        /// <summary>
        /// OpenGL vertex buffer object ID
        /// </summary>
        internal int Vbo;

        #endregion Internal Fields

        #region Public Properties

        /// <summary>
        /// Size of sprite
        /// </summary>
        public Vector2 Size => new Vector2(Width, Height);

        #endregion Public Properties
    }

    internal class SpriteAtlas : ISpriteAtlas
    {
        #region Internal Fields

        internal readonly List<SpriteData> data;

        #endregion Internal Fields

        #region Internal Constructors

        internal SpriteAtlas(SpriteAtlasBuilder builder)
        {
            Texture = builder.Texture;
            data = builder.GetSpriteData();
            Id = builder.Register(this);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Id of this sprite atlas
        /// </summary>
        public int Id { get; }

        #endregion Public Properties

        #region Internal Properties

        internal ITexture Texture { get; }

        #endregion Internal Properties

        #region Public Methods

        public Vector2 GetSpriteSize(int spriteId)
        {
            return data[spriteId].Size;
        }

        #endregion Public Methods
    }
}
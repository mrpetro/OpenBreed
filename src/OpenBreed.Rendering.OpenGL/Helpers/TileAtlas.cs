using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal struct TileData
    {
        #region Public Fields

        /// <summary>
        /// U coordinate of tile on texture
        /// </summary>
        public int U;

        /// <summary>
        /// V coordinate of tile on texture
        /// </summary>
        public int V;

        #endregion Public Fields

        #region Internal Fields

        /// <summary>
        /// OpenGL vertex buffer object ID
        /// </summary>
        internal int Vbo;

        #endregion Internal Fields
    }

    internal class TileAtlas : ITileAtlas
    {
        #region Internal Fields

        internal readonly List<TileData> data;

        #endregion Internal Fields

        #region Public Constructors

        public TileAtlas(TileAtlasBuilder builder)
        {
            Texture = builder.Texture;
            TileSize = builder.TileSize;
            data = builder.GetTileData();
            Id = builder.Register(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public ITexture Texture { get; }
        public float TileSize { get; }
        public int Id { get; }

        #endregion Public Properties
    }
}
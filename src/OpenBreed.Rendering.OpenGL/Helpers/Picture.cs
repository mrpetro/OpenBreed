using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class Picture : IPicture
    {
        #region Public Constructors

        public Picture(PictureBuilder builder)
        {
            Texture = builder.Texture;
            Id = builder.Register(this);
            Vbo = builder.GetVbo();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Id of this picture
        /// </summary>
        public int Id { get; }

        #endregion Public Properties

        #region Internal Properties

        internal int Vbo { get; }

        internal ITexture Texture { get; }

        #endregion Internal Properties
    }
}
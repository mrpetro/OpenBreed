using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class FontAtlas : IFont
    {
        //public const string Characters = @" qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789µ§½!""#¤%&/()=?^*@£€${[]}\~¨'-_.:,;<>|°©®±¥ł";

        #region Private Fields

        private readonly Dictionary<int, (int, float)> Lookup = new Dictionary<int, (int, float)>();
        private int ibo;
        private List<int> vboList;

        #endregion Private Fields

        #region Internal Constructors

        internal FontAtlas(FontAtlasBuilder builder)
        {
            Characters = builder.Characters;
            Id = builder.Id;
            vboList = builder.vboList;
            ibo = builder.ibo;
            Height = builder.Height;
            Lookup = builder.Lookup;
            Texture = builder.Texture;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int[] Characters { get; }

        /// <summary>
        /// Id of this sprite atlas
        /// </summary>
        public int Id { get; }

        public float Height { get; }

        #endregion Public Properties

        #region Internal Properties

        internal ITexture Texture { get; }

        #endregion Internal Properties

        #region Public Methods

        public float GetWidth(char character)
        {
            return Lookup[character].Item2;
        }

        public float GetWidth(string text)
        {
            var totalWidth = 0.0f;
            for (int i = 0; i < text.Length; i++)
                totalWidth += Lookup[text[i]].Item2;
            return totalWidth;
        }

        public void Draw(char character)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.InternalId);
            RenderTools.Draw(vboList[Lookup[character].Item1], ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Draw(string text)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.InternalId);

            var offset = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var key = Lookup[ch].Item1;

                GL.Translate(offset, 0.0f, 0.0f);
                RenderTools.Draw(vboList[key], ibo, 6);
                offset = Lookup[ch].Item2 * 0.75f;
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Draw(int spriteId)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.InternalId);
            RenderTools.Draw(vboList[spriteId], ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        #endregion Public Methods
    }
}
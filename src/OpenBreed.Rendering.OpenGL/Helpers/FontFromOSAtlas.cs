using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class FontFromOSAtlas : IFont
    {
        #region Private Fields

        private readonly Dictionary<int, (int, float)> Lookup = new Dictionary<int, (int, float)>();
        private readonly List<int> vboList;
        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Internal Constructors

        internal FontFromOSAtlas(FontFromOSAtlasGenerator builder)
        {
            primitiveRenderer = builder.PrimitiveRenderer;
            Characters = builder.Characters;
            Id = builder.Id;
            vboList = builder.vboList;
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

        public void Draw(char character, Box2 clipBox)
        {
            var found = Lookup[character];
            primitiveRenderer.DrawSprite(Texture, vboList[found.Item1], new Vector3(0, 0, 0), new Vector2(found.Item2, Height));
        }

        public void Draw(string text, Box2 clipBox)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.InternalId);

            var offsetX = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var key = Lookup[ch].Item1;

                primitiveRenderer.DrawSprite(Texture, vboList[key], new Vector3((int)offsetX, 0.0f, 0.0f), new Vector2(Lookup[ch].Item2, Height));

                offsetX += Lookup[ch].Item2;
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Render(string text, Box2 clipBox, Vector2 pos)
        {
            GL.Enable(EnableCap.Texture2D);

            primitiveRenderer.PushMatrix();
            primitiveRenderer.Translate(new Vector3((int)pos.X, (int)pos.Y, 0.0f));

            var caretPosX = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];

                switch (ch)
                {
                    case '\r':
                        primitiveRenderer.Translate(new Vector3(-caretPosX, 0.0f, 0.0f));
                        caretPosX = 0.0f;
                        continue;
                    case '\n':
                        primitiveRenderer.Translate(new Vector3(0.0f, -Height, 0.0f));
                        continue;
                    default:
                        break;
                }

                Draw(ch, clipBox);
                var width = GetWidth(ch);
                caretPosX += width;
                primitiveRenderer.Translate(new Vector3(width, 0.0f, 0.0f));
            }

            primitiveRenderer.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Public Methods
    }
}
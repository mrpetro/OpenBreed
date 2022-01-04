using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class FontFromSpritesAtlas : IFont
    {
        #region Private Fields

        private readonly ISpriteMan spriteMan;
        private readonly ISpriteRenderer spriteRenderer;
        private readonly int atlasId;

        private readonly Dictionary<int, (int, int, float)> Lookup = new Dictionary<int, (int, int, float)>();

        #endregion Private Fields

        #region Internal Constructors

        internal FontFromSpritesAtlas(FontFromSpritesAtlasBuilder builder)
        {
            spriteMan = builder.SpriteMan;
            spriteRenderer = builder.SpriteRenderer;
            Id = builder.Id;
            atlasId = builder.AtlasId;
            Lookup = builder.Lookup;
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

        public void Render(string text, Box2 clipBox, Vector2 pos)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate((int)pos.X, (int)pos.Y, 0.0f);

            var caretPosX = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];

                switch (ch)
                {
                    case '\r':
                        GL.Translate(-caretPosX, 0.0f, 0.0f);
                        caretPosX = 0.0f;
                        continue;
                    case '\n':
                        GL.Translate(0.0f, -Height, 0.0f);
                        continue;
                    default:
                        break;
                }

                Draw(ch, clipBox);
                var width = GetWidth(ch);
                caretPosX += width;
                GL.Translate(width, 0.0f, 0.0f);
            }

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        public void Draw(char ch, Box2 clipBox)
        {
            (int, int, float) data;
            if (Lookup.TryGetValue(ch, out data))
            {
                var atlasId = data.Item1;
                var spriteIndex = data.Item2;
                spriteRenderer.Render(new Vector3(0,0,100), atlasId, spriteIndex);
            }
        }

        public void Draw(string text, Box2 clipBox)
        {
            var offset = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var atlasId = Lookup[ch].Item1;
                var spriteIndex = Lookup[ch].Item2;

                GL.Translate(offset, 0.0f, 0.0f);

                spriteRenderer.Render(new Vector3(0, 0, 100), atlasId, spriteIndex);

                offset = Lookup[ch].Item3;
            }
        }

        public void Draw(int spriteId)
        {

        }

        #endregion Public Methods
    }
}
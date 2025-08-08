using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class FontAtlas : IFontAtlas
    {
        #region Private Fields

        private readonly ISpriteAtlas spriteAtlas;

        private readonly Dictionary<int, FontCharData> lookup;

        #endregion Private Fields

        #region Internal Constructors

        internal FontAtlas(FontAtlasBuilder builder)
        {
            Id = builder.Id;
            spriteAtlas = builder.SpriteAtlas;
            lookup = builder.Lookup;
            Height = builder.Height;
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
            return lookup[character].Width;
        }

        public float GetWidth(string text)
        {
            var totalWidth = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                totalWidth += lookup[text[i]].Width;
            }

            return totalWidth;
        }

        public void Draw(IRenderView view, string text, Color4 color, Box2 clipBox, bool ignoreScale = false)
        {
            var caretPosX = 0.0f;
            var caretPosY = 0.0f;

            var scaleCorrection = 1.0f;

            if (ignoreScale)
            {
                scaleCorrection = 1.0f / view.GetScale();
            }

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var data = lookup[ch];

                var width = data.Width * scaleCorrection;

                var spriteId = data.SpriteId;

                switch (ch)
                {
                    case '\r':
                        caretPosX = 0.0f;
                        continue;
                    case '\n':
                        caretPosY -= Height;
                        continue;
                    default:
                        break;
                }

                view.Context.SpriteRenderer.Render(view, new Vector3(caretPosX, caretPosY, 0.0f), Vector2.One, color, spriteAtlas.Id, spriteId, ignoreScale);

                caretPosX += width;
            }
        }

        #endregion Public Methods
    }
}
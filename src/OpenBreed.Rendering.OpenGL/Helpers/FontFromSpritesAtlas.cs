using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class FontFromSpritesAtlas : IFont
    {
        #region Private Fields

        private readonly ISpriteMan spriteMan;
        private readonly ISpriteRenderer spriteRenderer;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly int atlasId;

        private readonly Dictionary<int, (int, int, float, float)> Lookup = new Dictionary<int, (int, int, float, float)>();

        #endregion Private Fields

        #region Internal Constructors

        internal FontFromSpritesAtlas(FontFromSpritesAtlasBuilder builder)
        {
            spriteMan = builder.SpriteMan;
            spriteRenderer = builder.SpriteRenderer;
            primitiveRenderer = builder.PrimitiveRenderer; 
            Id = builder.Id;
            atlasId = builder.AtlasId;
            Lookup = builder.Lookup;
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
            return Lookup[character].Item2;
        }

        public float GetWidth(string text)
        {
            var totalWidth = 0.0f;
            for (int i = 0; i < text.Length; i++)
                totalWidth += Lookup[text[i]].Item2;
            return totalWidth;
        }

        public void Render(string text, Box2 clipBox, Vector2 pos, float order)
        {
            GL.Enable(EnableCap.Texture2D);
            primitiveRenderer.PushMatrix();

            primitiveRenderer.Translate((int)pos.X, (int)pos.Y, order);

            var caretPosX = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];

                switch (ch)
                {
                    case '\r':
                        primitiveRenderer.Translate(-caretPosX, 0.0f, 0.0f);
                        caretPosX = 0.0f;
                        continue;
                    case '\n':
                        primitiveRenderer.Translate(0.0f, -Height, 0.0f);
                        continue;
                    default:
                        break;
                }

                Draw(ch, clipBox);
                var width = GetWidth(ch);
                caretPosX += width;
                primitiveRenderer.Translate(width, 0.0f, 0.0f);
            }

            primitiveRenderer.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        public void Draw(char ch, Box2 clipBox)
        {
            (int, int, float, float) data;
            if (Lookup.TryGetValue(ch, out data))
            {
                var atlasId = data.Item1;
                var spriteIndex = data.Item2;
                spriteRenderer.Render(new Vector3(0, 0, 0), Vector2.One, Color4.White, atlasId, spriteIndex);
            }
        }

        public void Draw(string text, Color4 color, Box2 clipBox)
        {
            var caretPosX = 0.0f;
            var caretPosY = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var data = Lookup[ch];
                var atlasId = data.Item1;
                var spriteIndex = data.Item2;
                var w = data.Item3;
                var h = data.Item4;

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

                spriteRenderer.Render(new Vector3(caretPosX, caretPosY - h, 0.0f), Vector2.One, color, atlasId, spriteIndex);

                caretPosX += w;
            }
        }

        public void Draw(int spriteId)
        {

        }

        #endregion Public Methods
    }
}
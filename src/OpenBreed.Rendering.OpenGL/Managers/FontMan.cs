using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class FontMan : IFontMan
    {
        #region Private Fields

        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;
        private readonly ISpriteRenderer spriteRenderer;
        private readonly List<IFont> items = new List<IFont>();
        private readonly Dictionary<string, IFont> aliases = new Dictionary<string, IFont>();
        private readonly TextMeasurer textMeasurer = new TextMeasurer();
        private readonly Dictionary<string, FontFromOSAtlas> fonts = new Dictionary<string, FontFromOSAtlas>();

        #endregion Private Fields

        #region Internal Constructors

        internal FontMan(ITextureMan textureMan, ISpriteMan spriteMan, ISpriteRenderer spriteRenderer)
        {
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
            this.spriteRenderer = spriteRenderer;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IFont GetById(int id)
        {
            return items[id];
        }

        public IFontAtlasBuilder Create()
        {
            return new FontFromSpritesAtlasBuilder(this, spriteMan, spriteRenderer);
        }

        public void RenderPart(int fontId, string text, Vector2 origin, float order, Box2 clipBox)
        {
            GL.Translate(origin.X, origin.Y, 0.0f);
            GetById(fontId).Draw(text, clipBox);
        }

        public void RenderAppend(int fontId, string text, Box2 clipBox, Vector2 value)
        {
            GetById(fontId).Draw(text, clipBox);
        }

        public IFont GetGfxFont(string fontName)
        {
            var alias = $"Gfx/{fontName}";

            IFont result;
            if (aliases.TryGetValue(alias, out result))
                return result;
            else
                return null;
        }

        public IFont GetOSFont(string fontName, int fontSize)
        {
            fontName = fontName.Trim().ToLower();

            var alias = $"OS/{fontName}/{fontSize}";
            IFont result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            var fontGenerator = new FontFromOSAtlasGenerator(this, textMeasurer, textureMan);
            fontGenerator.SetName(fontName);
            fontGenerator.SetSize(fontSize);
            var font = fontGenerator.Build();
            Register(alias, font);
            return font;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Register(string alias, IFont font)
        {
            items.Add(font);
            aliases.Add(alias, font);
        }

        internal int GenerateNewId()
        {
            return items.Count;
        }

        public void Render(Box2 clipBox, float dt, FontRenderer fontRenderer)
        {
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            fontRenderer.Invoke(clipBox, dt);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        public void RenderStart(Vector2 pos)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(pos.X, pos.Y, 0.0f);

        }

        public void RenderEnd()
        {
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Internal Methods
    }
}
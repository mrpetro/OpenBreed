using OpenBreed.Common.Tools;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class FontMan : IFontMan
    {
        #region Private Fields

        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;
        private readonly ISpriteRenderer spriteRenderer;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly List<IFont> items = new List<IFont>();
        private readonly Dictionary<string, IFont> aliases = new Dictionary<string, IFont>();
        private readonly TextMeasurer textMeasurer = new TextMeasurer();
        private readonly Dictionary<string, FontFromOSAtlas> fonts = new Dictionary<string, FontFromOSAtlas>();

        #endregion Private Fields

        #region Public Constructors

        public FontMan(ITextureMan textureMan, ISpriteMan spriteMan, ISpriteRenderer spriteRenderer, IPrimitiveRenderer primitiveRenderer)
        {
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
            this.spriteRenderer = spriteRenderer;
            this.primitiveRenderer = primitiveRenderer;
        }

        #endregion Public Constructors

        #region Public Methods

        public IFont GetById(int id)
        {
            return items[id];
        }

        public IFontAtlasBuilder Create()
        {
            return new FontFromSpritesAtlasBuilder(this, spriteMan, spriteRenderer, primitiveRenderer);
        }

        public void RenderPart(IRenderView view, int fontId, string text, Vector2 origin, Color4 color, float order, Box2 clipBox)
        {
            view.Translate(new Vector3(origin.X, origin.Y, order));
            GetById(fontId).Draw(view, text, color, clipBox);
        }

        public void RenderAppend(IRenderView view, int fontId, string text, Box2 clipBox, Vector2 value)
        {
            GetById(fontId).Draw(view, text, Color4.White, clipBox);
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

            var fontGenerator = new FontFromOSAtlasGenerator(this, textMeasurer, textureMan, primitiveRenderer);
            fontGenerator.SetName(fontName);
            fontGenerator.SetSize(fontSize);
            var font = fontGenerator.Build();
            Register(alias, font);
            return font;
        }

        public void Render(IRenderView view, Box2 clipBox, float dt, FontRenderer fontRenderer)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);

            fontRenderer.Invoke(view, clipBox, dt);

            GL.Disable(EnableCap.Blend);
        }

        public void RenderStart(IRenderView view, Vector2 pos)
        {
            view.PushMatrix();
            view.Translate(new Vector3(pos.X, pos.Y, 0.0f));

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.BlendColor(Color4.Black);
        }

        public void RenderEnd(IRenderView view)
        {
            view.PopMatrix();

            GL.Disable(EnableCap.Blend);
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

        #endregion Internal Methods
    }
}
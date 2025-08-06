using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    public class FontAtlasBuilder : IFontAtlasBuilder
    {
        #region Private Fields

        private readonly FontMan fontMan;

        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Internal Constructors

        internal FontAtlasBuilder(FontMan fontMan,
            ISpriteMan spriteMan)
        {
            this.fontMan = fontMan;
            this.spriteMan = spriteMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal Dictionary<int, FontCharData> Lookup { get; } = new Dictionary<int, FontCharData>();
        internal ISpriteAtlas SpriteAtlas { get; private set; }
        internal int[] Characters { get; private set; }

        internal int Id { get; private set; }
        internal string Name { get; private set; }
        internal float Height { get; private set; }
        internal string Alias { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public IFontAtlas Build()
        {
            Id = fontMan.GenerateNewId();

            Characters = new int[256];

            for (int i = 0; i < 256; i++)
            {
                Characters[i] = i;
            }

            var newFont = new FontAtlas(this);

            fontMan.Register(Alias, newFont);

            return newFont;
        }

        public IFontAtlasBuilder SetSpriteAtlas(string spriteAtlasName)
        {
            if (!spriteMan.TryGetByName(spriteAtlasName, out ISpriteAtlas spriteAtlas))
            {
                throw new System.Exception($"Expected sprite atlas with name '{spriteAtlasName}'.");
            }

            SpriteAtlas = spriteAtlas;

            return this;
        }

        public IFontAtlasBuilder MapCharacterToSpriteId(int ch, int spriteId, float width = 0.0f, float xOffset = 0.0f)
        {
            var size = SpriteAtlas.GetSpriteSize(spriteId);
            Lookup.Add(ch, new FontCharData(spriteId, xOffset, width == 0.0f ? size.X : width));

            return this;
        }

        public IFontAtlasBuilder SetAlias(string alias)
        {
            Alias = alias;
            return this;
        }

        public IFontAtlasBuilder SetHeight(float height)
        {
            Height = height;
            return this;
        }

        public IFontAtlasBuilder SetName(string fontName)
        {
            Name = fontName;
            return this;
        }

        #endregion Public Methods
    }
}
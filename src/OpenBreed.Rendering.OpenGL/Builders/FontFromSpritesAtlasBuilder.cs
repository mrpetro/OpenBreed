﻿using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    public class FontFromSpritesAtlasBuilder : IFontAtlasBuilder
    {
        #region Private Fields

        private readonly FontMan fontMan;

        #endregion Private Fields

        #region Internal Constructors

        internal FontFromSpritesAtlasBuilder(FontMan fontMan,
                                             ISpriteMan spriteMan,
                                             ISpriteRenderer spriteRenderer,
                                             IPrimitiveRenderer primitiveRenderer)
        {
            this.fontMan = fontMan;
            SpriteMan = spriteMan;
            SpriteRenderer = spriteRenderer;
            PrimitiveRenderer = primitiveRenderer;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal Dictionary<int, (int, int, float, float)> Lookup { get; } = new Dictionary<int, (int, int, float, float)>();
        internal int AtlasId { get; private set; }

        internal ISpriteMan SpriteMan { get; }
        internal ISpriteRenderer SpriteRenderer { get; }
        internal IPrimitiveRenderer PrimitiveRenderer { get; }
        internal int[] Characters { get; private set; }

        internal int Id { get; private set; }
        internal string Name { get; private set; }
        internal float Height { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public IFont Build()
        {
            Id = fontMan.GenerateNewId();

            Characters = new int[256];
            for (int i = 0; i < 256; i++)
                Characters[i] = i;

            var newFont = new FontFromSpritesAtlas(this);

            fontMan.Register($"Gfx/{Name}", newFont);

            return newFont;
        }

        public IFontAtlasBuilder AddCharacterFromSprite(int ch, string spriteAtlasName, int spriteIndex, float width = 0.0f)
        {
            if (!SpriteMan.TryGetByName(spriteAtlasName, out ISpriteAtlas sptiteAtlas))
            {
                throw new System.Exception($"Expected sprite atlas with name '{spriteAtlasName}'.");
            }

            var size = sptiteAtlas.GetSpriteSize(spriteIndex);
            Lookup.Add(ch, (sptiteAtlas.Id, spriteIndex, width == 0.0f ? size.X : width, size.Y));

            return this;
        }

        //public IFontAtlasBuilder AddWhiteChar(int ch, float width)
        //{
        //    var sptiteAtlas = SpriteMan.GetByName(spriteAtlasName);
        //    var size = sptiteAtlas.GetSpriteSize(spriteIndex);
        //    Lookup.Add(ch, (sptiteAtlas.Id, spriteIndex, size.X, size.Y));

        //    return this;
        //}

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

        #region Internal Methods

        #endregion Internal Methods
    }
}
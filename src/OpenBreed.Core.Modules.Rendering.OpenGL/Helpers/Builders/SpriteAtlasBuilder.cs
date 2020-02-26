using OpenBreed.Core.Modules.Rendering.Managers;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers.Builders
{
    public class SpriteAtlasBuilder
    {
        #region Internal Fields

        internal readonly List<Vector2> coords = new List<Vector2>();

        internal Vector2 offset;

        #endregion Internal Fields

        #region Private Fields

        private SpriteMan spriteMan;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteAtlasBuilder(SpriteMan spriteMan)
        {
            this.spriteMan = spriteMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int SpriteWidth { get; private set; }

        internal int SpriteHeight { get; private set; }

        internal ITexture Texture { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void BuildCoords(int spriteRows, int spriteColumns)
        {
            coords.Clear();

            for (int y = 0; y < spriteRows; y++)
            {
                for (int x = 0; x < spriteColumns; x++)
                {
                    var coord = Vector2.Multiply(new Vector2(x, y), new Vector2(SpriteWidth, SpriteHeight));
                    coords.Add(coord);
                }
            }
        }

        internal void SetOffset(int offsetX, int offsetY)
        {
            offset = new Vector2(offsetX, offsetY);
        }

        public void SetSpriteSize(int width, int height)
        {
            SpriteWidth = width;
            SpriteHeight = height;
        }

        public void SetTexture(ITexture texture)
        {
            Texture = texture;
        }

        public ISpriteAtlas Build()
        {
            return new SpriteAtlas(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal int GetNewId()
        {
            return spriteMan.GenerateNewId();
        }

        #endregion Internal Methods
    }
}
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class SpriteMan : ISpriteMan
    {
        #region Private Fields

        private readonly List<ISpriteAtlas> items = new List<ISpriteAtlas>();

        #endregion Private Fields

        #region Public Methods

        public ISpriteAtlas Create(ITexture texture, int spriteWidth, int spriteHeight, int spriteColumns, int spriteRows)
        {
            var newSpriteAtlas = new SpriteAtlas(items.Count, texture, spriteWidth, spriteHeight, spriteColumns, spriteRows);
            items.Add(newSpriteAtlas);
            return newSpriteAtlas;
        }

        public ISpriteAtlas GetById(int id)
        {
            return items[id];
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
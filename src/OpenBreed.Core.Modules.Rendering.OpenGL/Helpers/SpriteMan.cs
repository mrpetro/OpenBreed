using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class SpriteMan : ISpriteMan
    {
        #region Private Fields

        private readonly List<ISpriteAtlas> items = new List<ISpriteAtlas>();

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteMan(OpenGLModule module)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
        }

        #endregion Internal Constructors

        #region Public Properties

        public OpenGLModule Module { get; }

        #endregion Public Properties

        #region Public Methods

        public ISpriteAtlas Create(ITexture texture, int spriteWidth, int spriteHeight, int spriteColumns, int spriteRows)
        {
            var saBuilder = new SpriteAtlasBuilder(this);
            saBuilder.SetTexture(texture);
            saBuilder.SetSpriteSize(spriteWidth, spriteHeight);
            saBuilder.BuildCoords(spriteRows, spriteColumns);
            var newSpriteAtlas = new SpriteAtlas(saBuilder);
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

        #region Internal Methods

        internal int GenerateNewId()
        {
            return items.Count;
        }

        #endregion Internal Methods
    }
}
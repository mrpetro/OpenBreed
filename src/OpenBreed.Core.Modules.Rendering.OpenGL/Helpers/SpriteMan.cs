using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Rendering.Helpers.Builders;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    internal class SpriteMan : ISpriteMan
    {
        #region Private Fields

        private readonly List<ISpriteAtlas> items = new List<ISpriteAtlas>();
        private readonly Dictionary<string, ISpriteAtlas> aliases = new Dictionary<string, ISpriteAtlas>();

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteMan(OpenGLModule module)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
        }

        #endregion Internal Constructors

        #region Public Properties

        internal  OpenGLModule Module { get; }

        #endregion Public Properties

        #region Public Methods

        public ISpriteAtlas Create(string alias, int textureId, int spriteWidth, int spriteHeight, int spriteColumns, int spriteRows, int offsetX = 0, int offsetY = 0)
        {
            ISpriteAtlas result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            var saBuilder = new SpriteAtlasBuilder(this);

            var texture = Module.Textures.GetById(textureId);
            saBuilder.SetTexture(texture);
            saBuilder.SetSpriteSize(spriteWidth, spriteHeight);
            saBuilder.SetOffset(offsetX, offsetY);
            saBuilder.BuildCoords(spriteRows, spriteColumns);
            result = saBuilder.Build();
            items.Add(result);
            aliases.Add(alias, result);
            return result;
        }

        public ISpriteAtlas GetById(int id)
        {
            return items[id];
        }

        public ISpriteAtlas GetByAlias(string alias)
        {
            ISpriteAtlas result = null;
            aliases.TryGetValue(alias, out result);
            return result;
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
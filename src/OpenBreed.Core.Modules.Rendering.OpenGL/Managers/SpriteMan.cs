using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Helpers.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Modules.Rendering.Managers
{
    internal class SpriteMan : ISpriteMan
    {
        #region Private Fields

        private readonly List<ISpriteAtlas> items = new List<ISpriteAtlas>();
        private readonly Dictionary<string, ISpriteAtlas> names = new Dictionary<string, ISpriteAtlas>();
        private ISpriteAtlas MissingSpriteAtlas;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteMan(OpenGLModule module)
        {
            Debug.Assert(module != null, $"Argument '{nameof(module)}' must be non-null.");

            Module = module ?? throw new ArgumentNullException(nameof(module));

            //MissingSpriteAtlas = Create("Animations/Missing", 1.0f);
        }

        #endregion Internal Constructors

        #region Public Properties

        internal  OpenGLModule Module { get; }

        #endregion Public Properties

        #region Public Methods

        public ISpriteAtlas Create(string alias, int textureId, int spriteWidth, int spriteHeight, int spriteColumns, int spriteRows, int offsetX = 0, int offsetY = 0)
        {
            ISpriteAtlas result;
            if (names.TryGetValue(alias, out result))
                return result;

            var saBuilder = new SpriteAtlasBuilder(this);

            var texture = Module.Textures.GetById(textureId);
            saBuilder.SetTexture(texture);
            saBuilder.SetSpriteSize(spriteWidth, spriteHeight);
            saBuilder.SetOffset(offsetX, offsetY);
            saBuilder.BuildCoords(spriteRows, spriteColumns);
            result = saBuilder.Build();
            items.Add(result);
            names.Add(alias, result);
            return result;
        }

        public ISpriteAtlas GetById(int id)
        {
            return items[id];
        }

        public ISpriteAtlas GetByName(string name)
        {
            if (names.TryGetValue(name, out ISpriteAtlas result))
                return result;

            Module.Core.Logging.Error($"Unable to find animation with name '{name}'");

            return MissingSpriteAtlas;

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
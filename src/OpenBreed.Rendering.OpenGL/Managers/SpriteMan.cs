using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class SpriteMan : ISpriteMan
    {
        #region Private Fields

        private readonly Queue<ISpriteAtlas> loadQueue = new Queue<ISpriteAtlas>();
        private readonly List<SpriteAtlas> items = new List<SpriteAtlas>();
        private readonly Dictionary<string, ISpriteAtlas> names = new Dictionary<string, ISpriteAtlas>();
        private readonly ITextureMan textureMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        public SpriteMan(ITextureMan textureMan,
                         ILogger logger)
        {
            this.textureMan = textureMan;
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public ISpriteAtlasBuilder CreateAtlas()
        {
            return new SpriteAtlasBuilder(this, textureMan);
        }

        public ISpriteAtlas GetById(int atlasId)
        {
            return InternalGetById(atlasId);
        }

        public bool Contains(string atlasName)
        {
            return names.ContainsKey(atlasName);
        }

        public string GetName(int atlasId)
        {
            //TODO: Very ineffective. Name should be part of ISpriteAtlas object.
            return names.First(pair => pair.Value == items[atlasId]).Key;
        }

        public ISpriteAtlas GetByName(string name)
        {
            if (names.TryGetValue(name, out ISpriteAtlas result))
                return result;

            logger.LogError("Unable to find sprite atlas with name '{0}'.", name);

            return null;
        }

        public bool TryGetByName(string atlasName, out ISpriteAtlas spriteAtlas )
        {
            return names.TryGetValue(atlasName, out spriteAtlas);
        }

        public void UnloadAll(IRenderContext context)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Internal Methods

        internal SpriteAtlas InternalGetById(int atlasId)
        {
            return items[atlasId];
        }

        internal int Register(string name, SpriteAtlas spriteAtlas)
        {
            items.Add(spriteAtlas);
            names.Add(name, spriteAtlas);
            loadQueue.Enqueue(spriteAtlas);

            logger.LogTrace("Sprite atlas '{0}' created with ID {1}.", name, items.Count - 1);

            return items.Count - 1;
        }

        #endregion Internal Methods

        #region Private Methods



        #endregion Private Methods
    }
}
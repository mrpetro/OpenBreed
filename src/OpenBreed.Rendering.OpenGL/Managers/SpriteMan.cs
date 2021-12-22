using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class SpriteMan : ISpriteMan
    {
        #region Private Fields

        private readonly List<SpriteAtlas> items = new List<SpriteAtlas>();
        private readonly Dictionary<string, SpriteAtlas> names = new Dictionary<string, SpriteAtlas>();
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
            if (names.TryGetValue(name, out SpriteAtlas result))
                return result;

            logger.Error($"Unable to find sprite with name '{name}'");

            return null;
        }

        public void UnloadAll()
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

            logger.Verbose($"Sprite atlas '{name}' created with ID {items.Count - 1}.");

            return items.Count - 1;
        }

        internal int CreateSpriteVertices(SpriteData spriteData, int width, int height)
        {
            var vertices = CreateVertices(spriteData, width, height);
            int vbo;
            RenderTools.CreateVertexArray(vertices, out vbo);
            return vbo;
        }

        #endregion Internal Methods

        #region Private Methods

        private Vertex[] CreateVertices(SpriteData data, int textureWidth, int textureHeight)
        {
            var uvCoord = Vector2.Divide(new Vector2(data.U, data.V), new Vector2(textureWidth, textureHeight));
            var uvSize = Vector2.Divide(new Vector2(data.Width, data.Height), new Vector2(textureWidth, textureHeight));

            var uvLD = new Vector2(uvCoord.X, uvCoord.Y);
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(data.Width,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(data.Width,  data.Height), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   data.Height),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        #endregion Private Methods
    }
}
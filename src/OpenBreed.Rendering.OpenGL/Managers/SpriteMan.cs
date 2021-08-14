using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class SpriteMan : ISpriteMan
    {
        #region Private Fields

        private readonly uint[] indicesArray = {
                                            0,1,2,
                                            0,2,3
                                       };

        private readonly int ibo;
        private readonly List<SpriteAtlas> items = new List<SpriteAtlas>();
        private readonly Dictionary<string, SpriteAtlas> names = new Dictionary<string, SpriteAtlas>();
        private readonly ITextureMan textureMan;
        private readonly ILogger logger;
        private ISpriteAtlas MissingSpriteAtlas;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteMan(ITextureMan textureMan, ILogger logger)
        {
            this.textureMan = textureMan;
            this.logger = logger;

            //MissingSpriteAtlas = Create("Animations/Missing", 1.0f);

            RenderTools.CreateIndicesArray(indicesArray, out ibo);
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int Ibo => ibo;

        #endregion Internal Properties

        #region Public Methods

        public ISpriteAtlasBuilder CreateAtlas()
        {
            return new SpriteAtlasBuilder(this, textureMan);
        }

        public ISpriteAtlas GetById(int id)
        {
            return items[id];
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

            logger.Error($"Unable to find animation with name '{name}'");

            return MissingSpriteAtlas;
        }

        public void Render(int atlasId, int imageId, Vector2 pos, float order, Box2 clipBox)
        {
            var atlas = items[atlasId];

            if (imageId >= atlas.data.Count)
                return;

            var spriteSize = atlas.GetSpriteSize(imageId);

            //Test viewport for clippling here
            if (pos.X + spriteSize.X < clipBox.Left)
                return;

            if (pos.X > clipBox.Right)
                return;

            if (pos.Y + spriteSize.Y < clipBox.Bottom)
                return;

            if (pos.Y > clipBox.Top)
                return;

            GL.PushMatrix();

            GL.Translate((int)pos.X, (int)pos.Y, order);

            GL.BindTexture(TextureTarget.Texture2D, atlas.Texture.InternalId);
            RenderTools.Draw(atlas.data[imageId].Vbo, ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.PopMatrix();
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Internal Methods

        internal int Register(string name, SpriteAtlas spriteAtlas)
        {
            items.Add(spriteAtlas);
            names.Add(name, spriteAtlas);

            logger.Verbose($"Sprite Atlas {items.Count - 1} ({name}) created.");

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
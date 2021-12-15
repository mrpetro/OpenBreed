using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    internal class SpriteAtlasBuilder : ISpriteAtlasBuilder
    {
        #region Internal Fields

        internal readonly List<Box2> bounds = new List<Box2>();

        internal Vector2 offset = Vector2.Zero;

        #endregion Internal Fields

        #region Private Fields

        private readonly ITextureMan textureMan;
        private SpriteMan spriteMan;

        #endregion Private Fields

        #region Internal Constructors

        internal SpriteAtlasBuilder(SpriteMan spriteMan, ITextureMan textureMan)
        {
            this.spriteMan = spriteMan;
            this.textureMan = textureMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal ITexture Texture { get; private set; }

        internal string Name { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public ISpriteAtlasBuilder SetName(string name)
        {
            if (spriteMan.Contains(name))
                throw new InvalidOperationException($"Atlas with name '{name}' already exists.");

            Name = name;
            return this;
        }

        public ISpriteAtlasBuilder AppendCoordsFromGrid(int cellWidth, int cellHeight, int columnsNo, int rowsNo, int offsetX = 0, int offsetY = 0)
        {
            if (Texture is null)
                throw new InvalidOperationException("Texture must be set first.");

            if (cellWidth <= 0)
                throw new InvalidOperationException("Appended grid cell width are less or equal zero.");

            if (cellHeight <= 0)
                throw new InvalidOperationException("Appended grid cell height are less or equal zero.");

            if (columnsNo <= 0)
                throw new InvalidOperationException("Appended grid cell columns number are less or equal zero.");

            if (rowsNo <= 0)
                throw new InvalidOperationException("Appended grid cell rows number are less or equal zero.");

            if (cellWidth * (columnsNo - 1) >= Texture.Width)
                throw new InvalidOperationException("Appended grid X coordinates of sprites in texture are greater than texture width.");

            if (cellHeight * (rowsNo - 1) >= Texture.Height)
                throw new InvalidOperationException("Appended grid Y coordinates of sprites in texture are greater than texture height.");

            for (int y = 0; y < rowsNo; y++)
            {
                for (int x = 0; x < columnsNo; x++)
                {
                    var coord = Vector2.Multiply(new Vector2(x, y), new Vector2(cellWidth, cellHeight));
                    coord = Vector2.Add(coord, new Vector2(offsetX, offsetY));
                    bounds.Add(new Box2(coord.X, coord.Y, coord.X + cellWidth, coord.Y + cellHeight));
                }
            }

            return this;
        }

        public ISpriteAtlasBuilder SetTexture(int textureId)
        {
            var texture = textureMan.GetById(textureId);

            if (texture is null)
                throw new InvalidOperationException($"Texture with ID '{textureId}' not found.");

            Texture = texture;

            return this;
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

        internal List<SpriteData> GetSpriteData()
        {
            var data = new List<SpriteData>();

            foreach (var bound in bounds)
            {
                bound.Translate(offset);
                var spriteData = new SpriteData();
                spriteData.U = (int)bound.Left;
                spriteData.V = (int)bound.Top;
                spriteData.Width = (int)bound.Width;
                spriteData.Height = (int)bound.Height;

                spriteData.Vbo = spriteMan.CreateSpriteVertices(spriteData, Texture.Width, Texture.Height);

                data.Add(spriteData);
            }

            return data;
        }

        internal int Register(SpriteAtlas atlas)
        {
            return spriteMan.Register(Name, atlas);
        }

        #endregion Internal Methods
    }
}
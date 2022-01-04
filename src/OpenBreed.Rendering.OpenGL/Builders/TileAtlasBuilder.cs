using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    internal class TileAtlasBuilder : ITileAtlasBuilder
    {
        #region Internal Fields

        internal readonly List<Box2> bounds = new List<Box2>();
        internal Vector2 offset = Vector2.Zero;

        #endregion Internal Fields

        #region Private Fields

        private readonly ITextureMan textureMan;
        private readonly TileMan tileMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TileAtlasBuilder(TileMan tileMan, ITextureMan textureMan)
        {
            this.tileMan = tileMan;
            this.textureMan = textureMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal ITexture Texture { get; private set; }
        internal int TileSize { get; private set; }
        internal string Name { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public ITileAtlasBuilder SetName(string name)
        {
            if (tileMan.Contains(name))
                throw new InvalidOperationException($"Atlas with name '{name}' already exists.");

            Name = name;
            return this;
        }

        public ITileAtlasBuilder SetTileSize(int size)
        {
            if (size <= 0)
                throw new InvalidOperationException("TileSize must be set to greater than zero.");

            TileSize = size;
            return this;
        }

        public ITileAtlasBuilder AppendCoordsFromGrid(int columnsNo, int rowsNo, int offsetX = 0, int offsetY = 0)
        {
            if (Texture is null)
                throw new InvalidOperationException("Texture must be set first.");

            if(TileSize <= 0)
                throw new InvalidOperationException("TileSize must be set to greater than zero.");

            if (columnsNo <= 0)
                throw new InvalidOperationException("Appended grid cell columns number are less or equal zero.");

            if (rowsNo <= 0)
                throw new InvalidOperationException("Appended grid cell rows number are less or equal zero.");

            if (TileSize * (columnsNo - 1) >= Texture.Width)
                throw new InvalidOperationException("Appended grid X coordinates of sprites in texture are greater than texture width.");

            if (TileSize * (rowsNo - 1) >= Texture.Height)
                throw new InvalidOperationException("Appended grid Y coordinates of sprites in texture are greater than texture height.");

            for (int y = 0; y < rowsNo; y++)
            {
                for (int x = 0; x < columnsNo; x++)
                {
                    var coord = Vector2.Multiply(new Vector2(x, y), new Vector2(TileSize, TileSize));
                    coord = Vector2.Add(coord, new Vector2(offsetX, offsetY));
                    bounds.Add(new Box2(coord.X, coord.Y, coord.X + TileSize, coord.Y + TileSize));
                }
            }

            return this;
        }

        public ITileAtlasBuilder AppendCoords(int u, int v)
        {
            if (Texture is null)
                throw new InvalidOperationException("Texture must be set first.");

            if (TileSize <= 0)
                throw new InvalidOperationException("TileSize must be set to greater than zero.");

            var coord = new Vector2(u, v);
            bounds.Add(new Box2(coord.X, coord.Y, coord.X + TileSize, coord.Y + TileSize));

            return this;
        }

        public ITileAtlasBuilder SetTexture(int textureId)
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

        public ITileAtlas Build()
        {
            return new TileAtlas(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal List<TileData> GetTileData()
        {
            var data = new List<TileData>();

            foreach (var bound in bounds)
            {
                bound.Translate(offset);
                var tileData = new TileData
                {
                    U = (int)bound.Min.X,
                    V = (int)bound.Max.Y
                };

                tileData.Vbo = tileMan.CreateTileVertices(tileData, TileSize, Texture.Width, Texture.Height);

                data.Add(tileData);
            }

            return data;
        }

        internal int Register(TileAtlas atlas)
        {
            return tileMan.Register(Name, atlas);
        }

        #endregion Internal Methods
    }
}
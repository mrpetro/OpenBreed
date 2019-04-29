using OpenBreed.Game.Common;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace OpenBreed.Game.Rendering.Components
{
    public class Tile : IRenderComponent
    {
        public static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        private int vbo;
        private int ibo;

        private readonly Transformation transformation;
        private TileAtlas tileAtlas;
        private int tileId;

        #region Public Constructors

        public Tile(TileAtlas tileAtlas, int tileId, Transformation transformation)
        {
            this.tileAtlas = tileAtlas;
            this.tileId = tileId;
            this.transformation = transformation;

            var vertices = tileAtlas.GetVertices(tileId);

            RenderTools.Create(vertices, indices, out vbo, out ibo);
        }

        #endregion Public Constructors

        #region Public Properties

        public Type SystemType { get { return typeof(RenderSystem); } }

        public void GetMapIndices(out int x, out int y)
        {
            var pos = transformation.Value.ExtractTranslation();
            x = (int)pos.X / tileAtlas.TileSize;
            y = (int)pos.Y / tileAtlas.TileSize;
        }

        //public int X { get { return (int)position.Transformation.M41 / tileAtlas.TileSize; } }
        //public int Y { get { return (int)position.Transformation.M42 / tileAtlas.TileSize; } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IWorldSystem system)
        {
            throw new NotImplementedException();
        }

        public void Draw(Viewport viewport)
        {
            GL.PushMatrix();

            GL.BindTexture(TextureTarget.Texture2D, tileAtlas.Texture.Id);

            GL.MultMatrix(ref transformation.Value);
            RenderTools.Draw(viewport, vbo, ibo, 6);

            GL.PopMatrix();
        }

        public void Initialize(IWorldSystem system)
        {
            //throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}
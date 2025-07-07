using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class TileRenderer : ITileRenderer
    {
        #region Private Fields

        private readonly TileMan tileMan;

        #endregion Private Fields

        #region Public Constructors

        public TileRenderer(TileMan tileMan)
        {
            this.tileMan = tileMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IRenderView view, int atlasId, int imageId)
        {
            var atlas = tileMan.InternalGetById(atlasId);
            var size = atlas.TileSize;
            var vao = atlas.data[imageId].Vbo;

            if (vao == -1)
            {
                return;
            }

            view.Context.Primitives.DrawSprite(
                view,
                atlas.Texture,
                vao,
                new Vector3(0, 0, 0),
                Vector2.One,
                Color4.White);
        }

        #endregion Public Methods
    }
}
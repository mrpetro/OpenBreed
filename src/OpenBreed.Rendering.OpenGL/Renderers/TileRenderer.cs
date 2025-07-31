using OpenBreed.Common.Tools.Collections;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Renderers
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

        public void Render(IRenderView view, int atlasId, int tileId)
        {
            var atlas = tileMan.InternalGetById(atlasId);
            var vao = atlas.GetTileVao(view.Context, tileId);

            if (vao == -1)
            {
                return;
            }

            view.Context.Primitives.SetTextureShader(
                view,
                atlas.Texture,
                Matrix4.Identity,
                Color4.White);

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

        #endregion Public Methods
    }
}
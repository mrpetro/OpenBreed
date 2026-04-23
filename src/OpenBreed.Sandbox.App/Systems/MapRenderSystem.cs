using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Input.Abstractions;
using OpenBreed.Input.Abstractions.Events;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Sandbox.App.Components;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace OpenBreed.Sandbox.App.Systems
{
    [RequireEntityWith(typeof(MapComponent))]
    public class MapRenderSystem : IRenderableSystem
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public MapRenderSystem()
        {

        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
            foreach (var entity in entities)
            {
                var map = entity.Get<MapComponent>();

                Render(map, context.View, context.ViewBox);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Render(MapComponent map, IRenderView view, Box2 clipBox)
        {
            var dataGrid = map.Grid;
            var clipIndices = map.GetIndices(clipBox);

            GL.Enable(EnableCap.Texture2D);

            for (int layerNo = 0; layerNo < 1; layerNo++)
            {
                view.PushMatrix();
                view.Translate(new Vector3(clipIndices.Min.X * map.CellSize, clipIndices.Min.Y * map.CellSize, 0.0f));

                for (int y = clipIndices.Min.Y; y < clipIndices.Max.Y; y++)
                {
                    view.PushMatrix();

                    for (int x = clipIndices.Min.X; x < clipIndices.Max.X; x++)
                    {
                        var cellData = dataGrid.Get(new Vector2i(x, y));

                        if (cellData is not null)
                        {
                            view.Context.TileRenderer.Render(view, 0, cellData.GfxId);
                        }

                        view.Translate(new Vector3(map.CellSize, 0.0f, 0.0f));
                    }

                    view.PopMatrix();
                    view.Translate(new Vector3(0.0f, map.CellSize, 0.0f));
                }

                view.PopMatrix();
            }

            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Private Methods
    }
}
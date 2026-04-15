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

        private const int CellSize = 16;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Public Constructors

        public MapRenderSystem(IInputsMan inputsMan)
        {
            this.inputsMan = inputsMan ?? throw new ArgumentNullException(nameof(inputsMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
            var cursorDownIds = 0;// context.View.GetCursorDownIds();

            var addObstacle = false;

            //if (cursorDownIds.Any())
            //{
            //    addObstacle = true;
            //}

            var cursorPos4 = context.View.FromHostToWorldPoint((Vector2i)inputsMan.CursorPos);
            var indexPos = new Vector2i((int)(cursorPos4.X / CellSize), (int)(cursorPos4.Y / CellSize));

            foreach (var entity in entities)
            {
                var map = entity.Get<MapComponent>();

                if (addObstacle)
                {
                    var data = map.Grid.Get(indexPos);

                    if (data is null)
                    {
                        data = new CellData();
                        map.Grid.Set(indexPos, data);
                    }

                    data.GfxId = 0;
                }

                Render(map.Grid, context.View, context.ViewBox);
            }

        }

        #endregion Public Methods

        #region Private Methods

        private void Render(IDataGrid<CellData> dataGrid, IRenderView view, Box2 clipBox)
        {
            var width = dataGrid.Width;
            var height = dataGrid.Height;

            int leftIndex = (int)clipBox.Min.X / CellSize;
            int bottomIndex = (int)clipBox.Min.Y / CellSize;
            int rightIndex = (int)clipBox.Max.X / CellSize + 1;
            int topIndex = (int)clipBox.Max.Y / CellSize + 1;

            leftIndex = MathHelper.Clamp(leftIndex, 0, width);
            rightIndex = MathHelper.Clamp(rightIndex, 0, width);
            bottomIndex = MathHelper.Clamp(bottomIndex, 0, height);
            topIndex = MathHelper.Clamp(topIndex, 0, height);

            //if (CellBordersVisible)
            //    DrawCellBorders(leftIndex, bottomIndex, rightIndex, topIndex);

            GL.Enable(EnableCap.Texture2D);

            for (int layerNo = 0; layerNo < 1; layerNo++)
            {
                view.PushMatrix();
                view.Translate(new Vector3(leftIndex * CellSize, bottomIndex * CellSize, 0.0f));

                for (int y = bottomIndex; y < topIndex; y++)
                {
                    view.PushMatrix();

                    for (int x = leftIndex; x < rightIndex; x++)
                    {
                        var index = layerNo * width * height + x + width * y;

                        var cellData = dataGrid.Get(new Vector2i(x, y));

                        if (cellData is not null)
                        {
                            view.Context.TileRenderer.Render(view, 0, cellData.GfxId);
                        }

                        view.Translate(new Vector3(CellSize, 0.0f, 0.0f));
                    }

                    view.PopMatrix();
                    view.Translate(new Vector3(0.0f, CellSize, 0.0f));
                }

                view.PopMatrix();
            }

            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Private Methods
    }
}
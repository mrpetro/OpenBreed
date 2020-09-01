using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Maps.Layers;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class LayerActionRenderer : RendererBase<MapLayerBaseVM>
    {
        #region Private Fields

        private readonly MapEditorActionsToolVM _actionsTool;

        #endregion Private Fields

        #region Public Constructors

        public LayerActionRenderer(MapEditorActionsToolVM actionsTool, RenderTarget target) : base(target)
        {
            _actionsTool = actionsTool;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapLayerBaseVM renderable)
        {
            Render((MapLayerActionVM)renderable);
        }

        #endregion Public Methods

        #region Private Methods

        private void DrawAction(ActionSetVM actionSet, int id, float x, float y, int tileSize)
        {
            if (id >= actionSet.Items.Count)
                return;

            var action = actionSet.Items[id];

            if (!action.Visibility)
                return;

            var image = action.Icon;

            Target.Gfx.DrawImage(image, x, y, tileSize, tileSize);

        }

        private void Render(MapLayerActionVM renderable)
        {
            RectangleF viewRect = Target.Gfx.ClipBounds;

            int tileSize = renderable.Layout.Owner.TileSize;
            int xFrom = renderable.Layout.GetMapIndexX(viewRect.Left);
            int xTo = renderable.Layout.GetMapIndexX(viewRect.Right);
            int yFrom = renderable.Layout.GetMapIndexY(viewRect.Top);
            int yTo = renderable.Layout.GetMapIndexY(viewRect.Bottom);

            var actionSet = renderable.Layout.Owner.ActionSet;

            if (actionSet == null)
                return;

            for (int xIndex = xFrom; xIndex <= xTo; xIndex++)
            {
                for (int yIndex = yFrom; yIndex <= yTo; yIndex++)
                {
                    var propertyId = renderable.GetCell(xIndex, yIndex);
                    var x = xIndex * tileSize;
                    var y = yIndex * tileSize;

                    DrawAction(actionSet, propertyId, x, y, tileSize);
                }
            }
        }

        #endregion Private Methods
    }
}

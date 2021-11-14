using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Actions;
using OpenBreed.Model.Maps;
using System.Drawing;

namespace OpenBreed.Editor.VM.Renderer
{
    public class LayerActionRenderer : RendererBase<MapLayoutModel>
    {
        #region Private Fields

        private const MapLayerType layerType = MapLayerType.Action;

        private readonly MapEditorActionsToolVM _actionsTool;

        #endregion Private Fields

        #region Public Constructors

        public LayerActionRenderer(MapEditorActionsToolVM actionsTool, RenderTarget target) : base(target)
        {
            _actionsTool = actionsTool;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapLayoutModel renderable)
        {
            var layerIndex = renderable.GetLayerIndex(layerType);

            RectangleF viewRect = Target.ClipBounds;

            //TODO: Get this from model
            int tileSize = 16;
            int xFrom, xTo, yFrom, yTo;
            renderable.GetClipIndices(viewRect, out xFrom, out yFrom, out xTo, out yTo);

            var actionSet = _actionsTool.Parent.ActionSet;

            if (actionSet == null)
                return;

            for (int xIndex = xFrom; xIndex <= xTo; xIndex++)
            {
                for (int yIndex = yFrom; yIndex <= yTo; yIndex++)
                {
                    var propertyId = renderable.GetCellValue(layerIndex, xIndex, yIndex);
                    var x = xIndex * tileSize;
                    var y = yIndex * tileSize;

                    DrawAction(actionSet, propertyId, x, y, tileSize);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void DrawAction(ActionSetModel actionSet, int id, float x, float y, int tileSize)
        {
            if (id >= actionSet.Items.Count)
                return;

            var action = actionSet.Items[id];

            if (!action.Visibility)
                return;

            var image = action.Icon;

            Target.DrawImage(image, x, y, tileSize, tileSize);
        }

        #endregion Private Methods
    }
}
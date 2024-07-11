using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Model.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class LayoutRenderer : RendererBase<MapLayoutModel>
    {
        #region Private Fields

        private MapEditorVM _editor;
        private Dictionary<MapLayerType, RendererBase<MapLayoutModel>> _layerRenderers;

        #endregion Private Fields

        #region Public Constructors

        public LayoutRenderer(MapEditorVM editor, IRenderTarget target) : base(target)
        {
            _editor = editor;

            _layerRenderers = new Dictionary<MapLayerType, RendererBase<MapLayoutModel>>();
            _layerRenderers.Add(MapLayerType.Gfx, new LayerGfxRenderer(_editor.TilesTool, target));
            _layerRenderers.Add(MapLayerType.Action, new LayerActionRenderer(_editor.ActionsTool, target));
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapLayoutModel renderable)
        {
            var visibleLayers = renderable.Layers.Where(item => item.IsVisible);

            foreach (var layer in visibleLayers)
                _layerRenderers[layer.LayerType].Render(renderable);
        }

        #endregion Public Methods

    }
}

using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Maps.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class LayoutRenderer : RendererBase<MapLayoutVM>
    {
        #region Private Fields

        private MapEditorVM _editor;
        private Dictionary<Type, RendererBase<MapLayerBaseVM>> _layerRenderers;

        #endregion Private Fields

        #region Public Constructors

        public LayoutRenderer(MapEditorVM editor, RenderTarget target) : base(target)
        {
            _editor = editor;

            _layerRenderers = new Dictionary<Type, RendererBase<MapLayerBaseVM>>();
            _layerRenderers.Add(typeof(MapLayerGfxVM), new LayerGfxRenderer(_editor.TilesTool, target));
            _layerRenderers.Add(typeof(MapLayerActionVM), new LayerActionRenderer(_editor.ActionsTool, target));
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapLayoutVM renderable)
        {
            var visibleLayers = renderable.Layers.Where(item => item.IsVisible);

            foreach (var layer in visibleLayers)
                _layerRenderers[layer.GetType()].Render(layer);
        }

        #endregion Public Methods

    }
}

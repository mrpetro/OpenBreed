using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class BodyRenderer : RendererBase<MapLayoutVM>
    {
        #region Private Fields

        private LayerRenderer _layerRenderer;

        #endregion Private Fields

        #region Public Constructors

        public BodyRenderer(RenderTarget target) : base(target)
        {
            _layerRenderer = new LayerRenderer(target);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Render(MapLayoutVM renderable)
        {
            var visibleLayers = renderable.Layers.Where(item => item.IsVisible);

            foreach (var layer in visibleLayers)
                _layerRenderer.Render(layer);
        }

        #endregion Public Methods
    }
}

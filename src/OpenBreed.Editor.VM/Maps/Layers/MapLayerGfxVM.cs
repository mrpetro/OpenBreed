using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps.Layers
{
    public class MapLayerGfxVM : MapLayerBaseVM
    {
        private TileRef[] _cells;

        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        internal MapLayerGfxVM(MapLayoutVM layout) : base(layout)
        {
            _cells = new TileRef[layout.Size.Width * layout.Size.Height];
        }

        #endregion Public Constructors

        #region Public Properties

        public void SetCell(int x, int y, TileRef value)
        {
            _cells[y * Layout.Size.Width + x] = value;
        }

        public TileRef GetCell(int x, int y)
        {
            return _cells[y * Layout.Size.Width + x];
        }

        #endregion Public Properties

        #region Internal Methods

        public override void DrawView(Graphics gfx, Rectangle rectangle)
        {
            int tileSize = 16;

            for (int xIndex = rectangle.Left; xIndex <= rectangle.Right; xIndex++)
            {
                for (int yIndex = rectangle.Bottom; yIndex <= rectangle.Top; yIndex++)
                {
                    var tileRef = GetCell(xIndex, yIndex);

                    //Body.Map.Root.LevelEditor.TileSelector.DrawTile(gfx, tileRef, xIndex * tileSize, yIndex * tileSize, tileSize);
                    //Body.Map.Editor.PropertySet.DrawProperty(gfx, tile.PropertyId, xIndex * tileSize, yIndex * tileSize, tileSize);
                }
            }
        }

        public override void Restore(IMapLayerModel layerModel)
        {
            var gfxLayerModel = layerModel as MapLayerModel<TileRef>;

            if (gfxLayerModel == null)
                throw new ArgumentException(nameof(layerModel));

            _cells = gfxLayerModel.Cells.ToArray();
        }

        #endregion Internal Methods

    }
}

using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps.Layers
{
    public class MapBodyPropertyLayerVM : MapBodyBaseLayerVM
    {
        private int[] _cells;

        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        internal MapBodyPropertyLayerVM(MapBodyVM body) : base(body)
        {
            _cells = new int[body.Size.Width * body.Size.Height];
        }

        #endregion Public Constructors

        #region Public Properties

        public void SetCell(int x, int y, int value)
        {
            _cells[y * Body.Size.Width + x] = value;
        }

        public int GetCell(int x, int y)
        {
            return _cells[y * Body.Size.Width + x];
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
                    var propertyId = GetCell(xIndex, yIndex);

                    Body.Map.Root.PropSets.DrawProperty(gfx, propertyId, xIndex * tileSize, yIndex * tileSize, tileSize);
                }
            }
        }

        public override void Restore(IMapBodyLayerModel layerModel)
        {
            var propertyLayerModel = layerModel as MapBodyLayerModel<int>;

            if (propertyLayerModel == null)
                throw new ArgumentException(nameof(layerModel));

            _cells = propertyLayerModel.Cells.ToArray();
        }

        #endregion Internal Methods

    }
}

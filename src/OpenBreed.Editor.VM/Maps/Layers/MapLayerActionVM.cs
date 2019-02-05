using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps.Layers
{
    public class MapLayerActionVM : MapLayerBaseVM
    {
        #region Private Fields

        private int[] _cells;

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerActionVM(MapLayoutVM layout) : base(layout)
        {
            _cells = new int[layout.Size.Width * layout.Size.Height];
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void DrawView(Graphics gfx, Rectangle rectangle)
        {
            return;
            //if (Body.Map.Root.LevelEditor.CurrentLevel.PropSet == null)
            //    return;

            int tileSize = 16;

            for (int xIndex = rectangle.Left; xIndex <= rectangle.Right; xIndex++)
            {
                for (int yIndex = rectangle.Bottom; yIndex <= rectangle.Top; yIndex++)
                {
                    var propertyId = GetCell(xIndex, yIndex);

                    //Body.Map.Root.LevelEditor.CurrentLevel.PropSet.DrawProperty(gfx, propertyId, xIndex * tileSize, yIndex * tileSize, tileSize);
                }
            }
        }

        public int GetCell(int x, int y)
        {
            return _cells[y * Layout.Size.Width + x];
        }

        public override void Restore(IMapLayerModel layerModel)
        {
            var propertyLayerModel = layerModel as MapLayerModel<int>;

            if (propertyLayerModel == null)
                throw new ArgumentException(nameof(layerModel));

            _cells = propertyLayerModel.Cells.ToArray();
        }

        public void SetCell(int x, int y, int value)
        {
            _cells[y * Layout.Size.Width + x] = value;
        }

        #endregion Public Methods
    }
}
